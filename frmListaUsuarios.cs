using System;
using System.Drawing;
using System.Windows.Forms;

namespace MesaPartesDigital
{
    // FORMULARIO 6: LISTA DE USUARIOS REGISTRADOS
    // Muestra los trabajadores registrados (no muestra las contraseñas)
    public class frmListaUsuarios : Form
    {
        DataGridView dgv;
        Label lblTotal;
        Usuario usuarioActual;

        public frmListaUsuarios(Usuario usuario)
        {
            usuarioActual = usuario;

            Text = "Usuarios registrados";
            ClientSize = new Size(600, 440);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = UI.Fondo;
            Font = UI.Fuente;

            UI.Encabezado(this, "Usuarios registrados", "Trabajadores que pueden ingresar al sistema");

            // ----- Tabla -----
            dgv = new DataGridView();
            dgv.Location = new Point(20, 85);
            dgv.Size = new Size(560, 290);
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.RowHeadersVisible = false;
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = UI.Gris;
            dgv.RowTemplate.Height = 28;

            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = UI.AzulOscuro;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = UI.AzulOscuro;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(243, 246, 252);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(191, 219, 254);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            Controls.Add(dgv);

            dgv.Columns.Add("colUsuario", "Usuario");
            dgv.Columns.Add("colNombre", "Nombre completo");
            dgv.Columns.Add("colRol", "Rol");

            Llenar();

            // ----- Parte inferior -----
            lblTotal = UI.Etiqueta(this, "", 20, 392);
            lblTotal.ForeColor = UI.TextoGris;
            ActualizarTotal();

            // El botón Eliminar solo lo ve el administrador
            if (usuarioActual.EsAdministrador)
            {
                Button btnEliminar = UI.Boton(this, "Eliminar usuario", 300, 387, 170);
                UI.EstiloBoton(btnEliminar, false);
                btnEliminar.BackColor = UI.Rojo;
                btnEliminar.ForeColor = Color.White;
                btnEliminar.Click += btnEliminar_Click;
            }

            Button btnCerrar = UI.Boton(this, "Cerrar", 480, 387, 100);
            UI.EstiloBoton(btnCerrar, false);
            btnCerrar.Click += (s, e) => Close();
            CancelButton = btnCerrar;
        }

        void Llenar()
        {
            dgv.Rows.Clear();
            foreach (Usuario u in Datos.Usuarios)
            {
                int fila = dgv.Rows.Add(u.Codigo, u.Nombre, u.EsAdministrador ? "Administrador" : "Personal");
                dgv.Rows[fila].Tag = u;   // guarda el usuario en la fila
            }
            dgv.ClearSelection();
        }

        void ActualizarTotal()
        {
            lblTotal.Text = "Total de usuarios: " + Datos.Usuarios.Count;
        }

        void btnEliminar_Click(object? sender, EventArgs e)
        {
            // Seguridad extra: solo el administrador puede eliminar
            if (!usuarioActual.EsAdministrador) return;

            if (dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un usuario de la lista.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Con estructuras no se puede usar "as"; se usa "is" (la fila guarda una copia del usuario)
            if (dgv.SelectedRows[0].Tag is not Usuario u) return;

            if (u.EsAdministrador)
            {
                MessageBox.Show("No se puede eliminar al administrador.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult r = MessageBox.Show(
                "¿Seguro que desea eliminar al usuario \"" + u.Codigo + "\" (" + u.Nombre + ")?",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r != DialogResult.Yes) return;

            Datos.EliminarUsuario(u.Codigo);   // elimina por código y guarda el .dat
            Llenar();
            ActualizarTotal();

            MessageBox.Show("Usuario eliminado correctamente.", "Eliminado",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
