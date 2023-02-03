// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

namespace CoreWCF.Channels
{
    internal class DeadLetterQueue : IQueueBase
    {
        private QueueClient _client;
        private ArraySegment<byte> _messageBuffer;

        public DeadLetterQueue(string connectionString, string queueName)
        {
            _client = new QueueClient(connectionString, queueName);
        }

        public QueueClient queueClient { get => _client; set => _client = value; }

        public Response DeleteMessage(string messageId, string popReceipt, CancellationToken cancellationToken = default)
        {
            return _client.DeleteMessage(messageId, popReceipt, cancellationToken);
        }

        public Task<Response<QueueMessage>> ReceiveMessageAsync(TimeSpan? visibilityTimeout = null, CancellationToken cancellationToken = default)
        {
            return _client.ReceiveMessageAsync(visibilityTimeout, cancellationToken);
        }

        public async Task SendMessageAsync(Message requestMessage)
        {
            TimeSpan timeSpan = default;
            CancellationTokenSource cts = new(timeSpan);

            try
            {
                //_messageBuffer = EncodeMessage(requestMessage);
                BinaryData binaryData = new(new ReadOnlyMemory<byte>(_messageBuffer.Array, _messageBuffer.Offset, _messageBuffer.Count));
                await _client.SendMessageAsync(binaryData, default, default, cts.Token).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw AzureQueueStorageChannelHelpers.ConvertTransferException(e);
            }
            finally
            {
                CleanupBuffer();
                cts.Dispose();
            }
        }

        private void CleanupBuffer()
        {
            if (_messageBuffer.Array != null)
            {
                //_parent.BufferManager.ReturnBuffer(_messageBuffer.Array); //TODO get parent property passed in here
                _messageBuffer = new ArraySegment<byte>();
            }
        }

        /// <summary>
        /// Address the Message and serialize it into a byte array.
        /// </summary>
        /*private ArraySegment<byte> EncodeMessage(Message message)
        {
        // TODO
            try
            {
                //this._remoteAddress.ApplyTo(message);
                //return _encoder.WriteMessage(message, int.MaxValue, _parent.BufferManager);
            }
            finally
            {
                // We have consumed the message by serializing it, so clean up
                message.Close();
            }
        }*/
    }
}
