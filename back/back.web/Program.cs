using back.Controllers;
using back.DAL;
using back.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var dbConnectionString = builder.Configuration.GetConnectionString("postgresDB");
builder.Services.AddDbContext<DatabaseContext>(o => o.UseNpgsql(dbConnectionString));

builder.Services.AddScoped<IUserStoryPropositionService, UserStoryPropositionService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserStoryService, UserStoryService>();


var app = builder.Build();

// Enable CORS
app.UseCors(builder =>
{
    builder.AllowAnyOrigin();
    builder.AllowAnyHeader();
    builder.AllowAnyMethod();
});

var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
};

app.UseWebSockets(webSocketOptions);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // use custom url for calls in swagger    
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<DatabaseContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}

app.Run();
