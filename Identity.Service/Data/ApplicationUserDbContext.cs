using Identity.Service.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Identity.Service.Data
{
    public class ApplicationUserDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationUserDbContext(DbContextOptions<ApplicationUserDbContext> options) : base(options)
        {

        }
    }
}
