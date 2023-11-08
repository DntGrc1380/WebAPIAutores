using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebAPIAutores.Filtros;
using WebAPIAutores.Middlewares;
using WebAPIAutores.Servicios;

namespace WebAPIAutores
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get;}

        public void ConfigureServices(IServiceCollection services) {
            // Add services to the container.
            //En la siguiente línea se realiza la configuración
            //para evitar ciclo infinito en propiedades de navegación de clases.
            //services.AddControllers(); //<---Original
            //Antes de usar Filtro global de excepciones
            //services.AddControllers().AddJsonOptions(x => 
            //    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            //Usando Filtro global de excepciones
            services.AddControllers(o => {
                o.Filters.Add(typeof(FiltroDeExcepcion));
            }).AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            //Configurar uso de SQL server
            services.AddDbContext<ApplicationDbContext>(o => 
                o.UseSqlServer(Configuration.GetConnectionString("defaultConn")));

            //Configurar Filtros: MiFiltroDeAccion
            services.AddTransient<MiFiltroDeAccion>();

            //Configurar servicio global (escribir archivo)
            services.AddHostedService<EscribirEnArchivo>();

            //configuración para usar cahcé en la aplicación
            services.AddResponseCaching();

            //configuración para utilizar autenticación
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        // Configure the HTTP request pipeline.
        //se pasa (inyecta el logger configurado en la clase program.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger) {
            //Aquí se configuran los middlewares, todos los middlewares llevan la sentencia Use
            //También se puede crear middlewares personalizados.

            //La siguientes líneas de código se llevarán al archivo LogRespuestaHTTPMiddleware (42 a la 59)

            ////No se utiliza Run para que los demás middlewares se ejecuten
            ////app.Use(async (contexto, siguiente) =>
            ////{
            ////    using (var ms = new MemoryStream())
            ////    {
            ////        var cuerpoOriginalRespuesta = contexto.Response.Body;
            ////        contexto.Response.Body = ms;

            ////        con la siguiente línea permitimos continuar con los demás middlwares
            ////        await siguiente.Invoke();  //<-- lo que venga después de ésta línea
            ////        se ejecutará cuando los middlewares posteriores estén devolviendo respuesta.
            ////        ms.Seek(0, SeekOrigin.Begin);
            ////        string respuesta = new StreamReader(ms).ReadToEnd();
            ////        regresamos el stream para que llegue correctamente al cliente
            ////        ms.Seek(0, SeekOrigin.Begin);

            ////        await ms.CopyToAsync(cuerpoOriginalRespuesta);
            ////        contexto.Response.Body = cuerpoOriginalRespuesta;

            ////        logger.LogInformation(respuesta);
            ////    }
            ////});
            ///

            //Forma 1
            //app.UseMiddleware<LogRespuestaHTTPMiddleware>();

            //Forma 2 (Mejor forma)
            app.UseLogRespuestaHTTP();

            //Condicionar middleware por ruta
            app.Map("/ruta1", app =>
            {
                //Con app.Run se ejecuta una ación y se corta la acción de los siguientes middlewares
                app.Run(async contexto =>
                {
                    await contexto.Response.WriteAsync("Interceptando tubería");
                });
            });

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //Filtro de caché, viene por defecto en .Net Core
            app.UseResponseCaching();

            //autorization debe ir antes de endPoints para poder asegurarlos
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
