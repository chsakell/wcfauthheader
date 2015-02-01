using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace BlogService.Lib
{
    [ServiceContract]
    public interface IBlogService
    {
        [OperationContract]
        List<Post> ReadPosts();
    }

    [DataContract]
    public class Post
    {
        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public DateTime DateCreated { get; set; }

        [DataMember]
        public string Category { get; set; }

        [DataMember]
        public string Contents { get; set; }
    }
}
