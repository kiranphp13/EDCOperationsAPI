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
    public class ContactCategoryQuery
    {
        public BoAppDB Db { get; }

        public ContactCategoryQuery(BoAppDB db)
        {
            Db = db;
        }

        public async Task<List<ContactCategory>> GetAll()
        {
            List<ContactCategory> list = new List<ContactCategory>();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "Call p_ContactCategory_GetAll()";
            using (var reader = cmd.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new ContactCategory()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = !reader.IsDBNull(2) ? reader.GetString(2) : "",
                        UpdateDate = reader.GetDateTime(3),
                        UpdatedBy = reader.GetString(5)
                    });
                }
            }
            return list;
        }

        public async Task<ContactCategory> GetByID(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "call p_ContactCategory_GetById(" + Convert.ToInt32(id) + ")";
            //p_GetContactTypeById
            List<ContactCategory> list = new List<ContactCategory>();
            using (var reader = cmd.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new ContactCategory()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = !reader.IsDBNull(2) ? reader.GetString(2) : "",
                        UpdateDate = reader.GetDateTime(3),
                        UpdatedBy = reader.GetString(5)
                    });
                }
            }

            return list.Count > 0 ? list[0] : null;
        }

        public async Task<ContactCategory> GetByName(string name, int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "call p_ContactCategory_GetByName('" + name + "'," + Convert.ToInt32(id) + ")";
            List<ContactCategory> list = new List<ContactCategory>();
            using (var reader = cmd.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new ContactCategory()
                    {
                        Name = reader.GetString(0)
                    });
                }
            }

            return list.Count > 0 ? list[0] : null;
        }


        public async Task<int> CreateRecord(ContactCategory inputData)
        {
            DateTime theDate = DateTime.Now;
            var sysDate = theDate.ToString("yyyy-MM-dd H:mm:ss");
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `contact_categories` (`CCAT_NAME`, `CCAT_DESC`,`CCAT_DATE`,`CCAT_UPDATED_BY`) VALUES (@sdes, @ldes, @dt, @uid);";
            cmd.Parameters.AddWithValue("@sdes", inputData.Name);
            cmd.Parameters.AddWithValue("@ldes", inputData.Description);
            cmd.Parameters.AddWithValue("@dt", sysDate);
            cmd.Parameters.AddWithValue("@uid", inputData.UpdatedByUserId);
            await cmd.ExecuteNonQueryAsync();
            return (int)cmd.LastInsertedId;
        }

        public async Task<int> UpdateRecord(int id, ContactCategory inputData)
        {
            DateTime theDate = DateTime.Now;
            var sysDate = theDate.ToString("yyyy-MM-dd H:mm:ss");
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `contact_categories` SET `CCAT_NAME` = @name, `CCAT_DESC` = @description, `CCAT_DATE` = @dt, `CCAT_UPDATED_BY` = @uid WHERE `CCAT_SEQNO` = @id;";
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

