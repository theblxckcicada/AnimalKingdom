using AnimalKingdom.API.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add API explorer and Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthenticationServices(builder.Configuration, builder.Environment.IsDevelopment());
builder.Services.AddApplicationServices(builder.Configuration, builder.Environment.IsDevelopment());


var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Animal Kingdom API");
});
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Configure CORS properly (you had an empty UseCors())
app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
