using BoService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDCOperationsAPI.Controllers.Test
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : Controller
    {
        public BoAppDB Db { get; }
        public TestController(BoAppDB db)
        {
            Db = db;
        }

        [Route("customers/{customerId}/orders")]
        [HttpGet]
        public string FindOrdersByCustomer(int customerId) {
            return "test";
        }

    }
}
