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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace HOSPITAL_MANAGEMENT_SYSTEM
{
    public partial class Doctor_Formcs : Form
    {
        string sqlconnection = @"Data Source=.\sqlexpress;Integrated Security=True;Initial Catalog=HospitalDB ;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public Doctor_Formcs()
        {
            InitializeComponent();
            comboBox1.Items.Add("Neurologist");
            comboBox1.Items.Add("Family medicine");
            comboBox1.Items.Add("Obstetrics and gynaecology");

        }
        private void dataload()
        {
            string query = "SELECT * FROM Doctors";
            using (SqlConnection pull = new SqlConnection(sqlconnection))
            {
                SqlDataAdapter get = new SqlDataAdapter(query, pull);
                DataTable table = new DataTable();
                get.Fill(table);
                dataGridView1.DataSource = table;
            }
        }
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "INSERT INTO Doctors(Name,Specialization,Contact,Email,AvailableDays)VALUES(@Name,@Specialization,@Contact,@Email,@AvailableDays)";
                using (SqlConnection post = new SqlConnection(sqlconnection))
                {
                    SqlCommand command = new SqlCommand(query, post);
                    command.Parameters.AddWithValue("@Name", textBox1.Text);
                    command.Parameters.AddWithValue("@Specialization", comboBox1.Text);
                    command.Parameters.AddWithValue("@Contact", textBox2.Text);
                    command.Parameters.AddWithValue("@Email", textBox3.Text);
                    var days = string.Join(", ",
    checkedListBox1.CheckedItems
                   .Cast<string>());
                    command.Parameters.AddWithValue("@AvailableDays",days);
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

        private void Doctor_Formcs_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Please Select a row To Delete!");
            }
            else
            {
                try
                {
                    int id = (int)Convert.ToInt64(dataGridView1.CurrentRow.Cells["DoctorID"].Value);
                    string query = "UPDATE Doctors SET Name=@Name,Specialization=@Specialization,Contact=@Contact,Email=@Email,AvailableDays=@AvailableDays WHERE DoctorID=@Id";
                    using (SqlConnection Update = new SqlConnection(sqlconnection))
                    {
                        SqlCommand command = new SqlCommand(query, Update);
                        command.Parameters.AddWithValue("@Name", textBox1.Text);
                        command.Parameters.AddWithValue("@Specialization", comboBox1.Text);
                        command.Parameters.AddWithValue("@Contact", textBox2.Text);
                        command.Parameters.AddWithValue("@Email", textBox3.Text);
                        var days = string.Join(", ",
        checkedListBox1.CheckedItems
                       .Cast<string>());
                        command.Parameters.AddWithValue("@AvailableDays", days);
                        command.Parameters.AddWithValue("@Id", id);
                        Update.Open();
                        command.ExecuteNonQuery();
                        MessageBox.Show("Updating Data Successfull!");
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
                    int id = (int)Convert.ToInt64(dataGridView1.CurrentRow.Cells["DoctorID"].Value);
                    string query = "DELETE  FROM Doctors WHERE DoctorID=@id";
                    using (SqlConnection delete = new SqlConnection(sqlconnection))
                    {
                        SqlCommand command = new SqlCommand(query, delete);
                        command.Parameters.AddWithValue("@id", id);
                        delete.Open();
                        command.ExecuteNonQuery();
                        MessageBox.Show("Data Deleted Sucessfully!");
                        dataload();

                    }
                }
                catch (Exception ex)
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                textBox1.Text = row.Cells["Name"].Value.ToString();
                textBox2.Text = row.Cells["Contact"].Value.ToString();
                textBox3.Text = row.Cells["Email"].Value.ToString();
                comboBox1.SelectedItem = row.Cells["Specialization"].Value.ToString();
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    checkedListBox1.SetItemChecked(i, false);

                
                var raw = row.Cells["AvailableDays"].Value?.ToString();
                if (!string.IsNullOrEmpty(raw))
                {
                    var days = raw.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    
                    foreach (var d in days)
                    {
                        var day = d.Trim();
                        int idx = checkedListBox1.Items.IndexOf(day);
                        if (idx >= 0)
                            checkedListBox1.SetItemChecked(idx, true);
                    }
                }

            }
        }
    }
}
