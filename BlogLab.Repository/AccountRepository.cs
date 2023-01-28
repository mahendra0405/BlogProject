using BlogLab.Models.Account;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using Dapper;

namespace BlogLab.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IConfiguration _config;
        public AccountRepository(IConfiguration config) 
        {
            _config = config;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUserIdentity user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var dataTble = new DataTable();
            dataTble.Columns.Add("UserName",typeof(string));
            dataTble.Columns.Add("NormalziedUserName", typeof(string));
            dataTble.Columns.Add("Email", typeof(string));
            dataTble.Columns.Add("NormalizedEmail", typeof(string));
            dataTble.Columns.Add("FullName", typeof(string));
            dataTble.Columns.Add("PasswordHash", typeof(string));

            dataTble.Rows.Add(user.UserName, user.NormalizedUserName, user.Email, user.NormalizedEmail, user.FullName, user.PasswordHash);
            using(var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync(cancellationToken);

                await connection.ExecuteAsync("Account_Insert",
                    new { Account = dataTble.AsTableValuedParameter("dbo.AccountType") }, commandType: CommandType.StoredProcedure);
            }

            return IdentityResult.Success;

        }

        public async Task<ApplicationUserIdentity> GetUsernameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ApplicationUserIdentity applicationUser;
            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync(cancellationToken);
                applicationUser = await connection.QuerySingleOrDefaultAsync<ApplicationUserIdentity>("Account_GetUserByName",
                    new { normalizedUserName = normalizedUserName }, commandType: CommandType.StoredProcedure);

            }
            return applicationUser;
        }
    }
}
