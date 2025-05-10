using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace HOSPITAL_MANAGEMENT_SYSTEM
{
    public partial class Appointment_Form : Form
    {
        private List<DayOfWeek> allowedDays = new List<DayOfWeek>();
        private DateTime lastValidDate = DateTime.Today;

        string sqlconnection =
          @"Data Source=.\sqlexpress;
            Integrated Security=True;
            Initial Catalog=HospitalDB;
            Connect Timeout=30;
            Encrypt=True;
            TrustServerCertificate=True;
            ApplicationIntent=ReadWrite;
            MultiSubnetFailover=False";

        public Appointment_Form()
        {
            InitializeComponent();
            LoadDoctorData();
            LoadPatientData();

            
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
            monthCalendar1.DateSelected += monthCalendar1_DateSelected;
        }

        private void LoadDoctorData()
        {
            const string sql = "SELECT DoctorID, Name FROM Doctors";
            var dt = new DataTable();
            using (var conn = new SqlConnection(sqlconnection))
            using (var da = new SqlDataAdapter(sql, conn))
            {
                da.Fill(dt);
            }

            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "DoctorID";

            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;
        }

        private void LoadPatientData()
        {
            const string sql = "SELECT PatientID, Name FROM Patients";
            var dt = new DataTable();
            using (var conn = new SqlConnection(sqlconnection))
            using (var da = new SqlDataAdapter(sql, conn))
            {
                da.Fill(dt);
            }

            comboBox2.DataSource = dt;
            comboBox2.DisplayMember = "Name";
            comboBox2.ValueMember = "PatientID";

            if (comboBox2.Items.Count > 0)
                comboBox2.SelectedIndex = 0;
        }

       
        private void EnforceAllowedDate()
        {
            if (allowedDays.Count == 0)
                return;

            DateTime dt = monthCalendar1.SelectionStart;
            int safety = 0;
            while (!allowedDays.Contains(dt.DayOfWeek) && safety++ < 7)
                dt = dt.AddDays(1);

            monthCalendar1.SetDate(dt);
            lastValidDate = dt;
        }

        
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            allowedDays.Clear();

            
            var name = comboBox1.Text;
            if (string.IsNullOrEmpty(name)) return;

            const string sql = "SELECT AvailableDays FROM Doctors WHERE Name = @Name";
            using (var conn = new SqlConnection(sqlconnection))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Name", name);
                conn.Open();
                var raw = (cmd.ExecuteScalar() as string) ?? "";
                allowedDays = raw
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .Where(s => Enum.TryParse<DayOfWeek>(s, out _))
                    .Select(s => (DayOfWeek)Enum.Parse(typeof(DayOfWeek), s))
                    .ToList();
            }

            EnforceAllowedDate();
        }
       
        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            ValidateCalendarSelection(e.Start);
        }


      
        private void ValidateCalendarSelection(DateTime picked)
        {
            if (allowedDays.Count == 0)
            {
                lastValidDate = picked;
                return;
            }

            if (allowedDays.Contains(picked.DayOfWeek))
            {
                lastValidDate = picked;
            }
            else
            {
                MessageBox.Show(
                    $"{picked:dddd, MMM dd yyyy} is not available for this doctor.",
                    "Unavailable Day",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                monthCalendar1.SetDate(lastValidDate);
            }
        }

      

        private void button2_Click(object sender, EventArgs e)
        {
            MainForm home = new MainForm();
            home.Show();
            this.Hide();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) { }
        private void Appointment_Form_Load(object sender, EventArgs e) { }
        private void button1_Click(object sender, EventArgs e)
        {
            int doctorId = (int)comboBox1.SelectedValue;
            int patientId = (int)comboBox2.SelectedValue;
            DateTime datePart = monthCalendar1.SelectionStart.Date;
            TimeSpan timePart = dateTimePicker1.Value.TimeOfDay;
            string notes = textBox1.Text.Trim();

            const string sql = @"
      INSERT INTO Appointments
        (PatientID, DoctorID, [Date], [Time], Notes)
      VALUES
        (@pid, @did, @dt, @tm, @notes)";

            using (var conn = new SqlConnection(sqlconnection))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@pid", patientId);
                cmd.Parameters.AddWithValue("@did", doctorId);
                cmd.Parameters.AddWithValue("@dt", datePart);
                cmd.Parameters.AddWithValue("@tm", timePart);
                cmd.Parameters.AddWithValue("@notes", notes);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show(
              $"Appointment booked on {datePart:d} at {timePart:c}.",
              "Saved",
              MessageBoxButtons.OK,
              MessageBoxIcon.Information
            );
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            ValidateCalendarSelection(e.Start);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
