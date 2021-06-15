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
    public class AssociationQuery
    {
        public BoAppDB Db { get; }

        public AssociationQuery(BoAppDB db)
        {
            Db = db;
        }

        public async Task<List<Association>> GetAll()
        {
            List<Association> list = new List<Association>();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "Call p_Association_GetAll()";
            using (var reader = cmd.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new Association()
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

        public async Task<Association> GetByID(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "call p_Association_GetById(" + Convert.ToInt32(id) + ")";
            //p_GetContactTypeById
            List<Association> list = new List<Association>();
            using (var reader = cmd.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new Association()
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

        public async Task<Association> GetByName(string name, int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "call p_Association_GetByName('" + name + "'," + Convert.ToInt32(id) + ")";
            List<Association> list = new List<Association>();
            using (var reader = cmd.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new Association()
                    {
                        Name = reader.GetString(0)
                    });
                }
            }

            return list.Count > 0 ? list[0] : null;
        }


        public async Task<int> CreateRecord(Association inputData)
        {
            DateTime theDate = DateTime.Now;
            var sysDate = theDate.ToString("yyyy-MM-dd H:mm:ss");
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `associations` (`ASSN_SDES`, `ASSN_LDES`,`ASSN_DATE`,`ASSN_UPDATED_BY_USER_ID`) VALUES (@sdes, @ldes, @dt, @uid);";
            cmd.Parameters.AddWithValue("@sdes", inputData.Name);
            cmd.Parameters.AddWithValue("@ldes", inputData.Description);
            cmd.Parameters.AddWithValue("@dt", sysDate);
            cmd.Parameters.AddWithValue("@uid", inputData.UpdatedByUserId);
            await cmd.ExecuteNonQueryAsync();
            return (int)cmd.LastInsertedId;
        }

        public async Task<int> UpdateRecord(int id, Association inputData)
        {
            DateTime theDate = DateTime.Now;
            var sysDate = theDate.ToString("yyyy-MM-dd H:mm:ss");
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `associations` SET `ASSN_SDES` = @name, `ASSN_LDES` = @description, `ASSN_DATE` = @dt, `ASSN_UPDATED_BY_USER_ID` = @uid WHERE `ASSN_SEQNO` = @id;";
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

