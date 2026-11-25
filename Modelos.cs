using System;

namespace MesaPartesDigital
{
    // Trabajador que usa el sistema (quien recepciona documentos)
    // Es una ESTRUCTURA (tipo por valor) con campos públicos simples:
    // sin constructores y sin propiedades (get/set).
    public struct Usuario
    {
        public string Codigo;       // usuario para el login
        public string Nombre;       // nombre completo
        public string Contraseña;
    }

    // Expediente: solo 4 datos
    public struct Expediente
    {
        public string Codigo;       // Ej: EXP-2026-001
        public string Ciudadano;    // Ciudadano / remitente (persona u organización)
        public string Documento;    // Documento de identidad: DNI (8) o RUC (11)
        public string Asunto;       // Asunto del trámite
    }
}
