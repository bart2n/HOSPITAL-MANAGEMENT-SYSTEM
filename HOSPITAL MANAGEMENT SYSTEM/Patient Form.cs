using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;


namespace HOSPITAL_MANAGEMENT_SYSTEM
{
    public partial class Patient_Form : Form
    {
        string sqlconnection = @"Data Source=.\sqlexpress;Integrated Security=True;Initial Catalog=HospitalDB ;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public Patient_Form()
        {
            InitializeComponent();
            comboBox1.Items.Add("Male");
            comboBox1.Items.Add("Female");
        }
        private void dataload()
        {
            string query = "SELECT * FROM Patients";
            using (SqlConnection pull = new SqlConnection(sqlconnection))
            {
                SqlDataAdapter get = new SqlDataAdapter(query, pull);
                DataTable table = new DataTable();
                get.Fill(table);
                dataGridView1.DataSource = table;
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row =  dataGridView1.Rows[e.RowIndex];
                textBox1.Text = row.Cells["Name"].Value.ToString();
                textBox2.Text = row.Cells["Age"].Value.ToString();
                textBox3.Text = row.Cells["Contact"].Value.ToString();
                textBox4.Text = row.Cells["Adress"].Value.ToString();
                textBox5.Text = row.Cells["Email"].Value.ToString();
                comboBox1.SelectedItem = row.Cells["Gender"].Value.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                string query = "INSERT INTO Patients(Name,Gender,Age,Contact,Adress,Email)VALUES(@Name,@Gender,@Age,@Contact,@Adress,@Email)";
                using (SqlConnection post = new SqlConnection(sqlconnection))
                {
                    SqlCommand command = new SqlCommand(query, post);
                    command.Parameters.AddWithValue("@Name", textBox1.Text);
                    command.Parameters.AddWithValue("@Age", textBox2.Text);
                    command.Parameters.AddWithValue("@Gender", comboBox1.Text);
                    command.Parameters.AddWithValue("@Contact", textBox3.Text);
                    command.Parameters.AddWithValue("@Adress", textBox4.Text);
                    command.Parameters.AddWithValue("@Email", textBox5.Text);
                    post.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Creating Data Successfull!");
                    dataload();
                    

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataload();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Please select a row to update!");
            }
            else
            {
                try
                {
                    int id = (int)Convert.ToInt64(dataGridView1.CurrentRow.Cells["PatientId"].Value);
                    string query = "UPDATE Patients SET Name=@Name,Age=@Age,Gender=@Gender,Contact=@Contact,Adress=@Adress,Email=@Email WHERE PatientID=@id";
                    using (SqlConnection Update = new SqlConnection(sqlconnection))
                    {
                        SqlCommand command = new SqlCommand(query, Update);
                        command.Parameters.AddWithValue("@Name", textBox1.Text);
                        command.Parameters.AddWithValue("@Age", textBox2.Text);
                        command.Parameters.AddWithValue("@Gender", comboBox1.Text);
                        command.Parameters.AddWithValue("@Contact", textBox3.Text);
                        command.Parameters.AddWithValue("@Adress", textBox4.Text);
                        command.Parameters.AddWithValue("@Email", textBox5.Text);
                        command.Parameters.AddWithValue("@id", id);
                        Update.Open();
                        command.ExecuteNonQuery();
                        MessageBox.Show("Update Data Successfull!");
                        dataload();


                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Please select a row to delete!");
            }
            else
            {
                try
                {
                    int id = (int)Convert.ToInt64(dataGridView1.CurrentRow.Cells["PatientId"].Value);
                    string query = "DELETE  FROM Patients WHERE PatientID=@id";
                    using(SqlConnection delete = new SqlConnection(sqlconnection))
                    {
                        SqlCommand command = new SqlCommand(query, delete);
                        command.Parameters.AddWithValue("@id", id);
                        delete.Open();
                        command.ExecuteNonQuery();
                        MessageBox.Show("Data Deleted Sucessfully!");
                        dataload();

                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MainForm home = new MainForm();
            home.Show();
            this.Hide();
        }
    }
}
