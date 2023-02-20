using Session05.Model;
using Session05.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Session05
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonLoadProducts_Click(object sender, EventArgs e)
        {
            loadData();
        }

        private void loadData()
        {
            // CRUD - create, read, update, delete


            //Linq -> Lazy
            //using (var ctx = new NorthWindEntities())
            //{
            //    ctx.Database.Log = Console.WriteLine;
            //    dataGridView1.DataSource = ctx.Products.Where(x => x.UnitPrice > 5000 ).ToList();                
            //}

            ////Wrong 👎
            //using (var ctx = new NorthWindEntities())
            //{
            //    var data = ctx.Products.Where(x => x.UnitPrice > 10).ToList();
            //    if (checkBoxDiscontinued.Checked)
            //    {
            //        dataGridView1.DataSource = data.Where(x => x.Discontinued == true).ToList();
            //    }
            //    else
            //    {
            //        dataGridView1.DataSource = data;
            //    }                
            //}

            ////Good 👍
            //using (var ctx = new NorthWindEntities())
            //{
            //    var data = ctx.Products.Where(x => x.UnitPrice > 10);
            //    if (checkBoxDiscontinued.Checked)
            //    {
            //        dataGridView1.DataSource = data.Where(x => x.Discontinued == true).ToList();
            //    }
            //    else
            //    {
            //        dataGridView1.DataSource = data.ToList();
            //    }
            //}

            ////Good 👍👍
            //using (var ctx = new NorthWindEntities())
            //{
            //    var query = ctx.Products.Where(x => x.UnitPrice > 10);
            //    if (checkBoxDiscontinued.Checked)
            //    {
            //       query = query.Where(x => x.Discontinued == true);
            //    }

            //    dataGridView1.DataSource = query.ToList();
            //}

            //Join
            using (var ctx = new NorthWindEntities())
            {
                //ctx.Database.Log = Console.WriteLine;
                //join -> using System.Data.Entity;
                var query = ctx.Products.Include(x => x.Category);
                if (checkBoxDiscontinued.Checked)
                {
                    query = query.Where(x => x.Discontinued == true);
                }

                if ((int)comboBoxCategory.SelectedValue > -1)
                {
                    query = query.Where(x => x.CategoryID == (int)comboBoxCategory.SelectedValue);
                }

                if (!string.IsNullOrWhiteSpace(textBoxFrom.Text))
                {
                    var from = Convert.ToInt32(textBoxFrom.Text);
                    query = query.Where(x => x.UnitPrice >= from);
                }
                if (!string.IsNullOrWhiteSpace(textBoxTo.Text))
                {
                    var to = Convert.ToInt32(textBoxTo.Text);
                    query = query.Where(x => x.UnitPrice <= to);
                }

                //
                dataGridView1.DataSource = query.Select(x => new
                {
                    Id = x.ProductID,
                    Name = x.ProductName,
                    Price = x.UnitPrice,
                    Category = x.Category.CategoryName,
                    x.Discontinued,
                }).ToList();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (var ctx = new NorthWindEntities())
            {
                List<Category> categories = ctx.Categories.ToList();
                //categories.Add()  اخر لیست
                categories.Insert(0, new Category { CategoryName = "All", CategoryID = -1 }); // اول لیست
                comboBoxCategory.DataSource = categories;
                comboBoxCategory.DisplayMember= "CategoryName";
                comboBoxCategory.ValueMember= "CategoryId";
            }
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            var frm = new FormProduct();
            frm.ShowDialog();
            loadData();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var id = (int)dataGridView1.Rows[e.RowIndex].Cells["ColumnProductId"].Value; //unbox
            if(dataGridView1.CurrentCell.OwningColumn.Name == "ColumnEdit")
            {
                var frm = new FormProduct(id);
                frm.ShowDialog();
                loadData();
            }

            if (dataGridView1.CurrentCell.OwningColumn.Name == "ColumnDelete")
            {
                if (MessageBox.Show("Are you sure?", "Delete",  MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    /**
                     * Delete
                     * using(var ctx = ...)
                     * var product = ctx.find(id)
                     * ctx.Products.remove(product)        
                     * ctx.SaveChanges(); 
                     */

                    using (var ctx = new NorthWindEntities())
                    {
                        ctx.Database.Log = Console.WriteLine;
                        var product = ctx.Products.Find(id);
                        ctx.Products.Remove(product);
                        //ctx.Database.ExecuteSqlCommand("delete from ....");

                        //Audit Log

                        ctx.SaveChanges();
                        loadData();
                    }
                }
            }
        }
    }
}
