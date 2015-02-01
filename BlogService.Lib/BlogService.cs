using AuthBehavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogService.Lib
{
    [AuthenticationInspectorBehavior]
    public class BlogService : IBlogService
    {
        public List<Post> ReadPosts()
        {
            return new List<Post>()
            {
                new Post() { 
                    Title = "Asynchronously upload multiple photos to Azure blob container", 
                    Category = "Azure", 
                    DateCreated = DateTime.Now.AddMonths(-5), 
                    Contents = "In this post we are going to see how to asynchronously upload photos to an Azure blob container.." 
                },
                new Post() { 
                    Title = "Insert millions of records in SQL Server table at once", 
                    Category = "SQL", 
                    DateCreated = DateTime.Now.AddMonths(-6), 
                    Contents = "In this post we will see how to insert a really large amount of records in a SQL Server Table at once.." 
                },
                new Post() { 
                    Title = "SqlCommands: What to choose between SqlDataAdapter, ExecuteScalar and ExecuteNonQuery", 
                    Category = "C#", 
                    DateCreated = DateTime.Now.AddMonths(-7), 
                    Contents = "SqlCommand class can be used in various ways to access database data and it’s true story.." 
                },
                new Post() { 
                    Title = "Ajax enabled lists using the Ext.NET component framework", 
                    Category = "Ext.NET", 
                    DateCreated = DateTime.Now.AddMonths(-8), 
                    Contents = "As you may know, there are many UI frameworks available, that you can make use of in order to create.." 
                },
                new Post() { 
                    Title = "Asynchronous programming using Tasks", 
                    Category = "C#", 
                    DateCreated = DateTime.Now.AddMonths(-9), 
                    Contents = "Asynchronous programming model allows you to run and complete several tasks in a parallel way.." 
                }
            };
        }
    }
}
