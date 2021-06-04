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
    public class ContactTypeQuery
    {
        public BoAppDB Db { get; }

        public ContactTypeQuery(BoAppDB db)
        {
            Db = db;
        }

        public async Task<ContactType> GetContactTypeByID(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM `contact_types` WHERE `CTYP_SEQNO` = @id";
            cmd.Parameters.AddWithValue("@id", id);
            List<ContactType> list = new List<ContactType>();
            using (var reader = cmd.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new ContactType()
                    {
                        Id = reader.GetInt32(0),
                        Type = reader.GetString(1),
                        Description = reader.GetString(2),
                    });
                }
            }

            return list.Count > 0 ? list[0] : null;
        }


        public async Task<List<ContactType>> GetAllContactTypes()
        {
            List<ContactType> list = new List<ContactType>();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "Call p_GetAllContactTypes()";
            using (var reader = cmd.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new ContactType()
                    {
                        Id = reader.GetInt32(0),
                        Type = reader.GetString(1),
                        Description = reader.GetString(2),
                        UpdatedBy = reader.GetString(7),
                        UpdatedAt = reader.GetDateTime(5),
                    });
                }
            }
            return list;
        }


        public async Task<int> CreateRecord(ContactType inputData)
        {
            DateTime theDate = DateTime.Now;
            var sysDate = theDate.ToString("yyyy-MM-dd H:mm:ss");
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `contact_types` (`CTYP_TYPE`, `CTYP_LDES`, `CTYP_CREATED_BY`,`CTYP_UPDATED_BY`,`CTYP_CREATED_AT`,`CTYP_UPDATED_AT`) VALUES (@type, @description, @cuser, @uuser, @cdt,  @udt);";
            cmd.Parameters.AddWithValue("@type", inputData.Type);
            cmd.Parameters.AddWithValue("@description", inputData.Description);
            cmd.Parameters.AddWithValue("@cuser", '2');
            cmd.Parameters.AddWithValue("@uuser", '2');
            cmd.Parameters.AddWithValue("@cdt", sysDate);
            cmd.Parameters.AddWithValue("@udt", sysDate);
            await cmd.ExecuteNonQueryAsync();
            return (int)cmd.LastInsertedId;
        }

        public async Task<int> UpdateRecord(int id, ContactType inputData)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `contact_types` SET `CTYP_TYPE` = @type, `CTYP_LDES` = @description, `CTYP_UPDATED_BY` = @uuser WHERE `CTYP_SEQNO` = @id;";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@type", inputData.Type);
            cmd.Parameters.AddWithValue("@description", inputData.Description);
            cmd.Parameters.AddWithValue("@uuser", '2');
            var recs =  await cmd.ExecuteNonQueryAsync();
            return recs;
        }
    }
}

