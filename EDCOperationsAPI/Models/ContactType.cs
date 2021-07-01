using BoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDCOperationsAPI.Models
{
    public class ContactType
    {
        public string Type { get; set; }

        public string Description { get; set; }

       // public DateTime UpdatedDate { get; set; }

        internal BoAppDB Db { get; set; }
        internal ContactType(BoAppDB db)
        {
            Db = db;
        }
    }
}
