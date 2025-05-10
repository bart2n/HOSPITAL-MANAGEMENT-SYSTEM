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
    public partial class Form1 : Form
    {
        string sqlconnection = @"Data Source=.\sqlexpress;Integrated Security=True;Initial Catalog=HospitalDB ;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public Form1()
        {
            InitializeComponent();
            textBox2.UseSystemPasswordChar = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM Users WHERE Username=@username AND Password=@password";
                using (SqlConnection login = new SqlConnection(sqlconnection))
                {
                    SqlCommand command = new SqlCommand(query, login);
                    command.Parameters.AddWithValue("@username", textBox1.Text);
                    command.Parameters.AddWithValue("@password", textBox2.Text);
                    login.Open();
                    int result = (int)Convert.ToInt64(command.ExecuteScalar());
                    if (result > 0)
                    {
                        MessageBox.Show("Password is True redirecting you to homepage");
                        MainForm homepage = new MainForm();
                        homepage.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Password or username is wrong!");
                        
                            textBox1.Clear();
                            textBox2.Clear();
                        
                    }

                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.UseSystemPasswordChar = false; // Show password
            }
            else
            {
                textBox2.UseSystemPasswordChar = true; // Hide password
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Registration_Form register = new Registration_Form();
            register.Show();
            this.Hide();
        }
    }
}
