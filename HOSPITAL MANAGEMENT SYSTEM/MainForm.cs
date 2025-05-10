using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HOSPITAL_MANAGEMENT_SYSTEM
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Patient_Form patientform = new Patient_Form();
            patientform.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Doctor_Formcs doctor = new Doctor_Formcs();
            doctor.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Appointment_Form appointment = new Appointment_Form();
            appointment.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Medical_Records medical = new Medical_Records();
            medical.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form1 logout = new Form1();
            logout.Show();
            this.Hide();
        }
    }
}
