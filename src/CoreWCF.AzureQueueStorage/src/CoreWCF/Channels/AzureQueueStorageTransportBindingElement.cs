// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Net;
using CoreWCF.Configuration;
using CoreWCF.Queue.Common;
using CoreWCF.Queue.Common.Configuration;
using Azure.Storage.Queues;

namespace CoreWCF.Channels
{
    public class AzureQueueStorageTransportBindingElement : QueueBaseTransportBindingElement
    {
        /// <summary>
        /// Creates a new instance of the AzureQueueStorageTransportBindingElement Class using the default protocol.
        /// </summary>
        public AzureQueueStorageTransportBindingElement()
        {
        }

        private AzureQueueStorageTransportBindingElement(AzureQueueStorageTransportBindingElement other)
        {
            //BrokerProtocol = other.BrokerProtocol;
            Credentials = other.Credentials;
            VirtualHost = other.VirtualHost;
        }

        public override BindingElement Clone()
        {
            return new AzureQueueStorageTransportBindingElement(this);
        }


        public override T GetProperty<T>(BindingContext context)
        {
            if (context == null)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull(nameof(context));
            }

            if (typeof(T) == typeof(ISecurityCapabilities))
            {
                return null;
            }

            return base.GetProperty<T>(context);
        }

        public override QueueTransportPump BuildQueueTransportPump(BindingContext context)
        {
            var serviceProvider = context.BindingParameters.Find<IServiceProvider>();
            var serviceDispatcher = context.BindingParameters.Find<IServiceDispatcher>();
            return new AzureQueueStorageTransportPump(serviceProvider, serviceDispatcher);
        }

        /// <summary>
        /// Gets the scheme used by the binding, soap.amqp
        /// </summary>
        public override string Scheme
        {
            get { return CurrentVersion.Scheme; }
        }

        /// <summary>
        /// The largest receivable encoded message
        /// </summary>
        public override long MaxReceivedMessageSize { get; set; }

        /// <summary>
        /// The credentials to use when authenticating with the broker
        /// </summary>
        internal ICredentials Credentials { get; set; }

        /// <summary>
        /// Specifies the broker virtual host
        /// </summary>
        internal string VirtualHost { get; set; }

        /// <summary>
        /// Specifies the version of the AMQP protocol that should be used to
        /// communicate with the broker
        /// </summary>
        //public IProtocol BrokerProtocol { get; set; }

    }
}
