using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceHub.Core.Entities
{
    public class Resource
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string Location { get; private set; } = string.Empty;
        public int Capacity { get; private set; }
        public bool IsAvailable { get; private set; } = true;

        // Navigation property
        public List<Booking> Bookings { get; private set; } = new();


        public Resource(string name, string description, string location, int capacity) 
        {
            if(string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Resource name is required");

            if (capacity <= 0)
                throw new ArgumentOutOfRangeException("Capacity must be greater than 0");

            Name = name;
            Description = description;
            Location = location;
            Capacity = capacity;
        }

        // Behaviour (business logic)
        public void SetAvailability(bool isAvailable)
        {
            IsAvailable = isAvailable;
        }

        public void UpdateDetails(string name, string description, string location, int capacity, bool isAvailable)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Resource name is required");

            if (capacity <= 0)
                throw new ArgumentException("Capacity must be greater than 0");

            Name = name;
            Description = description;
            Location = location;
            Capacity = capacity;
        }
    }
}
