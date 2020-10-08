using Adressbook.Models;
using Adressbook.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity;
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
            List<Contact> contactList = (from i in _context.Contacts where i.User.Id == userid && !i.IsDeleted select i).ToList();
            foreach (Contact contact in contactList)
            {
                contact.Addresses = (from i in _context.Adresses where i.Contact.Id == contact.Id && !i.IsDeleted select i).ToList();
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
            return View("Error");
        }

        public ActionResult EditContactForm (int id)
        {
            if (ContactBelongsToUser(id))
            {
                Contact contact = _context.Contacts.Find(id);
                ContactViewModel contactViewModel = new ContactViewModel
                {
                    Id = id,
                    Name = contact.Name,
                    Surname = contact.Surname

                };
                return View(contactViewModel);
            }
            return View("Error");
        }
        [HttpPost]
        public ActionResult EditContact (ContactViewModel viewModel)
        {
            Contact contact = _context.Contacts.Include(m => m.User).SingleOrDefault(m => m.Id == viewModel.Id);
            contact.Name = viewModel.Name;
            contact.Surname = viewModel.Surname;
            _context.Contacts.AddOrUpdate(contact);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeleteContact (int id)
        {
            if (ContactBelongsToUser(id))
            {
                Contact contact = _context.Contacts.Find(id);
                contact.IsDeleted = true;
                List<Address> contactaddresses = _context.Adresses.Where(m => m.Contact.Id == contact.Id).ToList();
                foreach (Address address in contactaddresses)
                {
                    address.IsDeleted = true;
                    _context.Adresses.AddOrUpdate(address);
                }
                _context.Contacts.AddOrUpdate(contact);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Error");
        }

        //ADDRESSES
        private bool AddressBelongsToUser(int id)
        {
            string userid = User.Identity.GetUserId();
            bool check = _context.Adresses.Where(m => m.Id == id && m.Contact.User.Id == userid).Count() > 0;
            return check;
        }

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

        public ActionResult EditAddressForm (int id)
        {
            if (AddressBelongsToUser(id))
            {
                Address address = _context.Adresses.Include(m => m.Contact).SingleOrDefault(m => m.Id == id);
                AddressViewModel addressViewModel = new AddressViewModel
                {
                    Id = id,
                    ContactId = address.Contact.Id,
                    AddressText = address.AddressText,
                    City = address.City,
                    ZipCode = address.ZipCode,
                    Phone = address.Phone
                };
                return View(addressViewModel);
            }
            return View("Error");
        }
        [HttpPost]
        public ActionResult EditAddress (AddressViewModel viewModel)
        {
            Contact contact = _context.Contacts.Find(viewModel.ContactId);
            Address address = new Address
            {
                Id = viewModel.Id,
                Contact = contact,
                IsDeleted = false,
                AddressText = viewModel.AddressText,
                City = viewModel.City,
                ZipCode = viewModel.ZipCode,
                Phone = viewModel.Phone
            };
            _context.Adresses.AddOrUpdate(address);
            _context.SaveChanges();

            return RedirectToAction("ContactDetails", new { id = viewModel.ContactId });
        }
        public ActionResult DeleteAddress (int id)
        {
            if (AddressBelongsToUser(id))
            {
                Address address = _context.Adresses.Include(m => m.Contact).Where(m => m.Id == id).First();
                address.IsDeleted = true;
                _context.Adresses.AddOrUpdate(address);
                _context.SaveChanges();
                return RedirectToAction("ContactDetails", new { id = address.Contact.Id });
            }
            return View("Error");
        }


    }
}