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
    public class UserStatusQuery
    {
        public BoAppDB Db { get; }

        public UserStatusQuery(BoAppDB db)
        {
            Db = db;
        }

        public async Task<List<UserStatus>> GetAll()
        {
            List<UserStatus> list = new List<UserStatus>();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "Call p_UserStatus_GetAll()";
            using (var reader = cmd.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new UserStatus()
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

        public async Task<UserStatus> GetByID(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "call p_UserStatus_GetById(" + Convert.ToInt32(id) + ")";
            //p_GetContactTypeById
            List<UserStatus> list = new List<UserStatus>();
            using (var reader = cmd.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new UserStatus()
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

        public async Task<UserStatus> GetByName(string name, int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "call p_UserStatus_GetByName('" + name + "'," + Convert.ToInt32(id) + ")";
            List<UserStatus> list = new List<UserStatus>();
            using (var reader = cmd.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new UserStatus()
                    {
                        Name = reader.GetString(0)
                    });
                }
            }

            return list.Count > 0 ? list[0] : null;
        }


        public async Task<int> CreateRecord(UserStatus inputData)
        {
            DateTime theDate = DateTime.Now;
            var sysDate = theDate.ToString("yyyy-MM-dd H:mm:ss");
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `user_status` (`ST_STATUS`, `ST_DATE`,`ST_UPDATED_BY_USER_ID`) VALUES (@name, @dt, @uid);";
            cmd.Parameters.AddWithValue("@name", inputData.Name);
            cmd.Parameters.AddWithValue("@dt", sysDate);
            cmd.Parameters.AddWithValue("@uid", inputData.UpdatedByUserId);
            await cmd.ExecuteNonQueryAsync();
            return (int)cmd.LastInsertedId;
        }

        public async Task<int> UpdateRecord(int id, UserStatus inputData)
        {
            DateTime theDate = DateTime.Now;
            var sysDate = theDate.ToString("yyyy-MM-dd H:mm:ss");
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `user_status` SET `ST_STATUS` = @name,`ST_DATE` = @dt, `ST_UPDATED_BY_USER_ID` = @uid WHERE `ST_SEQNO` = @id;";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@name", inputData.Name);
            cmd.Parameters.AddWithValue("@uid", inputData.UpdatedByUserId);
            cmd.Parameters.AddWithValue("@dt", sysDate);
            var recs = await cmd.ExecuteNonQueryAsync();
            return recs;
        }
    }
}

