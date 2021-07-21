using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using BoService;
using MySqlConnector;

namespace BoService.Models.Contacts
{
    public class ProspectQuery
    {
        public BoAppDB Db { get; }

        public ProspectQuery(BoAppDB db)
        {
            Db = db;
        }

        public async Task<List<AgencyList>> GetAgencyList()
        {
            List<AgencyList> list = new List<AgencyList>();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "Call p_Agency_GetAll()";
            using (var reader = cmd.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new AgencyList()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(2)
                    });
                }
            }
            return list;
        }

        public async Task<List<StateList>> GetStateList()
        {
            List<StateList> list = new List<StateList>();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "Call p_State_GetAll()";
            using (var reader = cmd.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new StateList()
                    {
                        Id = reader.GetInt32(0),
                        ShortName = reader.GetString(1),
                        Name = reader.GetString(2)
                    });
                }
            }
            return list;
        }

        public async Task<List<ProspectList>> GetAll()
        {
            List<ProspectList> list = new List<ProspectList>();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "Call p_Prospect_GetAll()";
            using (var reader = cmd.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new ProspectList()
                    {
                        Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CTCT_SEQNO"))),
                        UpdateDate = reader.GetDateTime(1),
                        //UpdatedBy = !reader.IsDBNull(reader.GetOrdinal("USR_UNAME")) ? reader.GetValue(reader.GetOrdinal("USR_UNAME")).ToString() : "",
                        //AgencyId = !reader.IsDBNull(reader.GetOrdinal("CTCT_AGCY_SEQNO")) ? Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CTCT_AGCY_SEQNO"))) : 0,
                        //UserNo = !reader.IsDBNull(reader.GetOrdinal("CTCT_USERNO")) ? Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CTCT_USERNO"))) : 0,
                        Salute = !reader.IsDBNull(reader.GetOrdinal("CTCT_SALUTE")) ? reader.GetValue(reader.GetOrdinal("CTCT_SALUTE")).ToString() : "",
                        First = !reader.IsDBNull(reader.GetOrdinal("CTCT_FIRST")) ? reader.GetValue(reader.GetOrdinal("CTCT_FIRST")).ToString() : "",
                        Last = !reader.IsDBNull(reader.GetOrdinal("CTCT_LAST")) ? reader.GetValue(reader.GetOrdinal("CTCT_LAST")).ToString() : "",
                        Middle = !reader.IsDBNull(reader.GetOrdinal("CTCT_MIDDLE")) ? reader.GetValue(reader.GetOrdinal("CTCT_MIDDLE")).ToString() : "",
                        //Title = !reader.IsDBNull(reader.GetOrdinal("CTCT_TITLE")) ? reader.GetValue(reader.GetOrdinal("CTCT_TITLE")).ToString() : "",
                        //Company = !reader.IsDBNull(reader.GetOrdinal("CTCT_COMPANY")) ? reader.GetValue(reader.GetOrdinal("CTCT_COMPANY")).ToString() : "",
                        //Address1 = !reader.IsDBNull(reader.GetOrdinal("CTCT_ADDR1")) ? reader.GetValue(reader.GetOrdinal("CTCT_ADDR1")).ToString() : "",
                        //Address2 = !reader.IsDBNull(reader.GetOrdinal("CTCT_ADDR2")) ? reader.GetValue(reader.GetOrdinal("CTCT_ADDR2")).ToString() : "",
                        //City = !reader.IsDBNull(reader.GetOrdinal("CTCT_CITY")) ? reader.GetValue(reader.GetOrdinal("CTCT_CITY")).ToString() : "",
                        //StateId = !reader.IsDBNull(reader.GetOrdinal("CTCT_STATE_SEQNO")) ? Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CTCT_STATE_SEQNO"))) : 0,
                        //Country = !reader.IsDBNull(reader.GetOrdinal("CTCT_COUNTRY")) ? reader.GetValue(reader.GetOrdinal("CTCT_COUNTRY")).ToString() : "",
                        //Zip = !reader.IsDBNull(reader.GetOrdinal("CTCT_ZIP")) ? reader.GetValue(reader.GetOrdinal("CTCT_ZIP")).ToString() : "",
                        Phone1 = !reader.IsDBNull(reader.GetOrdinal("CTCT_PHONE1")) ? reader.GetValue(reader.GetOrdinal("CTCT_PHONE1")).ToString() : "",
                        //Phone2 = !reader.IsDBNull(reader.GetOrdinal("CTCT_PHONE2")) ? reader.GetValue(reader.GetOrdinal("CTCT_PHONE2")).ToString() : "",
                        ActiveStatus = !reader.IsDBNull(reader.GetOrdinal("CTCT_ACTIVE_STATUS")) ? reader.GetValue(reader.GetOrdinal("CTCT_ACTIVE_STATUS")).ToString() : "",
                        Email = !reader.IsDBNull(reader.GetOrdinal("CTCT_EMAIL")) ? reader.GetValue(reader.GetOrdinal("CTCT_EMAIL")).ToString() : "",
                        //UpdatedByUserId = !reader.IsDBNull(reader.GetOrdinal("CTCT_UPDATED_BY_USER_ID")) ? Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CTCT_UPDATED_BY_USER_ID"))) : 0,
                        //AgencyName = !reader.IsDBNull(reader.GetOrdinal("AGCY_NAME")) ? reader.GetValue(reader.GetOrdinal("AGCY_NAME")).ToString() : "",

                    });
                }
            }
            return list;
        }

        public async Task<Prospect> GetByID(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "call p_Prospect_GetById(" + Convert.ToInt32(id) + ")";
            //p_GetContactTypeById
            List<Prospect> list = new List<Prospect>();
            using (var reader = cmd.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new Prospect()
                    {
                        Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CTCT_SEQNO"))),
                        UpdateDate = reader.GetDateTime(1),
                        UpdatedBy = !reader.IsDBNull(reader.GetOrdinal("USR_UNAME")) ? reader.GetValue(reader.GetOrdinal("USR_UNAME")).ToString() : "",
                        AgencyId = !reader.IsDBNull(reader.GetOrdinal("CTCT_AGCY_SEQNO")) ? Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CTCT_AGCY_SEQNO"))) : 0,
                        UserNo = !reader.IsDBNull(reader.GetOrdinal("CTCT_USERNO")) ? Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CTCT_USERNO"))) : 0,
                        Salute = !reader.IsDBNull(reader.GetOrdinal("CTCT_SALUTE")) ? reader.GetValue(reader.GetOrdinal("CTCT_SALUTE")).ToString() : "",
                        First = !reader.IsDBNull(reader.GetOrdinal("CTCT_FIRST")) ? reader.GetValue(reader.GetOrdinal("CTCT_FIRST")).ToString() : "",
                        Last = !reader.IsDBNull(reader.GetOrdinal("CTCT_LAST")) ? reader.GetValue(reader.GetOrdinal("CTCT_LAST")).ToString() : "",
                        Middle = !reader.IsDBNull(reader.GetOrdinal("CTCT_MIDDLE")) ? reader.GetValue(reader.GetOrdinal("CTCT_MIDDLE")).ToString() : "",
                        Title = !reader.IsDBNull(reader.GetOrdinal("CTCT_TITLE")) ? reader.GetValue(reader.GetOrdinal("CTCT_TITLE")).ToString() : "",
                        Company = !reader.IsDBNull(reader.GetOrdinal("CTCT_COMPANY")) ? reader.GetValue(reader.GetOrdinal("CTCT_COMPANY")).ToString() : "",
                        Address1 = !reader.IsDBNull(reader.GetOrdinal("CTCT_ADDR1")) ? reader.GetValue(reader.GetOrdinal("CTCT_ADDR1")).ToString() : "",
                        Address2 = !reader.IsDBNull(reader.GetOrdinal("CTCT_ADDR2")) ? reader.GetValue(reader.GetOrdinal("CTCT_ADDR2")).ToString() : "",
                        City = !reader.IsDBNull(reader.GetOrdinal("CTCT_CITY")) ? reader.GetValue(reader.GetOrdinal("CTCT_CITY")).ToString() : "",
                        State = !reader.IsDBNull(reader.GetOrdinal("CTCT_STATE")) ? reader.GetValue(reader.GetOrdinal("CTCT_STATE")).ToString() : "",
                        Country = !reader.IsDBNull(reader.GetOrdinal("CTCT_COUNTRY")) ? reader.GetValue(reader.GetOrdinal("CTCT_COUNTRY")).ToString() : "",
                        Zip = !reader.IsDBNull(reader.GetOrdinal("CTCT_ZIP")) ? reader.GetValue(reader.GetOrdinal("CTCT_ZIP")).ToString() : "",
                        Phone1 = !reader.IsDBNull(reader.GetOrdinal("CTCT_PHONE1")) ? reader.GetValue(reader.GetOrdinal("CTCT_PHONE1")).ToString() : "",
                        Phone2 = !reader.IsDBNull(reader.GetOrdinal("CTCT_PHONE2")) ? reader.GetValue(reader.GetOrdinal("CTCT_PHONE2")).ToString() : "",
                        ActiveStatus = !reader.IsDBNull(reader.GetOrdinal("CTCT_ACTIVE_STATUS")) ? reader.GetValue(reader.GetOrdinal("CTCT_ACTIVE_STATUS")).ToString() : "",
                        Email = !reader.IsDBNull(reader.GetOrdinal("CTCT_EMAIL")) ? reader.GetValue(reader.GetOrdinal("CTCT_EMAIL")).ToString() : "",
                        UpdatedByUserId = !reader.IsDBNull(reader.GetOrdinal("CTCT_UPDATED_BY_USER_ID")) ? Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CTCT_UPDATED_BY_USER_ID"))) : 0,
                        AgencyName = !reader.IsDBNull(reader.GetOrdinal("AGCY_NAME")) ? reader.GetValue(reader.GetOrdinal("AGCY_NAME")).ToString() : "",

                    });
                }
            }

            return list.Count > 0 ? list[0] : null;
        }

        public async Task<Prospect> GetByName(string name, int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "call p_Association_GetByName('" + name + "'," + Convert.ToInt32(id) + ")";
            List<Prospect> list = new List<Prospect>();
            using (var reader = cmd.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new Prospect()
                    {
                        //Name = reader.GetString(0)
                    });
                }
            }

            return list.Count > 0 ? list[0] : null;
        }


        public async Task<int> CreateRecord(Prospect inputData)
        {
            DateTime theDate = DateTime.Now;
            var sysDate = theDate.ToString("yyyy-MM-dd H:mm:ss");
            using var cmd = Db.Connection.CreateCommand();
            //cmd.CommandText = @"INSERT INTO `contacts` (`CTCT_DATE`,CTCT_UPDATED_BY_USER_ID) VALUES (@dt, @uid);";
            //cmd.Parameters.AddWithValue("@dt", sysDate);
            cmd.CommandText = @"INSERT INTO `contacts` (`CTCT_DATE`,CTCT_UPDATED_BY_USER_ID,`CTCT_SALUTE`,`CTCT_FIRST`,`CTCT_LAST`,`CTCT_TITLE`,`CTCT_COMPANY`,`CTCT_ADDR1`,`CTCT_ADDR2`,`CTCT_CITY`,`CTCT_STATE`,`CTCT_COUNTRY`,`CTCT_ZIP`,`CTCT_PHONE1`,`CTCT_PHONE2`,`CTCT_ACTIVE_STATUS`,`CTCT_EMAIL`,`CTCT_MIDDLE`,CTCT_AGCY_SEQNO,CTCT_CTYP_TYPE_ID)
VALUES (@dt, @uid, @salute, @fname, @lname, @title, @company, @addr1, @addr2, @city, @state, @country, @zip, @phone1, @phone2, @active_status, @email, @mname,@agency_id,@ctype_id);"; 
            cmd.Parameters.AddWithValue("@dt", sysDate);
            cmd.Parameters.AddWithValue("@uid", inputData.UpdatedByUserId);
            cmd.Parameters.AddWithValue("@salute", inputData.Salute);
            cmd.Parameters.AddWithValue("@fname", inputData.First);
            cmd.Parameters.AddWithValue("@lname", inputData.Last);
            cmd.Parameters.AddWithValue("@title", inputData.Title);
            cmd.Parameters.AddWithValue("@company", inputData.Company);
            cmd.Parameters.AddWithValue("@addr1", inputData.Address1);
            cmd.Parameters.AddWithValue("@addr2", inputData.Address2);
            cmd.Parameters.AddWithValue("@city", inputData.City);
            cmd.Parameters.AddWithValue("@state", inputData.State);
            cmd.Parameters.AddWithValue("@country", inputData.Country);
            cmd.Parameters.AddWithValue("@zip", inputData.Zip);
            cmd.Parameters.AddWithValue("@phone1", inputData.Phone1);
            cmd.Parameters.AddWithValue("@phone2", inputData.Phone2);
            cmd.Parameters.AddWithValue("@active_status", inputData.ActiveStatus);
            cmd.Parameters.AddWithValue("@email", inputData.Email);
            cmd.Parameters.AddWithValue("@mname", inputData.Middle);
            cmd.Parameters.AddWithValue("@agency_id", inputData.AgencyId);
            cmd.Parameters.AddWithValue("@ctype_id", inputData.ContactTypeId);

            await cmd.ExecuteNonQueryAsync();

            return (int)cmd.LastInsertedId;
        }

        public async Task<int> UpdateRecord(int id, Prospect inputData)
        {
            DateTime theDate = DateTime.Now;
            var sysDate = theDate.ToString("yyyy-MM-dd H:mm:ss");
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `associations` SET `ASSN_SDES` = @name, `ASSN_LDES` = @description, `ASSN_DATE` = @dt, `ASSN_UPDATED_BY_USER_ID` = @uid WHERE `ASSN_SEQNO` = @id;";
            cmd.Parameters.AddWithValue("@id", id);
            //cmd.Parameters.AddWithValue("@name", inputData.Name);
            //cmd.Parameters.AddWithValue("@description", inputData.Description);
            cmd.Parameters.AddWithValue("@uid", inputData.UpdatedByUserId);
            cmd.Parameters.AddWithValue("@dt", sysDate);
            var recs = await cmd.ExecuteNonQueryAsync();
            return recs;
        }
    }
}

