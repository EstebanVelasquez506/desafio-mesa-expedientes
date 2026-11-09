using System;
using System.Collections.Generic;
using System.IO;
 
namespace MesaPartesDigital
{
    // Guarda y carga la información en archivos de texto (.txt)
    // Cada línea es un registro y los campos se separan con el símbolo |
    public static class Datos
    {
        static string carpeta = AppDomain.CurrentDomain.BaseDirectory;
        static string rutaUsuarios = Path.Combine(carpeta, "usuarios.txt");
        static string rutaExpedientes = Path.Combine(carpeta, "expedientes.txt");
 
        public static List<Usuario> Usuarios = new List<Usuario>();
        public static List<Expediente> Expedientes = new List<Expediente>();
 
        // ---------- CARGAR ----------
        public static void Cargar()
        {
            CargarUsuarios();
            CargarExpedientes();
        }
 
        static void CargarUsuarios()
        {
            Usuarios.Clear();
 
            // Si no existe el archivo, se crea un usuario inicial: admin / admin123
            if (!File.Exists(rutaUsuarios))
            {
                Usuarios.Add(new Usuario { Codigo = "admin", Nombre = "Administrador", Contrasena = "admin123" });
                GuardarUsuarios();
                return;
            }
 
            foreach (string linea in File.ReadAllLines(rutaUsuarios))
            {
                string[] c = linea.Split('|');
                if (c.Length >= 3)
                {
                    Usuarios.Add(new Usuario { Codigo = c[0], Nombre = c[1], Contrasena = c[2] });
                }
            }
        }
 
        static void CargarExpedientes()
        {
            Expedientes.Clear();
            if (!File.Exists(rutaExpedientes)) return;
 
            foreach (string linea in File.ReadAllLines(rutaExpedientes))
            {
                string[] c = linea.Split('|');
 
                if (c.Length == 4)
                {
                    // Formato actual: Codigo|Ciudadano|Documento|Asunto
                    Expedientes.Add(new Expediente
                    {
                        Codigo = c[0],
                        Ciudadano = c[1],
                        Documento = c[2],
                        Asunto = c[3]
                    });
                }
                else if (c.Length >= 11)
                {
                    // Formato anterior (11 campos): se conservan solo los 4 datos que usa ahora el sistema
                    Expedientes.Add(new Expediente
                    {
                        Codigo = c[0],
                        Ciudadano = c[3],
                        Documento = c[2],
                        Asunto = c[8]
                    });
                }
            }
        }
 
        // ---------- GUARDAR ----------
        public static void GuardarUsuarios()
        {
            List<string> lineas = new List<string>();
            foreach (Usuario u in Usuarios)
            {
                lineas.Add(Limpiar(u.Codigo) + "|" + Limpiar(u.Nombre) + "|" + Limpiar(u.Contrasena));
            }
            File.WriteAllLines(rutaUsuarios, lineas);
        }
 
        public static void GuardarExpedientes()
        {
            List<string> lineas = new List<string>();
            foreach (Expediente e in Expedientes)
            {
                lineas.Add(Limpiar(e.Codigo) + "|" + Limpiar(e.Ciudadano) + "|" +
                           Limpiar(e.Documento) + "|" + Limpiar(e.Asunto));
            }
            File.WriteAllLines(rutaExpedientes, lineas);
        }
 
        // Evita que un texto escrito por el usuario rompa el formato del archivo
        static string Limpiar(string texto)
        {
            if (texto == null) return "";
            return texto.Replace("|", "/").Replace("\r", " ").Replace("\n", " ");
        }
 
        // ---------- UTILIDADES ----------
        public static bool CodigoExiste(string codigo)
        {
            foreach (Expediente e in Expedientes)
            {
                if (e.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase)) return true;
            }
            return false;
        }
 
        // ---------- ALGORITMO DE ORDENAMIENTO BURBUJA ----------
        // porNombre = true  -> ordena por ciudadano / remitente
        // porNombre = false -> ordena por código de expediente
        public static void OrdenarBurbuja(List<Expediente> lista, bool porNombre)
        {
            int n = lista.Count;
 
            for (int i = 0; i < n - 1; i++)
            {
                bool huboCambio = false;
 
                for (int j = 0; j < n - i - 1; j++)
                {
                    string a = porNombre ? lista[j].Ciudadano : lista[j].Codigo;
                    string b = porNombre ? lista[j + 1].Ciudadano : lista[j + 1].Codigo;
 
                    // Si el de la izquierda es mayor, se intercambian
                    if (string.Compare(a, b, StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        Expediente temp = lista[j];
                        lista[j] = lista[j + 1];
                        lista[j + 1] = temp;
                        huboCambio = true;
                    }
                }
 
                // Si en una vuelta no hubo cambios, ya está ordenado
                if (!huboCambio) break;
            }
        }
    }
}
