using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SQuiz.Identity.Models;

namespace SQuiz.Identity.Data
{
    public class IdentityContext : IdentityDbContext<SQuizUser>
    {
        public const string DefaultSchema = "idt";
        
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema(DefaultSchema);
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
