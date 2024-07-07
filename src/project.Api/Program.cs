using project.Core.Models;
using project.EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.Extensions.DependencyInjection;
using project.Service.Contracts;
using project.Service.Services;
using project.Core.Interfaces;
using project.EF.Repository;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(op =>
 {
     op.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
     {
         In = ParameterLocation.Header,
         Name = "Authorization",
         Type = SecuritySchemeType.ApiKey
     });
     op.OperationFilter<SecurityRequirementsOperationFilter>();
 });




builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("localServer"));
});


// builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
//         .AddEntityFrameworkStores<ApplicationDbContext>()
//         .AddDefaultTokenProviders();

// builder.Services.AddDefaultIdentity<ApplicationUser>()
//         .AddEntityFrameworkStores<ApplicationDbContext>()
//         .AddDefaultTokenProviders();

builder.Services.AddIdentity<User, IdentityRole>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager<SignInManager<User>>()
    .AddDefaultTokenProviders();

// builder.Services.AddIdentityCore<Merchant>(options => options.SignIn.RequireConfirmedAccount = true)
//     .AddRoles<IdentityRole>()
//     .AddEntityFrameworkStores<ApplicationDbContext>()
//     .AddDefaultTokenProviders();

builder.Services.AddTransient(typeof(ITokenService), typeof(TokenService));
// builder.Services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddTransient<IUnitOfWork,UnitOfWork>();


builder.Services.AddControllers();

builder.Services.Configure<IdentityOptions>(options =>
       {
           options.Password.RequireDigit = true;
           options.Password.RequireLowercase = true;
           options.Password.RequireNonAlphanumeric = true;
           options.Password.RequireUppercase = true;
           options.Password.RequiredLength = 6;
           options.Password.RequiredUniqueChars = 1;
       });

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);

builder.Services.AddAuthentication(options =>
      {
          options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
          options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(options =>
      {
          options.TokenValidationParameters = new TokenValidationParameters
          {
              ValidateIssuer = true,
              ValidateAudience = true,
              ValidateLifetime = true,
              ValidateIssuerSigningKey = true,
              ValidIssuer = jwtSettings["Issuer"],
              ValidAudience = jwtSettings["Audience"],
              IssuerSigningKey = new SymmetricSecurityKey(key)
          };
      });

builder.Services.AddAuthorization();




var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();
