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
        public string Describe { get; set; }

        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Token { get; set; }
        public ItemFromClient() { }

    }
}
