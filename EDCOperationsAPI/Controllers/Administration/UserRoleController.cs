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
    public class UserRoleController : Controller
    {
        public BoAppDB Db { get; }
        public UserRoleController(BoAppDB db)
        {
            Db = db;
        }

        // GET api/collateral
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            await Db.Connection.OpenAsync();
            var query = new BoService.Models.Administration.UserRoleQuery(Db);
            var result = await query.GetAll();
            int length = result.Count;

            return new OkObjectResult(result);
        }

        [HttpGet("{id}")]
        [Route("GetOne")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new BoService.Models.Administration.UserRoleQuery(Db);
            var result = await query.GetByID(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/contacttype
        [HttpPost]
        public async Task<Dictionary<string, object>> Post([FromBody] BoService.Models.Administration.UserRole body)
        {
            await Db.Connection.OpenAsync();
            var query = new BoService.Models.Administration.UserRoleQuery(Db);
            Dictionary<string, object> response = new Dictionary<string, object>();

            //Check Record Existws
            try
            {
                string name = body.Name;
                var result = await query.GetByName(name, 0);
                if (result != null)
                {
                    response.Add("status", "Error");
                    response.Add("message", "Name Already Exists");

                }
                else
                {
                    int rec_no = await query.CreateRecord(body);

                    if (rec_no > 0)
                    {
                        response.Add("status", "Success");
                        response.Add("message", "Record Created Exists");
                        response.Add("rec_no", rec_no);

                    }
                    else
                    {
                        throw new Exception("Record Insert Error");
                    }

                }

            }
            catch (Exception Ex)
            {
                response.Add("status", "Error");
                response.Add("message", Ex.Message);

            }
            finally
            {
                Db.Connection.Close();
            }
            return response;

        }


        // PUT api/contacttype/5
        [HttpPut("{id}")]
        public async Task<Dictionary<string, object>> PutOne(int id, [FromBody] BoService.Models.Administration.UserRole body)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();

            await Db.Connection.OpenAsync();
            var query = new BoService.Models.Administration.UserRoleQuery(Db);

            try
            {
                var rec_exists = await query.GetByID(id);
                if (rec_exists is null)
                {
                    throw new Exception("Record not found");
                }

                string name = body.Name;
                var result = await query.GetByName(name, id);
                if (result != null)
                {
                    response.Add("status", "Error");
                    response.Add("message", "Name Already Exists");

                }
                else
                {
                    int rec_no = await query.UpdateRecord(id, body);

                    if (rec_no > 0)
                    {
                        response.Add("status", "Success");
                        response.Add("message", "Record updated successfully");
                        response.Add("rec_no", rec_no);

                    }
                    else
                    {
                        throw new Exception("Record Insert Error");
                    }

                }

            }
            catch (Exception Ex)
            {
                response.Add("status", "Error");
                response.Add("message", Ex.Message);

            }
            finally
            {
                Db.Connection.Close();
            }
            return response;
        }
    }
}
