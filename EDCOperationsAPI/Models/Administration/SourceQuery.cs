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
    public class SourceQuery
    {
        public BoAppDB Db { get; }

        public SourceQuery(BoAppDB db)
        {
            Db = db;
        }

        public async Task<List<Source>> GetAll()
        {
            List<Source> list = new List<Source>();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "Call p_Source_GetAll()";
            using (var reader = cmd.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new Source()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = !reader.IsDBNull(2) ? reader.GetString(2) : "",
                        UpdateDate = reader.GetDateTime(3),
                        UpdatedByUserId = reader.GetInt32(4),
                        UpdatedBy = reader.GetString(5)
                    });
                }
            }
            return list;
        }

        public async Task<Source> GetByID(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "call p_Source_GetById(" + Convert.ToInt32(id) + ")";
            //p_GetContactTypeById
            List<Source> list = new List<Source>();
            using (var reader = cmd.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new Source()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = !reader.IsDBNull(2) ? reader.GetString(2) : "",
                        UpdateDate = reader.GetDateTime(3),
                        UpdatedByUserId = reader.GetInt32(4),
                        UpdatedBy = reader.GetString(5)
                    });
                }
            }

            return list.Count > 0 ? list[0] : null;
        }

        public async Task<Source> GetByName(string name, int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "call p_Association_GetByName('" + name + "'," + Convert.ToInt32(id) + ")";
            List<Source> list = new List<Source>();
            using (var reader = cmd.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new Source()
                    {
                        Name = reader.GetString(0)
                    });
                }
            }

            return list.Count > 0 ? list[0] : null;
        }


        public async Task<int> CreateRecord(Source inputData)
        {
            DateTime theDate = DateTime.Now;
            var sysDate = theDate.ToString("yyyy-MM-dd H:mm:ss");
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `sources` (`SRCE_SDES`, `SRCE_LDES`,`SRCE_DATE`,`SRCE_UPDATED_BY_USER_ID`) VALUES (@sdes, @ldes, @dt, @uid);";
            cmd.Parameters.AddWithValue("@sdes", inputData.Name);
            cmd.Parameters.AddWithValue("@ldes", inputData.Description);
            cmd.Parameters.AddWithValue("@dt", sysDate);
            cmd.Parameters.AddWithValue("@uid", inputData.UpdatedByUserId);
            await cmd.ExecuteNonQueryAsync();
            return (int)cmd.LastInsertedId;
        }

        public async Task<int> UpdateRecord(int id, Source inputData)
        {
            DateTime theDate = DateTime.Now;
            var sysDate = theDate.ToString("yyyy-MM-dd H:mm:ss");
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `sources` SET `SRCE_SDES` = @name, `SRCE_LDES` = @description, `SRCE_DATE` = @dt, `SRCE_UPDATED_BY_USER_ID` = @uid WHERE `SRCE_SEQNO` = @id;";
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

