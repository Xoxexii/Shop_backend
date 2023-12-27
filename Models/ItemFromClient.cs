using System.ComponentModel.DataAnnotations;

namespace shopbackend.Models
{
    public class ItemFromClient
    {
        public static string categoryy;
        public static string name;

        
        public string Category { get; set; }
        
        public string Name { get; set; }
        
        public int Price { get; set; }
        
        public int Amount { get; set; }
        
        public string Image { get; set; }
        public string Token { get; set; }
        public ItemFromClient() { }

    }
}
