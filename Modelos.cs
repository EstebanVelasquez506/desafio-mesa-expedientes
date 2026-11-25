using System;

namespace MesaPartesDigital
{
    // Trabajador que usa el sistema (quien recepciona documentos)
    // Es una ESTRUCTURA (tipo por valor): al asignarla o pasarla a un método se copia.
    // Se usan propiedades (no campos) porque el DataGridView solo enlaza propiedades.
    public struct Usuario
    {
        public string Codigo { get; set; }       // usuario para el login
        public string Nombre { get; set; }       // nombre completo
        public string Contraseña { get; set; }

        public Usuario(string codigo, string nombre, string contrasena)
        {
            Codigo = codigo;
            Nombre = nombre;
            Contraseña = contrasena;
        }

        // El administrador es el usuario "admin" (el que se crea automáticamente al inicio)
        // string.Equals admite null (una estructura "vacía" tiene los textos en null)
        public bool EsAdministrador
        {
            get { return string.Equals(Codigo, "admin", StringComparison.OrdinalIgnoreCase); }
        }
    }

    // Expediente: solo 4 datos
    public struct Expediente
    {
        public string Codigo { get; set; }       // Ej: EXP-2026-001
        public string Ciudadano { get; set; }    // Ciudadano / remitente (persona u organización)
        public string Documento { get; set; }    // Documento de identidad: DNI (8) o RUC (11)
        public string Asunto { get; set; }       // Asunto del trámite

        public Expediente(string codigo, string ciudadano, string documento, string asunto)
        {
            Codigo = codigo;
            Ciudadano = ciudadano;
            Documento = documento;
            Asunto = asunto;
        }
    }
}
