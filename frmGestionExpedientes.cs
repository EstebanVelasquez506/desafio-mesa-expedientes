using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MesaPartesDigital
{
    // FORMULARIO 4: GESTIÓN DE EXPEDIENTES
    // Lista los expedientes en un DataGridView, permite buscar, ordenar (burbuja) y eliminar
    public class frmGestionExpedientes : Form
    {
        DataGridView dgv;
        TextBox txtBuscar;
        Button btnBuscar, btnTodos, btnOrdenarNombre, btnOrdenarCodigo, btnEliminar, btnCerrar;

        // Lista que se está mostrando actualmente en la tabla
        List<Expediente> listaMostrada = new List<Expediente>();

        public frmGestionExpedientes()
        {
            Text = "Gestión de Expedientes";
            Width = 1000;
            Height = 540;
            StartPosition = FormStartPosition.CenterParent;

            // ----- Barra de búsqueda -----
            UI.Etiqueta(this, "Buscar (código o nombre):", 20, 20);
            txtBuscar = UI.Caja(this, 190, 17, 250);
            btnBuscar = UI.Boton(this, "Buscar", 460, 13, 90);
            btnTodos = UI.Boton(this, "Mostrar todos", 560, 13, 110);

            // ----- Tabla -----
            dgv = new DataGridView();
            dgv.Location = new System.Drawing.Point(20, 60);
            dgv.Size = new System.Drawing.Size(945, 340);
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            Controls.Add(dgv);

            // ----- Botones inferiores -----
            btnOrdenarNombre = UI.Boton(this, "Ordenar por nombre", 20, 420, 150);
            btnOrdenarCodigo = UI.Boton(this, "Ordenar por código", 180, 420, 150);
            btnEliminar = UI.Boton(this, "Eliminar seleccionado", 340, 420, 160);
            btnCerrar = UI.Boton(this, "Cerrar", 865, 420, 100);

            btnBuscar.Click += btnBuscar_Click;
            btnTodos.Click += (s, e) => MostrarTodos();
            btnOrdenarNombre.Click += (s, e) => Ordenar(true);
            btnOrdenarCodigo.Click += (s, e) => Ordenar(false);
            btnEliminar.Click += btnEliminar_Click;
            btnCerrar.Click += (s, e) => Close();

            MostrarTodos();
        }

        // Muestra en el DataGridView la lista que se le indique
        void Mostrar(List<Expediente> lista)
        {
            listaMostrada = lista;
            dgv.DataSource = null;
            dgv.DataSource = listaMostrada;

            if (dgv.Columns["FechaHora"] != null)
                dgv.Columns["FechaHora"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
        }

        void MostrarTodos()
        {
            txtBuscar.Clear();
            Mostrar(new List<Expediente>(Datos.Expedientes));
        }

        void btnBuscar_Click(object sender, EventArgs e)
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
                if (exp.Codigo.ToLower().Contains(texto) || exp.Nombre.ToLower().Contains(texto))
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

        void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgv.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un expediente de la tabla.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Expediente exp = (Expediente)dgv.CurrentRow.DataBoundItem;

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
