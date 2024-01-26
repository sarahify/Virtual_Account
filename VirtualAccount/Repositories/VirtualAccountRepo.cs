using Dapper;
using System.Data;
using System.Reflection.Metadata;
using VirtualAccount.Context;
using VirtualAccount.Contract;
using VirtualAccount.Entity;
using VirtualAccount.Services.Implementation;

namespace VirtualAccount.Repositories
{
    public class VirtualAccountRepo : IVirtualAccountRepo
    {
        private readonly DapperContext _context;
        private readonly ILogger<VirtualAccountRepo> _logger;
        public VirtualAccountRepo(DapperContext context, ILogger<VirtualAccountRepo> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<VirtualAcct> CreateVirtualAccount(VirtualAcct virtualAccount)
        {

            string query = "INSERT INTO m_new_virtual_account_view(client_id, fullname, firstname, lastname, email, phone, middlename) VALUES (@client_id, @fullname, @firstname, @lastname, @email_address, @mobile_no, @middlename)";

            var paramter = new DynamicParameters();
            paramter.Add("@client_id", virtualAccount.client_id.ToString(), DbType.Int32);
            paramter.Add("@fullname", virtualAccount.fullname, DbType.String);
            paramter.Add("@firstname", virtualAccount.firstname, DbType.String);
            paramter.Add("@lastname", virtualAccount.lastname, DbType.String);
            paramter.Add("@email", virtualAccount.email_address, DbType.String);
            paramter.Add("@phone", virtualAccount.mobile_no, DbType.String);
            paramter.Add("@middlename", virtualAccount.middlename, DbType.String);



            using (var connection = _context.CreateConnection())
            {
                var clientId = await connection.ExecuteScalarAsync<int>(query, paramter);

                var createdVirtualAccount = new VirtualAcct
                {
                    client_id = virtualAccount.client_id,
                    fullname = virtualAccount.fullname,
                    firstname = virtualAccount.firstname,
                    lastname = virtualAccount.lastname,
                    email_address = virtualAccount.email_address,
                    mobile_no = virtualAccount.mobile_no,
                    middlename = virtualAccount.middlename,
                };

                return createdVirtualAccount;
            }


        }

        public async Task<VirtualAcct?> GetVirtualAccount(int client_id)
        {
            string query = "select client_id,fullname, firstname, lastname, email_address, mobile_no, middlename from m_new_virtual_account_view where client_id=@client_id";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<VirtualAcct>(query, new { client_id });
            }
        }


    }
}
