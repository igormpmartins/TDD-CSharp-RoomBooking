using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Processors;
using RoomBookingApp.Persistence;
using RoomBookingApp.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionStr = "DataSource=:memory:";
var conn = new SqliteConnection(connectionStr);
conn.Open();

builder.Services.AddDbContext<RoomBookingAppDbContext>(opt => opt.UseSqlite(conn));
EnsureDatabaseCreated(conn);

static void EnsureDatabaseCreated(SqliteConnection conn)
{
    var dbBuilder = new DbContextOptionsBuilder<RoomBookingAppDbContext>();
    dbBuilder.UseSqlite(conn);

    using var context = new RoomBookingAppDbContext(dbBuilder.Options);
    context.Database.EnsureCreated();
}

builder.Services.AddScoped<IRoomBookingService, RoomBookingService>();
builder.Services.AddScoped<IRoomBookingRequestProcessor, RoomBookingRequestProcessor>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
