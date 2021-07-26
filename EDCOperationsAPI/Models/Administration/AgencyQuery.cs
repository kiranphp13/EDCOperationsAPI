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
    public class AgencyQuery
    {
        public BoAppDB Db { get; }

        public AgencyQuery(BoAppDB db)
        {
            Db = db;
        }

        public async Task<List<Agency>> GetAll()
        {
            List<Agency> list = new List<Agency>();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "Call p_Agency_GetAll()";
            using (var reader = cmd.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new Agency()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(2),
                        Description = !reader.IsDBNull(3) ? reader.GetString(3) : "",
                        UpdateDate = reader.GetDateTime(1),
                        UpdatedByUserId = reader.GetInt32(4),
                        UpdatedBy = reader.GetString(5)
                    });
                }
            }
            return list;
        }

        public async Task<Agency> GetByID(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "call p_Agency_GetById(" + Convert.ToInt32(id) + ")";
            //p_GetContactTypeById
            List<Agency> list = new List<Agency>();
            using (var reader = cmd.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new Agency()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(2),
                        Description = !reader.IsDBNull(3) ? reader.GetString(3) : "",
                        UpdateDate = reader.GetDateTime(1),
                        UpdatedByUserId = reader.GetInt32(4),
                        UpdatedBy = reader.GetString(5)
                    });
                }
            }

            return list.Count > 0 ? list[0] : null;
        }

        public async Task<Agency> GetByName(string name, int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "call p_Agency_GetByName('" + name + "'," + Convert.ToInt32(id) + ")";
            List<Agency> list = new List<Agency>();
            using (var reader = cmd.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new Agency()
                    {
                        Name = reader.GetString(0)
                    });
                }
            }

            return list.Count > 0 ? list[0] : null;
        }


        public async Task<int> CreateRecord(Agency inputData)
        {
            DateTime theDate = DateTime.Now;
            var sysDate = theDate.ToString("yyyy-MM-dd H:mm:ss");
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `agencies` (`AGCY_NAME`, `AGCY_LIC_USERNAME`,`AGCY_DATE`,`AGCY_UPDATED_BY`) VALUES (@sdes, @ldes, @dt, @uid);";
            cmd.Parameters.AddWithValue("@sdes", inputData.Name);
            cmd.Parameters.AddWithValue("@ldes", inputData.Description);
            cmd.Parameters.AddWithValue("@dt", sysDate);
            cmd.Parameters.AddWithValue("@uid", inputData.UpdatedByUserId);
            await cmd.ExecuteNonQueryAsync();
            return (int)cmd.LastInsertedId;
        }

        public async Task<int> UpdateRecord(int id, Agency inputData)
        {
            DateTime theDate = DateTime.Now;
            var sysDate = theDate.ToString("yyyy-MM-dd H:mm:ss");
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `agencies` SET `AGCY_NAME` = @name, `AGCY_LIC_USERNAME` = @description, `AGCY_DATE` = @dt, `AGCY_UPDATED_BY` = @uid WHERE `AGCY_SEQNO` = @id;";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@name", inputData.Name);
            cmd.Parameters.AddWithValue("@description", inputData.Description);
            cmd.Parameters.AddWithValue("@uid", inputData.UpdatedByUserId);
            cmd.Parameters.AddWithValue("@dt", sysDate);
            var recs = await cmd.ExecuteNonQueryAsync();
            return recs;
        }
    }
}

