﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO.Pipelines;
using System.Threading.Tasks;
using System;
using Azure.Storage.Queues;

namespace CoreWCF.Channels
{
    internal interface IQueueBase
    {
        Task Send(PipeReader message, Uri endpoint);

        public Task<Azure.Response<Azure.Storage.Queues.Models.QueueMessage>> ReceiveMessageAsync(TimeSpan? visibilityTimeout = default, System.Threading.CancellationToken cancellationToken = default);

        public Azure.Response DeleteMessage(string messageId, string popReceipt, System.Threading.CancellationToken cancellationToken = default);
    }
}
