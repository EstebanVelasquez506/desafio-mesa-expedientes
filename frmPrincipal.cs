using System;
using System.Drawing;
using System.Windows.Forms;

namespace MesaPartesDigital
{
    // FORMULARIO 2: MENÚ PRINCIPAL
    public class frmPrincipal : Form
    {
        public frmPrincipal(Usuario usuario)
        {
            Text = "Mesa de Partes Digital - Menú Principal";
            ClientSize = new Size(760, 380);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = UI.Fondo;
            Font = UI.Fuente;

            // El encabezado se agrega primero y el menú después,
            // así el menú queda arriba de todo
            UI.Encabezado(this, "Mesa de Partes Digital", "Bienvenido(a): " + usuario.Nombre);

            // ----- Menú superior -----
            MenuStrip menu = new MenuStrip();

            ToolStripMenuItem mnuExpedientes = new ToolStripMenuItem("Expedientes");
            mnuExpedientes.DropDownItems.Add("Registro de Expedientes", null, (s, e) => new frmRegistroExpedientes().ShowDialog());
            mnuExpedientes.DropDownItems.Add("Gestión de Expedientes", null, (s, e) => new frmGestionExpedientes().ShowDialog());

            ToolStripMenuItem mnuPersonal = new ToolStripMenuItem("Personal");
            mnuPersonal.DropDownItems.Add("Registro de Personal", null, (s, e) => new frmRegistroPersonal().ShowDialog());

            ToolStripMenuItem mnuSalir = new ToolStripMenuItem("Salir");
            mnuSalir.Click += (s, e) => Close();

            menu.Items.Add(mnuExpedientes);
            menu.Items.Add(mnuPersonal);
            menu.Items.Add(mnuSalir);
            MainMenuStrip = menu;
            Controls.Add(menu);

            // ----- Accesos rápidos -----
            Label pregunta = UI.Etiqueta(this, "¿Qué desea hacer?", 30, 125);
            pregunta.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            pregunta.ForeColor = UI.AzulOscuro;

            Button b1 = UI.Boton(this, "Registrar expediente", 30, 170, 220);
            Button b2 = UI.Boton(this, "Gestionar expedientes", 270, 170, 220);
            Button b3 = UI.Boton(this, "Registrar personal", 510, 170, 220);

            b1.Height = 110;
            b2.Height = 110;
            b3.Height = 110;
            UI.EstiloBoton(b1, true);
            UI.EstiloBoton(b2, true);
            UI.EstiloBoton(b3, false);

            b1.Click += (s, e) => new frmRegistroExpedientes().ShowDialog();
            b2.Click += (s, e) => new frmGestionExpedientes().ShowDialog();
            b3.Click += (s, e) => new frmRegistroPersonal().ShowDialog();
        }
    }
}