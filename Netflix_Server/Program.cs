using Microsoft.EntityFrameworkCore;
using Netflix_Server.IRepository;
using Netflix_Server.Models.Context;
using Netflix_Server.Models.MovieGroup;
using Netflix_Server.Models.UserGroup;
using Netflix_Server.Repository.MovieGroup;
using Netflix_Server.Repository.UserGroup;
using Netflix_Server.Services.PasswordGroup;
using Netflix_Server.Services.UserGroup;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.




string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<IRepository<Actor>, ActorRepository>();
builder.Services.AddScoped<IRepository<ActorImage>, ActorImageRepository>();
builder.Services.AddScoped<IRepository<Genre>, GenreRepository>();
builder.Services.AddScoped<IRepository<Movie>, MovieRepository>();



builder.Services.AddScoped<IRepository<MovieImage>, MovieImageRepository>();
builder.Services.AddScoped<IRepository<MovieStatus>, MovieStatusRepository>();
builder.Services.AddScoped<IRepository<Playback>, PlaybackRepository>();

builder.Services.AddScoped<IRepository<Feature>, FeatureRepository>();
builder.Services.AddScoped<IRepository<PricingPlan>, PricingPlanRepository>();
builder.Services.AddScoped<IRepository<User>, UserRepository>();

builder.Services.AddScoped<IUserAuthentication, UserAuthentication>();
builder.Services.AddScoped<IPasswordHashing, PasswordHashing>();


// ��������� �������� ApplicationContext � �������� ������� � ����������
builder.Services.AddDbContext<MovieContext>(options => options.UseSqlServer(connection));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseCors(builder => builder.WithOrigins("https://localhost:7247")
                    .AllowAnyHeader().AllowAnyMethod());


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
