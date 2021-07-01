using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySqlConnector;

namespace BoService.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }


        internal BoAppDB Db { get; set; }

        public BlogPost()
        {
        }

        internal BlogPost(BoAppDB db)
        {
            Db = db;
        }
    }
     
}
