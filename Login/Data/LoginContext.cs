using Login.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Login.Data
{
    public class LoginContext: DbContext
    {
        public LoginContext()
        {
        }

        public LoginContext(DbContextOptions<LoginContext> options): base(options)
        {
            
        }
        public DbSet<LoginModel> logins { get; set; }
        public DbSet<RegisterModel> Registers { get; set; }
        public DbSet<VotesCalculation> Voteeee { get; set; }
        public DbSet<VoteCount> VoteCounts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<VoteCount>(a => {
                a.HasNoKey();

            });

            builder.Entity<RegisterModel>(a => { 
                a.HasKey(x => x.Id);
               
            });

            builder.Entity<LoginModel>(a =>
            {  a.HasKey(x => x.Id);
            });

            builder.Entity<VotesCalculation>(a => {
                a.HasKey(x=>x.key);
            });
        }

    }
}
