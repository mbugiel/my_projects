using AuthenticationAPI.Database;
using AuthenticationAPI.Encryption;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<DatabaseContext>(context => context.UseNpgsql(
    "Server="+builder.Configuration.GetValue<string>("Database:Server")+
    ";Port="+builder.Configuration.GetValue<string>("Database:Port")+
    ";Database="+builder.Configuration.GetValue<string>("Database:DB")+
    ";User Id="+builder.Configuration.GetValue<string>("Database:User")+
    ";Password="+Crypto.GetPasswd()+";"
    )
);


var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

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
