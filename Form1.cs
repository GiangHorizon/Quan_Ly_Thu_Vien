
using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        // Biến toàn cục để nhớ vị trí dòng vừa click ở Tab Sinh Viên
        private int lastClickedRowIndexSV = -1;
        private int lastClickedRowIndexTSV = -1;
        private int lastClickedRowIndexTTV = -1;

        private readonly DatabaseHelper dbHelper = new DatabaseHelper();

        public Form1()
        {
            InitializeComponent();
        }

        
        // =========================
        // 1. KHOI TAO FORM
        // =========================
        private void Form1_Load(object? sender, EventArgs e)
        {
            GioiTinh.SelectedIndex = 0;
            cboTinhTrangTSV.SelectedIndex = 0;
            cboTinhTrangTTV.SelectedIndex = 0;

            dtbNgaySinh.Value = DateTime.Now.AddYears(-18);
            dtpNgayCapTSV.Value = DateTime.Now;
            dtpNgayCapTTV.Value = DateTime.Now;
            dtpHanSD_TTV.Value = DateTime.Now.AddYears(4);

            // Cấu hình chọn cả dòng cho cả 3 bảng
            DataGridView[] grids = { dgvSinhVien, dgvTheSV, dgvTheTV };
            foreach (var dgv in grids)
            {
                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgv.MultiSelect = false;
                dgv.ReadOnly = true;
                dgv.AllowUserToAddRows = false;
            }

            // Giúp click vào bất kỳ vị trí nào trên ô của bảng Tab 1 đều nhận (không cần click trúng chữ)
            // dgvSinhVien.CellClick += dgvSinhVien_CellContentClick;

            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvSinhVien.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvTheSV.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvTheTV.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            txtMaSV.ReadOnly = true;
            txtMaTheSV.ReadOnly = true;
            txtMaTheTV.ReadOnly = true;

            txtMaSV.BackColor = Color.LightGray;
            txtMaTheSV.BackColor = Color.LightGray;
            txtMaTheTV.BackColor = Color.LightGray;

            // Tạo nút Làm mới cho Tab 2 (nằm ngay bên phải nút Xóa TSV, cùng kích thước)
            if (btnXoaTSV.Parent != null)
            {
                Button btnMoiTab2 = new Button();
                btnMoiTab2.Text = "Làm mới";
                btnMoiTab2.Size = btnXoaTSV.Size;
                btnMoiTab2.Location = new Point(btnXoaTSV.Right + 12, btnXoaTSV.Top);
                btnMoiTab2.Click += btnLamMoiTSV_Click;
                btnXoaTSV.Parent.Controls.Add(btnMoiTab2);
                btnMoiTab2.BringToFront();
            }

            // Tạo nút Làm mới cho Tab 3 (nằm ngay bên phải nút Xóa TTV, cùng kích thước)
            if (btnXoaTTV.Parent != null)
            {
                Button btnMoiTab3 = new Button();
                btnMoiTab3.Text = "Làm mới";
                btnMoiTab3.Size = btnXoaTTV.Size;
                btnMoiTab3.Location = new Point(btnXoaTTV.Right + 12, btnXoaTTV.Top);
                btnMoiTab3.Click += btnLamMoiTTV_Click;
                btnXoaTTV.Parent.Controls.Add(btnMoiTab3);
                btnMoiTab3.BringToFront();
            }

            LoadData();
            LoadTheSVData();
            LoadTheTVData();
            LoadComboBoxMaSV();
        }
        // =========================
        // 2. HIEN THI DU LIEU
        // =========================
        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = dbHelper.GetConnection())
                {
                    conn.Open();

                    string query = @"
                        SELECT
                            sv.MaSV AS [Mã SV],
                            sv.HoTen AS [Họ Tên],
                            sv.NgaySinh AS [Ngày Sinh],
                            sv.GioiTinh AS [Giới Tính],
                            sv.Lop AS [Lớp],
                            sv.Khoa AS [Khoa],
                            sv.SoDienThoai AS [Số Điện Thoại],
                            sv.Email AS [Email],

                            tsv.MaTheSV AS [Mã Thẻ SV],
                            tsv.TinhTrang AS [Thẻ SV],
                            tsv.NgayCap AS [Ngày Cấp Thẻ SV],

                            ttv.MaTheTV AS [Mã Thẻ TV],
                            ttv.TinhTrang AS [Thẻ Thư Viện],
                            ttv.NgayCap AS [Ngày Cấp Thẻ TV],
                            ttv.HanSuDung AS [Ngày Hết Hạn]

                        FROM dbo.SinhVien sv

                        LEFT JOIN dbo.TheSinhVien tsv
                            ON sv.MaSV = tsv.MaSV

                        LEFT JOIN dbo.TheThuVien ttv
                            ON sv.MaSV = ttv.MaSV

                        ORDER BY sv.MaSV";

                    SqlDataAdapter adapter =
                        new SqlDataAdapter(query, conn);

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvSinhVien.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi lấy dữ liệu: " + ex.Message,
                    "Thông báo lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // =========================
        // 3. TAO MA TU DONG
        // =========================
        private string TaoMa(string tenBang, string tenCot, string tienTo)
        {
            using (SqlConnection conn = dbHelper.GetConnection())
            {
                conn.Open();

                string query = $@"
                    SELECT ISNULL(MAX(
                        TRY_CONVERT(INT, SUBSTRING({tenCot}, LEN(@TienTo) + 1, 20))
                    ), 0) + 1
                    FROM {tenBang}
                    WHERE {tenCot} LIKE @TienTo + '%'";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TienTo", tienTo);

                    int so = Convert.ToInt32(cmd.ExecuteScalar());

                    return tienTo + so.ToString("D3");
                }
            }
        }

        // =========================
        // 4. KIEM TRA DU LIEU
        // =========================
        private bool KiemTraDuLieu()
        {
            if (string.IsNullOrWhiteSpace(txtTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên sinh viên!");
                txtTen.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtLop.Text))
            {
                MessageBox.Show("Vui lòng nhập lớp!");
                txtLop.Focus();
                return false;
            }

            if (dtbNgaySinh.Value.Date >= DateTime.Now.Date)
            {
                MessageBox.Show("Ngày sinh không hợp lệ!");
                return false;
            }

            return true;
        }

        // =========================
        // 5. HIEN THI LEN CAC O NHAP
        // =========================
        private void dgvSinhVien_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.RowIndex == lastClickedRowIndexSV)
            {
                dgvSinhVien.ClearSelection();
                lastClickedRowIndexSV = -1;

                txtMaSV.Clear();
                txtTen.Clear();
                txtLop.Clear();
                txtKhoa.Clear();
                txtSDT.Clear();
                txtEmail.Clear();
                GioiTinh.SelectedIndex = 0;
                dtbNgaySinh.Value = DateTime.Now.AddYears(-18);
                return; 
            }

            DataGridViewRow row = dgvSinhVien.Rows[e.RowIndex];

            txtMaSV.Text = row.Cells["Mã SV"].Value?.ToString();
            txtTen.Text = row.Cells["Họ Tên"].Value?.ToString();
            txtLop.Text = row.Cells["Lớp"].Value?.ToString();
            GioiTinh.Text = row.Cells["Giới Tính"].Value?.ToString();
            txtKhoa.Text = row.Cells["Khoa"].Value?.ToString();
            txtSDT.Text = row.Cells["Số Điện Thoại"].Value?.ToString();
            txtEmail.Text = row.Cells["Email"].Value?.ToString();

            if (DateTime.TryParse(row.Cells["Ngày Sinh"].Value?.ToString(), out DateTime ngaySinh))
                dtbNgaySinh.Value = ngaySinh;

            lastClickedRowIndexSV = e.RowIndex;
        }

        // =========================
        // 6. THEM SINH VIEN
        // =========================
        private void btnThem_Click(object? sender, EventArgs e)
        {
            if (!KiemTraDuLieu()) return;

            using (SqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    // ==========================================================
                    // KIỂM TRA TRÙNG LẶP SỐ ĐIỆN THOẠI (Cho phép trùng tên)
                    // ==========================================================
                    string sdt = txtSDT.Text.Trim();
                    
                    // Chỉ kiểm tra nếu ô Số điện thoại không bị bỏ trống
                    if (!string.IsNullOrEmpty(sdt)) 
                    {
                        string checkQuery = "SELECT COUNT(*) FROM SinhVien WHERE SoDienThoai = @CheckSDT";
                        using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                        {
                            checkCmd.Parameters.AddWithValue("@CheckSDT", sdt);
                            
                            int count = (int)checkCmd.ExecuteScalar();
                            if (count > 0)
                            {
                                MessageBox.Show("Số điện thoại này đã tồn tại trong hệ thống! Vui lòng kiểm tra lại.", "Cảnh báo trùng lặp", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return; // Dừng việc thêm sinh viên
                            }
                        }
                    }
                    // ==========================================================
                    string maSV = TaoMa("SinhVien", "MaSV", "SV");

                    // Bổ sung Khoa, SDT, Email vào câu lệnh INSERT
                    string sqlSV = @"
                        INSERT INTO SinhVien (MaSV, HoTen, NgaySinh, GioiTinh, Lop, Khoa, SoDienThoai, Email)
                        VALUES (@MaSV, @HoTen, @NgaySinh, @GioiTinh, @Lop, @Khoa, @SoDienThoai, @Email)";

                    using (SqlCommand cmd = new SqlCommand(sqlSV, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaSV", maSV);
                        cmd.Parameters.AddWithValue("@HoTen", txtTen.Text.Trim());
                        cmd.Parameters.AddWithValue("@NgaySinh", dtbNgaySinh.Value.Date);
                        cmd.Parameters.AddWithValue("@GioiTinh", GioiTinh.Text);
                        cmd.Parameters.AddWithValue("@Lop", txtLop.Text.Trim());
                        
                        // Xử lý NULL nếu người dùng không nhập
                        cmd.Parameters.AddWithValue("@Khoa", string.IsNullOrWhiteSpace(txtKhoa.Text) ? (object)DBNull.Value : txtKhoa.Text.Trim());
                        cmd.Parameters.AddWithValue("@SoDienThoai", string.IsNullOrWhiteSpace(txtSDT.Text) ? (object)DBNull.Value : txtSDT.Text.Trim());
                        cmd.Parameters.AddWithValue("@Email", string.IsNullOrWhiteSpace(txtEmail.Text) ? (object)DBNull.Value : txtEmail.Text.Trim());

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Thêm sinh viên thành công!\nMã SV: " + maSV, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    LoadData(); // Cập nhật lưới Tab 1
                    LoadComboBoxMaSV(); // Cập nhật ComboBox bên Tab 2 và Tab 3 để họ có thể cấp thẻ
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm sinh viên: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // =========================
        // 8. SUA SINH VIEN
        // =========================
        private void btnSua_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSV.Text))
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần sửa!");
                return;
            }

            if (!KiemTraDuLieu()) return;

            using (SqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();

                    // Kiểm tra SĐT có bị trùng với sinh viên KHÁC không (MaSV <> @MaSV)
                    string sdt = txtSDT.Text.Trim();
                    if (!string.IsNullOrEmpty(sdt))
                    {
                        string checkQuery = "SELECT COUNT(*) FROM SinhVien WHERE SoDienThoai = @CheckSDT AND MaSV <> @MaSV";
                        using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                        {
                            checkCmd.Parameters.AddWithValue("@CheckSDT", sdt);
                            checkCmd.Parameters.AddWithValue("@MaSV", txtMaSV.Text.Trim());
                            if ((int)checkCmd.ExecuteScalar() > 0)
                            {
                                MessageBox.Show("Số điện thoại này đã thuộc về một sinh viên khác!", "Cảnh báo trùng lặp", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }

                    string sqlSV = @"
                        UPDATE SinhVien
                        SET HoTen = @HoTen,
                            NgaySinh = @NgaySinh,
                            GioiTinh = @GioiTinh,
                            Lop = @Lop,
                            Khoa = @Khoa,
                            SoDienThoai = @SoDienThoai,
                            Email = @Email
                        WHERE MaSV = @MaSV";

                    using (SqlCommand cmd = new SqlCommand(sqlSV, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaSV", txtMaSV.Text.Trim());
                        cmd.Parameters.AddWithValue("@HoTen", txtTen.Text.Trim());
                        cmd.Parameters.AddWithValue("@NgaySinh", dtbNgaySinh.Value.Date);
                        cmd.Parameters.AddWithValue("@GioiTinh", GioiTinh.Text);
                        cmd.Parameters.AddWithValue("@Lop", txtLop.Text.Trim());
                        cmd.Parameters.AddWithValue("@Khoa", string.IsNullOrWhiteSpace(txtKhoa.Text) ? (object)DBNull.Value : txtKhoa.Text.Trim());
                        cmd.Parameters.AddWithValue("@SoDienThoai", string.IsNullOrWhiteSpace(txtSDT.Text) ? (object)DBNull.Value : txtSDT.Text.Trim());
                        cmd.Parameters.AddWithValue("@Email", string.IsNullOrWhiteSpace(txtEmail.Text) ? (object)DBNull.Value : txtEmail.Text.Trim());

                        if (cmd.ExecuteNonQuery() == 0)
                            throw new Exception("Không tìm thấy sinh viên!");
                    }

                    MessageBox.Show("Cập nhật thông tin sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi sửa: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // =========================
        // 9. XOA SINH VIEN
        // =========================
        private void btnXoa_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSV.Text))
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần xóa!");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa sinh viên " +
                txtMaSV.Text + " và các thẻ liên quan không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result != DialogResult.Yes)
                return;

            using (SqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();

                    // Xóa thẻ liên quan trước để không bị lỗi Khóa ngoại (Foreign Key)
                    string query = @"
                        DELETE FROM dbo.TheSinhVien WHERE MaSV = @MaSV;
                        DELETE FROM dbo.TheThuVien WHERE MaSV = @MaSV;
                        DELETE FROM dbo.SinhVien WHERE MaSV = @MaSV;";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaSV", txtMaSV.Text.Trim());
                        int kq = cmd.ExecuteNonQuery();

                        if (kq > 0)
                        {
                            MessageBox.Show("Xóa sinh viên thành công!");
                            btnLamMoi_Click(sender, e);
                            LoadTheSVData();
                            LoadTheTVData();
                            LoadComboBoxMaSV();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy sinh viên!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Lỗi khi xóa: " + ex.Message,
                        "Thông báo lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
        }

        // =========================
        // 10. LAM MOI DU LIEU
        // =========================
        private void btnHienThi_Click(object? sender, EventArgs e)
        {
            btnLamMoi_Click(sender, e);
        }
        // =========================
        // 11. XỬ LÝ THẺ SINH VIÊN
        // =========================
        private void btnThemTSV_Click(object? sender, EventArgs e)
        {
            if (cboMaSV_TSV.SelectedValue == null && cboMaSV_TSV.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn mã sinh viên!");
                return;
            }

            using (SqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string maTheSV = TaoMa("TheSinhVien", "MaTheSV", "TSV");
                    string maSV = cboMaSV_TSV.Text.Trim();
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
                    LoadTheSVData();
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cấp thẻ SV: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSuaTSV_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaTheSV.Text))
            {
                MessageBox.Show("Vui lòng chọn thẻ sinh viên cần cập nhật!");
                return;
            }

            using (SqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
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
                            LoadData();
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
                            LoadTheSVData();
                            LoadData();
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

        private void dgvTheSV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.RowIndex == lastClickedRowIndexTSV)
            {
                dgvTheSV.ClearSelection();
                lastClickedRowIndexTSV = -1;
                btnLamMoiTSV_Click(sender, e); // Gọi luôn hàm làm mới cho lẹ
                return;
            }
            DataGridViewRow row = dgvTheSV.Rows[e.RowIndex];

            txtMaTheSV.Text = row.Cells["Mã Thẻ SV"].Value?.ToString();
            cboMaSV_TSV.Text = row.Cells["Mã SV"].Value?.ToString();
            cboTinhTrangTSV.Text = row.Cells["Tình Trạng"].Value?.ToString();
            txtGhiChuTSV.Text = row.Cells["Ghi Chú"].Value?.ToString();

            if (DateTime.TryParse(row.Cells["Ngày Cấp"].Value?.ToString(), out DateTime ngayCap))
                dtpNgayCapTSV.Value = ngayCap;
            lastClickedRowIndexTSV = e.RowIndex;
        }

        // =========================
        // 12. XỬ LÝ THẺ THƯ VIỆN
        // =========================
        private void btnThemTTV_Click(object? sender, EventArgs e)
        {
            if (cboMaSV_TTV.SelectedValue == null && cboMaSV_TTV.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn mã sinh viên!");
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
                    string maTheTV = TaoMa("TheThuVien", "MaTheTV", "TTV");
                    string maSV = cboMaSV_TTV.Text.Trim();
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
                            LoadTheTVData();
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
            cboMaSV_TTV.Text = row.Cells["Mã SV"].Value?.ToString();
            cboTinhTrangTTV.Text = row.Cells["Tình Trạng"].Value?.ToString();

            if (DateTime.TryParse(row.Cells["Ngày Cấp"].Value?.ToString(), out DateTime ngayCap))
                dtpNgayCapTTV.Value = ngayCap;

            if (DateTime.TryParse(row.Cells["Hạn Sử Dụng"].Value?.ToString(), out DateTime hanSD))
                dtpHanSD_TTV.Value = hanSD;
            lastClickedRowIndexTTV = e.RowIndex;
        }

        // =========================
        // 13. HỖ TRỢ LOAD DỮ LIỆU THẺ & COMBOBOX
        // =========================
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

        private void LoadComboBoxMaSV()
        {
            using (SqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT MaSV FROM dbo.SinhVien";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    cboMaSV_TSV.DataSource = dt;
                    cboMaSV_TSV.DisplayMember = "MaSV";
                    cboMaSV_TSV.ValueMember = "MaSV";

                    // Tạo bản sao DataTable cho cboMaSV_TTV để độc lập DataSource
                    DataTable dt2 = dt.Copy();
                    cboMaSV_TTV.DataSource = dt2;
                    cboMaSV_TTV.DisplayMember = "MaSV";
                    cboMaSV_TTV.ValueMember = "MaSV";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tải danh sách mã sinh viên: " + ex.Message);
                }
            }
        }
        //======================
        //Nút làm mới Tab1
        //======================
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            // 1. Xóa trắng toàn bộ TextBox nhập liệu
            txtMaSV.Clear(); 
            txtTen.Clear();
            txtLop.Clear();
            txtKhoa.Clear();
            txtSDT.Clear();
            txtEmail.Clear();

            // 2. Đưa ComboBox và Ngày sinh về giá trị mặc định ban đầu
            if (GioiTinh.Items.Count > 0)
            {
                GioiTinh.SelectedIndex = 0;
            }
            dtbNgaySinh.Value = DateTime.Now.AddYears(-18);

            // 3. Xóa dải màu xanh đang được chọn trên bảng dữ liệu
            dgvSinhVien.ClearSelection();

            // 4. Reset lại biến nhớ dòng (biến mà chúng ta vừa tạo ở bước trước)
            lastClickedRowIndexSV = -1;

            // 5. Tải lại toàn bộ dữ liệu mới nhất từ cơ sở dữ liệu
            LoadData();
        }
        // =========================================================
        // NÚT LÀM MỚI TAB 2 (CẤP THẺ SINH VIÊN)
        // =========================================================
        private void btnLamMoiTSV_Click(object sender, EventArgs e)
        {
            // Xóa trắng TextBox
            txtMaTheSV.Clear();
            
            // Xóa trắng ô Ghi chú (Kiểm tra lại tên biến txtGhiChu của bạn xem đúng chưa nhé)
            // Nếu bạn đặt tên khác thì sửa lại tên ở dòng dưới
            txtGhiChuTSV.Clear(); 

            // Đưa ComboBox Tình trạng về mặc định
            if (cboTinhTrangTSV.Items.Count > 0) cboTinhTrangTSV.SelectedIndex = 0;
            
            // Đưa ngày cấp về hôm nay
            dtpNgayCapTSV.Value = DateTime.Now;

            // Xóa dòng đang chọn và tải lại dữ liệu
            dgvTheSV.ClearSelection();
            lastClickedRowIndexTSV = -1;
            LoadTheSVData(); 
        }

        // =========================================================
        // NÚT LÀM MỚI TAB 3 (CẤP THẺ THƯ VIỆN)
        // =========================================================
        private void btnLamMoiTTV_Click(object sender, EventArgs e)
        {
            txtMaTheTV.Clear();

            if (cboTinhTrangTTV.Items.Count > 0) cboTinhTrangTTV.SelectedIndex = 0;

            dtpNgayCapTTV.Value = DateTime.Now;
            dtpHanSD_TTV.Value = DateTime.Now.AddYears(4);

            dgvTheTV.ClearSelection();
            lastClickedRowIndexTTV = -1;
            LoadTheTVData(); 
        }
        
    }
}
