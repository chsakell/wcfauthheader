using BlogService.ExternalClient.BlogService;
using BlogService.ExternalClient.Contracts;
using BlogService.ExternalClient.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace BlogService.ExternalClient
{
    class Program
    {
        static string hostURI = "http://localhost:9001/BlogService";

        static void Main(string[] args)
        {
            try
            {
                ReadPosts();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }

        static void ReadPosts()
        {
            try
            {
                // Create a simple client
                BlogServiceClient blogClient = CreateBlogServiceClient();
                using (new OperationContextScope(blogClient.InnerChannel))
                {
                    var headerData = GenerateEncryptedHeader("chris", "sakell");

                    MessageHeader aMessageHeader = MessageHeader.CreateHeader("authentication-header", "chsakell.com", headerData);
                    OperationContext.Current.OutgoingMessageHeaders.Add(aMessageHeader);

                    List<Post> blogPosts = blogClient.ReadPosts();

                    foreach (Post post in blogPosts)
                    {
                        Console.WriteLine(post.Title);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Create Service Client

        private static BlogServiceClient CreateBlogServiceClient()
        {
            BlogServiceClient blogServiceClient = null;
            System.ServiceModel.BasicHttpBinding basicHttpbinding = new System.ServiceModel.BasicHttpBinding(System.ServiceModel.BasicHttpSecurityMode.None);
            basicHttpbinding.Name = "BasicHttpBinding_IBlogService";
            basicHttpbinding.MaxReceivedMessageSize = 2147483646;
            basicHttpbinding.MaxBufferSize = 2147483646;
            basicHttpbinding.MaxBufferPoolSize = 2147483646;

            basicHttpbinding.ReaderQuotas.MaxArrayLength = 2147483646;
            basicHttpbinding.ReaderQuotas.MaxStringContentLength = 5242880;
            basicHttpbinding.SendTimeout = new TimeSpan(0, 5, 0);
            basicHttpbinding.CloseTimeout = new TimeSpan(0, 5, 0);

            basicHttpbinding.Security.Mode = BasicHttpSecurityMode.None;
            basicHttpbinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;

            System.ServiceModel.EndpointAddress endpointAddress = new System.ServiceModel.EndpointAddress(hostURI);

            blogServiceClient = new BlogServiceClient(basicHttpbinding, endpointAddress);

            return blogServiceClient;
        }

        #endregion

        #region Prepare header data

        private static AuthenticationHeader GenerateEncryptedHeader(string username, string password)
        {
            try
            {
                // Prepare authentication data to send
                AuthenticationData authData = PrepareAuthenticationData(username, password);

                // Serialize data
                string serializedAuthData = Serializer.JsonSerializer<AuthenticationData>(authData);

                // Encrypt data
                string signature = Encryption.Encrypt(serializedAuthData, true);

                var encryptedHeader = new AuthenticationHeader
                {
                    EncryptedSignature = signature
                };

                return encryptedHeader;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static AuthenticationData PrepareAuthenticationData(string username, string password)
        {
            try
            {
                // This will be the seed..
                DateTime now = DateTime.UtcNow;
                string timestamp = now.ToString("yyyy-MM-ddTHH:mm:ssZ", System.Globalization.CultureInfo.InvariantCulture);

                AuthenticationData authData = new AuthenticationData
                {
                    Username = username,
                    Password = password,
                    Timespan = timestamp
                };

                return authData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
