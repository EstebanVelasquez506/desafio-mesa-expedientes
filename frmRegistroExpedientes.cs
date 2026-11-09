using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
 
namespace MesaPartesDigital
{
    // FORMULARIO 3: REGISTRO DE EXPEDIENTES
    // Solo pide 4 datos: código, ciudadano/remitente, documento de identidad y asunto.
    // Los valida y los guarda en expedientes.txt
    public class frmRegistroExpedientes : Form
    {
        TextBox txtCodigo, txtCiudadano, txtDocumento, txtAsunto;
        Button btnGuardar, btnLimpiar, btnCancelar;
 
        public frmRegistroExpedientes()
        {
            Text = "Registro de Expedientes";
            ClientSize = new Size(560, 465);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = UI.Fondo;
            Font = UI.Fuente;
 
            UI.Encabezado(this, "Nuevo expediente", "Complete los 4 datos para registrar el trámite");
 
            // ----- Tarjeta con los 4 campos -----
            Panel card = UI.Tarjeta(this, "Datos del expediente", 20, 85, 520, 305);
 
            UI.EtiquetaCampo(card, "Código (ejemplo: EXP-2026-001)", 20, 45);
            txtCodigo = UI.Caja(card, 20, 67, 480);
            txtCodigo.MaxLength = 12;
            txtCodigo.CharacterCasing = CharacterCasing.Upper;   // escribe en mayúsculas
 
            UI.EtiquetaCampo(card, "Ciudadano / remitente", 20, 110);
            txtCiudadano = UI.Caja(card, 20, 132, 480);
            txtCiudadano.MaxLength = 100;
 
            UI.EtiquetaCampo(card, "Documento de identidad (DNI de 8 dígitos o RUC de 11)", 20, 175);
            txtDocumento = UI.Caja(card, 20, 197, 480);
            txtDocumento.MaxLength = 11;
 
            UI.EtiquetaCampo(card, "Asunto del trámite", 20, 240);
            txtAsunto = UI.Caja(card, 20, 262, 480);
            txtAsunto.MaxLength = 200;
 
            // ----- Botones -----
            btnGuardar = UI.Boton(this, "Validar y guardar", 20, 405, 190);
            btnLimpiar = UI.Boton(this, "Limpiar campos", 220, 405, 150);
            btnCancelar = UI.Boton(this, "Cancelar", 380, 405, 120);
 
            btnGuardar.Height = 38;
            btnLimpiar.Height = 38;
            btnCancelar.Height = 38;
            UI.EstiloBoton(btnGuardar, true);
            UI.EstiloBoton(btnLimpiar, false);
            UI.EstiloBoton(btnCancelar, false);
 
            // ----- Eventos -----
            txtDocumento.KeyPress += SoloDigitos;
            btnGuardar.Click += btnGuardar_Click;
            btnLimpiar.Click += (s, e) => LimpiarCampos();
            btnCancelar.Click += (s, e) => Close();
 
            AcceptButton = btnGuardar;      // Enter = guardar
            CancelButton = btnCancelar;     // Esc = cerrar
        }
 
        // Solo deja escribir números en el documento de identidad
        void SoloDigitos(object? sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
 
        void LimpiarCampos()
        {
            txtCodigo.Clear();
            txtCiudadano.Clear();
            txtDocumento.Clear();
            txtAsunto.Clear();
            txtCodigo.Focus();
        }
 
        void btnGuardar_Click(object? sender, EventArgs e)
        {
            string codigo = txtCodigo.Text.Trim();
            string ciudadano = txtCiudadano.Text.Trim();
            string documento = txtDocumento.Text.Trim();
            string asunto = txtAsunto.Text.Trim();
 
            // ----- VALIDACIONES -----
 
            // 1. Código con formato EXP-AAAA-NNN (año de 4 dígitos y número de 3 dígitos)
            if (!Regex.IsMatch(codigo, @"^EXP-[0-9]{4}-[0-9]{3}$"))
            {
                Error("El código debe tener el formato EXP-2026-001\n(EXP, año de 4 dígitos y número de 3 dígitos).", txtCodigo);
                return;
            }
 
            // 2. Código no repetido
            if (Datos.CodigoExiste(codigo))
            {
                Error("Ya existe un expediente con el código " + codigo + ".", txtCodigo);
                return;
            }
 
            // 3. Ciudadano / remitente
            if (ciudadano.Length < 3)
            {
                Error("Ingrese el nombre del ciudadano o la razón social (mínimo 3 caracteres).", txtCiudadano);
                return;
            }
 
            // 4. Documento: DNI (8 dígitos) o RUC (11 dígitos), solo números
            if (!SoloNumeros(documento) || (documento.Length != 8 && documento.Length != 11))
            {
                Error("El documento debe ser un DNI de 8 dígitos o un RUC de 11 dígitos.", txtDocumento);
                return;
            }
 
            // 5. Asunto
            if (asunto == "")
            {
                Error("Ingrese el asunto del trámite.", txtAsunto);
                return;
            }
 
            // ----- GUARDAR -----
            Expediente exp = new Expediente();
            exp.Codigo = codigo;
            exp.Ciudadano = ciudadano;
            exp.Documento = documento;
            exp.Asunto = asunto;
 
            Datos.Expedientes.Add(exp);
            Datos.GuardarExpedientes();
 
            MessageBox.Show("Expediente registrado correctamente.\nCódigo: " + exp.Codigo,
                "Registro exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
 
            LimpiarCampos();
        }
 
        bool SoloNumeros(string texto)
        {
            if (texto == "") return false;
            foreach (char c in texto)
            {
                if (!char.IsDigit(c)) return false;
            }
            return true;
        }
 
        // Muestra el error y lleva el cursor al campo que falló
        void Error(string mensaje, Control campo)
        {
            MessageBox.Show(mensaje, "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            campo.Focus();
        }
    }
}
