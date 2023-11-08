namespace WebAPIAutores.Middlewares
{
    //Para la forma 2 de exponer (llamar) un middleware se debe crear un método de extensión
    //mismo que debe ser estático
    //Forma 2
    public static class LogRespuestaHTTPMiddlewareExtensions {
        public static IApplicationBuilder UseLogRespuestaHTTP(this IApplicationBuilder app) { 
            return app.UseMiddleware<LogRespuestaHTTPMiddleware>();
        }
    }


    public class LogRespuestaHTTPMiddleware
    {
        private readonly RequestDelegate siguiente;
        private readonly ILogger<LogRespuestaHTTPMiddleware> logger;

        public LogRespuestaHTTPMiddleware(RequestDelegate siguiente, 
                        ILogger<LogRespuestaHTTPMiddleware> logger)
        {
            this.siguiente = siguiente;
            this.logger = logger;
        }

        //Es requisito para que la clase sea un middleware, debe tener un método Invoke o InvokeAsync
        //Debe regresar un Task y aceptar como primer parámetro un HttpContent
        public async Task InvokeAsync(HttpContext contexto) {        
                using (var ms = new MemoryStream())
                {
                    var cuerpoOriginalRespuesta = contexto.Response.Body;
                    contexto.Response.Body = ms;

                    //con la siguiente línea permitimos continuar con los demás middlwares
                    await siguiente(contexto);  //<-- lo que venga después de ésta línea
                    //se ejecutará cuando los middlewares posteriores estén devolviendo respuesta.
                    ms.Seek(0, SeekOrigin.Begin);
                    string respuesta = new StreamReader(ms).ReadToEnd();
                    //regresamos el stream para que llegue correctamente al cliente
                    ms.Seek(0, SeekOrigin.Begin);

                    await ms.CopyToAsync(cuerpoOriginalRespuesta);
                    contexto.Response.Body = cuerpoOriginalRespuesta;

                    logger.LogInformation(respuesta);
                }
        }
    }
}
