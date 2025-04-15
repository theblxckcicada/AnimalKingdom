using AnimalKingdom.API.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// builder
//     .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddMicrosoftIdentityWebApi(
//         options =>
//         {
//             builder.Configuration.GetSection("AzureAdB2C").Bind(options);
//             options.TokenValidationParameters.ValidAudience = builder.Configuration[
//                 "AzureAdB2C:ClientId"
//             ];
//         },
//         options => builder.Configuration.GetSection("AzureAdB2C").Bind(options)
//     );

// Add API explorer and Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices(builder.Configuration, builder.Environment.IsDevelopment());

builder.Services.AddAuthorization();
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "HairSpot API V1");
});
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Configure CORS properly (you had an empty UseCors())
app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
