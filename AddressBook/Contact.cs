namespace AddressBook
{
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

        public override string ToString() //*
        {
            return $"Name: {Name}, Email: {Email}, PhoneNumbers: {string.Join(", ", PhoneNumbers)}";
        }

        //Main list to store contacts
        private List<Contact> contacts = new List<Contact>(); //*

        //method to add a new contact
        public void AddContact(string name, string email, List<string> phoneNumbers)
        {
            contacts.Add(new Contact(name, email, phoneNumbers)); // *
            Console.WriteLine("Contact added successfully");
        }

        //Search for contact by name or email method 
        public void SearchContact(string query)
        {
            var results = contacts.Where(c => c.Name.Equals(query, StringComparison.OrdinalIgnoreCase) ||
                                              c.Email.Equals(query, StringComparison.OrdinalIgnoreCase)).ToList();

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

        // Edit contact method
        public void EditContact(string name)
        {
            var contact = contacts.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)); //*
            if (contact != null)
            {
                Console.Write(" Enter new email address: ");
                contact.Email = Console.ReadLine();

                Console.Write(" Enter new phone number: (comma separated): ");
                contact.PhoneNumbers = Console.ReadLine().Split(',').Select(p => p.Trim()).ToList();

                Console.WriteLine("Contact updated successfully");
            }
            else
            {
                Console.WriteLine("Contact not found");
            }
        }

        // Delete contact method
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
                contacts.Add(new Contact(name, duplicates.First().Email, mergedPhones)); // Use duplicates.First() here
                Console.WriteLine("Contacts merged successfully");
            }
            else
            {
                Console.WriteLine("No duplicate contacts found");
            }

        }

        public void ExportContacts(string fileName) // Method to export contacts to a file //*
        {
            using (var writer = new StreamWriter(fileName))
            {
                foreach (var contact in contacts)
                {
                    writer.WriteLine($"{contact.Name}, {contact.Email}, {string.Join(",", contact.PhoneNumbers)}");
                    Console.WriteLine($"{contact.Name},{contact.Email},{string.Join(",", contact.PhoneNumbers)}");
                }

            }

            Console.WriteLine($"Contacts exported to {fileName}");
        }

        // Method to import contacts to a file
        public void ImportContacts(string fileName)
        {
            if (File.Exists(fileName)) //*
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

                Console.WriteLine($"Contacts imported successfully");
            }
            else
            {
                Console.WriteLine("File not found");
            }

        }

        public void DisplayContacts(string fileName) // Method to display contacts
        {
            var sortedContacts = contacts.OrderBy(c => c.Name).ToList();
            Console.WriteLine("\nContacts");
            foreach (var contact in sortedContacts)
            {
                Console.WriteLine(contact);
            }
        }


    }
}
