namespace shopbackend.Models
{
    public class Author
    {
        public static string tokens;
        public string Token { get; set; }
        public Author()
        {

        }
        public Author(string token)
        {
            tokens = token;
        }
    }
}
