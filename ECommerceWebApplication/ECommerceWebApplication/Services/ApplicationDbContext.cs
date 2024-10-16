using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ECommerceWebApplication.Services
{
    public class ApplicationDbContext :DbContext
    {

        public ApplicationDbContext(DbContextOptions options): base (options)
        {
            


        }
    }
}
