using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace WinFormsApp
{
    public partial class Form1
    {
        // -------------------------------------------------------------
        // BIẾN TOÀN CỤC TAB 3 (THẺ THƯ VIỆN)
        // -------------------------------------------------------------
        private int lastClickedRowIndexTTV = -1;
        private TextBox txtTimTTV = new TextBox();

        // ==============================================================
        // 12. CÁC CHỨC NĂNG XỬ LÝ THẺ THƯ VIỆN (TAB 3)
        // (Tương tự như Thẻ Sinh Viên nhưng có thêm điều kiện Hạn Sử Dụng)
        // ==============================================================
        private void btnThemTTV_Click(object? sender, EventArgs e)
        {
            if (cboMaSV_TTV.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn mã sinh viên!");
                return;
            }

            // Ràng buộc nghiệp vụ: Ngày hết hạn thư viện không được lùi về quá khứ so với ngày cấp
            if (dtpHanSD_TTV.Value.Date < dtpNgayCapTTV.Value.Date)
            {
                MessageBox.Show("Lỗi logic: Hạn sử dụng thẻ thư viện phải diễn ra sau hoặc bằng ngày cấp!");
                return;
            }

            using (SqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string maTheTV = TaoMa("TheThuVien", "MaTheTV", "TTV");
                    string maSV = cboMaSV_TTV.SelectedValue.ToString()!.Trim();

                    string checkCard = "SELECT COUNT(*) FROM dbo.TheThuVien WHERE MaSV = @MaSV";
                    using (SqlCommand checkCmd = new SqlCommand(checkCard, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@MaSV", maSV);
                        if ((int)checkCmd.ExecuteScalar() > 0)
                        {
                            MessageBox.Show("Sinh viên " + maSV + " đã được cấp thẻ thư viện rồi!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    string query = @"
                        INSERT INTO dbo.TheThuVien (MaTheTV, MaSV, NgayCap, HanSuDung, TinhTrang)
                        VALUES (@MaTheTV, @MaSV, @NgayCap, @HanSuDung, @TinhTrang)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaTheTV", maTheTV);
                        cmd.Parameters.AddWithValue("@MaSV", maSV);
                        cmd.Parameters.AddWithValue("@NgayCap", dtpNgayCapTTV.Value.Date);
                        cmd.Parameters.AddWithValue("@HanSuDung", dtpHanSD_TTV.Value.Date);
                        cmd.Parameters.AddWithValue("@TinhTrang", cboTinhTrangTTV.Text);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Cấp thẻ thư viện thành công!\nMã thẻ: " + maTheTV, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadTheTVData();
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cấp thẻ thư viện: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSuaTTV_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaTheTV.Text))
            {
                MessageBox.Show("Vui lòng chọn thẻ thư viện cần cập nhật!");
                return;
            }

            if (dtpHanSD_TTV.Value.Date < dtpNgayCapTTV.Value.Date)
            {
                MessageBox.Show("Hạn sử dụng thẻ thư viện phải sau ngày cấp!");
                return;
            }

            using (SqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = @"
                        UPDATE dbo.TheThuVien
                        SET NgayCap = @NgayCap,
                            HanSuDung = @HanSuDung,
                            TinhTrang = @TinhTrang
                        WHERE MaTheTV = @MaTheTV";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaTheTV", txtMaTheTV.Text.Trim());
                        cmd.Parameters.AddWithValue("@NgayCap", dtpNgayCapTTV.Value.Date);
                        cmd.Parameters.AddWithValue("@HanSuDung", dtpHanSD_TTV.Value.Date);
                        cmd.Parameters.AddWithValue("@TinhTrang", cboTinhTrangTTV.Text);

                        int kq = cmd.ExecuteNonQuery();
                        if (kq > 0)
                        {
                            MessageBox.Show("Cập nhật thẻ thư viện thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadTheTVData();
                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy mã thẻ thư viện cần sửa!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi sửa thẻ thư viện: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnXoaTTV_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaTheTV.Text))
            {
                MessageBox.Show("Vui lòng chọn thẻ thư viện cần xóa!");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa thẻ thư viện " + txtMaTheTV.Text + " không?",
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
                    string query = "DELETE FROM dbo.TheThuVien WHERE MaTheTV = @MaTheTV";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaTheTV", txtMaTheTV.Text.Trim());
                        int kq = cmd.ExecuteNonQuery();

                        if (kq > 0)
                        {
                            MessageBox.Show("Xóa thẻ thư viện thành công!");
                            btnLamMoiTTV_Click(sender, e);
                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy thẻ thư viện!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa thẻ thư viện: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Đổ dữ liệu khi click dòng lưới Tab 3 (Thẻ Thư Viện)
        private void dgvTheTV_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.RowIndex == lastClickedRowIndexTTV)
            {
                dgvTheTV.ClearSelection();
                lastClickedRowIndexTTV = -1;
                btnLamMoiTTV_Click(sender, e);
                return;
            }
            DataGridViewRow row = dgvTheTV.Rows[e.RowIndex];

            txtMaTheTV.Text = row.Cells["Mã Thẻ TV"].Value?.ToString();
            cboMaSV_TTV.SelectedValue = row.Cells["Mã SV"].Value?.ToString() ?? "";
            cboTinhTrangTTV.Text = row.Cells["Tình Trạng"].Value?.ToString();

            if (DateTime.TryParse(row.Cells["Ngày Cấp"].Value?.ToString(), out DateTime ngayCap))
                dtpNgayCapTTV.Value = ngayCap;

            if (DateTime.TryParse(row.Cells["Hạn Sử Dụng"].Value?.ToString(), out DateTime hanSD))
                dtpHanSD_TTV.Value = hanSD;
            lastClickedRowIndexTTV = e.RowIndex;
        }

        // ==============================================================
        // HÀM TẢI DỮ LIỆU & LÀM MỚI TAB 3
        // ==============================================================
        private void LoadTheTVData()
        {
            using (SqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT MaTheTV AS [Mã Thẻ TV], MaSV AS [Mã SV], NgayCap AS [Ngày Cấp], HanSuDung AS [Hạn Sử Dụng], TinhTrang AS [Tình Trạng] FROM dbo.TheThuVien";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvTheTV.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tải dữ liệu thẻ thư viện: " + ex.Message);
                }
            }
        }

        private void btnLamMoiTTV_Click(object? sender, EventArgs e)
        {
            txtTimTTV.Clear();
            txtMaTheTV.Clear();

            if (cboTinhTrangTTV.Items.Count > 0) cboTinhTrangTTV.SelectedIndex = 0;

            dtpNgayCapTTV.Value = DateTime.Now;
            dtpHanSD_TTV.Value = DateTime.Now.AddYears(4);

            dgvTheTV.ClearSelection();
            lastClickedRowIndexTTV = -1;
            LoadTheTVData();
        }
        // Hàm tìm kiếm riêng của Tab 3 (Thẻ Thư Viện)
        private void txtTimTTV_TextChanged(object? sender, EventArgs e)
        {
            if (dgvTheTV.DataSource is DataTable dt)
            {
                string kw = txtTimTTV.Text.Trim().Replace("'", "''");
                dt.DefaultView.RowFilter = string.IsNullOrEmpty(kw) ? "" :
                    $"[Mã Thẻ TV] LIKE '%{kw}%' OR [Mã SV] LIKE '%{kw}%' OR [Tình Trạng] LIKE '%{kw}%'";
                dgvTheTV.ClearSelection();
                lastClickedRowIndexTTV = -1;
            }
        }
    }
}