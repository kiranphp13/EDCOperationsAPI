using BoService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDCOperationsAPI.Controllers.Administration
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactTypeController : Controller
    {
        public BoAppDB Db { get; }
        public ContactTypeController(BoAppDB db)
        {
            Db = db;
        }

        // GET api/contacttype
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            await Db.Connection.OpenAsync();
            var query = new BoService.Models.Administration.ContactTypeQuery(Db);
            var result = await query.GetAllContactTypes();
            int length = result.Count;

            return new OkObjectResult(result);
        }


        [HttpGet("{id}")]
        [Route("GetOne")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new BoService.Models.Administration.ContactTypeQuery(Db);
            var result = await query.GetContactTypeByID(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/contacttype
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BoService.Models.Administration.ContactType body)
        {
            await Db.Connection.OpenAsync();
            var query = new BoService.Models.Administration.ContactTypeQuery(Db);
            var result = await query.CreateRecord(body);
            return new OkObjectResult(result);
        }

        // PUT api/contacttype/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOne(int id, [FromBody] BoService.Models.Administration.ContactType body)
        {
            await Db.Connection.OpenAsync();
            var query = new BoService.Models.Administration.ContactTypeQuery(Db);
            var result = await query.GetContactTypeByID(id);
            if (result is null)
                return new NotFoundResult();
            var result1 = await query.UpdateRecord(id, body);
            return new OkObjectResult(result1);
        }
    }
}
