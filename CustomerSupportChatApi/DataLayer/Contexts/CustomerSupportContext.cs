using Microsoft.EntityFrameworkCore;

namespace CustomerSupportChatApi.DataLayer.Contexts
{
    public class CustomerSupportContext : DbContext
    {
        public CustomerSupportContext(DbContextOptions<CustomerSupportContext> options)
            : base(options)
        {
        }


    }
}
