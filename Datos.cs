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
                        Usuario u = new Usuario();
                        u.Codigo = br.ReadString();
                        u.Nombre = br.ReadString();
                        u.Contraseña = br.ReadString();
                        Usuarios.Add(u);
                    }
                }
                return;
            }

            // Si no existe el archivo, se crea un usuario inicial: admin / admin123
            Usuario admin = new Usuario();
            admin.Codigo = "admin";
            admin.Nombre = "Administrador";
            admin.Contraseña = "admin123";
            Usuarios.Add(admin);
            GuardarUsuarios();
        }

        static void CargarExpedientes()
        {
            Expedientes.Clear();

            if (!File.Exists(rutaExpedientes)) return;

            using (BinaryReader br = new BinaryReader(File.Open(rutaExpedientes, FileMode.Open, FileAccess.Read)))
            {
                int cantidad = br.ReadInt32();
                for (int i = 0; i < cantidad; i++)
                {
                    Expediente exp = new Expediente();
                    exp.Codigo = br.ReadString();
                    exp.Ciudadano = br.ReadString();
                    exp.Documento = br.ReadString();
                    exp.Asunto = br.ReadString();
                    Expedientes.Add(exp);
                }
            }
        }

        // ---------- GUARDAR ----------
        public static void GuardarUsuarios()
        {
            using (BinaryWriter bw = new BinaryWriter(File.Open(rutaUsuarios, FileMode.Create, FileAccess.Write)))
            {
                bw.Write(Usuarios.Count);
                foreach (Usuario u in Usuarios)
                {
                    bw.Write(u.Codigo);
                    bw.Write(u.Nombre);
                    bw.Write(u.Contraseña);
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
                    bw.Write(e.Codigo);
                    bw.Write(e.Ciudadano);
                    bw.Write(e.Documento);
                    bw.Write(e.Asunto);
                }
            }
        }

        // ---------- UTILIDADES ----------
        // El administrador es el usuario "admin" (el que se crea automáticamente al inicio)
        public static bool EsAdministrador(Usuario u)
        {
            return string.Equals(u.Codigo, "admin", StringComparison.OrdinalIgnoreCase);
        }

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
            for (int i = 0; i < Usuarios.Count; i++)
            {
                if (string.Equals(Usuarios[i].Codigo, codigo, StringComparison.OrdinalIgnoreCase))
                {
                    Usuarios.RemoveAt(i);
                    break;
                }
            }
            GuardarUsuarios();
        }

        public static void EliminarExpediente(string codigo)
        {
            for (int i = 0; i < Expedientes.Count; i++)
            {
                if (string.Equals(Expedientes[i].Codigo, codigo, StringComparison.OrdinalIgnoreCase))
                {
                    Expedientes.RemoveAt(i);
                    break;
                }
            }
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
