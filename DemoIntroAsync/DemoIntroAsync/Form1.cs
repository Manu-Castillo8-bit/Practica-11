using System.Diagnostics;

namespace DemoIntroAsync
{
    public partial class Form1 : Form
    {
        HttpClient cliente = new HttpClient();
        public Form1()
        {
            InitializeComponent();
            cliente.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
        }

        //Peligroso: async void debe ser evitado, EXCEPTO en eventos.
        private async void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;

            var directorioActual = AppDomain.CurrentDomain.BaseDirectory;
            var destinoBaseSecuencial = Path.Combine(directorioActual, @"Imagenes\resultado-secuencial");
            var destinoBaseParalelo = Path.Combine(directorioActual, @"Imagenes\resultado-paralelo");
            PrepararEjecución(destinoBaseParalelo, destinoBaseSecuencial);


            Console.WriteLine("Inicio");
            List<Imagen> imagenes = Obtenerimagenes();

            



            //Proceso lento
            /*Thread.Sleep(5000); //Es sincrono porque devuelve VOID al pasar el cursor sobre él
           
            await Task.Delay(5000); //Método asincrono porque al pasar el cursor sobre él devuelve TASK */

            var sw = new Stopwatch();
            sw.Start();
            
            foreach(var imagen in imagenes)
            {
                await ProcesarImagen(destinoBaseSecuencial, imagen);
            }

            Console.WriteLine("Secuencial - duración en segundos: {0}", 
                sw.ElapsedMilliseconds/1000.0);

            sw.Reset();
            sw.Start();

            var tareasEnumerable = imagenes.Select(async imagen =>
            {
                await ProcesarImagen(destinoBaseParalelo, imagen);
            });

            await Task.WhenAll(tareasEnumerable);

            Console.WriteLine("Paralelo - duración en segundos: {0}", 
                sw.ElapsedMilliseconds/1000.0);




            sw.Stop();


          
        
            pictureBox1.Visible = false;
        }

        private async Task ProcesarImagen(string directorio, Imagen imagen)
        {
            var respueta = await cliente.GetAsync(imagen.URL);
            var contenido = await respueta.Content.ReadAsByteArrayAsync();
            Bitmap bitmap;
            using(var ms = new MemoryStream(contenido))
            {
                bitmap = new Bitmap(ms);
            }
            bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
            var destino = Path.Combine(directorio, imagen.Nombre);
            bitmap.Save(destino);
        }
   private static List<Imagen> Obtenerimagenes()
        {
            var imagenes = new List<Imagen>();
            for (int i=0; i<7; i++)
            {
                imagenes.Add(
                    new Imagen()
                    {
                        Nombre = $"Cacicazgos {i}.png",
                        URL = "https://picsum.photos/200/300"
                    });
                imagenes.Add(
                    new Imagen()
                    {
                        Nombre = $"Desangles {i}.png",
                        URL = "https://picsum.photos/200/300"
                    });
                imagenes.Add(
                    new Imagen()
                    {
                        Nombre = $"Cacicazgos {i}.png",
                        URL = "https://picsum.photos/200/300"
                    });
            }
            return imagenes;
            
        }


        private void BorrarArchivos(string directorio)
        {
            var archivos = Directory.EnumerateFiles (directorio); 

foreach  (var archivo in archivos)
                {
                File.Delete(archivo);
            }
        }

        private void PrepararEjecución(string destinoBaseParalelo, string destinoBaseSecuencial)
        {
            if(!Directory.Exists(destinoBaseParalelo))
            {
                Directory.CreateDirectory(destinoBaseParalelo);
            }
           if(!Directory.Exists(destinoBaseSecuencial))
            {
                Directory.CreateDirectory(destinoBaseSecuencial);
            }
           BorrarArchivos(destinoBaseSecuencial);
            BorrarArchivos(destinoBaseParalelo);
        }



    private async Task<string> ProcesamientoLargo()
        {
            await Task.Delay(3000); //Asincrono
            return "Felipe";
        }

        private async Task RealizarProcesamientoLargoA()
        {
            await Task.Delay(1000); //Asincrono
            Console.WriteLine("Proseso A finalizado");

        }

        private async Task RealizarProcesamientoLargoB()
        {
            await Task.Delay(1000); //Asincrono
            Console.WriteLine("Proseso B finalizado");

        }

        private async Task RealizarProcesamientoLargoC()
        {
            await Task.Delay(1000); //Asincrono
            Console.WriteLine("Proseso C finalizado");

        }

    }

}
