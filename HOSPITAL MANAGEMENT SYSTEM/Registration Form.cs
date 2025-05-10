using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;

namespace HOSPITAL_MANAGEMENT_SYSTEM
{
    public partial class Registration_Form : Form
    {
        
       string sqlconnection = @"Data Source=.\sqlexpress;Integrated Security=True;Initial Catalog=HospitalDB ;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public Registration_Form()
        {
            InitializeComponent();
            comboBox1.Items.Add("Admin");
            comboBox1.Items.Add("Staff");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox2.Text.Length != textBox3.Text.Length)
                {
                    MessageBox.Show("Password Do not match!");
                }
                else
                {
                    string query = "INSERT INTO Users(Username,Password,Role) VALUES(@Username,@Password,@Role)";
                    using (SqlConnection register = new SqlConnection(sqlconnection))
                    {
                        SqlCommand command = new SqlCommand(query, register);
                        command.Parameters.AddWithValue("@Username", textBox1.Text);
                        command.Parameters.AddWithValue("@Password", textBox2.Text);
                        command.Parameters.AddWithValue("@Role", comboBox1.SelectedItem.ToString());
                        register.Open();
                        command.ExecuteNonQuery();
                        MessageBox.Show("Registeration Successful redirecting you to Loginpage!");
                        Form1 login = new Form1();
                        login.Show();
                        this.Hide();


                    }
                }
            }catch(Exception ex){
                MessageBox.Show(ex.Message);
            }
        }
    }
}
