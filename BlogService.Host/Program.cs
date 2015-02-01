using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;

namespace BlogService.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            string hostURI = "http://127.0.0.1:9001/BlogService";
            ServiceHost svcHost = null;
            try
            {
                Uri baseAddress = new Uri(hostURI);
                svcHost = new ServiceHost(typeof(BlogService.Lib.BlogService), baseAddress);
                ServiceMetadataBehavior servBehavior = new ServiceMetadataBehavior();
                ServiceDebugBehavior servDebugBehavior = new ServiceDebugBehavior();
                servBehavior.HttpGetEnabled = true;
                servDebugBehavior.IncludeExceptionDetailInFaults = true;

                svcHost.Description.Behaviors.Add(servBehavior);
                svcHost.Description.Behaviors.Find<ServiceDebugBehavior>().IncludeExceptionDetailInFaults = true;

                svcHost.Open();

                Console.WriteLine("\n\nBlog Service is Running  at following address");
                Console.WriteLine(baseAddress);

            }
            catch (Exception ex)
            {
                svcHost = null;
                Console.WriteLine("Blog Service can not be started \n\nError Message [" + ex.Message + "]");
            }

            if (svcHost != null)
            {
                Console.WriteLine("\nPress any key to close the Service");
                Console.ReadKey();
                svcHost.Close();
                svcHost = null;
            }

            Console.ReadKey();
        }
    }
}
