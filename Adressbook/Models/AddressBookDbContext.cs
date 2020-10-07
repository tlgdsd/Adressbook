using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Adressbook.Models
{

    public class AddressBookDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Address> Adresses { get; set; }

        public AddressBookDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static AddressBookDbContext Create()
        {
            return new AddressBookDbContext();
        }
    }
}