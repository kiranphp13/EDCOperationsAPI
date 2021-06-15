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
        public async Task<Dictionary<string, object>> Post([FromBody] BoService.Models.Administration.ContactType body)
        {
            await Db.Connection.OpenAsync();
            var query = new BoService.Models.Administration.ContactTypeQuery(Db);
            Dictionary<string, object> response = new Dictionary<string, object>();

            //Check Record Existws
            try
            {
                string cname = body.Type;
                var result = await query.GetContactTypeByName(cname, 0);
                if(result != null)
                {
                    response.Add("status", "Error");
                    response.Add("message", "Name Already Exists");

                }
                else
                {
                    int rec_no = await query.CreateRecord(body);

                    if(rec_no > 0)
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
            catch(Exception Ex)
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
        public async Task<Dictionary<string, object>> PutOne(int id, [FromBody] BoService.Models.Administration.ContactType body)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();

            await Db.Connection.OpenAsync();
            var query = new BoService.Models.Administration.ContactTypeQuery(Db);
            //var result = await query.GetContactTypeByID(id);
            //if (result is null)
              //  return new NotFoundResult();
            //var result1 = await query.UpdateRecord(id, body);
            //return new OkObjectResult(result1);

            try
            {
                var rec_exists = await query.GetContactTypeByID(id);
                if (rec_exists is null)
                {
                    throw new Exception("Record not found");
                }

                string cname = body.Type;
                var result = await query.GetContactTypeByName(cname, id);
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
