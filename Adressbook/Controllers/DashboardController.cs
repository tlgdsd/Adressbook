using Adressbook.Models;
using Adressbook.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace Adressbook.Controllers
{
    [Authorize]
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
        //CONTACTS
        private bool ContactBelongsToUser(int id)
        {
            string userid = User.Identity.GetUserId();
            bool check = _context.Contacts.Where(m => m.User.Id == userid && m.Id == id).Count() > 0;
            return (check);
        }
        public ActionResult NewContactForm ()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddContact (ContactViewModel viewModel)
        {
            Contact newcontact = new Contact
            {
                Name = viewModel.Name,
                Surname = viewModel.Surname,
                IsDeleted = false,
            };
            newcontact.User = _context.Users.Find(User.Identity.GetUserId());
            Address newaddress = new Address
            {
                AddressText = viewModel.AddressText,
                City = viewModel.AddressCity,
                ZipCode = viewModel.AddressZipCode,
                Phone = viewModel.AddressPhone,
                Contact = newcontact,
                IsDeleted = false,
            };
            _context.Contacts.Add(newcontact);
            _context.Adresses.Add(newaddress);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult ContactDetails (int id)
        {
            if (ContactBelongsToUser(id))
            {
                Contact contact = _context.Contacts.Find(id);
                contact.Addresses = (from i in _context.Adresses where i.Contact.Id == contact.Id && !i.IsDeleted select i).ToList();

                return View(contact);
            }
            return RedirectToAction("Index");
        }

        //ADDRESSES

        public ActionResult NewAddressForm(int id)
        {
            if (ContactBelongsToUser(id))
            {
                AddressViewModel addressViewModel = new AddressViewModel
                {
                    ContactId = id
                };
                return View(addressViewModel);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddAddress (AddressViewModel viewModel)
        {
            Address address = new Address
            {
                AddressText = viewModel.AddressText,
                City = viewModel.City,
                ZipCode = viewModel.ZipCode,
                Phone = viewModel.Phone,
                IsDeleted = false,
            };
            address.Contact = _context.Contacts.Find(viewModel.ContactId);
            _context.Adresses.Add(address);
            _context.SaveChanges();

            return RedirectToAction("ContactDetails", new { id = viewModel.ContactId});
        }


    }
}