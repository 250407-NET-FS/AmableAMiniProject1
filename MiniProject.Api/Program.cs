using System.Text;
using MiniProject.Data;
using MiniProject.Models;
using MiniProject.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore;
using System.Security.Claims;
using MiniProject.Api;


var builder = WebApplication.CreateBuilder(args);
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string"
        + "'DefaultConnection' not found.");



builder.Services.AddDbContext<FitnessContext>(o =>
{
    o.UseSqlServer(connectionString);
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddMediatR(options => {

    options.RegisterServicesFromAssemblyContaining<MiniProject.Services.Users.Commands.LoginUser>();
    
});



builder.Services.AddControllers();
// Learn more about configuring OpenApi at https://aka.ms/aspnet/openApi
builder.Services.AddOpenApi();

builder
    .Services.AddIdentityCore<User>(options =>
    {
        options.Lockout.AllowedForNewUsers = false;
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = true;
    })
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<FitnessContext>()
    .AddSignInManager()
    .AddRoleManager<RoleManager<IdentityRole<Guid>>>();

SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));

builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            NameClaimType = ClaimTypes.Name,
            RoleClaimType = ClaimTypes.Role,
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = ctx =>
            {
                // grab the cookie named "jwt" and then User.Identity?.IsAuthenticated should work
                if (ctx.Request.Cookies.TryGetValue("jwt", out var token))
                    ctx.Token = token;
                return Task.CompletedTask;
            },
        };
    });


builder.Services.AddAuthorization();

//Services

builder.Services.AddScoped<IUserService, UserService>();





//swagger
//Adding swagger support
builder.Services.AddEndpointsApiExplorer();

//Modifying this AddSwaggerGen() call to allow us to test/debug our Auth scheme setup in swagger
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer"
        }
    );
    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        }
    );
});

//Can use http requests
builder.Services.AddHttpClient();

//cORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // React dev server
              .AllowAnyHeader()
              .AllowAnyMethod();
    });

    // options.AddPolicy("AllowFrontend", policy =>
    // {

    //     policy.WithOrigins("https://jaza-bnerbvbkfadkhkbf.canadaeast-01.azurewebsites.net") // azure

    //           .AllowAnyHeader()
    //           .AllowAnyMethod();
    // });

});


var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    // app.UseCors("AllowReactApp");
    // Console.WriteLine("allowed dev");
    
//}
// else
// {
//     app.UseCors("AllowFrontend");

//     app.UseExceptionHandler("/Error");
//     app.UseHsts();

// }



app.UseHttpsRedirection();


app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();


// For first timec
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        await Seeder.SeedAdmin(services);
        await Seeder.SeedUser(services);

    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error seeding roles");
    }
}


app.Run();
