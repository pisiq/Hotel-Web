namespace Hotel.Models
{
    public class Room
    {  
        public int Id { get; set; }
        public string Type { get; set; }
        public Location Location { get; set; }
        public int LocationId { get; set; }
        public int Price { get; set; }
    }
}
