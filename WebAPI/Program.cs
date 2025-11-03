using BL.AutoMapper;
using BL.Dtos;
using BL.Services;
using BL.Services.Repo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<BL.Models.DatabaseContext>(options =>
{
    options.UseSqlServer("Name=ConnectionStrings:DatabaseConnStr");
});

// Configure JWT security services
var secureKey = builder.Configuration["JWT:SecureKey"];
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        var Key = Encoding.UTF8.GetBytes(secureKey);
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = new SymmetricSecurityKey(Key)
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1",
        new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Craftsman finder Web API", Version = "v1" });

    option.AddSecurityDefinition("Bearer",
         new OpenApiSecurityScheme
         {
             In = ParameterLocation.Header,
             Description = "Please enter valid JWT",
             Name = "Authorization",
             Type = SecuritySchemeType.Http,
             BearerFormat = "JWT",
             Scheme = "Bearer"
         }
    );

    option.AddSecurityRequirement(
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
                new List<string>()
            }
       });

});

builder.Services.AddScoped<IAuthentication, PersonService>();
builder.Services.AddScoped<PersonService>();

builder.Services.AddScoped<ISqlRepository<ResponseRoleDto, CreateRoleDto, EditRoleDto>, RoleService>();
builder.Services.AddScoped<ISqlRepository<ResponseCountryDto, CreateCountryDto, EditCountryDto>, CountryService>();
builder.Services.AddScoped<ISqlRepository<ResponseTownDto, CreateTownDto, EditTownDto>, TownService>();
builder.Services.AddScoped<ISqlRepository<ResponseLocationDto, CreateLocationDto, EditLocationDto>, LocationService>();
builder.Services.AddScoped<ISqlRepository<ResponseJobTypeDto, CreateJobTypeDto, EditJobTypeDto>, JobTypeService>();
builder.Services.AddScoped<ISqlRepository<ResponseContractorDto, CreateContractorDto, EditContractorDto>, ContractorService>();
builder.Services.AddScoped<ISqlRepository<ResponseContractorLocationDto, CreateContractorLocationDto, EditContractorLocationDto>, ContractorLocationService>();
builder.Services.AddScoped<ISqlRepository<ResponseJobApplicationDto, CreateJobApplicationDto, EditJobApplicationDto>, JobApplicationService>();
builder.Services.AddScoped<ISqlRepository<ResponseJobPostDto, CreateJobPostDto, EditJobPostDto>, JobPostService>();
builder.Services.AddScoped<ISqlRepository<ResponseRoleDto, CreateRoleDto, EditRoleDto>, RoleService>();
builder.Services.AddScoped<RoleService>();

builder.Services.AddScoped<CountryService>();
builder.Services.AddScoped<TownService>();
builder.Services.AddScoped<LocationService>();
builder.Services.AddScoped<JobTypeService>();
builder.Services.AddScoped<ContractorService>();
builder.Services.AddScoped<ContractorLocationService>();
builder.Services.AddScoped<JobApplicationService>();
builder.Services.AddScoped<JobPostService>();
builder.Services.AddScoped<LogService>();

builder.Services.AddAutoMapper(typeof(ContractorMappingProfile).Assembly);
builder.Services.AddAutoMapper(typeof(ContractorLocationMappingProfile).Assembly);
builder.Services.AddAutoMapper(typeof(CountryMappingProfile).Assembly);
builder.Services.AddAutoMapper(typeof(JobApplicationMappingProfile).Assembly);
builder.Services.AddAutoMapper(typeof(JobPostMappingProfile).Assembly);
builder.Services.AddAutoMapper(typeof(JobTypeMappingProfile).Assembly);
builder.Services.AddAutoMapper(typeof(LocationMappingProfile).Assembly);
builder.Services.AddAutoMapper(typeof(PersonMappingProfile).Assembly);
builder.Services.AddAutoMapper(typeof(RoleMappingProfile).Assembly);
builder.Services.AddAutoMapper(typeof(TownMappingProfile).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
