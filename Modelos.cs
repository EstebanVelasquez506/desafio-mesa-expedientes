using System;

namespace MesaPartesDigital
{
    // Trabajador que usa el sistema (quien recepciona documentos)
    public class Usuario
    {
        public string Codigo { get; set; } = "";       // usuario para el login
        public string Nombre { get; set; } = "";       // nombre completo
        public string Contrasena { get; set; } = "";
    }

    // Expediente = datos del ciudadano/organización + datos del documento
    public class Expediente
    {
        public string Codigo { get; set; } = "";            // Ej: EXP-0001
        public string TipoTramitante { get; set; } = "";    // Persona / Organización
        public string Documento { get; set; } = "";         // DNI (8) o RUC (11)
        public string Nombre { get; set; } = "";            // Nombres o Razón Social
        public string Telefono { get; set; } = "";
        public string Correo { get; set; } = "";
        public string Direccion { get; set; } = "";
        public string TipoDocumento { get; set; } = "";     // Oficio, Solicitud, Carta...
        public string Asunto { get; set; } = "";
        public int Folios { get; set; }
        public DateTime FechaHora { get; set; }
    }
}