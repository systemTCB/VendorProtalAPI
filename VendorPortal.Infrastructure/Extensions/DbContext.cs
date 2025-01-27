using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace VendorPortal.Infrastructure.Extensions
{
    public class DbContext
    {
        private readonly IConfiguration _configuration;

        public DbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnectionRead() => new SqlConnection(_configuration.GetConnectionString(""));
        public IDbConnection CreateConnectionWrite() => new SqlConnection(_configuration.GetConnectionString(""));
    }
}