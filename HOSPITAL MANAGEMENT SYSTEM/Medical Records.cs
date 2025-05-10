using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HOSPITAL_MANAGEMENT_SYSTEM
{
    public partial class Medical_Records : Form
    {
        // your connection string
        private readonly string _connString =
          @"Data Source=.\sqlexpress;
            Initial Catalog=HospitalDB;
            Integrated Security=True;
            Connect Timeout=30;
            Encrypt=True;
            TrustServerCertificate=True;
            ApplicationIntent=ReadWrite;
            MultiSubnetFailover=False";

        public Medical_Records()
        {
            InitializeComponent();

            // wire up events
            

            // initial load
            LoadData();
            dataGridView1.CellClick += dataGridView1_CellClick;

        }

        private void LoadData()
        {
            using (var conn = new SqlConnection(_connString))
            {
                using (var da = new SqlDataAdapter("SELECT * FROM MedicalRecords", conn))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            
            
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            const string sql = @"
                INSERT INTO MedicalRecords
                  (PatientID, Diagnosis, Treatment, Prescriptions, [Date])
                VALUES
                  (@pid, @diag, @treat, @presc, @dt)";

            using (var conn = new SqlConnection(_connString))
            {
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@pid", txtPatientID.Text.Trim());
                    cmd.Parameters.AddWithValue("@diag", txtDiagnosis.Text.Trim());
                    cmd.Parameters.AddWithValue("@treat", txtTreatment.Text.Trim());
                    cmd.Parameters.AddWithValue("@presc", txtPrescriptions.Text.Trim());
                    cmd.Parameters.AddWithValue("@dt", dtpRecordDate.Value);
                    conn.Open();
                        cmd.ExecuteNonQuery();
                }
            }
        



            MessageBox.Show("Record added.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadData();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > 0) { 

                var row = dataGridView1.Rows[e.RowIndex];
                txtPatientID.Text = row.Cells["PatientID"].Value.ToString();
                txtDiagnosis.Text = row.Cells["Diagnosis"].Value.ToString();
                txtTreatment.Text = row.Cells["Treatment"].Value.ToString();
                txtPrescriptions.Text = row.Cells["Prescriptions"].Value.ToString();
                dtpRecordDate.Value = DateTime.Today
                    .Add((TimeSpan)row.Cells["Date"].Value);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["RecordID"].Value);
            const string sql = @"
                UPDATE MedicalRecords
                   SET PatientID     = @pid
                     , Diagnosis    = @diag
                     , Treatment    = @treat
                     , Prescriptions= @presc
                     , [Date]       = @dt
                 WHERE RecordID     = @id";

            using (var conn = new SqlConnection(_connString))
            {
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@pid", txtPatientID.Text.Trim());
                    cmd.Parameters.AddWithValue("@diag", txtDiagnosis.Text.Trim());
                    cmd.Parameters.AddWithValue("@treat", txtTreatment.Text.Trim());
                    cmd.Parameters.AddWithValue("@presc", txtPrescriptions.Text.Trim());
                    cmd.Parameters.AddWithValue("@dt", dtpRecordDate.Value.TimeOfDay);
                    cmd.Parameters.AddWithValue("@id", id);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

            }
            MessageBox.Show("Record updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["RecordID"].Value);
            const string sql = @"DELETE FROM MedicalRecords WHERE RecordID = @id";

            using (var conn = new SqlConnection(_connString))
            {
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Record deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadData();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // simply forward to your real handler:
            btnView_Click(sender, e);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            btnUpdate_Click(sender, e);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            btnAdd_Click(sender, e);
        }
        private void Medical_Records_Load(object sender, EventArgs e)
        {
            // if you want to do anything on load, otherwise leave empty
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            btnDelete_Click(sender, e);
        }

        private void txtPatientID_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            MainForm main = new MainForm();
            main.Show();
            this.Hide();
        }
    }
}
