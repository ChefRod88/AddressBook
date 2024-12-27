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
    public class Program
    {
        static void Main(string[] args)
        {
            // Create an instance of the AddressBook
            AddressBook addressBook = new AddressBook();

            // Example usage
            addressBook.AddContact("John Doe", "john@example.com", new List<string> { "123-456-7890", "987-654-3210" });
            addressBook.AddContact("Jane Smith", "jane@example.com", new List<string> { "555-123-4567" });

            // Display all contacts
            addressBook.DisplayContacts();

            // Search for a contact
            addressBook.SearchContact("John");

            // Edit a contact
            addressBook.EditContact("John Doe");

            // Delete a contact
            addressBook.DeleteContact("Jane Smith");

            // Display all contacts after changes
            addressBook.DisplayContacts();
        }
    }
}
