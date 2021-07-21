using BoService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDCOperationsAPI.Controllers.Contacts
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : Controller
    {
        public BoAppDB Db { get; }
        public StateController(BoAppDB db)
        {
            Db = db;
        }

        // GET api/state
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            await Db.Connection.OpenAsync();
            var query = new BoService.Models.Contacts.ProspectQuery(Db);
            var result = await query.GetStateList();
            int length = result.Count;

            return new OkObjectResult(result);
        }
    }
}
