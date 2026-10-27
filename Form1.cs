using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;
namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnHienThi_Click(object sender, EventArgs e)
        {
            DatabaseHelper dbHelper = new DatabaseHelper();

            using (NpgsqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();

                    string query = "SELECT * FROM SinhVien";

                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();

                    adapter.Fill(dt);

                    dgvSinhVien.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Loi lay du lieu: " + ex.Message, "Thong bao loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvSinhVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void txtMaSV_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtTen_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtLop_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            DatabaseHelper dbHelper = new DatabaseHelper();
            using (NpgsqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();

                    // Input
                    string query = "INSERT INTO SinhVien (MaSV, HoTen, NgaySinh, GioiTinh, Lop) VALUES (@MaSV, @HoTen, @NgaySinh, @GioiTinh, @Lop)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        // Gan gia tri
                        cmd.Parameters.AddWithValue("@MaSV", txtMaSV.Text);
                        cmd.Parameters.AddWithValue("@HoTen", txtTen.Text);
                        cmd.Parameters.AddWithValue("@NgaySinh", dtbNgaySinh.Value.Date);
                        cmd.Parameters.AddWithValue("@GioiTinh", GioiTinh.Text);
                        cmd.Parameters.AddWithValue("@Lop", txtLop.Text);

                        // Thay doi
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Thêm sinh viên thành công!", "Thông báo");

                        // Cập nhật 
                        btnHienThi_Click(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DatabaseHelper dbHelper = new DatabaseHelper();
            using (NpgsqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();

                    // Input
                    string query = "DELETE FROM SinhVien WHERE MaSV = @MaSV";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        // Gán giá trị
                        cmd.Parameters.AddWithValue("@MaSV", txtMaSV.Text);
                        cmd.Parameters.AddWithValue("@HoTen", txtTen.Text);
                        cmd.Parameters.AddWithValue("@NgaySinh", dtbNgaySinh.Value.Date);
                        cmd.Parameters.AddWithValue("@GioiTinh", GioiTinh.Text);
                        cmd.Parameters.AddWithValue("@Lop", txtLop.Text);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Xóa sinh viên thành công!", "Thông báo");

                        btnHienThi_Click(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            DatabaseHelper dbHelper = new DatabaseHelper();
            using (NpgsqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();

                    string query = "UPDATE SinhVien SET HoTen = @HoTen, Lop = @Lop WHERE MaSV = @MaSV";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaSV", txtMaSV.Text);
                        cmd.Parameters.AddWithValue("@HoTen", txtTen.Text);
                        cmd.Parameters.AddWithValue("@NgaySinh", dtbNgaySinh.Value.Date);
                        cmd.Parameters.AddWithValue("@GioiTinh", GioiTinh.Text);
                        cmd.Parameters.AddWithValue("@Lop", txtLop.Text);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Sửa sinh viên thành công!", "Thông báo");

                        btnHienThi_Click(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi sửa : " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void GioiTinh_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
