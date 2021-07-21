using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using BoService;
using MySqlConnector;

namespace BoService.Models.Contacts
{
    public class ProspectList
    {
       
        public int Id { get; set; }

        public DateTime UpdateDate { get; set; }


        public string Salute { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
        public string Middle { get; set; }



        public string Phone1 { get; set; }
    


        public string Email { get; set; }

        public string ActiveStatus { get; set; }

        public ProspectList()
        {
        }
    }
}

