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
    public class CollateralController : Controller
    {
        public BoAppDB Db { get; }
        public CollateralController(BoAppDB db)
        {
            Db = db;
        }

        // GET api/collateral
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            await Db.Connection.OpenAsync();
            var query = new BoService.Models.Administration.CollateralQuery(Db);
            var result = await query.GetAll();
            int length = result.Count;

            return new OkObjectResult(result);
        }
    }
}
