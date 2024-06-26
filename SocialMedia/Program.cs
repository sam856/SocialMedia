
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Models;
using SocialMedia.Services;
using System.Text;
using System.Text.Json.Serialization;

namespace SocialMedia
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddEndpointsApiExplorer();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder .Services.AddHostedService<StoriesService>();
            builder.Services.AddDbContext<AppDbContext>();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
             .AddEntityFrameworkStores<AppDbContext>()
             .AddDefaultTokenProviders();
            builder.Services.AddCors(option =>
            {
                option.AddPolicy("MyPolicy", options =>
                {

                    options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });

            });


            builder.Services.AddControllers()
                                    .AddJsonOptions(options =>
                                    {
                                        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                                    });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
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
             ValidIssuer = builder.Configuration["Jwt:Issuer"],
             ValidAudience = builder.Configuration["Jwt:Audience"],
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
         };
     });



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseRouting();
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors("MyPolicy");
            app.MapControllers();

            app.Run();
        }
    }
}