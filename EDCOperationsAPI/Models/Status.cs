using BoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDCOperationsAPI.Models
{
    public class Status
    {
        public string StatusName { get; set; }

     
        public int StatusId { get; set; }

        internal BoAppDB Db { get; set; }
        internal Status(BoAppDB db)
        {
            Db = db;
            
        }
    }
}
