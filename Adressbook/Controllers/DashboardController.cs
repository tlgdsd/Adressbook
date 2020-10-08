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
            try
            {
                string userid = User.Identity.GetUserId();
                List<Contact> contactList = (from i in _context.Contacts where i.User.Id == userid && !i.IsDeleted select i).ToList();
                foreach (Contact contact in contactList)
                {
                    contact.Addresses = (from i in _context.Adresses where i.Contact.Id == contact.Id && !i.IsDeleted select i).ToList();
                }
                return View(contactList);
            }
            catch (Exception)
            {
                if (_context != null)
                    _context.Dispose();
                return View("Error");
            }
        }

        #region Contact

        private bool CheckContact(Contact contact)
        {
            if (contact == null)
                return false;
            return contact.User.Id == User.Identity.GetUserId() && !contact.IsDeleted;
        }
        public ActionResult NewContactForm()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddContact(ContactViewModel viewModel)
        {
            try
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
            catch (Exception)
            {
                if (_context != null)
                    _context.Dispose();
                return View("Error");
            }
        }
        public ActionResult ContactDetails(int id)
        {
            try
            {
                Contact contact = _context.Contacts.Include("User").FirstOrDefault(d => d.Id == id);
                if (CheckContact(contact))
                {
                    contact.Addresses = (from i in _context.Adresses where i.Contact.Id == contact.Id && !i.IsDeleted select i).ToList();

                    return View(contact);
                }
                return View("NotFound");
            }
            catch (Exception)
            {
                if (_context != null)
                    _context.Dispose();
                return View("Error");
            }
        }

        public ActionResult EditContactForm(int id)
        {
            try
            {
                Contact contact = _context.Contacts.Include("User").FirstOrDefault(m => m.Id == id);
                if (CheckContact(contact))
                {
                    ContactViewModel contactViewModel = new ContactViewModel
                    {
                        Id = id,
                        Name = contact.Name,
                        Surname = contact.Surname

                    };
                    return View(contactViewModel);
                }
                return View("NotFound");
            }
            catch (Exception)
            {
                if (_context != null)
                    _context.Dispose();
                return View("Error");
            }

        }

        [HttpPost]
        public ActionResult EditContact(ContactViewModel viewModel)
        {
            try
            {
                Contact contact = _context.Contacts.Include("User").FirstOrDefault(m => m.Id == viewModel.Id);
                if (CheckContact(contact))
                {
                    contact.Name = viewModel.Name;
                    contact.Surname = viewModel.Surname;
                    _context.Contacts.AddOrUpdate(contact);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View("NotFound");
            }
            catch (Exception)
            {
                if (_context != null)
                    _context.Dispose();
                return View("Error");
            }

        }
        public ActionResult DeleteContact(int id)
        {
            try
            {
                Contact contact = _context.Contacts.Include("User").FirstOrDefault(m => m.Id == id);
                if (CheckContact(contact))
                {
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
                return View("NotFound");
            }
            catch (Exception)
            {
                if (_context != null)
                    _context.Dispose();
                return View("Error");
            }
        }

        #endregion

        #region Address

        private bool CheckAddress(Address address)
        {
            if (address == null)
                return false;

            return address.Contact.User.Id == User.Identity.GetUserId() && !address.IsDeleted;
        }

        public ActionResult NewAddressForm(int id)
        {
            try
            {
                Contact contact = _context.Contacts.Include("User").FirstOrDefault(m => m.Id == id);
                if (CheckContact(contact))
                {
                    AddressViewModel addressViewModel = new AddressViewModel
                    {
                        ContactId = id
                    };
                    return View(addressViewModel);
                }
                return View("NotFound");
            }
            catch (Exception)
            {
                if (_context != null)
                    _context.Dispose();
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult AddAddress(AddressViewModel viewModel)
        {
            try
            {
                Contact contact = _context.Contacts.Include("User").FirstOrDefault(m => m.Id == viewModel.ContactId);
                if (CheckContact(contact))
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

                    return RedirectToAction("ContactDetails", new { id = viewModel.ContactId });
                }
                return View("NotFound");
            }
            catch (Exception)
            {
                if (_context != null)
                    _context.Dispose();
                return View("Error");
            }

        }

        public ActionResult EditAddressForm(int id)
        {
            try
            {
                Address address = _context.Adresses.Include("Contact").Include("Contact.User").FirstOrDefault(m => m.Id == id);
                if (CheckAddress(address))
                {
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
                return View("NotFound");
            }
            catch (Exception)
            {
                if (_context != null)
                    _context.Dispose();
                return View("Error");
            }
        }
        [HttpPost]
        public ActionResult EditAddress(AddressViewModel viewModel)
        {
            try
            {
                Contact contact = _context.Contacts.Include("User").FirstOrDefault(m => m.Id == viewModel.ContactId); ;
                if (CheckContact(contact))
                {
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
                return View("NotFound");
            }
            catch (Exception)
            {
                if (_context != null)
                    _context.Dispose();
                return View("Error");
            }

        }
        public ActionResult DeleteAddress(int id)
        {
            try
            {
                Address address = _context.Adresses.Include("Contact").Include("Contact.User").FirstOrDefault(m => m.Id == id);
                if (CheckAddress(address))
                {
                    address.IsDeleted = true;
                    _context.Adresses.AddOrUpdate(address);
                    _context.SaveChanges();
                    return RedirectToAction("ContactDetails", new { id = address.Contact.Id });
                }
                return View("NotFound");
            }
            catch (Exception)
            {
                if (_context != null)
                    _context.Dispose();
                return View("Error");
            }
        }

        #endregion
    }
}