using System;
using System.Drawing;
using System.Windows.Forms;

namespace MesaPartesDigital
{
    // FORMULARIO 5: REGISTRO DE PERSONAL
    // Registra a los trabajadores que podrán entrar al sistema
    public class frmRegistroPersonal : Form
    {
        TextBox txtCodigo, txtNombre, txtContrasena, txtConfirmar;
        Button btnGuardar, btnLimpiar, btnCancelar;

        public frmRegistroPersonal()
        {
            Text = "Registro de Personal";
            ClientSize = new Size(460, 460);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = UI.Fondo;
            Font = UI.Fuente;

            UI.Encabezado(this, "Registro de Personal", "Trabajadores que pueden ingresar al sistema");

            Panel card = UI.Tarjeta(this, "Datos del trabajador", 20, 85, 420, 305);

            UI.EtiquetaCampo(card, "Usuario (código)", 20, 45);
            txtCodigo = UI.Caja(card, 20, 67, 380);

            UI.EtiquetaCampo(card, "Nombre completo", 20, 110);
            txtNombre = UI.Caja(card, 20, 132, 380);

            UI.EtiquetaCampo(card, "Contraseña", 20, 175);
            txtContrasena = UI.Caja(card, 20, 197, 380);
            txtContrasena.UseSystemPasswordChar = true;

            UI.EtiquetaCampo(card, "Confirmar contraseña", 20, 240);
            txtConfirmar = UI.Caja(card, 20, 262, 380);
            txtConfirmar.UseSystemPasswordChar = true;

            btnGuardar = UI.Boton(this, "Guardar", 20, 405, 150);
            btnLimpiar = UI.Boton(this, "Limpiar", 180, 405, 110);
            btnCancelar = UI.Boton(this, "Cancelar", 300, 405, 110);
            btnGuardar.Height = 38;
            btnLimpiar.Height = 38;
            btnCancelar.Height = 38;
            UI.EstiloBoton(btnGuardar, true);
            UI.EstiloBoton(btnLimpiar, false);
            UI.EstiloBoton(btnCancelar, false);

            btnGuardar.Click += btnGuardar_Click;
            btnLimpiar.Click += (s, e) => LimpiarCampos();
            btnCancelar.Click += (s, e) => Close();
            CancelButton = btnCancelar;
        }

        void LimpiarCampos()
        {
            txtCodigo.Clear();
            txtNombre.Clear();
            txtContrasena.Clear();
            txtConfirmar.Clear();
            txtCodigo.Focus();
        }

        void btnGuardar_Click(object? sender, EventArgs e)
        {
            string codigo = txtCodigo.Text.Trim();
            string nombre = txtNombre.Text.Trim();

            if (codigo == "" || nombre == "" || txtContrasena.Text == "")
            {
                Aviso("Complete todos los campos.");
                return;
            }
            if (txtContrasena.Text.Length < 4)
            {
                Aviso("La contraseña debe tener al menos 4 caracteres.");
                return;
            }
            if (txtContrasena.Text != txtConfirmar.Text)
            {
                Aviso("Las contraseñas no coinciden.");
                return;
            }

            // Verifica que el usuario no exista
            foreach (Usuario u in Datos.Usuarios)
            {
                if (u.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase))
                {
                    Aviso("Ese usuario ya está registrado.");
                    return;
                }
            }

            Usuario nuevo = new Usuario();
            nuevo.Codigo = codigo;
            nuevo.Nombre = nombre;
            nuevo.Contrasena = txtContrasena.Text;

            Datos.Usuarios.Add(nuevo);
            Datos.GuardarUsuarios();

            MessageBox.Show("Personal registrado correctamente.", "Registro exitoso",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            LimpiarCampos();
        }

        void Aviso(string mensaje)
        {
            MessageBox.Show(mensaje, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}