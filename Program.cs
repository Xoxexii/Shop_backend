using shopbackend.CouponStore;
using shopbackend.Models;
using JWT.Algorithms;
using JWT.Serializers;
using JWT;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shopbackend.GetValueFromDatabase;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var myOrigins = "_myOrigins";
builder.Services.AddCors(option =>
{
    option.AddPolicy(name: myOrigins, policy =>
    {
        policy.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
    });
});


builder.Services.AddDbContext<CommandDB>(options =>

    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))

    );

builder.Services.AddEndpointsApiExplorer();





var app = builder.Build();

app.UseCors(myOrigins);
app.UseStaticFiles();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();



app.MapGet("/api/getitem", async (CommandDB db) =>
{

    
        return Results.Ok(await db.Items.ToListAsync());
    

}

);





app.MapPost("/api/login", ([FromBody] Admin admin) =>
{

    GetValue getValue = new GetValue();
    MakeApi.api[0] = admin;
    if (MakeApi.api[0].Pass == getValue.password && MakeApi.api[0].User == getValue.username)
    {
        var payload = new Dictionary<string, string>
{
    { "user", MakeApi.api[0].User },

}; IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
        IJsonSerializer serializer = new JsonNetSerializer();
        IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
        IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
        const string key = "itim"; // not needed if algorithm is asymmetric
        var token = encoder.Encode(payload, key);
        Author.tokens = token;
        var res = new Author();
        res.Token = token;



        MakeApi.api2[0] = res;

        return Results.Ok(MakeApi.api2);
    }
    else
    {
        return Results.Ok(MakeApi.api3);
    }
});


app.MapPost("/api/additem", async (ItemFromClient item, CommandDB db) =>
{


    if (Author.tokens == item.Token)
    {

        ItemFromClient.categoryy = item.Category;
        ItemFromClient.name = item.Name;

        ItemToDatabase model = new ItemToDatabase(item.Category, item.Name, item.Price, item.Amount, item.Image);

        db.Items.Add(model);
        db.SaveChanges();

        return Results.Ok(true);

    }
    else
    {
        return Results.Ok(false);
    }
});


app.MapPost("/api/addimage", async (IFormFile file, CommandDB db) =>
{
    
    string str = @"C:\Users\chano\source\repos\shopbackend\wwwroot\Image\" + $"{ItemFromClient.categoryy}\\" + ItemFromClient.name;
    Directory.CreateDirectory(str);
    
    var filename = $"wwwroot/Image/{ItemFromClient.categoryy}/{ItemFromClient.name}/" + $"{ItemFromClient.categoryy}_{ItemFromClient.name}_" + file.FileName;
    using var stream = File.OpenWrite(filename);
    await file.CopyToAsync(stream);
    stream.Close();


    return Results.Ok(true);


});


app.MapPut("/api/edititem/{id}", async (EditItemFromClient item, CommandDB db, int id) =>
{


    if (Author.tokens == item.Token)
    {
        var stuff = await db.Items.FindAsync(id);
        stuff.Category = item.Category;
        stuff.Name = item.Name;
        stuff.Price = item.Price;
        stuff.Amount = item.Amount;
        await db.SaveChangesAsync();

        return Results.Ok(true);

    }
    else
    {
        return Results.Ok(false);
    }
});

app.MapDelete("/api/delete/{id}/{category}/{name}", async (int id, string category , string name ,CommandDB db) =>
{
    var pizza = await db.Items.FindAsync(id);



    if (pizza is null)
    {
        return Results.NotFound();
    }
    db.Items.Remove(pizza);
    await db.SaveChangesAsync();

    string str = @"C:\Users\chano\source\repos\shopbackend\wwwroot\Image\" + category + "\\" + name;
    Directory.Delete(str, true);
    
    return Results.Ok(true);
});

app.MapPost("/api/addcategory", (AddCategoryToServer items , CommandDB db) => {

    CategoryToDatabase cate = new CategoryToDatabase();
    cate.Category = items.Category;
    db.Categories.Add(cate);
    db.SaveChanges();
    return Results.Ok(db.Categories.ToList());
});

app.MapGet("/api/getcategory", (CommandDB db) =>
{
    return Results.Ok(db.Categories.ToList());

});
app.MapDelete("/api/deletecategory/{id}", async (int id, CommandDB db) =>
{
    var cate = await db.Categories.FindAsync(id);

    db.Categories.Remove(cate);
    await db.SaveChangesAsync();
    return Results.Ok(true);

});

app.Run();

