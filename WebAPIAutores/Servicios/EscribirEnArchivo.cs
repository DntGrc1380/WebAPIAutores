namespace WebAPIAutores.Servicios
{
    //Servicio que escribe en un archivo cada 5 segundos (a través de IHostedService
    //También se puede hacer mediante una función de Azure
    public class EscribirEnArchivo : IHostedService
    {
        private readonly IWebHostEnvironment env;
        private readonly string nombreArchivo = "Archivo1.txt";
        private Timer timer;

        public EscribirEnArchivo(IWebHostEnvironment env)
        {
            this.env = env;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Escribir("Proceso iniciado");
            timer = new Timer(DoWork,null,TimeSpan.Zero, TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }
        //No siempre se ejecuta porque en situaciones excepcionales puede que no de tiempo de ejecutarse
        //Ej. Si la aplicación se detiene de manera repentina por un error catastrófico
        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer.Dispose();
            Escribir("Proceso detenido");
            return Task.CompletedTask;
        }

        //Método para escribir en el archivo cada x tiempo
        private void DoWork(object state)
        {
            Escribir("Proceso en ejecución: " + DateTime.Now.ToString("dd/MM/yy HH:mm:ss"));

        }

        //Método auxiliar para escribir archivo
        private void Escribir(string mensaje) {
            var ruta = $@"{env.ContentRootPath}\wwwroot\{nombreArchivo}";
            using (StreamWriter writer = new StreamWriter(ruta, append: true))
            {
                writer.WriteLine(mensaje);

            }
        }
    }
}
