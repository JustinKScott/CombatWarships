using Serilog;
using WarshipSearchAPI.Data;
using WarshipSearchAPI.Interfaces;
using WarshipSearchAPI.Middleware;

Log.Logger = new LoggerConfiguration()
	.Enrich.FromLogContext()
	//	//.WriteTo.Seq("http://warship_seq:5341")
	.WriteTo.Console()
	.CreateLogger();


var builder = WebApplication.CreateBuilder(args);

var dbConnection = builder.Configuration.GetConnectionString("dbConnection");
Log.Information($"DB Connection: {dbConnection}");

var seqConnection = builder.Configuration.GetValue<string>("seqConnection");
Log.Information($"SEQ Connection: {seqConnection}");

//// Add services to the container.
//builder.Services.AddScoped<IWarshipDatabase, WarshipDatabase>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//builder.Services.AddLogging(loggingBuilder =>
//{
//	loggingBuilder.AddSerilog(dispose: true);
//});

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandler>();

app.UseCors(options =>
	options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
);

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
