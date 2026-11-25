using System;
using System.Collections.Generic;
using System.IO;

namespace MesaPartesDigital
{
    // Guarda y carga la información en archivos BINARIOS (.dat)
    // Formato: [cantidad de registros: int] seguido de cada registro (cada registro es una
    // ESTRUCTURA), donde cada campo de texto se guarda con BinaryWriter.Write(string)
    public static class Datos
    {
        static string carpeta = AppDomain.CurrentDomain.BaseDirectory;
        static string rutaUsuarios = Path.Combine(carpeta, "usuarios.dat");
        static string rutaExpedientes = Path.Combine(carpeta, "expedientes.dat");

        // Archivos de texto de versiones anteriores (solo se leen una vez para migrar los datos)
        static string rutaUsuariosTxt = Path.Combine(carpeta, "usuarios.txt");
        static string rutaExpedientesTxt = Path.Combine(carpeta, "expedientes.txt");

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

            if (File.Exists(rutaUsuarios))
            {
                using (BinaryReader br = new BinaryReader(File.Open(rutaUsuarios, FileMode.Open, FileAccess.Read)))
                {
                    int cantidad = br.ReadInt32();
                    for (int i = 0; i < cantidad; i++)
                    {
                        // Los argumentos se evalúan en orden: Codigo, Nombre, Contrasena
                        Usuarios.Add(new Usuario(br.ReadString(), br.ReadString(), br.ReadString()));
                    }
                }
                return;
            }

            // Migración: si existe el usuarios.txt antiguo, se importa y se guarda en binario
            if (File.Exists(rutaUsuariosTxt))
            {
                foreach (string linea in File.ReadAllLines(rutaUsuariosTxt))
                {
                    string[] c = linea.Split('|');
                    if (c.Length >= 3)
                    {
                        Usuarios.Add(new Usuario(c[0], c[1], c[2]));
                    }
                }
            }

            // Si no había nada que migrar, se crea un usuario inicial: admin / admin123
            if (Usuarios.Count == 0)
            {
                Usuarios.Add(new Usuario("admin", "Administrador", "admin123"));
            }
            GuardarUsuarios();
        }

        static void CargarExpedientes()
        {
            Expedientes.Clear();

            if (File.Exists(rutaExpedientes))
            {
                using (BinaryReader br = new BinaryReader(File.Open(rutaExpedientes, FileMode.Open, FileAccess.Read)))
                {
                    int cantidad = br.ReadInt32();
                    for (int i = 0; i < cantidad; i++)
                    {
                        // Los argumentos se evalúan en orden: Codigo, Ciudadano, Documento, Asunto
                        Expedientes.Add(new Expediente(br.ReadString(), br.ReadString(), br.ReadString(), br.ReadString()));
                    }
                }
                return;
            }

            // Migración: si existe el expedientes.txt antiguo, se importa y se guarda en binario
            if (!File.Exists(rutaExpedientesTxt)) return;

            foreach (string linea in File.ReadAllLines(rutaExpedientesTxt))
            {
                string[] c = linea.Split('|');

                if (c.Length == 4)
                {
                    // Formato: Codigo|Ciudadano|Documento|Asunto
                    Expedientes.Add(new Expediente(c[0], c[1], c[2], c[3]));
                }
                else if (c.Length >= 11)
                {
                    // Formato anterior (11 campos): se conservan solo los 4 datos que usa ahora el sistema
                    Expedientes.Add(new Expediente(c[0], c[3], c[2], c[8]));
                }
            }
            GuardarExpedientes();
        }

        // ---------- GUARDAR ----------
        public static void GuardarUsuarios()
        {
            using (BinaryWriter bw = new BinaryWriter(File.Open(rutaUsuarios, FileMode.Create, FileAccess.Write)))
            {
                bw.Write(Usuarios.Count);
                foreach (Usuario u in Usuarios)
                {
                    bw.Write(u.Codigo ?? "");
                    bw.Write(u.Nombre ?? "");
                    bw.Write(u.Contraseña ?? "");
                }
            }
        }

        public static void GuardarExpedientes()
        {
            using (BinaryWriter bw = new BinaryWriter(File.Open(rutaExpedientes, FileMode.Create, FileAccess.Write)))
            {
                bw.Write(Expedientes.Count);
                foreach (Expediente e in Expedientes)
                {
                    bw.Write(e.Codigo ?? "");
                    bw.Write(e.Ciudadano ?? "");
                    bw.Write(e.Documento ?? "");
                    bw.Write(e.Asunto ?? "");
                }
            }
        }

        // ---------- UTILIDADES ----------
        public static bool CodigoExiste(string codigo)
        {
            foreach (Expediente e in Expedientes)
            {
                if (string.Equals(e.Codigo, codigo, StringComparison.OrdinalIgnoreCase)) return true;
            }
            return false;
        }

        // Con estructuras se elimina por CÓDIGO (la clave), no por referencia al objeto
        public static void EliminarUsuario(string codigo)
        {
            Usuarios.RemoveAll(u => string.Equals(u.Codigo, codigo, StringComparison.OrdinalIgnoreCase));
            GuardarUsuarios();
        }

        public static void EliminarExpediente(string codigo)
        {
            Expedientes.RemoveAll(e => string.Equals(e.Codigo, codigo, StringComparison.OrdinalIgnoreCase));
            GuardarExpedientes();
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
