using Auora.ConDB;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var connectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION");
    return new MongoClient(connectionString);
});

builder.Services.AddScoped(sp =>
{
    var databaseName = Environment.GetEnvironmentVariable("MONGO_DATABASE");
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(databaseName);
});

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Strict;
    options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
    options.Secure = CookieSecurePolicy.Always;
});


builder.Services.AddSession();

builder.Services.AddScoped<ProdutoService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddAntiforgery();
builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
   
    app.UseHsts();
}


app.UseCookiePolicy();
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseSession();
app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.MapControllers();

app.Run();
