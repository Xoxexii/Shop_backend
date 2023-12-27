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
        
        public  string Path { get; set; }
        public ItemToDatabase()
        {
            
        }
        public ItemToDatabase (string category,string name ,int price,int amount,string file)
        { 
            Category = category;
            Name = name;
            Price = price; 
            Amount = amount;
            Path = $"https://localhost:7067//Image/{category}/{name}/{category}_{name}_{file}";
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
