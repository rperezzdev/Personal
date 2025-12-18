using Modelos;
using Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rodrigo
{
    public partial class PProductos : Form
    {
        List<Productos> productos = new List<Productos>();
        Productos productoSeleccionado = new Productos();
        public PProductos()
        {
            InitializeComponent();
        }



        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbDescripcion.Text != "")
                {

                    string descripcion = tbDescripcion.Text;
                    Productos p = new Productos
                    {
                        Descripcion = descripcion
                    };
                    NProductos.Create(p);
                    productos = NProductos.Get();
                    productosBindingSource.DataSource = null;
                    productosBindingSource.DataSource = productos;
                    MessageBox.Show("Registro agregado", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    throw new Exception("No puede tener campos vacios.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void PProductos_Load(object sender, EventArgs e)
        {
            productos = NProductos.Get();
            productosBindingSource.DataSource = productos;
        }


        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            productoSeleccionado = (Productos)productosBindingSource.Current;
            if (productoSeleccionado != null)
            {
                tbDescripcion.Text = productoSeleccionado.Descripcion;

            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            {
                try
                {
                    DialogResult result = MessageBox.Show(
                        "Desea borrar el registro?",
                        "Confirmacion Eliminar",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                        );
                    if (result == DialogResult.Yes)
                    {
                        NProductos.Delete(productoSeleccionado.Id);
                        MessageBox.Show("Registro eliminado", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        productos.RemoveAll(p => p.Id == productoSeleccionado.Id);
                        productosBindingSource.DataSource = null;
                        productosBindingSource.DataSource = productos;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
           
            try
            {
                if (tbDescripcion.Text != "")
                {
                    string descripcion = tbDescripcion.Text;
                    productoSeleccionado.Descripcion = descripcion;
                    NProductos.Update(productoSeleccionado);
                    Productos producto = productos.Find(p => p.Id == productoSeleccionado.Id);
                    producto.Descripcion = productoSeleccionado.Descripcion;

                    productosBindingSource.DataSource = null;
                    productosBindingSource.DataSource = productos;
                    MessageBox.Show("Registro modificado", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    throw new Exception("No puede tener campos vacios.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    
}
