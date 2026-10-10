namespace Baitap5._3
{
    public partial class Form1 : Form
    {
        private System.ComponentModel.BindingList<Product> products;
        private BindingSource bs = new BindingSource();

        public Form1()
        {
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            products = new System.ComponentModel.BindingList<Product>();
            bs.DataSource = products;
            dgvProducts.DataSource = bs;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProductId.Text))
            {
                MessageBox.Show("Mã SP không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (products.Any(p => p.ProductId == txtProductId.Text.Trim()))
            {
                MessageBox.Show("Mã SP đã tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var product = new Product
            {
                ProductId = txtProductId.Text.Trim(),
                ProductName = txtProductName.Text.Trim(),
                Category = txtCategory.Text.Trim()
            };

            if (decimal.TryParse(txtUnitPrice.Text.Trim(), out var price))
                product.UnitPrice = price;

            if (int.TryParse(txtQuantity.Text.Trim(), out var qty))
                product.Quantity = qty;

            products.Add(product);
            bs.ResetBindings(false);
            ClearInputs();
        }

        private void ClearInputs()
        {
            txtProductId.Clear();
            txtProductName.Clear();
            txtUnitPrice.Clear();
            txtQuantity.Clear();
            txtCategory.Clear();
            txtProductId.Focus();
        }

        private void dgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            if (bs.Current is Product p)
            {
                txtProductId.Text = p.ProductId;
                txtProductName.Text = p.ProductName;
                txtUnitPrice.Text = p.UnitPrice.ToString();
                txtQuantity.Text = p.Quantity.ToString();
                txtCategory.Text = p.Category;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (bs.Current is Product p)
            {
                var res = MessageBox.Show($"Xóa sản phẩm {p.ProductName} (Mã: {p.ProductId})?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    products.Remove(p);
                    bs.ResetBindings(false);
                    ClearInputs();
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (bs.Current is Product p)
            {
                p.ProductName = txtProductName.Text.Trim();
                p.Category = txtCategory.Text.Trim();
                if (decimal.TryParse(txtUnitPrice.Text.Trim(), out var price)) p.UnitPrice = price;
                if (int.TryParse(txtQuantity.Text.Trim(), out var qty)) p.Quantity = qty;
                bs.ResetBindings(false);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var keyword = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                bs.DataSource = products;
            }
            else
            {
                var filtered = products.Where(p => p.ProductName.Contains(keyword, StringComparison.CurrentCultureIgnoreCase)).ToList();
                bs.DataSource = new System.ComponentModel.BindingList<Product>(filtered);
            }
            bs.ResetBindings(false);
        }

        private void txtCategory_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
