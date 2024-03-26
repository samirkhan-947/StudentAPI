using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using StudentAdminPortal.API.DataModel;
using StudentAdminPortal.API.Profiles;
using StudentAdminPortal.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<StudentAdminContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddDbContext<StudentAdminContext>(options => options.UseMySql(connection,ServerVersion.AutoDetect(connection)));
builder.Services.AddScoped<IStudentRepostory, SqlStudentRepository>();
builder.Services.AddScoped<IImageRepository, LocalStorageImageRepository>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddCors((options) =>
{
    options.AddPolicy("angularAppliaction", (builder) =>
    {
        builder.WithOrigins("http://localhost:4200")
        .AllowAnyHeader().WithMethods("GET","POST","PUT","DELETE").WithExposedHeaders("*");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, "Resouces")),
    RequestPath = "/Resouces"
});
app.UseRouting();
app.UseCors("angularAppliaction");
app.UseAuthorization();

app.MapControllers();

app.Run();
