using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using BoService;
using MySqlConnector;

namespace BoService.Models.Administration
{
    public class UserRoleQuery
    {
        public BoAppDB Db { get; }

        public UserRoleQuery(BoAppDB db)
        {
            Db = db;
        }

        public async Task<List<UserRole>> GetAll()
        {
            List<UserRole> list = new List<UserRole>();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "Call p_UserRole_GetAll()";
            using (var reader = cmd.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new UserRole()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        UpdateDate = reader.GetDateTime(2),
                        UpdatedByUserId = reader.GetInt32(3),
                        UpdatedBy = reader.GetString(4)
                    });
                }
            }
            return list;
        }

        public async Task<UserRole> GetByID(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "call p_UserRole_GetById(" + Convert.ToInt32(id) + ")";
            //p_GetContactTypeById
            List<UserRole> list = new List<UserRole>();
            using (var reader = cmd.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new UserRole()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        UpdateDate = reader.GetDateTime(2),
                        UpdatedByUserId = reader.GetInt32(3),
                        UpdatedBy = reader.GetString(4)
                    });
                }
            }

            return list.Count > 0 ? list[0] : null;
        }

        public async Task<UserRole> GetByName(string name, int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "call p_UserRole_GetByName('" + name + "'," + Convert.ToInt32(id) + ")";
            List<UserRole> list = new List<UserRole>();
            using (var reader = cmd.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new UserRole()
                    {
                        Name = reader.GetString(0)
                    });
                }
            }

            return list.Count > 0 ? list[0] : null;
        }


        public async Task<int> CreateRecord(UserRole inputData)
        {
            DateTime theDate = DateTime.Now;
            var sysDate = theDate.ToString("yyyy-MM-dd H:mm:ss");
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `user_roles` (`RL_NAME`, `RL_DATE`,`RL_UPDATED_BY_USER_ID`) VALUES (@name, @dt, @uid);";
            cmd.Parameters.AddWithValue("@name", inputData.Name);
            cmd.Parameters.AddWithValue("@dt", sysDate);
            cmd.Parameters.AddWithValue("@uid", inputData.UpdatedByUserId);
            await cmd.ExecuteNonQueryAsync();
            return (int)cmd.LastInsertedId;
        }

        public async Task<int> UpdateRecord(int id, UserRole inputData)
        {
            DateTime theDate = DateTime.Now;
            var sysDate = theDate.ToString("yyyy-MM-dd H:mm:ss");
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `user_roles` SET `RL_NAME` = @name,`RL_DATE` = @dt, `RL_UPDATED_BY_USER_ID` = @uid WHERE `RL_SEQNO` = @id;";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@name", inputData.Name);
            cmd.Parameters.AddWithValue("@uid", inputData.UpdatedByUserId);
            cmd.Parameters.AddWithValue("@dt", sysDate);
            var recs = await cmd.ExecuteNonQueryAsync();
            return recs;
        }
    }
}

