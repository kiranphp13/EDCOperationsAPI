using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using BoService;
using MySqlConnector;

namespace BoService.Models.Contacts
{
    public class Prospect
    {
        /*
   
        UpdatedBy 
UpdatedByUserID 
AgencyID 
UserNo 
ContactType 
ContactTypeID 
Salute 
First 
Last 
Middle 
Title 
Company 
Address1 
Address2 
Phone1 
Phone2 
City 
StateID 
Zip 
Country 
Email 
ActiveStatus
         */
        public int Id { get; set; }

        public DateTime UpdateDate { get; set; }

        public string UpdatedBy { get; set; }
        public int UpdatedByUserId { get; set; }

        public int AgencyId { get; set; }

        public string AgencyName { get; set; }

        public int UserNo { get; set; }

        public int ContactTypeId { get; set; }

        public string ContactType { get; set; }

        public string Salute { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
        public string Middle { get; set; }



        public string Title { get; set; }
        public string Company { get; set; }
        
       
        public string Address1 { get; set; }

        public string Address2 { get; set; }


        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string City { get; set; }

        public int StateId { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public string Country { get; set; }


        public string Email { get; set; }

        public string ActiveStatus { get; set; }

        public string Profession { get; set; }

        public int ContactCategoryId { get; set; }



        public Prospect()
        {
        }
    }
}

