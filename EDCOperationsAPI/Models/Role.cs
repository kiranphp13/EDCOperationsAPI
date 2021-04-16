using BoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDCOperationsAPI.Models
{
    public class Role
    {
        public string RoleName { get; set; }


        public int RoleId { get; set; }
        internal BoAppDB Db { get; set; }
        internal Role(BoAppDB db)
        {
            Db = db;

        }
    }
}
