using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        // -------------------------------------------------------------
        // BIẾN TOÀN CỤC TAB 1 & DÙNG CHUNG (GLOBAL VARIABLES)
        // -------------------------------------------------------------
        // Biến dùng để ghi nhớ chỉ số (Index) của dòng đang được click trên bảng Sinh Viên.
        // Giá trị -1 nghĩa là hiện tại không có dòng nào đang được chọn.
        private int lastClickedRowIndexSV = -1;

        // Khai báo đối tượng TextBox dùng làm thanh tìm kiếm cho Tab 1
        private TextBox txtTimSV = new TextBox();

        // Khởi tạo đối tượng kết nối với cơ sở dữ liệu SQL Server thông qua class DatabaseHelper
        private readonly DatabaseHelper dbHelper = new DatabaseHelper();

        // Hàm khởi tạo Form mặc định của WinForms
        public Form1()
        {
            InitializeComponent();
        }

        // ==============================================================
        // 1. SỰ KIỆN KHI FORM VỪA MỞ LÊN (LOAD FORM)
        // ==============================================================
        private void Form1_Load(object? sender, EventArgs e)
        {
            // Thiết lập giá trị mặc định cho các ComboBox
            GioiTinh.SelectedIndex = 0;
            cboTinhTrangTSV.SelectedIndex = 0;
            cboTinhTrangTTV.SelectedIndex = 0;

            // Thiết lập giá trị mặc định cho các ô chọn ngày tháng
            dtbNgaySinh.Value = DateTime.Now.AddYears(-18);
            dtpNgayCapTSV.Value = DateTime.Now;
            dtpNgayCapTTV.Value = DateTime.Now;
            dtpHanSD_TTV.Value = DateTime.Now.AddYears(4);

            // Cấu hình chọn cả dòng và khóa sửa trực tiếp cho 3 bảng dữ liệu
            DataGridView[] grids = { dgvSinhVien, dgvTheSV, dgvTheTV };
            foreach (var dgv in grids)
            {
                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgv.MultiSelect = false;
                dgv.ReadOnly = true;
                dgv.AllowUserToAddRows = false;
            }

            // Neo bảng vào 4 cạnh để tự giãn khi phóng to cửa sổ
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvSinhVien.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvTheSV.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvTheTV.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            // Khóa các ô Mã tự động sinh
            txtMaSV.ReadOnly = true;
            txtMaTheSV.ReadOnly = true;
            txtMaTheTV.ReadOnly = true;
            txtMaSV.BackColor = Color.LightGray;
            txtMaTheSV.BackColor = Color.LightGray;
            txtMaTheTV.BackColor = Color.LightGray;

            // --- 1. TẠO THANH TÌM KIẾM TAB 1 (Căn thẳng hàng bên phải nút Làm mới - button1) ---
            Label lblTimSV = new Label { Text = "Tìm kiếm:", AutoSize = true };
            lblTimSV.Location = new Point(button1.Right + 15, button1.Top + (button1.Height - lblTimSV.PreferredHeight) / 2);
            tabSinhVien.Controls.Add(lblTimSV);

            int xTimSV = lblTimSV.Left + lblTimSV.PreferredWidth + 5;
            txtTimSV.Location = new Point(xTimSV, button1.Top + (button1.Height - txtTimSV.Height) / 2);
            txtTimSV.Width = dgvSinhVien.Right - xTimSV;
            txtTimSV.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtTimSV.PlaceholderText = "Tên, Mã SV, Lớp, SĐT, Tình trạng...";
            txtTimSV.TextChanged += (s, ev) =>
            {
                if (dgvSinhVien.DataSource is DataTable dt)
                {
                    string kw = txtTimSV.Text.Trim().Replace("'", "''");
                    dt.DefaultView.RowFilter = string.IsNullOrEmpty(kw) ? "" :
                        $"[Mã SV] LIKE '%{kw}%' OR [Họ Tên] LIKE '%{kw}%' OR [Lớp] LIKE '%{kw}%' OR " +
                        $"ISNULL([Khoa], '') LIKE '%{kw}%' OR ISNULL([Số Điện Thoại], '') LIKE '%{kw}%' OR " +
                        $"ISNULL([Mã Thẻ SV], '') LIKE '%{kw}%' OR ISNULL([Thẻ SV], 'Chưa có thẻ') LIKE '%{kw}%' OR " +
                        $"ISNULL([Mã Thẻ TV], '') LIKE '%{kw}%' OR ISNULL([Thẻ Thư Viện], 'Chưa có thẻ') LIKE '%{kw}%'";
                    dgvSinhVien.ClearSelection();
                    lastClickedRowIndexSV = -1;
                }
            };
            tabSinhVien.Controls.Add(txtTimSV);

            // --- 2. TẠO NÚT LÀM MỚI & THANH TÌM KIẾM TAB 2 (Căn thẳng hàng theo nút Xóa Thẻ SV) ---
            if (btnXoaTSV.Parent != null)
            {
                Button btnMoiTab2 = new Button();
                btnMoiTab2.Text = "Làm mới";
                btnMoiTab2.Size = btnXoaTSV.Size;
                btnMoiTab2.Location = new Point(btnXoaTSV.Right + 12, btnXoaTSV.Top);
                btnMoiTab2.Click += btnLamMoiTSV_Click;
                tabTheSV.Controls.Add(btnMoiTab2);

                Label lblTimTSV = new Label { Text = "Tìm kiếm:", AutoSize = true };
                lblTimTSV.Location = new Point(btnMoiTab2.Right + 15, btnMoiTab2.Top + (btnMoiTab2.Height - lblTimTSV.PreferredHeight) / 2);
                tabTheSV.Controls.Add(lblTimTSV);

                int xTimTSV = lblTimTSV.Left + lblTimTSV.PreferredWidth + 5;
                txtTimTSV.Location = new Point(xTimTSV, btnMoiTab2.Top + (btnMoiTab2.Height - txtTimTSV.Height) / 2);
                txtTimTSV.Width = dgvTheSV.Right - xTimTSV;
                txtTimTSV.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                txtTimTSV.PlaceholderText = "Mã thẻ, Mã SV, Tình trạng...";
                txtTimTSV.TextChanged += (s, ev) =>
                {
                    if (dgvTheSV.DataSource is DataTable dt)
                    {
                        string kw = txtTimTSV.Text.Trim().Replace("'", "''");
                        dt.DefaultView.RowFilter = string.IsNullOrEmpty(kw) ? "" :
                            $"[Mã Thẻ SV] LIKE '%{kw}%' OR [Mã SV] LIKE '%{kw}%' OR [Tình Trạng] LIKE '%{kw}%' OR ISNULL([Ghi Chú], '') LIKE '%{kw}%'";
                        dgvTheSV.ClearSelection();
                        lastClickedRowIndexTSV = -1;
                    }
                };
                tabTheSV.Controls.Add(txtTimTSV);
            }

            // --- 3. TẠO NÚT LÀM MỚI & THANH TÌM KIẾM TAB 3 (Căn thẳng hàng theo nút Xóa Thẻ TV) ---
            if (btnXoaTTV.Parent != null)
            {
                Button btnMoiTab3 = new Button();
                btnMoiTab3.Text = "Làm mới";
                btnMoiTab3.Size = btnXoaTTV.Size;
                btnMoiTab3.Location = new Point(btnXoaTTV.Right + 12, btnXoaTTV.Top);
                btnMoiTab3.Click += btnLamMoiTTV_Click;
                tabTheTV.Controls.Add(btnMoiTab3);

                Label lblTimTTV = new Label { Text = "Tìm kiếm:", AutoSize = true };
                lblTimTTV.Location = new Point(btnMoiTab3.Right + 15, btnMoiTab3.Top + (btnMoiTab3.Height - lblTimTTV.PreferredHeight) / 2);
                tabTheTV.Controls.Add(lblTimTTV);

                int xTimTTV = lblTimTTV.Left + lblTimTTV.PreferredWidth + 5;
                txtTimTTV.Location = new Point(xTimTTV, btnMoiTab3.Top + (btnMoiTab3.Height - txtTimTTV.Height) / 2);
                txtTimTTV.Width = dgvTheTV.Right - xTimTTV;
                txtTimTTV.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                txtTimTTV.PlaceholderText = "Mã thẻ, Mã SV, Tình trạng...";
                txtTimTTV.TextChanged += (s, ev) =>
                {
                    if (dgvTheTV.DataSource is DataTable dt)
                    {
                        string kw = txtTimTTV.Text.Trim().Replace("'", "''");
                        dt.DefaultView.RowFilter = string.IsNullOrEmpty(kw) ? "" :
                            $"[Mã Thẻ TV] LIKE '%{kw}%' OR [Mã SV] LIKE '%{kw}%' OR [Tình Trạng] LIKE '%{kw}%'";
                        dgvTheTV.ClearSelection();
                        lastClickedRowIndexTTV = -1;
                    }
                };
                tabTheTV.Controls.Add(txtTimTTV);
            }

            // Tải dữ liệu từ Database lên giao diện
            LoadData();
            LoadTheSVData();
            LoadTheTVData();
            LoadComboBoxMaSV();
        }

        // ==============================================================
        // 2. HÀM TẢI DỮ LIỆU TỔNG HỢP CHO TAB 1
        // ==============================================================
        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = dbHelper.GetConnection())
                {
                    conn.Open();

                    // Sử dụng LEFT JOIN để lấy tất cả sinh viên, kể cả sinh viên chưa làm thẻ
                    // ISNULL dùng để thay thế ô trống thành chữ "Chưa có thẻ" cho trực quan
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
                            ISNULL(tsv.TinhTrang, N'Chưa có thẻ') AS [Thẻ SV],
                            tsv.NgayCap AS [Ngày Cấp Thẻ SV],

                            ttv.MaTheTV AS [Mã Thẻ TV],
                            ISNULL(ttv.TinhTrang, N'Chưa có thẻ') AS [Thẻ Thư Viện],
                            ttv.NgayCap AS [Ngày Cấp Thẻ TV],
                            ttv.HanSuDung AS [Ngày Hết Hạn]

                        FROM dbo.SinhVien sv
                        LEFT JOIN dbo.TheSinhVien tsv ON sv.MaSV = tsv.MaSV
                        LEFT JOIN dbo.TheThuVien ttv ON sv.MaSV = ttv.MaSV
                        ORDER BY sv.MaSV";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);             // Đổ dữ liệu từ SQL Server vào DataTable trung gian
                    dgvSinhVien.DataSource = dt;  // Gắn DataTable làm nguồn dữ liệu cho DataGridView
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy dữ liệu: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==============================================================
        // 3. HÀM SINH MÃ TỰ ĐỘNG (Dùng chung cho cả SV, TSV và TTV)
        // ==============================================================
        private string TaoMa(string tenBang, string tenCot, string tienTo)
        {
            using (SqlConnection conn = dbHelper.GetConnection())
            {
                conn.Open();
                // Lấy phần số sau chữ cái (vd SV005 -> 5), tìm số lớn nhất rồi cộng 1
                string query = $@"
                    SELECT ISNULL(MAX(
                        TRY_CONVERT(INT, SUBSTRING({tenCot}, LEN(@TienTo) + 1, 20))
                    ), 0) + 1
                    FROM {tenBang}
                    WHERE {tenCot} LIKE @TienTo + '%'";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TienTo", tienTo);
                    int so = Convert.ToInt32(cmd.ExecuteScalar()); // ExecuteScalar trả về 1 ô duy nhất (chứa con số MAX)
                    return tienTo + so.ToString("D3"); // D3 là định dạng 3 chữ số, ví dụ số 5 sẽ thành 005 -> SV005
                }
            }
        }

        // ==============================================================
        // 4. HÀM KIỂM TRA TÍNH HỢP LỆ CỦA Ô NHẬP TRƯỚC KHI LƯU
        // ==============================================================
        private bool KiemTraDuLieu()
        {
            if (string.IsNullOrWhiteSpace(txtTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên sinh viên!");
                txtTen.Focus(); // Trỏ chuột thẳng vào ô đó
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
                MessageBox.Show("Ngày sinh không hợp lệ (Phải nhỏ hơn ngày hiện tại)!");
                return false;
            }

            // Kiểm tra số điện thoại: Phải đủ 10 số và chỉ chứa chữ số (TryParse)
            string sdt = txtSDT.Text.Trim();
            if (!string.IsNullOrEmpty(sdt) && (sdt.Length != 10 || !long.TryParse(sdt, out _)))
            {
                MessageBox.Show("Số điện thoại phải gồm đúng 10 chữ số hợp lệ!");
                txtSDT.Focus();
                return false;
            }

            return true;
        }

        // ==============================================================
        // 5. XỬ LÝ SỰ KIỆN CLICK VÀO LƯỚI DANH SÁCH SINH VIÊN (TAB 1)
        // ==============================================================
        private void dgvSinhVien_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // Bỏ qua nếu click vào hàng tiêu đề (Index -1)

            // Tính năng Toggle (Nhấn lần 1 bôi xanh, nhấn lần 2 hủy bôi xanh)
            if (e.RowIndex == lastClickedRowIndexSV)
            {
                dgvSinhVien.ClearSelection(); // Xóa mảng màu xanh trên lưới
                lastClickedRowIndexSV = -1;   // Đặt lại biến nhớ

                // Xóa trắng các ô nhập liệu vì người dùng đã bỏ chọn
                txtMaSV.Clear();
                txtTen.Clear();
                txtLop.Clear();
                txtKhoa.Clear();
                txtSDT.Clear();
                txtEmail.Clear();
                GioiTinh.SelectedIndex = 0;
                dtbNgaySinh.Value = DateTime.Now.AddYears(-18);
                return; // Ngắt hàm tại đây
            }

            // Nếu người dùng chọn 1 dòng mới, bốc dữ liệu từ hàng đó đẩy lên giao diện
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

            lastClickedRowIndexSV = e.RowIndex; // Nhớ lại vị trí dòng vừa bấm
        }

        // ==============================================================
        // 6. CHỨC NĂNG THÊM SINH VIÊN (Nút "Thêm SV")
        // ==============================================================
        private void btnThem_Click(object? sender, EventArgs e)
        {
            if (!KiemTraDuLieu()) return; // Dừng nếu nhập thiếu trường bắt buộc

            using (SqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string sdt = txtSDT.Text.Trim();

                    // Kiểm tra xem số điện thoại nhập vào đã bị dùng bởi người khác chưa
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
                                return;
                            }
                        }
                    }

                    string maSV = TaoMa("SinhVien", "MaSV", "SV"); // Tự động sinh mã tiếp theo

                    // Cấu trúc INSERT INTO chuẩn ADO.NET với tham số chống SQL Injection
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
                        // Dùng DBNull.Value nếu người dùng bỏ trống các trường không bắt buộc (Khoa, SĐT, Email)
                        cmd.Parameters.AddWithValue("@Khoa", string.IsNullOrWhiteSpace(txtKhoa.Text) ? (object)DBNull.Value : txtKhoa.Text.Trim());
                        cmd.Parameters.AddWithValue("@SoDienThoai", string.IsNullOrWhiteSpace(txtSDT.Text) ? (object)DBNull.Value : txtSDT.Text.Trim());
                        cmd.Parameters.AddWithValue("@Email", string.IsNullOrWhiteSpace(txtEmail.Text) ? (object)DBNull.Value : txtEmail.Text.Trim());

                        cmd.ExecuteNonQuery(); // Thực thi lệnh Ghi dữ liệu
                    }

                    MessageBox.Show("Thêm sinh viên thành công!\nMã SV: " + maSV, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadData();           // Tải lại lưới Tab 1
                    LoadComboBoxMaSV();   // Cập nhật sinh viên mới vào ComboBox ở Tab 2 và 3
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm sinh viên: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ==============================================================
        // 7. CHỨC NĂNG SỬA SINH VIÊN (Nút "Sửa SV")
        // ==============================================================
        private void btnSua_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSV.Text))
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần sửa từ danh sách bên dưới!");
                return;
            }

            if (!KiemTraDuLieu()) return;

            using (SqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();

                    // Chặn việc sửa số điện thoại bị trùng với 1 sinh viên KHÁC (MaSV <> @MaSV)
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

                    // Lệnh UPDATE dựa vào khóa chính MaSV
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

                        if (cmd.ExecuteNonQuery() == 0) // Trả về số dòng bị ảnh hưởng, nếu là 0 nghĩa là ko tìm thấy SV để sửa
                            throw new Exception("Không tìm thấy sinh viên!");
                    }

                    MessageBox.Show("Cập nhật thông tin sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    LoadComboBoxMaSV(); // Tải lại danh sách tên vì tên có thể vừa được sửa đổi
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi sửa: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ==============================================================
        // 8. CHỨC NĂNG XÓA SINH VIÊN VÀ XÓA DÂY CHUYỀN THẺ
        // ==============================================================
        private void btnXoa_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSV.Text))
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần xóa!");
                return;
            }

            // Hiện thông báo xác nhận an toàn trước khi xóa
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa sinh viên " + txtMaSV.Text + " và toàn bộ các thẻ liên quan không?",
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

                    // Viết gộp 3 câu lệnh Delete để Xóa thẻ phụ thuộc trước, xóa Sinh Viên sau
                    // Giúp tránh lỗi vi phạm Khóa Ngoại (Foreign Key Constraint)
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
                            btnLamMoi_Click(sender, e); // Xóa trắng màn hình vừa chọn
                            LoadTheSVData();            // Xóa luôn thẻ ở Tab 2
                            LoadTheTVData();            // Xóa luôn thẻ ở Tab 3
                            LoadComboBoxMaSV();         // Xóa tên người này khỏi danh sách chọn
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy sinh viên!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Chuyển hướng nút click cũ sang hàm làm mới chuẩn
        private void btnHienThi_Click(object? sender, EventArgs e)
        {
            btnLamMoi_Click(sender, e);
        }

        // Hàm này bốc danh sách Sinh viên đưa vào menu xổ xuống ở Tab 2 và Tab 3
        private void LoadComboBoxMaSV()
        {
            using (SqlConnection conn = dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    // Thay vì chỉ hiện mỗi Mã SV, nối thêm Họ tên bằng dấu '-' cho dễ nhìn khi chọn
                    string query = "SELECT MaSV, (MaSV + N' - ' + HoTen) AS HienThi FROM dbo.SinhVien ORDER BY MaSV";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    // Setup cho ComboBox Tab 2
                    cboMaSV_TSV.DataSource = dt;
                    cboMaSV_TSV.DisplayMember = "HienThi"; // Chữ hiện lên màn hình: "SV001 - Nguyễn Trường Giang"
                    cboMaSV_TSV.ValueMember = "MaSV";      // Giá trị ngầm bên dưới dùng để gán SQL: "SV001"

                    // Setup cho ComboBox Tab 3 (cần tạo bản Copy để 2 Tab chọn độc lập không bị dính vào nhau)
                    DataTable dt2 = dt.Copy();
                    cboMaSV_TTV.DataSource = dt2;
                    cboMaSV_TTV.DisplayMember = "HienThi";
                    cboMaSV_TTV.ValueMember = "MaSV";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tải danh sách mã sinh viên: " + ex.Message);
                }
            }
        }

        // ==============================================================
        // XÓA TRẮNG FORM TAB 1 (Làm Mới)
        // ==============================================================
        private void btnLamMoi_Click(object? sender, EventArgs e)
        {
            txtTimSV.Clear(); // Xóa từ khóa tìm kiếm (bảng sẽ tự bung ra đầy đủ)
            txtMaSV.Clear();
            txtTen.Clear();
            txtLop.Clear();
            txtKhoa.Clear();
            txtSDT.Clear();
            txtEmail.Clear();

            if (GioiTinh.Items.Count > 0) GioiTinh.SelectedIndex = 0;
            dtbNgaySinh.Value = DateTime.Now.AddYears(-18);

            dgvSinhVien.ClearSelection();
            lastClickedRowIndexSV = -1;
            LoadData();
        }
    }
}