using BL.Dtos;
using BL.Services;
using BL.Services.Repo;
using Microsoft.EntityFrameworkCore;
using WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BL.Models.DatabaseContext>(options => {
    options.UseSqlServer("Name=ConnectionStrings:DatabaseConnStr");
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

builder.Services.AddAutoMapper(typeof(BL.AutoMapper.ContractorMappingProfile).Assembly);
builder.Services.AddAutoMapper(typeof(BL.AutoMapper.ContractorLocationMappingProfile).Assembly);
builder.Services.AddAutoMapper(typeof(BL.AutoMapper.CountryMappingProfile).Assembly);
builder.Services.AddAutoMapper(typeof(BL.AutoMapper.JobApplicationMappingProfile).Assembly);
builder.Services.AddAutoMapper(typeof(BL.AutoMapper.JobPostMappingProfile).Assembly);
builder.Services.AddAutoMapper(typeof(BL.AutoMapper.JobTypeMappingProfile).Assembly);
builder.Services.AddAutoMapper(typeof(BL.AutoMapper.LocationMappingProfile).Assembly);
builder.Services.AddAutoMapper(typeof(BL.AutoMapper.PersonMappingProfile).Assembly);
builder.Services.AddAutoMapper(typeof(BL.AutoMapper.RoleMappingProfile).Assembly);
builder.Services.AddAutoMapper(typeof(BL.AutoMapper.TownMappingProfile).Assembly);

//jos treba navesti mapping profile od mapiranja iz dto u vm    !!!!!
builder.Services.AddAutoMapper(typeof(WebApp.AutoMapper.ContractorMappingProfile).Assembly);
builder.Services.AddAutoMapper(typeof(WebApp.AutoMapper.LocationMappingProfile).Assembly);
builder.Services.AddAutoMapper(typeof(WebApp.AutoMapper.PersonMappingProfile).Assembly);



builder.Services.AddAuthentication()
  .AddCookie(options =>
  {
      options.LoginPath = "/User/Login";
      options.LogoutPath = "/User/Logout";
      options.AccessDeniedPath = "/User/Forbidden";
      options.SlidingExpiration = true;
      options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
  });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/User/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}");

app.Run();