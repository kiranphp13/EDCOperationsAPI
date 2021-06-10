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
    public class CollateralQuery
    {
        public BoAppDB Db { get; }

        public CollateralQuery(BoAppDB db)
        {
            Db = db;
        }

        public async Task<List<Collateral>> GetAll()
        {
            List<Collateral> list = new List<Collateral>();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "Call p_Collateral_GetAll()";
            using (var reader = cmd.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new Collateral()
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

