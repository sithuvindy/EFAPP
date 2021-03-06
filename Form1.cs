using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EFAPP
{
    public partial class Form1 : Form
    {

        Customer model = new Customer();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        void Clear()
        {
            txtFirstName.Text = txtLastName.Text = txtCity.Text = txtAddress.Text = "";
            btnSave.Text = "Save";
            btnDelete.Enabled = false;
            model.CustomerID = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
            PopulateDataGridView();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            model.FirstName = txtFirstName.Text.Trim();
            model.LastName = txtLastName.Text.Trim();
            model.City = txtCity.Text.Trim();
            model.Address = txtAddress.Text.Trim();
            using (DBEntities db = new DBEntities())
            {
                if (model.CustomerID == 0)//Insert
                    db.Customers.Add(model);
                else//update
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            Clear();
            PopulateDataGridView();
            MessageBox.Show("Submitted Successfully!");

        }

        void PopulateDataGridView()
        {
            dgvCustomer.AutoGenerateColumns = false;
            using (DBEntities db = new DBEntities())
            {
                dgvCustomer.DataSource = db.Customers.ToList<Customer>();
            }
        }

        private void dgvCustomer_DoubleClick(object sender, EventArgs e)
        {
            if(dgvCustomer.CurrentRow.Index != -1)
            {
                model.CustomerID = Convert.ToInt32(dgvCustomer.CurrentRow.Cells["CustomerID"].Value);
                using (DBEntities db = new DBEntities())
                {
                    model = db.Customers.Where(x => x.CustomerID == model.CustomerID).FirstOrDefault();
                    txtFirstName.Text = model.FirstName;
                    txtLastName.Text = model.LastName;
                    txtCity.Text = model.City;
                    txtAddress.Text = model.Address;
                }
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are You Sure to Delete this Record?","EF CRUD Operation",MessageBoxButtons.YesNo)== DialogResult.Yes)
            {
                using (DBEntities db = new DBEntities())
                {
                    var entry = db.Entry(model);
                    if (entry.State == System.Data.Entity.EntityState.Detached)
                        db.Customers.Attach(model);
                    db.Customers.Remove(model);
                    db.SaveChanges();
                    PopulateDataGridView();
                    Clear();
                    MessageBox.Show("Deleted Successfully");
                }
            }
        }
    }
}
