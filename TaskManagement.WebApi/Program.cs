using TaskManagement.WebApi;
using TaskManagement.WebApi.MIddlewares;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<Irepository, Repository>();

var app = builder.Build();



// Configure the HTTP request pipeline.
app.UseMiddleware<RoleMiddleware>();



app.UseSwagger();
app.UseSwaggerUI();



app.UseHttpsRedirection();



app.UseAuthorization();



app.MapControllers();



app.Run();