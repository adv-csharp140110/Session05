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
        public int? Id { get; }

        public FormProduct(int? id = null)
        {
            InitializeComponent();
            Id = id;
        }

        private void FormProduct_Load(object sender, EventArgs e)
        {
            using (var ctx = new NorthWindEntities())
            {
                List<Category> categories = ctx.Categories.ToList();
                comboBoxCategory.DataSource = categories;
                comboBoxCategory.DisplayMember = "CategoryName";
                comboBoxCategory.ValueMember = "CategoryId";

                if (Id.HasValue)
                {
                    //load by id
                    //ctx.Products.Where(x => x.ProductID == Id);

                    //var product1 = ctx.Products.First(x => x.ProductID == Id);
                    //// Not found: Exception, Multiple: avali

                    //var product2 = ctx.Products.FirstOrDefault(x => x.ProductID == Id);
                    //// Not found: null, Multiple: avali

                    //var product3 = ctx.Products.Single(x => x.ProductID == Id);
                    //// Not found: Exception, Multiple: Exception

                    //var product4 = ctx.Products.SingleOrDefault(x => x.ProductID == Id);
                    //// Not found: null, Multiple: Exception

                    var product = ctx.Products.Find(Id); // short hand FirstOrDefault
                    textBoxName.Text = product.ProductName;
                    textBoxPrice.Text = product.UnitPrice.ToString();
                    comboBoxCategory.SelectedValue= product.CategoryID;
                }
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
             * 
             * Edit
             * using(var ctx = ...)
             * var product = ctx.find(id)
             * product.price = 11        
             * ctx.SaveChanges();   
             * 
             */
            //using (var ctx = new NorthWindEntities())
            //{
            //    if (Id.HasValue)
            //    {
            //        var product = ctx.Products.Find(Id);
            //        product.ProductName = textBoxName.Text;
            //        product.UnitPrice = Convert.ToDecimal(textBoxPrice.Text);
            //        product.CategoryID = (int)comboBoxCategory.SelectedValue;
            //    }
            //    else
            //    {
            //        var product = new Product
            //        {
            //            ProductName = textBoxName.Text,
            //            UnitPrice = Convert.ToInt32(textBoxPrice.Text),
            //            CategoryID = (int)comboBoxCategory.SelectedValue
            //        };
            //        ctx.Products.Add(product);
            //    }
            //    ctx.SaveChanges();
            //}


            using (var ctx = new NorthWindEntities())
            {
                var product = new Product();
                if (Id.HasValue)
                {
                    product = ctx.Products.Find(Id);                    
                }
                else
                {                    
                    ctx.Products.Add(product);
                }
                product.ProductName = textBoxName.Text;
                product.UnitPrice = Convert.ToDecimal(textBoxPrice.Text);
                product.CategoryID = (int)comboBoxCategory.SelectedValue;

                ctx.SaveChanges();
            }


            MessageBox.Show("🎉");
            Close();

        }
    }
}
