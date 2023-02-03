// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Configuration;
using CoreWCF.Channels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace CoreWCF.Configuration
{
    public static class ServiceCollectionExtension
    {
        //TODO get Configuration manager here or move this entire thing to startup
        public static IServiceCollection AddServiceModelAzureQueueStorageSupport( IServiceCollection services)
        {
            string messageQueueConnectionString = ConfigurationManager.AppSettings["MessageQueueStorageConnectionString"];
            string deadLetterQueueConnectionString = ConfigurationManager.AppSettings["DeadLetterQueueStorageConnectionString"];

            string messageQueueName = ConfigurationManager.AppSettings["MessageQueueName"];
            string deadLetterQueueName = ConfigurationManager.AppSettings["DeadletterQueueName"];

            services.AddSingleton<MessageQueue, MessageQueue>(x =>
            {
                return new MessageQueue(messageQueueConnectionString, messageQueueName);

            });
            
            services.AddSingleton<DeadLetterQueue, DeadLetterQueue>(x =>
            {
                return new DeadLetterQueue(deadLetterQueueConnectionString, deadLetterQueueName);

            });
            return services;
        }
    }
}
