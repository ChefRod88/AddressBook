using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AddressBook
{
    // Contact class represents individual contact details
    public class Contact
    {
        public string Name;
        public string Email;
        public List<string> PhoneNumbers;

        public Contact(string name, string email, List<string> phoneNumbers)
        {
            Name = name;
            Email = email;
            PhoneNumbers = phoneNumbers;
        }

        public override string ToString()
        {
            return $"Name: {Name}, Email: {Email}, PhoneNumbers: {string.Join(", ", PhoneNumbers)}";
        }
    }

    // AddressBook class manages a list of contacts and various operations
    public class AddressBook
    {
        // List to store contacts
        private List<Contact> contacts = new List<Contact>();

        // Method to add a new contact
        public void AddContact(string name, string email, List<string> phoneNumbers)
        {
            contacts.Add(new Contact(name, email, phoneNumbers));
            Console.WriteLine("Contact added successfully");
        }

        // Method to search for a contact by name or email
        public void SearchContact(string query)
        {
            var results = contacts.Where(c => c.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                                              c.Email.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();

            if (results.Count > 0)
            {
                Console.WriteLine("\nSearch results:");
                foreach (var contact in results)
                {
                    Console.WriteLine(contact);
                }
            }
            else
            {
                Console.WriteLine("\nNo contacts found");
            }
        }

        // Method to edit a contact by name
        public void EditContact(string name)
        {
            var contact = contacts.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (contact != null)
            {
                Console.Write("Enter new email address: ");
                contact.Email = Console.ReadLine();

                Console.Write("Enter new phone numbers (comma separated): ");
                contact.PhoneNumbers = Console.ReadLine().Split(',').Select(p => p.Trim()).ToList();

                Console.WriteLine("Contact updated successfully");
            }
            else
            {
                Console.WriteLine("Contact not found");
            }
        }

        // Method to delete a contact by name
        public void DeleteContact(string name)
        {
            var contact = contacts.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (contact != null)
            {
                contacts.Remove(contact);
                Console.WriteLine("Contact deleted successfully");
            }
            else
            {
                Console.WriteLine("Contact not found");
            }
        }

        // Method to merge contacts with the same name
        public void MergeContacts(string name)
        {
            var duplicates = contacts.Where(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
            if (duplicates.Count > 1)
            {
                var mergedPhones = duplicates.SelectMany(c => c.PhoneNumbers).Distinct().ToList();
                contacts.RemoveAll(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                contacts.Add(new Contact(name, duplicates.First().Email, mergedPhones));

                Console.WriteLine("Contacts merged successfully");
            }
            else
            {
                Console.WriteLine("No duplicate contacts found");
            }
        }

        // Method to export contacts to a file
        public void ExportContacts(string fileName)
        {
            using (var writer = new StreamWriter(fileName))
            {
                foreach (var contact in contacts)
                {
                    writer.WriteLine($"{contact.Name}, {contact.Email}, {string.Join(",", contact.PhoneNumbers)}");
                }
            }

            Console.WriteLine($"Contacts exported to {fileName}");
        }

        // Method to import contacts from a file
        public void ImportContacts(string fileName)
        {
            if (File.Exists(fileName))
            {
                var lines = File.ReadAllLines(fileName);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    var name = parts[0];
                    var email = parts[1];
                    var phoneNumbers = parts[2].Split(';').ToList();

                    contacts.Add(new Contact(name, email, phoneNumbers));
                }

                Console.WriteLine("Contacts imported successfully");
            }
            else
            {
                Console.WriteLine("File not found");
            }
        }

        // Method to display all contacts
        public void DisplayContacts()
        {
            var sortedContacts = contacts.OrderBy(c => c.Name).ToList();
            Console.WriteLine("\nContacts:");
            foreach (var contact in sortedContacts)
            {
                Console.WriteLine(contact);
            }
        }
    }

    // Main class with entry point
    public class Program // entry point of Program
    {
        static void Main(string[] args)
        {
            // Create an instance of the AddressBook Class
            var addressBook = new AddressBook();
            while (true)
            {
                Console.WriteLine("\nAddress Book Menu:");
                Console.WriteLine("1. Add Contact");
                Console.WriteLine("2. Search Contact");
                Console.WriteLine("3. Edit Contact");
                Console.WriteLine("4. Delete Contact");
                Console.WriteLine("5. Merge Contacts");
                Console.WriteLine("6. Export Contacts");
                Console.WriteLine("7. Import Contacts");
                Console.WriteLine("8. Display Contacts");
                Console.WriteLine("9. Exit");
                
                Console.Write("Choose an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                   case "1":
                       Console.Write("Enter Name: ");
                       var name = Console.ReadLine();
                       Console.Write("Enter Email: ");
                       var email = Console.ReadLine();
                       Console.Write("Enter Phone Numbers: (comma separated): ");
                       var phoneNumbers = Console.ReadLine().Split(',').Select(p => p.Trim()).ToList();
                       addressBook.AddContact(name, email, phoneNumbers);
                       break;
                   
                   case "2":
                       Console.Write("Enter Name or Email to search: ");
                       var query = Console.ReadLine();
                       addressBook.SearchContact(query);
                       break;
                   
                   case "3":
                       Console.Write("Enter Name of Contact to Edit: ");
                       var editName = Console.ReadLine();
                       addressBook.EditContact(editName);
                       break;
                   
                   case "4":
                       Console.Write("Enter Name of Contact to Delete: ");
                       var deleteName = Console.ReadLine();
                       addressBook.DeleteContact(deleteName);
                       break;
                   
                   case "5":
                       Console.Write("Enter Name of Contact to Merge: ");
                       var mergeName = Console.ReadLine();
                       addressBook.MergeContacts(mergeName);
                       break;
                   
                   case "6":
                       Console.Write("Enter File Name to Export Contacts: ");
                       var exportFile = Console.ReadLine();
                       addressBook.ExportContacts(exportFile);
                       break;
                   
                   case "7":
                       Console.Write("Enter File Name to Import Contacts: ");
                       var importFile = Console.ReadLine();
                       addressBook.ImportContacts(importFile);
                       break;
                   
                   case "8":
                       addressBook.DisplayContacts();
                       break;
                   
                   case "9":
                       Console.WriteLine("Exiting program.");
                       return;
                   
                   default:
                       Console.WriteLine("Invalid option. Please try again.");
                       break;
                }
            }
            

            
        }
    }
}
