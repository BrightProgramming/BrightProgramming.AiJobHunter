using BrightProgramming.AiJobHunter.Api.StartupConfiguration;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddValidation();
builder.Services.AddApplication();
builder.Services.AddPostgreSql(builder.Configuration);

var app = builder.Build();

DatabaseConfiguration.ApplyPostgreSqlMigrations(builder.Configuration);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

public partial class Program { }
