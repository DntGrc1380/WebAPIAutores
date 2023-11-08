using WebAPIAutores;

var builder = WebApplication.CreateBuilder(args);

//Las siguientes 2 líneas son para configurar los servicios desde la clase Startup
var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();

//Configurar servico de logger para poder usar en la clase program
var servicioLogger = (ILogger<Startup>)app.Services.GetService(typeof(ILogger<Startup>));

//La siguiente línea es para configurar en env desde la clase Startup
//también se pasa (inyecta el servicioLogger
startup.Configure(app, app.Environment, servicioLogger);

app.Run();
