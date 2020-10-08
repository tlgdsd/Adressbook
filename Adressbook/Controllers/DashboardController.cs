using Adressbook.Models;
using Adressbook.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Adressbook.Controllers
{
    public class DashboardController : Controller
    {
        private AddressBookDbContext _context;
        public DashboardController()
        {
            _context = new AddressBookDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        [Authorize]
        public ActionResult Index()
        {
            string userid = User.Identity.GetUserId();
            List<Contact> contactList = (from i in _context.Contacts where i.User.Id == userid select i).ToList();
            foreach (Contact contact in contactList)
            {
                contact.Addresses = (from i in _context.Adresses where i.Contact.Id == contact.Id select i).ToList();
            }
            return View(contactList);
        }
        [Authorize]
        public ActionResult NewContactForm ()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult Add (ContactViewModel model)
        {
            Contact newcontact = new Contact
            {
                Name = model.Name,
                Surname = model.Surname,
                IsDeleted = false,
            };
            newcontact.User = _context.Users.Find(User.Identity.GetUserId());
            Address newaddress = new Address
            {
                AddressText = model.AddressText,
                City = model.AddressCity,
                ZipCode = model.AddressZipCode,
                Phone = model.AddressPhone,
                Contact = newcontact,
                IsDeleted = false,
            };
            _context.Contacts.Add(newcontact);
            _context.Adresses.Add(newaddress);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}