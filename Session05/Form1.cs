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
            //Linq -> Lazy
            using (var ctx = new NorthWindEntities())
            {
                ctx.Database.Log = Console.WriteLine;
                dataGridView1.DataSource = ctx.Products.Where(x => x.UnitPrice > 5000).ToList();                
            }
            
        }
    }
}
