using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PerezRodrigo
{
    public partial class Form1 : Form
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["PerezRodrigo.Properties.Settings.cadena"].ConnectionString;

        public class Producto
        {
            public string descripcion { get; set; }
            public decimal precio { get; set; }
            public override string ToString()
            {
                return descripcion;
            }

        }
        public Form1()
        {
            InitializeComponent();
            tbId.Enabled = false;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'recuperatorioDataSet.Productos' Puede moverla o quitarla según sea necesario.
            this.productosTableAdapter.Fill(this.recuperatorioDataSet.Productos);

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbDescripcion.Text) || string.IsNullOrWhiteSpace(tbPrecio.Text))
            {
                MessageBox.Show("Complete los datos necesarios", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Producto producto = new Producto();
                producto.descripcion = tbDescripcion.Text.Trim();
                producto.precio = decimal.Parse(tbPrecio.Text);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sql = "INSERT INTO Productos (descripcion, precio) VALUES (@descripcion, @precio)";
                    SqlCommand cmd = new SqlCommand(sql, connection);

                    cmd.Parameters.AddWithValue("@descripcion", producto.descripcion);
                    cmd.Parameters.AddWithValue("@precio", producto.precio);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                }

                tbDescripcion.Clear();
                tbPrecio.Clear();
                tbDescripcion.Focus();

                this.productosTableAdapter.Fill(this.recuperatorioDataSet.Productos);

                MessageBox.Show("Producto agregado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (FormatException)
            {
                MessageBox.Show("El precio debe ser un número válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbId.Text))
            {
                MessageBox.Show("Seleccione un producto de la grilla para eliminar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult resultado = MessageBox.Show(
                $"¿Está seguro que desea eliminar el producto '{tbDescripcion.Text}'?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string sql = "DELETE FROM Productos WHERE id = @id";
                        SqlCommand cmd = new SqlCommand(sql, connection);
                        cmd.Parameters.AddWithValue("@id", int.Parse(tbId.Text));

                        connection.Open();
                        int filasAfectadas = cmd.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
                            MessageBox.Show("Producto eliminado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);


                            btnLimpiar_Click(sender, e);


                            this.productosTableAdapter.Fill(this.recuperatorioDataSet.Productos);
                        }
                    }

                }
                catch
                {
                    MessageBox.Show("Error al eliminar el producto", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

                }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvProductos.Rows[e.RowIndex];

            
                tbId.Text = row.Cells[0].Value?.ToString() ?? "";
                tbDescripcion.Text = row.Cells[1].Value?.ToString() ?? "";
                tbPrecio.Text = row.Cells[2].Value?.ToString() ?? "";
            }
        }


        private void btnLimpiar_Click(object sender, EventArgs e)
        {

            tbId.Clear();
            tbDescripcion.Clear();
            tbPrecio.Clear();


            tbDescripcion.Focus();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbId.Text))
            {
                MessageBox.Show("Seleccione un producto de la grilla para modificar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            if (string.IsNullOrWhiteSpace(tbDescripcion.Text) || string.IsNullOrWhiteSpace(tbPrecio.Text))
            {
                MessageBox.Show("Complete todos los campos", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                decimal precio = decimal.Parse(tbPrecio.Text);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sql = "UPDATE Productos SET descripcion = @descripcion, precio = @precio WHERE id = @id";
                    SqlCommand cmd = new SqlCommand(sql, connection);

                    cmd.Parameters.AddWithValue("@id", int.Parse(tbId.Text));
                    cmd.Parameters.AddWithValue("@descripcion", tbDescripcion.Text.Trim());
                    cmd.Parameters.AddWithValue("@precio", precio);

                    connection.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        MessageBox.Show("Producto modificado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);


                        btnLimpiar_Click(sender, e);


                        this.productosTableAdapter.Fill(this.recuperatorioDataSet.Productos);
                    }
                    else
                    {
                        MessageBox.Show("No se encontró el producto", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("El precio debe ser un número válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
          
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (string.IsNullOrWhiteSpace(tbDesde.Text) && string.IsNullOrWhiteSpace(tbHasta.Text))
                {
                    MessageBox.Show("Ingrese al menos un valor de precio (mínimo o máximo)", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                
                string sql = "SELECT * FROM Productos WHERE 1=1";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;

                    
                    if (!string.IsNullOrWhiteSpace(tbDesde.Text))
                    {
                        decimal precioMin = decimal.Parse(tbDesde.Text);
                        sql += " AND precio >= @precioMin";
                        cmd.Parameters.AddWithValue("@precioMin", precioMin);
                    }

                    
                    if (!string.IsNullOrWhiteSpace(tbHasta.Text))
                    {
                        decimal precioMax = decimal.Parse(tbHasta.Text);
                        sql += " AND precio <= @precioMax";
                        cmd.Parameters.AddWithValue("@precioMax", precioMax);
                    }

                    cmd.CommandText = sql;
                    connection.Open();

                    
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    
                    dgvProductos.DataSource = dt;

                    
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No se encontraron productos en ese rango de precios", "Sin resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Los precios deben ser números válidos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        

        private void btnDesfltrar_Click(object sender, EventArgs e)
        {
            tbDesde.Clear();
            tbHasta.Clear();
            this.productosTableAdapter.Fill(this.recuperatorioDataSet.Productos);
        }
    }

    }
