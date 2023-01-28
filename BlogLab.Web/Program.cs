using BlogLab.Identity;
using BlogLab.Models.Account;
using BlogLab.Models.Settings;
using BlogLab.Repository;
using BlogLab.Services;
using BlogLab.Web.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.Configure<Cloudinaryoptions>(builder.Configuration.GetSection("CloudinaryOptions"));
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddControllers();
builder.Services.AddCors();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<IBlogCommentRepository, BlogCommentRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IPhotoRepository, PhotoRepository>();

builder.Services.AddIdentityCore<ApplicationUserIdentity>(opt =>
{
    opt.Password.RequireNonAlphanumeric = false;
})
    .AddUserStore<UserStore>()
    .AddDefaultTokenProviders()
    .AddSignInManager<SignInManager<ApplicationUserIdentity>>();


builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(
            options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer =true,
                    ValidateAudience = true,
                    ValidateLifetime =true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                    ClockSkew= TimeSpan.Zero

                };
            }
        );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.ConfigureExceptionHandler();



if(app.Environment.IsDevelopment())
{
    app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
}
else
{
    app.UseCors(options => options.WithOrigins("https://"));
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
