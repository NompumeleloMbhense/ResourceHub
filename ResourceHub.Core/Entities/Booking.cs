using System;
using System.Collections.Generic;
using System.Text;


/// <summary>
/// 
/// </summary>
namespace ResourceHub.Core.Entities
{
    public class Booking
    {
        public int Id { get; private set; }
        public int ResourceId { get; private set; }

        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public string BookedBy { get; private set; } = string.Empty;
        public string Purpose { get; private set; } = string.Empty;

        // Navigation Property
        public Resource Resource { get; private set; } = null!;

        public Booking(int resourceId, DateTime startTime, DateTime endTime, string bookedBy, string purpose)
        {
            if (resourceId <= 0)
                throw new ArgumentException("Valid ResourceId is required");

            if (startTime >= endTime)
                throw new ArgumentException("End time must be after start time");

            if (string.IsNullOrWhiteSpace(bookedBy))
                throw new ArgumentException("BookedBy is required");

            if (string.IsNullOrWhiteSpace(purpose))
                throw new ArgumentException("Purpose is required");

            ResourceId = resourceId;
            StartTime = startTime;
            EndTime = endTime;
            BookedBy = bookedBy;
            Purpose = purpose;
        }

        // Behaviour 
        public void UpdateTime(DateTime startTime, DateTime endTime)
        {
            if (startTime >= endTime)
                throw new ArgumentException("End time must be after start time");

            StartTime = startTime;
            EndTime = endTime;
        }

        public void UpdateDetails(string bookedBy, string purpose)
        {
            if (string.IsNullOrWhiteSpace(purpose))
                throw new ArgumentException("Purpose is required");

            if (string.IsNullOrWhiteSpace(bookedBy))
                throw new ArgumentException("BookedBy is required");

            BookedBy = bookedBy;
            Purpose = purpose;
        }
    }
}
