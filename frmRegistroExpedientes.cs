using System;
using System.Net.Mail;
using System.Windows.Forms;

namespace MesaPartesDigital
{
    // FORMULARIO 3: REGISTRO DE EXPEDIENTES
    // Recibe los datos, los valida y los guarda en expedientes.txt
    public class frmRegistroExpedientes : Form
    {
        TextBox txtCodigo, txtDocumento, txtNombre, txtTelefono, txtCorreo, txtDireccion, txtAsunto;
        ComboBox cboTramitante, cboTipoDoc;
        NumericUpDown numFolios;
        DateTimePicker dtpFecha;
        Button btnGuardar, btnLimpiar, btnCancelar;

        public frmRegistroExpedientes()
        {
            Text = "Registro de Expedientes";
            Width = 520;
            Height = 560;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            int x1 = 30, x2 = 190, y = 20, salto = 40;

            UI.Etiqueta(this, "Código de expediente:", x1, y + 3);
            txtCodigo = UI.Caja(this, x2, y, 280);
            txtCodigo.ReadOnly = true;          // se genera solo
            y += salto;

            UI.Etiqueta(this, "Tipo de tramitante:", x1, y + 3);
            cboTramitante = UI.Combo(this, x2, y, 280, "Persona", "Organización");
            y += salto;

            UI.Etiqueta(this, "DNI / RUC:", x1, y + 3);
            txtDocumento = UI.Caja(this, x2, y, 280);
            y += salto;

            UI.Etiqueta(this, "Nombres / Razón social:", x1, y + 3);
            txtNombre = UI.Caja(this, x2, y, 280);
            y += salto;

            UI.Etiqueta(this, "Teléfono:", x1, y + 3);
            txtTelefono = UI.Caja(this, x2, y, 280);
            y += salto;

            UI.Etiqueta(this, "Correo electrónico:", x1, y + 3);
            txtCorreo = UI.Caja(this, x2, y, 280);
            y += salto;

            UI.Etiqueta(this, "Dirección:", x1, y + 3);
            txtDireccion = UI.Caja(this, x2, y, 280);
            y += salto;

            UI.Etiqueta(this, "Tipo de documento:", x1, y + 3);
            cboTipoDoc = UI.Combo(this, x2, y, 280, "Solicitud", "Oficio", "Carta", "Informe", "Otro");
            y += salto;

            UI.Etiqueta(this, "Asunto:", x1, y + 3);
            txtAsunto = UI.Caja(this, x2, y, 280);
            y += salto;

            UI.Etiqueta(this, "Número de folios:", x1, y + 3);
            numFolios = new NumericUpDown();
            numFolios.Location = new System.Drawing.Point(x2, y);
            numFolios.Width = 80;
            numFolios.Minimum = 1;
            numFolios.Maximum = 1000;
            Controls.Add(numFolios);
            y += salto;

            UI.Etiqueta(this, "Fecha y hora de recepción:", x1, y + 3);
            dtpFecha = new DateTimePicker();
            dtpFecha.Location = new System.Drawing.Point(x2, y);
            dtpFecha.Width = 280;
            dtpFecha.Format = DateTimePickerFormat.Custom;
            dtpFecha.CustomFormat = "dd/MM/yyyy HH:mm";
            Controls.Add(dtpFecha);
            y += salto + 10;

            btnGuardar = UI.Boton(this, "Guardar", 80, y, 100);
            btnLimpiar = UI.Boton(this, "Limpiar", 200, y, 100);
            btnCancelar = UI.Boton(this, "Cancelar", 320, y, 100);

            btnGuardar.Click += btnGuardar_Click;
            btnLimpiar.Click += (s, e) => LimpiarCampos();
            btnCancelar.Click += (s, e) => Close();

            LimpiarCampos();
        }

        void LimpiarCampos()
        {
            txtCodigo.Text = Datos.GenerarCodigo();
            cboTramitante.SelectedIndex = 0;
            txtDocumento.Clear();
            txtNombre.Clear();
            txtTelefono.Clear();
            txtCorreo.Clear();
            txtDireccion.Clear();
            cboTipoDoc.SelectedIndex = 0;
            txtAsunto.Clear();
            numFolios.Value = 1;
            dtpFecha.Value = DateTime.Now;
            txtDocumento.Focus();
        }

        void btnGuardar_Click(object sender, EventArgs e)
        {
            // ----- VALIDACIONES -----
            string documento = txtDocumento.Text.Trim();
            bool esPersona = cboTramitante.Text == "Persona";

            if (!SoloNumeros(documento))
            {
                Error("El DNI / RUC solo debe contener números.");
                return;
            }
            if (esPersona && documento.Length != 8)
            {
                Error("El DNI debe tener 8 dígitos.");
                return;
            }
            if (!esPersona && documento.Length != 11)
            {
                Error("El RUC debe tener 11 dígitos.");
                return;
            }
            if (txtNombre.Text.Trim() == "")
            {
                Error("Ingrese los nombres o la razón social.");
                return;
            }
            if (!SoloNumeros(txtTelefono.Text.Trim()) || txtTelefono.Text.Trim().Length != 9)
            {
                Error("El teléfono debe tener 9 dígitos.");
                return;
            }
            if (!CorreoValido(txtCorreo.Text.Trim()))
            {
                Error("El correo electrónico no es válido.");
                return;
            }
            if (txtDireccion.Text.Trim() == "")
            {
                Error("Ingrese la dirección.");
                return;
            }
            if (txtAsunto.Text.Trim() == "")
            {
                Error("Ingrese el asunto del documento.");
                return;
            }
            if (Datos.CodigoExiste(txtCodigo.Text))
            {
                Error("El código de expediente ya existe.");
                return;
            }

            // ----- GUARDAR -----
            Expediente exp = new Expediente();
            exp.Codigo = txtCodigo.Text;
            exp.TipoTramitante = cboTramitante.Text;
            exp.Documento = documento;
            exp.Nombre = txtNombre.Text.Trim();
            exp.Telefono = txtTelefono.Text.Trim();
            exp.Correo = txtCorreo.Text.Trim();
            exp.Direccion = txtDireccion.Text.Trim();
            exp.TipoDocumento = cboTipoDoc.Text;
            exp.Asunto = txtAsunto.Text.Trim();
            exp.Folios = (int)numFolios.Value;
            exp.FechaHora = dtpFecha.Value;

            Datos.Expedientes.Add(exp);
            Datos.GuardarExpedientes();

            MessageBox.Show("Expediente registrado correctamente.\nCódigo: " + exp.Codigo,
                "Registro exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LimpiarCampos();
        }

        // ----- Funciones de validación -----
        bool SoloNumeros(string texto)
        {
            if (texto == "") return false;
            foreach (char c in texto)
            {
                if (!char.IsDigit(c)) return false;
            }
            return true;
        }

        bool CorreoValido(string correo)
        {
            try
            {
                MailAddress m = new MailAddress(correo);
                return m.Address == correo;
            }
            catch
            {
                return false;
            }
        }

        void Error(string mensaje)
        {
            MessageBox.Show(mensaje, "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
