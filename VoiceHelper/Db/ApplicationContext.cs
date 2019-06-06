using Microsoft.EntityFrameworkCore;

namespace VoiceHelper.Db
{
    public sealed class ApplicationContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
         
        public ApplicationContext() : base()
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
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(@"Server=localhost;Port=5432;Database=voice-helper;Uid=postgres;Pwd=123456;");
        }
    }
}