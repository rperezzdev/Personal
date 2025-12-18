using Modelos;
using Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rodrigo
{
    public partial class PPrecios : Form
    {
        List<Precios> precios = new List<Precios>();
        List<Productos> productos = new List<Productos>();
        Precios precioSeleccionado = new Precios();
        public PPrecios()
        {
            InitializeComponent();
        }

        private void PPrecios_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'finalDataSet.precios' Puede moverla o quitarla según sea necesario.
            CargarDatos();
        }
        private void CargarDatos()
        {

            // Cargar lista de precios
            precios = NPrecios.Get();
            preciosBindingSource.DataSource = null;
            preciosBindingSource.DataSource = precios;
            // Cargar lista de productos para el ComboBox
            productos = NProductos.Get();
            cboProducto.DataSource = null;
            cboProducto.DataSource = productos;
            cboProducto.DisplayMember = "Descripcion";
            cboProducto.ValueMember = "Id";
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarCampos(); // Ya no necesitas el if porque lanza excepción si falla

                Precios p = new Precios
                {
                    Fecha = dtpFecha.Value.Date,
                    Monto = Convert.ToInt32(tbMonto.Text),
                    idProducto = Convert.ToInt32(cboProducto.SelectedValue)
                };

                NPrecios.Create(p);
                CargarDatos();
                LimpiarCampos();
                MessageBox.Show("Registro agregado", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       

       

     

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            if (preciosBindingSource.Current != null)
            {
                precioSeleccionado = (Precios)preciosBindingSource.Current;
                dtpFecha.Value = precioSeleccionado.Fecha;
                tbMonto.Text = precioSeleccionado.Monto.ToString();
                cboProducto.SelectedValue = precioSeleccionado.idProducto;
            }
           
        }

        

        public void ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(tbMonto.Text))
            {
                throw new Exception("El monto no puede estar vacío.");
            }

            if (!int.TryParse(tbMonto.Text, out int monto) || monto <= 0)
            {
                throw new Exception("El monto debe ser un número entero positivo.");
            }

            if (cboProducto.SelectedValue == null)
            {
                throw new Exception("Debe seleccionar un producto.");
            }
        }

        private void LimpiarCampos()
        {
            dtpFecha.Value = DateTime.Now;
            tbMonto.Clear();
            if (cboProducto.Items.Count > 0)
                cboProducto.SelectedIndex = 0;
            precioSeleccionado = new Precios();
            tbMonto.Focus();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (precioSeleccionado == null || precioSeleccionado.Id == 0)
                {
                    MessageBox.Show("Debe seleccionar un precio", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show(
                    $"¿Desea borrarlo?",
                    "Confirmación Eliminar",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes)
                {
                    NPrecios.Delete(precioSeleccionado.Id);
                    CargarDatos();
                    LimpiarCampos();
                    MessageBox.Show("Registro eliminado", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (precioSeleccionado == null || precioSeleccionado.Id == 0)
                {
                    MessageBox.Show("Debe seleccionar un precio", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ValidarCampos(); // Ya no necesitas el if

                precioSeleccionado.Fecha = dtpFecha.Value.Date;
                precioSeleccionado.Monto = Convert.ToInt32(tbMonto.Text);
                precioSeleccionado.idProducto = Convert.ToInt32(cboProducto.SelectedValue);

                NPrecios.Update(precioSeleccionado);
                CargarDatos();
                MessageBox.Show("Registro modificado", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tbMontoMin.Text) || string.IsNullOrWhiteSpace(tbMontoMax.Text))
                {
                    MessageBox.Show("Debe ingresar ambos valores de monto", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(tbMontoMin.Text, out int montoMin) || !int.TryParse(tbMontoMax.Text, out int montoMax))
                {
                    MessageBox.Show("Los montos deben ser números enteros", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (montoMin > montoMax)
                {
                    MessageBox.Show("El monto mínimo no puede ser mayor al monto máximo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                precios = NPrecios.GetByMonto(montoMin, montoMax);
                preciosBindingSource.DataSource = null;
                preciosBindingSource.DataSource = precios;

                if (precios.Count == 0)
                {
                    MessageBox.Show("No se encontraron registros con ese rango de monto", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDesfiltrar_Click(object sender, EventArgs e)
        {
            tbMontoMin.Clear();
            tbMontoMax.Clear();
            CargarDatos();
        }
    }
}
