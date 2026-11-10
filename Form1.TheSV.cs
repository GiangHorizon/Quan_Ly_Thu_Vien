using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace WinFormsApp
{
    public partial class Form1
    {
        // -------------------------------------------------------------
        // BIẾN TOÀN CỤC TAB 2 (THẺ SINH VIÊN)
        // -------------------------------------------------------------
        private int lastClickedRowIndexTSV = -1;
        private TextBox txtTimTSV = new TextBox();

        // ==============================================================
        // 9. CHỨC NĂNG CẤP THẺ SINH VIÊN MỚI (TAB 2)
        // ==============================================================
        private void btnThemTSV_Click(object? sender, EventArgs e)
        {
            if (cboMaSV_TSV.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn mã sinh viên cần cấp thẻ!");
                return;
            }

            using (SqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string maTheSV = TaoMa("TheSinhVien", "MaTheSV", "TSV");
                    // Lấy MaSV ẩn phía sau ComboBox (vì mình đang hiển thị dạng "MaSV - Họ Tên")
                    string maSV = cboMaSV_TSV.SelectedValue.ToString()!.Trim(); 

                    // --- CHẶN TRÙNG THẺ: 1 SV chỉ được có 1 thẻ duy nhất ---
                    string checkCard = "SELECT COUNT(*) FROM dbo.TheSinhVien WHERE MaSV = @MaSV";
                    using (SqlCommand checkCmd = new SqlCommand(checkCard, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@MaSV", maSV);
                        if ((int)checkCmd.ExecuteScalar() > 0)
                        {
                            MessageBox.Show("Sinh viên " + maSV + " đã được cấp thẻ sinh viên rồi!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    string query = @"
                        INSERT INTO dbo.TheSinhVien (MaTheSV, MaSV, NgayCap, TinhTrang, GhiChu)
                        VALUES (@MaTheSV, @MaSV, @NgayCap, @TinhTrang, @GhiChu)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaTheSV", maTheSV);
                        cmd.Parameters.AddWithValue("@MaSV", maSV);
                        cmd.Parameters.AddWithValue("@NgayCap", dtpNgayCapTSV.Value.Date);
                        cmd.Parameters.AddWithValue("@TinhTrang", cboTinhTrangTSV.Text);
                        cmd.Parameters.AddWithValue("@GhiChu", string.IsNullOrWhiteSpace(txtGhiChuTSV.Text) ? (object)DBNull.Value : txtGhiChuTSV.Text.Trim());

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Cấp thẻ sinh viên thành công!\nMã thẻ: " + maTheSV, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadTheSVData(); // Hiện thẻ lên Tab 2
                    LoadData();      // Cập nhật thẻ vào bảng tổng hợp ở Tab 1
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cấp thẻ SV: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ==============================================================
        // 10. CHỨC NĂNG CẬP NHẬT TRẠNG THÁI THẺ SINH VIÊN
        // ==============================================================
        private void btnSuaTSV_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaTheSV.Text))
            {
                MessageBox.Show("Vui lòng chọn thẻ sinh viên cần cập nhật từ danh sách!");
                return;
            }

            using (SqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    // Cập nhật dựa vào mã thẻ. Chỉ cập nhật Ngày Cấp, Tình Trạng và Ghi Chú, KHÔNG đổi chủ sở hữu (MaSV)
                    string query = @"
                        UPDATE dbo.TheSinhVien
                        SET NgayCap = @NgayCap,
                            TinhTrang = @TinhTrang,
                            GhiChu = @GhiChu
                        WHERE MaTheSV = @MaTheSV";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaTheSV", txtMaTheSV.Text.Trim());
                        cmd.Parameters.AddWithValue("@NgayCap", dtpNgayCapTSV.Value.Date);
                        cmd.Parameters.AddWithValue("@TinhTrang", cboTinhTrangTSV.Text);
                        cmd.Parameters.AddWithValue("@GhiChu", string.IsNullOrWhiteSpace(txtGhiChuTSV.Text) ? (object)DBNull.Value : txtGhiChuTSV.Text.Trim());

                        int kq = cmd.ExecuteNonQuery();
                        if (kq > 0)
                        {
                            MessageBox.Show("Cập nhật thẻ sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadTheSVData();
                            LoadData(); // Cập nhật trạng thái "Bị Khóa/Đang HĐ" sang bảng Tab 1
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy mã thẻ sinh viên cần sửa!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi sửa thẻ SV: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ==============================================================
        // 11. CHỨC NĂNG XÓA THẺ SINH VIÊN (THU HỒI THẺ)
        // ==============================================================
        private void btnXoaTSV_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaTheSV.Text))
            {
                MessageBox.Show("Vui lòng chọn thẻ sinh viên cần xóa!");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa thẻ sinh viên " + txtMaTheSV.Text + " không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result != DialogResult.Yes) return;

            using (SqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM dbo.TheSinhVien WHERE MaTheSV = @MaTheSV";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaTheSV", txtMaTheSV.Text.Trim());
                        int kq = cmd.ExecuteNonQuery();

                        if (kq > 0)
                        {
                            MessageBox.Show("Xóa thẻ sinh viên thành công!");
                            btnLamMoiTSV_Click(sender, e); // Xóa trắng phần ô nhập và thanh tìm kiếm
                            LoadData(); // Tự động trả trạng thái "Chưa có thẻ" về Tab 1
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy thẻ sinh viên!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa thẻ SV: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Đổ dữ liệu khi click dòng lưới của Tab 2 (Thẻ Sinh Viên)
        private void dgvTheSV_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.RowIndex == lastClickedRowIndexTSV)
            {
                dgvTheSV.ClearSelection();
                lastClickedRowIndexTSV = -1;
                btnLamMoiTSV_Click(sender, e);
                return;
            }
            DataGridViewRow row = dgvTheSV.Rows[e.RowIndex];

            txtMaTheSV.Text = row.Cells["Mã Thẻ SV"].Value?.ToString();
            cboMaSV_TSV.SelectedValue = row.Cells["Mã SV"].Value?.ToString() ?? "";
            cboTinhTrangTSV.Text = row.Cells["Tình Trạng"].Value?.ToString();
            txtGhiChuTSV.Text = row.Cells["Ghi Chú"].Value?.ToString();

            if (DateTime.TryParse(row.Cells["Ngày Cấp"].Value?.ToString(), out DateTime ngayCap))
                dtpNgayCapTSV.Value = ngayCap;
            lastClickedRowIndexTSV = e.RowIndex;
        }

        // ==============================================================
        // HÀM TẢI DỮ LIỆU & LÀM MỚI TAB 2
        // ==============================================================
        private void LoadTheSVData()
        {
            using (SqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT MaTheSV AS [Mã Thẻ SV], MaSV AS [Mã SV], NgayCap AS [Ngày Cấp], TinhTrang AS [Tình Trạng], GhiChu AS [Ghi Chú] FROM dbo.TheSinhVien";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvTheSV.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tải dữ liệu thẻ SV: " + ex.Message);
                }
            }
        }

        private void btnLamMoiTSV_Click(object? sender, EventArgs e)
        {
            txtTimTSV.Clear();
            txtMaTheSV.Clear();
            txtGhiChuTSV.Clear();

            if (cboTinhTrangTSV.Items.Count > 0) cboTinhTrangTSV.SelectedIndex = 0;
            dtpNgayCapTSV.Value = DateTime.Now;

            dgvTheSV.ClearSelection();
            lastClickedRowIndexTSV = -1;
            LoadTheSVData();
        }
    }
}