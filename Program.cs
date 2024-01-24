using shopbackend.CouponStore;
using shopbackend.Models;
using JWT.Algorithms;
using JWT.Serializers;
using JWT;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shopbackend.GetValueFromDatabase;
using shopbackend.Api;
using shopbackend.Api.Service;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(o  =>
{

    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
        
    };
});
builder.Services.AddAuthorization();


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

builder.Services.Configure<EmailSetting>(builder.Configuration.GetSection("EmailSetting"));
builder.Services.AddTransient<IEmailService, EmailService>();



var app = builder.Build();

app.UseCors(myOrigins);
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

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
    

}).RequireAuthorization();





app.MapPost("/api/login", ([FromBody] Admin admin) =>
{

    GetValue getValue = new GetValue();
    MakeApi.api[0] = admin;
    if (MakeApi.api[0].Pass == getValue.password && MakeApi.api[0].User == getValue.username)
    {
       var issuer = builder.Configuration["Jwt:Issuer"];
        var audience = builder.Configuration["Jwt:Audience"];
        var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
        var tokenDiscriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id",Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, admin.User),
                new Claim(JwtRegisteredClaimNames.Email, admin.User),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

            }),
            Expires = DateTime.UtcNow.AddMinutes(5),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key) , SecurityAlgorithms.HmacSha256),
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDiscriptor);
        var jwttoken = tokenHandler.WriteToken(token);
        var res = tokenHandler.WriteToken(token);
        



        

        return Results.Ok(res);
    }
    else
    {
        return Results.Unauthorized();
    }
});


app.MapPost("/api/additem", async (ItemFromClient item, CommandDB db) =>
{


    if (Author.tokens == item.Token)
    {

        ItemFromClient.categoryy = item.Category;
        ItemFromClient.name = item.Name;

        ItemToDatabase model = new ItemToDatabase(item.Category, item.Name, item.Price, item.Amount, item.Describe, item.Image1, item.Image2, item.Image3, item.Image4);

        db.Items.Add(model);
        db.SaveChanges();

        return Results.Ok(true);

    }
    else
    {
        return Results.Ok(false);
    }
});


app.MapPost("/api/addimage", async (IFormFileCollection files, CommandDB db) =>
{
        foreach(var file in files)
        {
            string str = @"C:\Users\chano\source\repos\shopbackend\wwwroot\Image\" + $"{ItemFromClient.categoryy}\\" + ItemFromClient.name;
            Directory.CreateDirectory(str);

            var filename = $"wwwroot/Image/{ItemFromClient.categoryy}/{ItemFromClient.name}/" + $"{ItemFromClient.categoryy}_{ItemFromClient.name}_" + file.FileName;
            using var stream = File.OpenWrite(filename);

            await file.CopyToAsync(stream);
            stream.Close();
        }
    


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

app.MapPost("/api/submitpayment", async (DetailFromCustomer emailFromCustomer,IEmailService emailService) =>
{
    try
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Chanokchon Kongsumrit", "chanokchonkzr@gmail.com"));
        message.To.Add(new MailboxAddress($"{emailFromCustomer.Firstname}{emailFromCustomer.Lastname}", $"{emailFromCustomer.Email}"));
        message.Subject = "คำสั่งซื้อ";

        message.Body = new TextPart("plain")
        {
            Text = @"Hey Chandler,

I just wanted to let you know that Monica and I were going to go play some paintball, you in?

-- Joey"
        };

        using (var client = new SmtpClient())
        {
            client.Connect("smtp.gmail.com", 587, false);

            // Note: only needed if the SMTP server requires authentication
            client.Authenticate("chanokchonkzr@gmail.com", "dnournxvfjhwlmvf");

            client.Send(message);
            client.Disconnect(true);
        }

        return Results.Ok();
    }
    catch (Exception ex)
    {
        throw;
    }
});

app.Run();

