// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Queues.Models;

namespace CoreWCF.Channels
{
    internal class MessageQueue : IQueueBase
    {
        public Response DeleteMessage(string messageId, string popReceipt, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public Task<Response<QueueMessage>> ReceiveMessageAsync(TimeSpan? visibilityTimeout = null, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public Task Send(PipeReader message, Uri endpoint) => throw new NotImplementedException();
    }
}
