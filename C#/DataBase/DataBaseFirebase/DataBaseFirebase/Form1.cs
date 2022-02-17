using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System.Windows.Forms;

namespace DataBaseFirebase
{
    public partial class Form1 : Form
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "eOEAXsG0mQvcTIxUf44kgs7LDU5CALscmUnGti6I",
            BasePath = "https://fir-database-e801a-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
                if (client != null)
                {
                    MessageBox.Show("Connection Success");
                }
            }
            catch
            {
                MessageBox.Show("Connection Fail");
            }
            //amount.Enabled = false;
            loadProduct();

        }
        
        public  void loadProduct()
        {
           try
            {
                FirebaseResponse response = client.Get("Product/");
                Dictionary<string, DBProduct> getDBProduct = response.ResultAs<Dictionary<string, DBProduct>>();
                foreach (var get in getDBProduct)
                {

                    dataGridView1.Rows.Add(
                        get.Value.ID,
                        get.Value.Namep,
                        get.Value.Amount,
                        get.Value.Price
                        );

                }

            }
            catch
            {
                MessageBox.Show("No Data ");
            }

        }

        private void save_Click(object sender, EventArgs e)
        {
            DBProduct Prd = new DBProduct()
            {
                ID = id.Text,
                Namep = namep.Text,
                Amount = amount.Text,
                Price = price.Text
            };
            //if u want to save
            FirebaseResponse response = client.Set("Product/" + id.Text, Prd);
            MessageBox.Show("Save Success");
        }

        private void remove_Click(object sender, EventArgs e)
        {
            FirebaseResponse response = client.Delete("Product/" + id.Text);
            MessageBox.Show("Delete Success");
            id.Text = string.Empty;
            namep.Text = string.Empty;
            amount.Text = string.Empty;
            price.Text = string.Empty;
        }

        private void update_Click(object sender, EventArgs e)
        {
           // FirebaseResponse response = client.Get();
            var Prd = new DBProduct
            {
                ID = id.Text,
                Namep = namep.Text,
                Amount = amount.Text,
                Price = price.Text          
            };
            FirebaseResponse response = client.Update("Product/" + id.Text, Prd);
            MessageBox.Show("Data Update Success");
            id.Text = string.Empty;
            namep.Text = string.Empty;
            amount.Text = string.Empty;
            price.Text = string.Empty;
        }

        private void retrieve_Click(object sender, EventArgs e)
        {
            FirebaseResponse response = client.Get("Product/" + id.Text);
            DBProduct Prd = response.ResultAs<DBProduct>();
            if (id.Text.Equals(Prd.ID))
            {
                namep.Text = Prd.Namep;
                amount.Text = Prd.Amount;
                price.Text = Prd.Price;
                MessageBox.Show("Data Found.");
            }

        }

        private void refresh_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            loadProduct();
        }

        private void rmlist_Click(object sender, EventArgs e)
        {
            var Prd = new DBProduct
            {
                ID = id.Text,
                Namep = namep.Text,
                Amount = "Remove",
                Price = price.Text
            };
            FirebaseResponse response = client.Update("Product/" + id.Text, Prd);
            MessageBox.Show("Data Update Success");
            id.Text = string.Empty;
            namep.Text = string.Empty;
            amount.Text = string.Empty;
            price.Text = string.Empty;
        }
    }
}
