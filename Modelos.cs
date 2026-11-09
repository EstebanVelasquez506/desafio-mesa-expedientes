namespace MesaPartesDigital
{
    // Trabajador que usa el sistema (quien recepciona documentos)
    public class Usuario
    {
        public string Codigo { get; set; } = "";       // usuario para el login
        public string Nombre { get; set; } = "";       // nombre completo
        public string Contrasena { get; set; } = "";
    }
 
    // Expediente: solo 4 datos
    public class Expediente
    {
        public string Codigo { get; set; } = "";       // Ej: EXP-2026-001
        public string Ciudadano { get; set; } = "";    // Ciudadano / remitente (persona u organización)
        public string Documento { get; set; } = "";    // Documento de identidad: DNI (8) o RUC (11)
        public string Asunto { get; set; } = "";       // Asunto del trámite
    }
}
