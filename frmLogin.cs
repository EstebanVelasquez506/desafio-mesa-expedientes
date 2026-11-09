using System;
using System.Drawing;
using System.Windows.Forms;

namespace MesaPartesDigital
{
    // FORMULARIO 1: LOGIN
    public class frmLogin : Form
    {
        TextBox txtUsuario;
        TextBox txtContrasena;
        Button btnIngresar;
        Button btnSalir;

        public frmLogin()
        {
            Text = "Mesa de Partes Digital - Login";
            ClientSize = new Size(400, 375);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            BackColor = UI.Fondo;
            Font = UI.Fuente;

            UI.Encabezado(this, "Mesa de Partes Digital", "Municipalidad Distrital");

            // ----- Tarjeta con el formulario -----
            Panel card = UI.Tarjeta(this, "Iniciar sesión", 30, 95, 340, 255);

            UI.EtiquetaCampo(card, "Usuario", 20, 50);
            txtUsuario = UI.Caja(card, 20, 72, 300);

            UI.EtiquetaCampo(card, "Contraseña", 20, 115);
            txtContrasena = UI.Caja(card, 20, 137, 300);
            txtContrasena.UseSystemPasswordChar = true;   // oculta lo que se escribe

            btnIngresar = UI.Boton(card, "Ingresar", 20, 195, 145);
            btnSalir = UI.Boton(card, "Salir", 175, 195, 145);
            btnIngresar.Height = 38;
            btnSalir.Height = 38;
            UI.EstiloBoton(btnIngresar, true);
            UI.EstiloBoton(btnSalir, false);

            btnIngresar.Click += btnIngresar_Click;
            btnSalir.Click += (s, e) => Application.Exit();

            AcceptButton = btnIngresar;   // Enter = Ingresar
        }

        void btnIngresar_Click(object? sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string clave = txtContrasena.Text;

            if (usuario == "" || clave == "")
            {
                MessageBox.Show("Ingrese usuario y contraseña.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Busca el usuario en la lista
            foreach (Usuario u in Datos.Usuarios)
            {
                if (u.Codigo == usuario && u.Contrasena == clave)
                {
                    // Acceso concedido: abre el formulario principal
                    frmPrincipal principal = new frmPrincipal(u);
                    principal.FormClosed += (s, ev) => this.Close();
                    this.Hide();
                    principal.Show();
                    return;
                }
            }

            MessageBox.Show("Usuario o contraseña incorrectos.", "Acceso denegado",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            txtContrasena.Clear();
            txtContrasena.Focus();
        }
    }
}