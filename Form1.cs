using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace _492989_Daniel_Anantadhirya_A.L._ResponsiJuniorProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private NpgsqlConnection conn;
        string connstring = "Host=localhost;Port=5432;Username=postgres;Password=informatika;Database=Responsi";
        public DataTable dt;
        public static NpgsqlCommand cmd;
        private string sql { get; set; } = null;
        private DataGridViewRow r { get; set; }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connstring);
            btnLoad.PerformClick();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                dgvData.DataSource = null;
                sql = "select * from select_karyawan()";
                cmd = new NpgsqlCommand(sql, conn);
                dt = new DataTable();
                NpgsqlDataReader rd = cmd.ExecuteReader();
                dt.Load(rd);
                dgvData.DataSource = dt;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "FAIL!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int getDepartmentId(string department)
        {
            switch (department)
            {
                case "HR": return 1;
                case "Engineer": return 2;
                case "Developer": return 3;
                case "Product M": return 4;
                case "Finance": return 5;
                default: return -1;
            }
        }

        private int getJabatanId(string jabatan)
        {
            switch (jabatan)
            {
                case "Intern": return 1;
                case "Junior": return 2;
                case "Senior": return 3;
                default: return -1;
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (cbDepartemen.SelectedItem == null)
            {
                MessageBox.Show("Departemen tidak boleh kosong", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbJabatan.SelectedItem == null)
            {
                MessageBox.Show("Jabatan tidak boleh kosong", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int dept_id = getDepartmentId(cbDepartemen.SelectedItem.ToString());
            int jabatan_id = getJabatanId(cbJabatan.SelectedItem.ToString());
            try
            {
                conn.Open();
                sql = @"select * from insert_karyawan(:_nama,:_id_departemen,:_id_jabatan)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_nama", tbNama.Text);
                cmd.Parameters.AddWithValue("_id_departemen", dept_id);
                cmd.Parameters.AddWithValue("_id_jabatan", jabatan_id);
                if ((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("Data Users Berhasil diinputkan", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    btnLoad.PerformClick();
                    tbNama.Text = null;
                    cbDepartemen.SelectedItem = cbJabatan.SelectedItem = null;
                }
                else
                {
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Insert FAIL!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                r = dgvData.Rows[e.RowIndex];
                tbNama.Text = r.Cells["_nama"].Value.ToString();
                cbDepartemen.SelectedItem = r.Cells["_departemen"].Value.ToString();
                cbJabatan.SelectedItem = r.Cells["_jabatan"].Value.ToString(); ;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (r == null)
            {
                MessageBox.Show("Mohon pilih baris data yang akan diedit", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                conn.Open();
                sql = @"select * from update_karyawan(:_id,:_nama,:_id_departemen,:_id_jabatan)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_id", r.Cells["_id"].Value);
                cmd.Parameters.AddWithValue("_nama", tbNama.Text);
                int dept_id = getDepartmentId(cbDepartemen.SelectedItem.ToString());
                int jabatan_id = getJabatanId(cbJabatan.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("_id_departemen", dept_id);
                cmd.Parameters.AddWithValue("_id_jabatan", jabatan_id);
                if ((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("Data Users Berhasil diupdate", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    btnLoad.PerformClick();
                    tbNama.Text = null;
                    cbDepartemen.SelectedItem = cbJabatan.SelectedItem = null;
                    r = null;
                }
                else
                {
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Update FAIL!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            if (r == null)
            {
                MessageBox.Show("Mohon pilih baris data yang akan didelete", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Apakah benar anda ingin menghapus data " + r.Cells["_nama"].Value.ToString() + " ?", "Hapus data terkonfirmasi",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                try
                {
                    conn.Open();
                    sql = @"select * from delete_karyawan(:_id)";
                    cmd = new NpgsqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("_id", r.Cells["_id"].Value.ToString());
                    if ((int)cmd.ExecuteScalar() == 1)
                    {
                        MessageBox.Show("Data Users Berhasil dihapus", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        conn.Close();
                        btnLoad.PerformClick();
                        tbNama.Text = null;
                        cbDepartemen.SelectedItem = cbJabatan.SelectedItem = null;
                        r = null;
                    }
                    else
                    {
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Delete FAIL!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
