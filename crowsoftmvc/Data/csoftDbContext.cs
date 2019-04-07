using crowsoftmvc.Models;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace crowsoftmvc.Data
{
    public class CsoftDbContext : DbContext
    {
        public string ConnectionString { get; set; }

        public CsoftDbContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public DbSet<crowsoftmvc.Models.UserAccount> UserAccount { get; set; }

    }
}
