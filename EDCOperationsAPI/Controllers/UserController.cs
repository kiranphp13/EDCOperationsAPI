using BoService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BoService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IConfiguration _config;
        public BoAppDB Db { get; }
        public UserController(BoAppDB db, IConfiguration config)
        {
            Db = db;
            _config = config;
        }

        [HttpGet]
        [Route("GetUsers")]
        public List<User> GetUsers()
        {
            try
            {
                Db.Connection.Open();
                List<User> returnList = new List<User>();
                try
                {
                    using var cmd = Db.Connection.CreateCommand();
                    string commandText = "SELECT FullName,Password,Email,Phone,Role,Id,Address,UserName,Status,DATE_FORMAT(CreatedDate,'%m/%d/%Y') AS CreatedDate from edc.bousers;";
                    cmd.CommandText = commandText;
                    returnList = ReadAllAsync(cmd.ExecuteReader());
                }
                catch (Exception Ex)
                {
                    returnList = null;
                }
                return returnList;

            }
            catch (Exception)
            {
                throw;
            }
        }
        private List<User> ReadAllAsync(System.Data.Common.DbDataReader reader)
        {
            List<User> posts = new List<User>();
            try
            {
                using (reader)
                {
                    while (reader.Read())
                    {
                        var post = new User(Db)
                        {
                            FullName = reader.GetString(0),
                            Password = DecryptPassword(reader.GetString(1)),
                            Email = reader.GetString(2),
                            Phone = reader.GetString(3),
                            Role = reader.GetString(4),
                            Id = reader.GetInt32(5),
                            Address = Convert.ToString(reader.GetString(6)),
                            UserName = reader.GetString(7),
                            Status = reader.GetString(8),
                            CreatedDate = Convert.ToDateTime(reader.GetString(9)),
                        };
                        posts.Add(post);
                    }
                }
            }
            catch (Exception Ex)
            {
                posts = null;
            }
            return posts;
        }

        public string DecryptPassword(string strPassword)
        {
            string strReturnPassword = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(strPassword))
                {
                    throw new ArgumentNullException("Password should not be null or empty...");
                }
                else
                {
                    strReturnPassword = SecurePassword.DecryptPassword(strPassword, SecurePassword.EncDecType.BASE64);
                }
            }
            catch (Exception Ex)
            {

            }
            return strReturnPassword;
        }

        // GET api/<controller>/5  
        [HttpGet]
        [Route("GetUser/{userId}")]
        public User GetUserById(string userId)
        {
            try
            {
                Db.Connection.Open();
                List<User> returnList = new List<User>();
                try
                {
                    using var cmd = Db.Connection.CreateCommand();
                    string commandText = "SELECT FullName,Password,Email,Phone,Role,Id,Address,UserName,Status,DATE_FORMAT(CreatedDate,'%m/%d/%Y') AS CreatedDate FROM edc.bousers where id=" + Convert.ToInt32(userId);
                    cmd.CommandText = commandText;
                    returnList = ReadAllAsync(cmd.ExecuteReader());
                }
                catch (Exception Ex)
                {
                    returnList = null;
                }
                return returnList.FirstOrDefault();

            }
            catch (Exception)
            {
                throw;
            }
        }
        //// POST api/<controller>  
        //[System.Web.Http.HttpPost]
        //[System.Web.Http.Route("InsertUser")]
        //public IHttpActionResult PostUser(User data)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        return (IHttpActionResult)BadRequest(ModelState);
        //    }
        //    try
        //    {
        //       // objEntity.Employees.Add(data);
        //        //objEntity.SaveChanges();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        [HttpPost]
        [Route("CreateUser")]
        public Dictionary<string, object> CreateUser([FromBody] BoService.Models.User value)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            try
            {
                Db.Connection.Open();
                bool bIsValidUser = false;

                BoService.Models.User objUser = new BoService.Models.User(Db);

                bool bIsUserCreated = false;
                objUser.UserName = value.UserName;
                objUser.Password = value.Password;

                bIsValidUser = objUser.IsUserNameExists();
                if (bIsValidUser == false)
                {
                    bIsUserCreated = objUser.CreateUser(value);
                    if (bIsUserCreated == true)
                    {
                        var jwt = new BoService.Authentication.JwtService(_config);
                        var token = jwt.GenerateSecurityToken(value.UserName);

                        response.Add("Status", "Success");
                        response.Add("Message", "User is added successfully...");
                        response.Add("token", token);
                        response.Add("user", objUser);
                    }
                    else
                    {
                        response.Add("Status", "Error");
                        response.Add("Message", "There is something happend while inserting record");
                    }
                }
                else
                {
                    response.Add("Status", "Error");
                    response.Add("Message", "User Name Already Exists");
                }
            }
            catch (Exception Ex)
            {
                response.Add("status", "Error");
                response.Add("Message", Ex.Message);
            }
            return response;
        }


        [HttpPost]
        [Route("UpdateUser")]
        public Dictionary<string, object> UpdateUser([FromBody] BoService.Models.User value)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            try
            {
                Db.Connection.Open();
                bool bIsValidUser = false;

                BoService.Models.User objUser = new BoService.Models.User(Db);

                bool bIsUserCreated = false;
                objUser.UserName = value.UserName;
                objUser.Id = value.Id;

                bIsValidUser = objUser.IsUserNameExistsEdit();
                if (bIsValidUser == false)
                {
                    bIsUserCreated = objUser.UpdateUser(value);
                    if (bIsUserCreated == true)
                    {
                        var jwt = new BoService.Authentication.JwtService(_config);
                        var token = jwt.GenerateSecurityToken(value.UserName);

                        response.Add("Status", "Success");
                        response.Add("Message", "User is added successfully...");
                        response.Add("token", token);
                        response.Add("user", objUser);
                    }
                    else
                    {
                        response.Add("Status", "Error");
                        response.Add("Message", "User Name Already Exists");
                    }
                }
            }
            catch (Exception Ex)
            {
                response.Add("status", "Error");
                response.Add("Message", Ex.Message);
            }
            return response;
        }

        [HttpPost]
        [Route("UpdateProfile")]
        public Dictionary<string, object> UpdateProfile([FromBody] BoService.Models.User value)
        {


            Dictionary<string, object> response = new Dictionary<string, object>();
            try
            {
                Db.Connection.Open();
                bool bIsValidUser = false;

                BoService.Models.User objUser = new BoService.Models.User(Db);

                bool bIsUserCreated = false;
                objUser.UserName = value.UserName;
                objUser.Id = value.Id;

                bIsValidUser = objUser.IsUserNameExistsEdit();
                if (bIsValidUser == false)
                {
                    bIsUserCreated = objUser.UpdateProfile(value);
                    if (bIsUserCreated == true)
                    {
                        var jwt = new BoService.Authentication.JwtService(_config);
                        var token = jwt.GenerateSecurityToken(value.UserName);

                        response.Add("Status", "Success");
                        response.Add("Message", "User is added successfully...");
                        response.Add("token", token);
                        response.Add("user", objUser);
                    }
                    else
                    {
                        response.Add("Status", "Error");
                        response.Add("Message", "User Name Already Exists");
                    }
                }
            }
            catch (Exception Ex)
            {
                response.Add("status", "Error");
                response.Add("Message", Ex.Message);
            }
            return response;
        }
    }
}
