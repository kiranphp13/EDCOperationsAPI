using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using BoService;
using MySqlConnector;

namespace BoService.Models.Contacts
{
    public class StateList
    {
        public int Id { get; set; }


        public string ShortName { get; set; }

        public string Name { get; set; }

        

        public StateList()
        {
        }
    }
}

