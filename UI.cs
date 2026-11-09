using System.Drawing;
using System.Windows.Forms;
 
namespace MesaPartesDigital
{
    // Funciones para crear controles y darles un diseño uniforme
    public static class UI
    {
        // ---------- COLORES Y FUENTE ----------
        public static readonly Color AzulOscuro = Color.FromArgb(28, 58, 110);
        public static readonly Color Azul = Color.FromArgb(37, 99, 235);
        public static readonly Color Fondo = Color.FromArgb(244, 246, 250);
        public static readonly Color Gris = Color.FromArgb(226, 230, 236);
        public static readonly Color TextoGris = Color.FromArgb(75, 85, 99);
        public static readonly Color Rojo = Color.FromArgb(220, 38, 38);
 
        public static readonly Font Fuente = new Font("Segoe UI", 10F);
 
        // ---------- CONTROLES BÁSICOS ----------
        public static Label Etiqueta(Control padre, string texto, int x, int y)
        {
            Label l = new Label();
            l.Text = texto;
            l.Location = new Point(x, y);
            l.AutoSize = true;
            padre.Controls.Add(l);
            return l;
        }
 
        // Etiqueta pequeña y gris que va encima de cada campo
        public static Label EtiquetaCampo(Control padre, string texto, int x, int y)
        {
            Label l = Etiqueta(padre, texto, x, y);
            l.ForeColor = TextoGris;
            l.Font = new Font("Segoe UI", 9F);
            return l;
        }
 
        public static TextBox Caja(Control padre, int x, int y, int ancho)
        {
            TextBox t = new TextBox();
            t.Location = new Point(x, y);
            t.Width = ancho;
            padre.Controls.Add(t);
            return t;
        }
 
        public static Button Boton(Control padre, string texto, int x, int y, int ancho)
        {
            Button b = new Button();
            b.Text = texto;
            b.Location = new Point(x, y);
            b.Size = new Size(ancho, 32);
            padre.Controls.Add(b);
            return b;
        }
 
        public static ComboBox Combo(Control padre, int x, int y, int ancho, params string[] opciones)
        {
            ComboBox c = new ComboBox();
            c.Location = new Point(x, y);
            c.Width = ancho;
            c.DropDownStyle = ComboBoxStyle.DropDownList;
            c.Items.AddRange(opciones);
            c.SelectedIndex = 0;
            padre.Controls.Add(c);
            return c;
        }
 
        // ---------- DISEÑO ----------
        // Franja azul con título y subtítulo en la parte superior del formulario
        public static Panel Encabezado(Form form, string titulo, string subtitulo)
        {
            Panel panel = new Panel();
            panel.Dock = DockStyle.Top;
            panel.Height = 70;
            panel.BackColor = AzulOscuro;
 
            Label t = new Label();
            t.Text = titulo;
            t.AutoSize = true;
            t.ForeColor = Color.White;
            t.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            t.Location = new Point(20, 8);
            panel.Controls.Add(t);
 
            Label s = new Label();
            s.Text = subtitulo;
            s.AutoSize = true;
            s.ForeColor = Color.FromArgb(191, 210, 240);
            s.Font = new Font("Segoe UI", 9.5F);
            s.Location = new Point(22, 42);
            panel.Controls.Add(s);
 
            form.Controls.Add(panel);
            return panel;
        }
 
        // Tarjeta blanca con un título, para agrupar campos
        public static Panel Tarjeta(Control padre, string titulo, int x, int y, int ancho, int alto)
        {
            Panel p = new Panel();
            p.Location = new Point(x, y);
            p.Size = new Size(ancho, alto);
            p.BackColor = Color.White;
 
            Label t = new Label();
            t.Text = titulo;
            t.AutoSize = true;
            t.ForeColor = AzulOscuro;
            t.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            t.Location = new Point(20, 12);
            p.Controls.Add(t);
 
            padre.Controls.Add(p);
            return p;
        }
 
        // principal = true  -> botón azul con letras blancas
        // principal = false -> botón gris
        public static void EstiloBoton(Button b, bool principal)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Cursor = Cursors.Hand;
            b.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
 
            if (principal)
            {
                b.BackColor = Azul;
                b.ForeColor = Color.White;
            }
            else
            {
                b.BackColor = Gris;
                b.ForeColor = Color.FromArgb(31, 41, 55);
            }
        }
    }
}
