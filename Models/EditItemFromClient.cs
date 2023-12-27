namespace shopbackend.Models
{
    public class EditItemFromClient
    {
        public string Category { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Amount { get; set; }
        public string Token { get; set; }
        public EditItemFromClient() { }
    }
}
