using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using BoService;
using MySqlConnector;

namespace BoService.Models.Administration
{
    public class Agency
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UpdatedBy { get; set; }
        public int UpdatedByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
       
        public Agency()
        {
        }
    }
}

