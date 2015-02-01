using System;
using System.Collections.Generic;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using AuthBehavior.Helpers;

namespace AuthBehavior
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AuthenticationInspectorBehavior : Attribute, IDispatchMessageInspector,
        IClientMessageInspector, IEndpointBehavior, IServiceBehavior
    {
        #region IDispatchMessageInspector

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            try
            {
                var header = request.Headers.GetHeader<AuthenticationHeader>("authentication-header", "chsakell.com");

                if (header != null)
                {
                    string headerPassword = string.Empty;
                    string headerUsername = string.Empty;
                    string headerTimeSpan = string.Empty;

                    try
                    {
                        string decryptedSignature = Encryption.Decrypt(header.EncryptedSignature, true);

                        AuthenticationData headerData = Serializer.JsonDeserialize<AuthenticationData>(decryptedSignature);

                        headerUsername = headerData.Username;
                        headerPassword = headerData.Password;
                        headerTimeSpan = headerData.Timespan;
                    }
                    catch
                    {
                        throw new UnauthorizedAccessException("Unable to decrypt signature");
                    }

                    if (!string.IsNullOrEmpty(headerPassword) && (!string.IsNullOrEmpty(headerUsername)) && (!string.IsNullOrEmpty(headerTimeSpan)))
                    {
                        if (IsRequestValid(headerPassword, headerUsername, headerTimeSpan))
                            return null;
                        else
                            throw new UnauthorizedAccessException("Wrong credentials");
                    }
                    else
                    {
                        throw new MessageHeaderException("Missing credentials from request");
                    }
                }
                else
                    throw new MessageHeaderException("Authentication header not found");
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new FaultException(ex.Message);
            }
            catch (MessageHeaderException ex)
            {
                throw new FaultException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
        }

        #endregion

        #region IClientMessageInspector

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            DateTime now = DateTime.UtcNow;
            string timestamp = now.ToString("yyyy-MM-ddTHH:mm:ssZ", System.Globalization.CultureInfo.InvariantCulture);

            AuthenticationData authData = new AuthenticationData
            {
                Username = ClientAuthenticationHeaderContext.HeaderInformation.Username,
                Password = ClientAuthenticationHeaderContext.HeaderInformation.Password,
                Timespan = timestamp // This is the seed..
            };

            string serializedAuthData = Serializer.JsonSerializer<AuthenticationData>(authData);

            string signature = string.Empty;

            signature = Encryption.Encrypt(serializedAuthData, true);

            var encryptedHeader = new AuthenticationHeader
            {
                EncryptedSignature = signature
            };

            var typedHeader = new MessageHeader<AuthenticationHeader>(encryptedHeader);
            var untypedHeader = typedHeader.GetUntypedHeader("authentication-header", "chsakell.com");

            request.Headers.Add(untypedHeader);
            return null;
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
        }

        #endregion

        #region IEndpointBehavior

        public void Validate(ServiceEndpoint endpoint)
        {

        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            var channelDispatcher = endpointDispatcher.ChannelDispatcher;
            if (channelDispatcher == null) return;
            foreach (var ed in channelDispatcher.Endpoints)
            {
                var inspector = new AuthenticationInspectorBehavior();
                ed.DispatchRuntime.MessageInspectors.Add(inspector);
            }
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            var inspector = new AuthenticationInspectorBehavior();
            clientRuntime.MessageInspectors.Add(inspector);
        }

        #endregion

        #region IServiceBehaviour

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher cDispatcher in serviceHostBase.ChannelDispatchers)
            {
                foreach (var eDispatcher in cDispatcher.Endpoints)
                {
                    eDispatcher.DispatchRuntime.MessageInspectors.Add(new AuthenticationInspectorBehavior());
                }
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Authenticate request. Use Database for checking user credentials instead
        /// </summary>
        /// <param name="headerPassword"></param>
        /// <param name="headerUsername"></param>
        /// <param name="requestTimeUTCCreated"></param>
        /// <returns></returns>
        private bool IsRequestValid(string headerPassword, string headerUsername, string requestTimeUTCCreated)
        {
            bool isAuthenticatedRequest = new bool();

            try
            {
                string authUsername = "chris";
                string authPassword = "sakell";

                DateTime utcNow = DateTime.UtcNow;
                DateTime requestSent = DateTime.ParseExact(requestTimeUTCCreated, "yyyy-MM-ddTHH:mm:ssZ",
                    System.Globalization.CultureInfo.InvariantCulture).ToUniversalTime();

                int timeDiffInMinutes = (int)utcNow.Subtract(requestSent).TotalMinutes;

                if (headerPassword == authPassword && headerUsername == authUsername && Math.Abs(timeDiffInMinutes) < 5)
                    isAuthenticatedRequest = true;

                return isAuthenticatedRequest;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}