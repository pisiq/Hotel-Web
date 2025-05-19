using Hotel.Models;
using System.ComponentModel.DataAnnotations;

public class RoomViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Room type is required")]
    public string Type { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Room count is required")]
    [Range(1, int.MaxValue, ErrorMessage = "At least one room must be available")]
    public int RoomCount { get; set; }

    [Required(ErrorMessage = "Location is required")]
    public int LocationId { get; set; }

    public string? Description { get; set; }

    // Current main photo path (for edit mode)
    public string? MainPhotoPath { get; set; }

    // File upload properties - separate from entity properties
    public IFormFile? MainPhoto { get; set; }

    public List<IFormFile>? AdditionalPhotos { get; set; }

    // For display purposes in views
    public string? LocationName { get; set; }

    public List<RoomPhotoViewModel>? ExistingPhotos { get; set; }

    // Helper for mapping to entity
    public Room ToEntity()
    {
        return new Room
        {
            Id = Id,
            Type = Type,
            Price = Price,
            RoomCount = RoomCount,
            LocationId = LocationId,
            Description = Description,
            MainPhotoPath = MainPhotoPath
        };
    }
}

public class RoomPhotoViewModel
{
    public int Id { get; set; }
    public string FilePath { get; set; }
    public string? Caption { get; set; }
    public bool IsFeatured { get; set; }
    public int DisplayOrder { get; set; }
}
