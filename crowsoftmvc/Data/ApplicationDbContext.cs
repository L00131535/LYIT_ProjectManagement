using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using crowsoftmvc.Models;

namespace crowsoftmvc.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public string ConnectionString { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public DbSet<crowsoftmvc.Models.UserAccount> UserAccount { get; set; }
    }
}
