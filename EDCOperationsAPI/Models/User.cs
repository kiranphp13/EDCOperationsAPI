using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoService.Models
{
    public class User
    {
        public string FullName { get; set; }
        public string Password { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }

        public string Address { get; set; }
        public string UserName { get; set; }

        public string Role { get; set; }

        public string Token { get; set; }

        public string Status { get; set; }

        public int RoleId { get; set; }

       public int StatusId { get; set; }
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        internal BoAppDB Db { get; set; }

        public User()
        {
            FullName = string.Empty;
            Password = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;

        }

        internal User(BoAppDB db)
        {
            Db = db;
            FullName = string.Empty;
            Password = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;
            Status = string.Empty;
        }

        public List<User> GetUsersList(string strUserName, string strUserOassword)
        {
            List<User> returnList = new List<User>();
            try
            {
                string strEncryptedPassword = EncryptPassword(strUserOassword);
                using var cmd = Db.Connection.CreateCommand();
                string commandText = "Call p_GetAllUsers('" + strUserName + "','" + strEncryptedPassword + "')";
                cmd.CommandText = commandText;
                returnList = ReadAllAsync(cmd.ExecuteReader());
            }
            catch (Exception Ex)
            {
                returnList = null;
            }
            return returnList;
        }
        public List<User> GetUsersListByUserName(string strUserName)
        {
            List<User> returnList = new List<User>();
            try
            {
                using var cmd = Db.Connection.CreateCommand();
                string commandText = "Call p_GetAllUsers('" + strUserName + "',NULL)";
                cmd.CommandText = commandText;
                returnList = ReadAllAsync(cmd.ExecuteReader());
            }
            catch (Exception Ex)
            {
                returnList = null;
            }
            return returnList;
        }
        public List<User> GetUsersListByUserNameEdit(string strUserName, int userId)
        {
            List<User> returnList = new List<User>();
            try
            {
                using var cmd = Db.Connection.CreateCommand();
                string commandText = "call p_GetAllUsersByUserNameEdit('" + strUserName + "'," + userId+")";
                cmd.CommandText = commandText;
                returnList = ReadAllAsync(cmd.ExecuteReader());
            }
            catch (Exception Ex)
            {
                returnList = null;
            }
            return returnList;
        }

        public static string SafeGetString(System.Data.Common.DbDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetString(colIndex);
            return string.Empty;
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
                            Id = reader.GetInt32(0),
                            FullName = SafeGetString(reader, 1),
                            UserName = SafeGetString(reader, 2),
                            Email = SafeGetString(reader, 3),
                            Password = DecryptPassword(SafeGetString(reader, 4)),
                            Phone = SafeGetString(reader, 5),
                            Address = SafeGetString(reader, 6),
                            Status = SafeGetString(reader, 7),
                            CreatedDate = Convert.ToDateTime(reader.GetString(8)),
                            Role = SafeGetString(reader, 9),
                            RoleId = reader.GetInt32(10),
                            StatusId = reader.GetInt32(11),
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

        public bool IsUserNameExists()
        {
            bool bIsUserRegistered = false;
            try
            {
                List<User> result = GetUsersListByUserName(UserName);
                if (result != null && result.Count > 0)
                {
                    bIsUserRegistered = true;

                }
                else
                {
                    bIsUserRegistered = false;
                }
            }
            catch (Exception Ex)
            {
                bIsUserRegistered = false;
            }
            return bIsUserRegistered;
        }
        public bool IsUserNameExistsEdit()
        {
            bool bIsUserRegistered = false;
            try
            {
                List<User> result = GetUsersListByUserNameEdit(UserName, Id);
                if (result != null && result.Count > 0)
                {
                    bIsUserRegistered = true;

                }
                else
                {
                    bIsUserRegistered = false;
                }
            }
            catch (Exception Ex)
            {
                bIsUserRegistered = false;
            }
            return bIsUserRegistered;
        }
        public bool IsUserRegistered()
        {
            bool bIsUserRegistered = false;
            try
            {
                List<User> result = GetUsersList(UserName, Password);
                if (result != null && result.Count > 0)
                {
                    bIsUserRegistered = true;

                }
                else
                {
                    bIsUserRegistered = false;
                }
            }
            catch (Exception Ex)
            {
                bIsUserRegistered = false;
            }
            return bIsUserRegistered;
        }

        public bool CreateUser(User userData)
        {
            bool bIsRecordAdded = false;
            try
            {
                string strEncryptedPassword = EncryptPassword(userData.Password);
                //string Query = "insert into edc.bousers(FullName,UserName,Password,Email,Phone,Address,Role,Status,CreatedDate) values('" + userData.FullName + "','" + userData.UserName + "','" + strEncryptedPassword + "','" + userData.Email + "','" + userData.Phone + "','" + userData.Address + "','" + userData.Role + "','" + userData.Status 
                //    + "','"+ System.DateTime.Now.Date.ToString("yyyy-MM-dd") + "')";
                string Query = "call p_insertUsers('" + userData.FullName + "','" + userData.UserName + "','" + userData.Email + "','" + strEncryptedPassword + "','" + userData.Phone + "','" + userData.Address +
                     "'," + userData.StatusId + ",'" + System.DateTime.Now.Date.ToString("yyyy-MM-dd") + "'," + userData.RoleId + ")";
                var cmd = Db.Connection.CreateCommand();
                cmd.CommandText = Query;
                cmd.ExecuteNonQuery();
                bIsRecordAdded = true;
            }
            catch (Exception ex)
            {
                bIsRecordAdded = false;
            }
            return bIsRecordAdded;
        }

        public bool UpdateUser(User userData)
        {
            bool bIsRecordAdded = false;
            try
            {
                string strEncryptedPassword = EncryptPassword(userData.Password);
                //string Query = "call p_UpdateUsers set FullName='" + userData.FullName + "',UserName='" + userData.UserName + "',Password='" + strEncryptedPassword + "',Email='" + userData.Email + "'," +
                //    "Phone='" + userData.Phone + "',Role='" + userData.Role + "',Status='" + userData.Status + "',Address='" + userData.Address + "' where id=" + userData.Id + ";";
                string Query = "call p_UpdateUsers('" + userData.FullName + "','" + userData.UserName + "','" + userData.Email + "','" + strEncryptedPassword + "','" + userData.Phone + "','" + userData.Address +
                     "'," + userData.StatusId + ",'" + System.DateTime.Now.Date.ToString("yyyy-MM-dd") + "'," + userData.RoleId + ","+ userData.Id+ ")";
                var cmd = Db.Connection.CreateCommand();
                cmd.CommandText = Query;
                cmd.ExecuteNonQuery();
                bIsRecordAdded = true;
            }
            catch (Exception ex)
            {
                bIsRecordAdded = false;
            }
            return bIsRecordAdded;
        }

        public string EncryptPassword(string strPassword)
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
                    strReturnPassword = SecurePassword.EncryptPassword(strPassword, SecurePassword.EncDecType.BASE64);
                }
            }
            catch (Exception Ex)
            {

            }
            return strReturnPassword;
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


        public bool UpdateProfile(User userData)
        {
            bool bIsRecordAdded = false;
            try
            {
                string strEncryptedPassword = EncryptPassword(userData.Password);
                string Query = "call p_UpdateUsers('" + userData.FullName + "','" + userData.UserName + "','" + userData.Email + "','" + strEncryptedPassword + "','" + userData.Phone + "','" + userData.Address +
                     "'," + userData.StatusId + ",'" + System.DateTime.Now.Date.ToString("yyyy-MM-dd") + "'," + userData.RoleId + "," + userData.Id + ")";
                var cmd = Db.Connection.CreateCommand();
                cmd.CommandText = Query;
                cmd.ExecuteNonQuery();
                bIsRecordAdded = true;
            }
            catch (Exception ex)
            {
                bIsRecordAdded = false;
            }
            return bIsRecordAdded;
        }


    }
}
