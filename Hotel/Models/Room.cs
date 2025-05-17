namespace Hotel.Models
{
    public class Room
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public decimal Price { get; set; }

        // Count for each room type
        public int RoomCount { get; set; }

        public string? Description { get; set; }

        // Main photo and additional photos (3 total)
        public string? MainPhotoPath { get; set; }

        public List<RoomPhoto> Photos { get; set; } = new List<RoomPhoto>();

        // Navigation properties for the location relationship
        public Location Location { get; set; }

        public int LocationId { get; set; }
    }
}

namespace Hotel.Models
{
    public class RoomPhoto
    {
        public int Id { get; set; }

        public int RoomId { get; set; }

        public Room Room { get; set; }

        public string FilePath { get; set; }

        public string? Caption { get; set; }

        public bool IsFeatured { get; set; }

        public int DisplayOrder { get; set; }
    }
}