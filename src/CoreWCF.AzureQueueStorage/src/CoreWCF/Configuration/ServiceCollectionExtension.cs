﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CoreWCF.Channels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CoreWCF.Configuration
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddServiceModelAzureQueueStorageSupport(this IServiceCollection services)
        {
            services.AddSingleton<MessageQueue, MessageQueue>();
            services.AddSingleton<DeadLetterQueue, DeadLetterQueue>();
            return services;
        }
    }
}
