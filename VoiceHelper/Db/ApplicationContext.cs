using System;
using Microsoft.EntityFrameworkCore;

namespace VoiceHelper.Db
{
    public sealed class ApplicationContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
         
        public ApplicationContext(DbContextOptions opt) : base(opt)
        {
//            try
//            {
//                Database.EnsureCreated();
//                Database.Migrate();
//            }
//            catch
//            {
//                // ignored
//            }
        }
    }
}