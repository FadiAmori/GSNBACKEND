using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace Am.Infrastructure
{
    public class AmContextFactory : IDesignTimeDbContextFactory<AmContext>
    {
        public AmContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AmContext>();

            var conn = Environment.GetEnvironmentVariable("AM_CONNECTION_STRING")
                       ?? "Host=localhost;Port=5432;Database=Budget;Username=postgres;Password=12345678";

            builder.UseNpgsql(conn);

            return new AmContext(builder.Options);
        }
    }
}