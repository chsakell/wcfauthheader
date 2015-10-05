using AuthBehavior;
using BlogService.InternalClient.BlogService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace BlogService.InternalClient
{
    class Program
    {
        static string hostURI = "http://localhost:9001/BlogService";

        static void Main(string[] args)
        {
            try
            {
                ReadBlogPosts();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }

        #region Service Methods

        static void ReadBlogPosts()
        {
            try
            {
                BlogServiceClient client = CreateBlogServiceClient("chris","sakell");
                List<Post> blogPosts = client.ReadPosts();

                foreach(Post post in blogPosts)
                {
                    Console.WriteLine(post.Title);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            Console.ReadKey();
        }

        #endregion

        #region Create Service Client

        private static BlogServiceClient CreateBlogServiceClient(string username, string password)
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

            blogServiceClient.ChannelFactory.Endpoint.Behaviors.Add(new AuthenticationInspectorBehavior());
            ClientAuthenticationHeaderContext.HeaderInformation.Username = username;
            ClientAuthenticationHeaderContext.HeaderInformation.Password = password;

            return blogServiceClient;
        }

        #endregion
    }
}
