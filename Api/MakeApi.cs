using shopbackend.Models;

namespace shopbackend.CouponStore
{
    public class MakeApi
    {
        public static List<Admin> api = new List<Admin>()
        {
            new Admin() { User = "" , Pass = ""}
        };
        public static List<Author> api2 = new List<Author>()
        {
            new Author(){ Token = ""}
        };
        public static List<Unauth> api3 = new List<Unauth>()
        {
            new Unauth() {Token = "invalid"}
        };
        public static List<string> categorylist = new List<string>()
        {
            
        };
    }
}
