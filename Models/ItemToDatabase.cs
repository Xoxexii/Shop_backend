using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace shopbackend.Models
{
    public class ItemToDatabase
    {
        public static int idd;
        [Key] 
        public  int Id { get; set; }
        
        public  string Category { get; set; }
        
        public  string Name { get; set; }
        
        public  int Price { get; set; }
        
        public  int Amount { get; set; }
        public string Describe { get; set; }

        public  string ImPath1 { get; set; }
        public string ImPath2 { get; set; }
        public string ImPath3 { get; set; }
        public string ImPath4 { get; set; }
        public ItemToDatabase()
        {
            
        }
        public ItemToDatabase (string category,string name ,int price,int amount,string descibe,string im1, string im2, string im3, string im4)
        { 
            Category = category;
            Name = name;
            Price = price; 
            Amount = amount;
            Describe = descibe;
            for (int i = 1; i <= 4; i++)
            {
                if (i == 1)
                {
                    ImPath1 = $"https://localhost:7067//Image/{category}/{name}/{category}_{name}_{im1}";
                }
                if (i == 2)
                {
                    ImPath2 = $"https://localhost:7067//Image/{category}/{name}/{category}_{name}_{im2}";
                }
                if (i == 3)
                {
                    ImPath3 = $"https://localhost:7067//Image/{category}/{name}/{category}_{name}_{im3}";
                }
                if (i == 4)
                {
                    ImPath4 = $"https://localhost:7067//Image/{category}/{name}/{category}_{name}_{im4}";
                }
            }
            
                
            
        }
    }
    public class CategoryToDatabase
    {
        [Key]
        public int Id { get; set; }

        public string Category { get; set; }

        
        public CategoryToDatabase()
        {

        }
        public CategoryToDatabase(string category, string name, int price, int amount, string file)
        {
            Category = category;
        }
    }
    public class CommandDB : DbContext
    {
        public CommandDB(DbContextOptions<CommandDB> options) : base(options)
        {


        }
        public DbSet<ItemToDatabase> Items { get; set; }

        public DbSet<CategoryToDatabase> Categories { get; set; }
    }
}
