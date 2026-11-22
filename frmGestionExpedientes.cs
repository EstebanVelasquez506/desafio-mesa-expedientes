using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MesaPartesDigital
{
    // FORMULARIO 4: GESTIÓN DE EXPEDIENTES
    // Lista los expedientes en un DataGridView, permite buscar, ordenar (burbuja) y eliminar
    public class frmGestionExpedientes : Form
    {
        DataGridView dgv;
        TextBox txtBuscar;
        Label lblTotal;
        Button btnBuscar, btnTodos, btnOrdenarNombre, btnOrdenarCodigo, btnEliminar, btnCerrar;

        // Lista que se está mostrando actualmente en la tabla
        List<Expediente> listaMostrada = new List<Expediente>();

        public frmGestionExpedientes()
        {
            Text = "Gestión de Expedientes";
            ClientSize = new Size(1100, 620);
            MinimumSize = new Size(900, 500);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = UI.Fondo;
            Font = UI.Fuente;

            UI.Encabezado(this, "Gestión de Expedientes",
                "Consulte, busque, ordene y elimine los expedientes registrados");

            // ----- Barra de búsqueda (tarjeta blanca) -----
            Panel barra = new Panel();
            barra.Location = new Point(20, 85);
            barra.Size = new Size(1060, 56);
            barra.BackColor = Color.White;
            barra.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Controls.Add(barra);

            UI.Etiqueta(barra, "Buscar por código o ciudadano:", 15, 18);
            txtBuscar = UI.Caja(barra, 225, 14, 300);
            btnBuscar = UI.Boton(barra, "Buscar", 540, 12, 90);
            btnTodos = UI.Boton(barra, "Mostrar todos", 640, 12, 130);
            UI.EstiloBoton(btnBuscar, true);
            UI.EstiloBoton(btnTodos, false);

            lblTotal = UI.Etiqueta(barra, "", 845, 17);
            lblTotal.AutoSize = false;
            lblTotal.Size = new Size(200, 24);
            lblTotal.TextAlign = ContentAlignment.MiddleRight;
            lblTotal.ForeColor = UI.TextoGris;
            lblTotal.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // ----- Tabla -----
            dgv = new DataGridView();
            dgv.Location = new Point(20, 155);
            dgv.Size = new Size(1060, 395);
            dgv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv.RowHeadersVisible = false;
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = UI.Gris;
            dgv.RowTemplate.Height = 28;

            // Encabezado de la tabla en azul
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = UI.AzulOscuro;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = UI.AzulOscuro;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            // Filas alternadas y color de selección
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(243, 246, 252);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(191, 219, 254);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            Controls.Add(dgv);

            // ----- Botones inferiores -----
            btnOrdenarNombre = UI.Boton(this, "Ordenar por nombre", 20, 565, 160);
            btnOrdenarCodigo = UI.Boton(this, "Ordenar por código", 190, 565, 160);
            btnEliminar = UI.Boton(this, "Eliminar seleccionado", 360, 565, 180);
            btnCerrar = UI.Boton(this, "Cerrar", 980, 565, 100);

            btnOrdenarNombre.Height = 36;
            btnOrdenarCodigo.Height = 36;
            btnEliminar.Height = 36;
            btnCerrar.Height = 36;

            UI.EstiloBoton(btnOrdenarNombre, true);
            UI.EstiloBoton(btnOrdenarCodigo, true);
            UI.EstiloBoton(btnEliminar, true);
            btnEliminar.BackColor = UI.Rojo;
            UI.EstiloBoton(btnCerrar, false);

            btnOrdenarNombre.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnOrdenarCodigo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnEliminar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            // ----- Eventos -----
            btnBuscar.Click += btnBuscar_Click;
            btnTodos.Click += (s, e) => MostrarTodos();
            btnOrdenarNombre.Click += (s, e) => Ordenar(true);
            btnOrdenarCodigo.Click += (s, e) => Ordenar(false);
            btnEliminar.Click += btnEliminar_Click;
            btnCerrar.Click += (s, e) => Close();

            // Se llena la tabla cuando el formulario ya está listo (no en el constructor)
            Load += (s, e) => MostrarTodos();
        }

        // Muestra en el DataGridView la lista que se le indique
        void Mostrar(List<Expediente> lista)
        {
            listaMostrada = lista;
            dgv.DataSource = null;
            dgv.DataSource = listaMostrada;

            ConfigurarColumnas();
            lblTotal.Text = "Total: " + listaMostrada.Count + " expediente(s)";
        }

        // Nombres bonitos para los encabezados de la tabla
        void ConfigurarColumnas()
        {
            Titulo("Codigo", "Código");
            Titulo("Ciudadano", "Ciudadano / remitente");
            Titulo("Documento", "DNI / RUC");
            Titulo("Asunto", "Asunto");

            // La columna Asunto ocupa el espacio que sobre
            DataGridViewColumn? asunto = dgv.Columns["Asunto"];
            if (asunto != null)
            {
                asunto.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        void Titulo(string columna, string texto)
        {
            DataGridViewColumn? c = dgv.Columns[columna];
            if (c != null)
            {
                c.HeaderText = texto;
            }
        }

        void MostrarTodos()
        {
            txtBuscar.Clear();
            Mostrar(new List<Expediente>(Datos.Expedientes));
        }

        void btnBuscar_Click(object? sender, EventArgs e)
        {
            string texto = txtBuscar.Text.Trim().ToLower();
            if (texto == "")
            {
                MostrarTodos();
                return;
            }

            List<Expediente> encontrados = new List<Expediente>();
            foreach (Expediente exp in Datos.Expedientes)
            {
                if (exp.Codigo.ToLower().Contains(texto) || exp.Ciudadano.ToLower().Contains(texto))
                {
                    encontrados.Add(exp);
                }
            }

            Mostrar(encontrados);

            if (encontrados.Count == 0)
            {
                MessageBox.Show("No se encontraron expedientes.", "Búsqueda",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Usa el algoritmo de la burbuja que está en la clase Datos
        void Ordenar(bool porNombre)
        {
            Datos.OrdenarBurbuja(listaMostrada, porNombre);
            Mostrar(listaMostrada);
        }

        void btnEliminar_Click(object? sender, EventArgs e)
        {
            Expediente? exp = dgv.CurrentRow?.DataBoundItem as Expediente;

            if (exp == null)
            {
                MessageBox.Show("Seleccione un expediente de la tabla.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult r = MessageBox.Show("¿Eliminar el expediente " + exp.Codigo + "?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (r == DialogResult.Yes)
            {
                Datos.Expedientes.Remove(exp);
                Datos.GuardarExpedientes();
                MostrarTodos();
            }
        }
    }
}
