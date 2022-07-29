using Hays.Data.DataAccess;
using Microsoft.EntityFrameworkCore;
using Hays.API.Configuration;

var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false)
        .Build();

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHaysDependency(config);




var app = builder.Build();

//starts the database
using (var scope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Context>();
    db.Database.EnsureCreated();
    if (db.Database.GetPendingMigrations().Count() > 0)
        db.Database.Migrate();
}



app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
