using Session05.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Session05.UI
{
    public partial class FormProduct : Form
    {
        public FormProduct()
        {
            InitializeComponent();
        }

        private void FormProduct_Load(object sender, EventArgs e)
        {
            using (var ctx = new NorthWindEntities())
            {
                List<Category> categories = ctx.Categories.ToList();
                comboBoxCategory.DataSource = categories;
                comboBoxCategory.DisplayMember = "CategoryName";
                comboBoxCategory.ValueMember = "CategoryId";
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            /**
             * New
             * using(var ctx = ...)
             * var product = new Product{...}
             * ctx.Products.Add(product)
             * ctx.SaveChanges();
             */
            using (var ctx = new NorthWindEntities())
            {
                var product = new Product
                {
                    ProductName = textBoxName.Text,
                    UnitPrice = Convert.ToInt32(textBoxPrice.Text),
                    CategoryID = (int)comboBoxCategory.SelectedValue
                };
                ctx.Products.Add(product);
                ctx.SaveChanges();
            }

            MessageBox.Show("🎉");
            Close();

        }
    }
}
