using System;
using System.Windows.Forms;
 
namespace MesaPartesDigital
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
 
            // Carga los datos guardados en los archivos .txt
            Datos.Cargar();
 
            // El programa inicia con el formulario de Login
            Application.Run(new frmLogin());
        }
    }
}
