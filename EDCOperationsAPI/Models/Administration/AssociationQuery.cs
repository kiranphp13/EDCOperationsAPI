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
    }
}

