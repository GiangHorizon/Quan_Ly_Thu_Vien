namespace WinFormsApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tabControl1 = new TabControl();
            tabSinhVien = new TabPage();
            tabTheSV = new TabPage();
            tabTheTV = new TabPage();

            dgvSinhVien = new DataGridView();
            txtMaSV = new TextBox();
            txtTen = new TextBox();
            GioiTinh = new ComboBox();
            dtbNgaySinh = new DateTimePicker();
            txtLop = new TextBox();
            txtKhoa = new TextBox();
            txtSDT = new TextBox();
            txtEmail = new TextBox();
            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            button1 = new Button();
            lblSV1 = new Label();
            lblSV2 = new Label();
            lblSV3 = new Label();
            lblSV4 = new Label();
            lblSV5 = new Label();
            lblSV6 = new Label();
            lblSV7 = new Label();
            lblSV8 = new Label();

            dgvTheSV = new DataGridView();
            txtMaTheSV = new TextBox();
            cboMaSV_TSV = new ComboBox();
            dtpNgayCapTSV = new DateTimePicker();
            cboTinhTrangTSV = new ComboBox();
            txtGhiChuTSV = new TextBox();
            btnThemTSV = new Button();
            btnSuaTSV = new Button();
            btnXoaTSV = new Button();
            lblTSV1 = new Label();
            lblTSV2 = new Label();
            lblTSV3 = new Label();
            lblTSV4 = new Label();
            lblTSV5 = new Label();

            dgvTheTV = new DataGridView();
            txtMaTheTV = new TextBox();
            cboMaSV_TTV = new ComboBox();
            dtpNgayCapTTV = new DateTimePicker();
            dtpHanSD_TTV = new DateTimePicker();
            cboTinhTrangTTV = new ComboBox();
            btnThemTTV = new Button();
            btnSuaTTV = new Button();
            btnXoaTTV = new Button();
            lblTTV1 = new Label();
            lblTTV2 = new Label();
            lblTTV3 = new Label();
            lblTTV4 = new Label();
            lblTTV5 = new Label();

            tabControl1.SuspendLayout();
            tabSinhVien.SuspendLayout();
            tabTheSV.SuspendLayout();
            tabTheTV.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSinhVien).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvTheSV).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvTheTV).BeginInit();
            SuspendLayout();

            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabSinhVien);
            tabControl1.Controls.Add(tabTheSV);
            tabControl1.Controls.Add(tabTheTV);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(950, 520);
            tabControl1.TabIndex = 0;

            // 
            // tabSinhVien
            // 
            tabSinhVien.Text = "1. Quản lý Sinh Viên";
            tabSinhVien.Location = new Point(4, 29);
            tabSinhVien.Name = "tabSinhVien";
            tabSinhVien.Size = new Size(942, 487);

            lblSV1.Text = "Mã SV:";
            lblSV1.Location = new Point(20, 23);
            lblSV1.AutoSize = true;
            txtMaSV.Location = new Point(110, 20);
            txtMaSV.Size = new Size(150, 27);
            txtMaSV.Name = "txtMaSV";

            lblSV2.Text = "Họ tên:";
            lblSV2.Location = new Point(20, 58);
            lblSV2.AutoSize = true;
            txtTen.Location = new Point(110, 55);
            txtTen.Size = new Size(150, 27);
            txtTen.Name = "txtTen";

            lblSV3.Text = "Giới tính:";
            lblSV3.Location = new Point(20, 93);
            lblSV3.AutoSize = true;
            GioiTinh.Items.AddRange(new object[] { "Nam", "Nữ" });
            GioiTinh.Location = new Point(110, 90);
            GioiTinh.Size = new Size(150, 28);
            GioiTinh.DropDownStyle = ComboBoxStyle.DropDownList;
            GioiTinh.Name = "GioiTinh";

            lblSV4.Text = "Ngày sinh:";
            lblSV4.Location = new Point(290, 23);
            lblSV4.AutoSize = true;
            dtbNgaySinh.Format = DateTimePickerFormat.Short;
            dtbNgaySinh.Location = new Point(390, 20);
            dtbNgaySinh.Size = new Size(160, 27);
            dtbNgaySinh.Name = "dtbNgaySinh";

            lblSV5.Text = "Lớp:";
            lblSV5.Location = new Point(290, 58);
            lblSV5.AutoSize = true;
            txtLop.Location = new Point(390, 55);
            txtLop.Size = new Size(160, 27);
            txtLop.Name = "txtLop";

            lblSV6.Text = "Khoa:";
            lblSV6.Location = new Point(290, 93);
            lblSV6.AutoSize = true;
            txtKhoa.Location = new Point(390, 90);
            txtKhoa.Size = new Size(160, 27);
            txtKhoa.Name = "txtKhoa";

            lblSV7.Text = "SĐT:";
            lblSV7.Location = new Point(580, 23);
            lblSV7.AutoSize = true;
            txtSDT.Location = new Point(680, 20);
            txtSDT.Size = new Size(180, 27);
            txtSDT.Name = "txtSDT";

            lblSV8.Text = "Email:";
            lblSV8.Location = new Point(580, 58);
            lblSV8.AutoSize = true;
            txtEmail.Location = new Point(680, 55);
            txtEmail.Size = new Size(180, 27);
            txtEmail.Name = "txtEmail";

            btnThem.Text = "Thêm SV";
            btnThem.Location = new Point(110, 135);
            btnThem.Size = new Size(110, 35);
            btnThem.Name = "btnThem";
            btnThem.Click += btnThem_Click;

            btnSua.Text = "Sửa SV";
            btnSua.Location = new Point(235, 135);
            btnSua.Size = new Size(110, 35);
            btnSua.Name = "btnSua";
            btnSua.Click += btnSua_Click;

            btnXoa.Text = "Xóa SV";
            btnXoa.Location = new Point(360, 135);
            btnXoa.Size = new Size(110, 35);
            btnXoa.Name = "btnXoa";
            btnXoa.Click += btnXoa_Click;

            button1.Text = "Làm mới";
            button1.Location = new Point(485, 135);
            button1.Size = new Size(110, 35);
            button1.Name = "button1";
            button1.Click += btnHienThi_Click;

            dgvSinhVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSinhVien.Location = new Point(15, 180);
            dgvSinhVien.Name = "dgvSinhVien";
            dgvSinhVien.ReadOnly = true;
            dgvSinhVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSinhVien.Size = new Size(910, 290);
            dgvSinhVien.CellClick += dgvSinhVien_CellContentClick;

            tabSinhVien.Controls.Add(lblSV1);
            tabSinhVien.Controls.Add(txtMaSV);
            tabSinhVien.Controls.Add(lblSV2);
            tabSinhVien.Controls.Add(txtTen);
            tabSinhVien.Controls.Add(lblSV3);
            tabSinhVien.Controls.Add(GioiTinh);
            tabSinhVien.Controls.Add(lblSV4);
            tabSinhVien.Controls.Add(dtbNgaySinh);
            tabSinhVien.Controls.Add(lblSV5);
            tabSinhVien.Controls.Add(txtLop);
            tabSinhVien.Controls.Add(lblSV6);
            tabSinhVien.Controls.Add(txtKhoa);
            tabSinhVien.Controls.Add(lblSV7);
            tabSinhVien.Controls.Add(txtSDT);
            tabSinhVien.Controls.Add(lblSV8);
            tabSinhVien.Controls.Add(txtEmail);
            tabSinhVien.Controls.Add(btnThem);
            tabSinhVien.Controls.Add(btnSua);
            tabSinhVien.Controls.Add(btnXoa);
            tabSinhVien.Controls.Add(button1);
            tabSinhVien.Controls.Add(dgvSinhVien);

            // 
            // tabTheSV
            // 
            tabTheSV.Text = "2. Cấp Thẻ Sinh Viên";
            tabTheSV.Location = new Point(4, 29);
            tabTheSV.Name = "tabTheSV";
            tabTheSV.Size = new Size(942, 487);

            lblTSV1.Text = "Mã Thẻ SV:";
            lblTSV1.Location = new Point(20, 23);
            lblTSV1.AutoSize = true;
            txtMaTheSV.Location = new Point(130, 20);
            txtMaTheSV.Size = new Size(160, 27);
            txtMaTheSV.Name = "txtMaTheSV";

            lblTSV2.Text = "Chọn Mã SV:";
            lblTSV2.Location = new Point(20, 63);
            lblTSV2.AutoSize = true;
            cboMaSV_TSV.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMaSV_TSV.Location = new Point(130, 60);
            cboMaSV_TSV.Size = new Size(160, 28);
            cboMaSV_TSV.Name = "cboMaSV_TSV";

            lblTSV3.Text = "Ngày cấp:";
            lblTSV3.Location = new Point(320, 23);
            lblTSV3.AutoSize = true;
            dtpNgayCapTSV.Format = DateTimePickerFormat.Short;
            dtpNgayCapTSV.Location = new Point(430, 20);
            dtpNgayCapTSV.Size = new Size(160, 27);
            dtpNgayCapTSV.Name = "dtpNgayCapTSV";

            lblTSV4.Text = "Tình trạng:";
            lblTSV4.Location = new Point(320, 63);
            lblTSV4.AutoSize = true;
            cboTinhTrangTSV.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTinhTrangTSV.Items.AddRange(new object[] { "Đang hoạt động", "Bị khóa", "Đã thu hồi", "Chờ cấp lại" });
            cboTinhTrangTSV.Location = new Point(430, 60);
            cboTinhTrangTSV.Size = new Size(160, 28);
            cboTinhTrangTSV.Name = "cboTinhTrangTSV";

            lblTSV5.Text = "Ghi chú:";
            lblTSV5.Location = new Point(615, 23);
            lblTSV5.AutoSize = true;
            txtGhiChuTSV.Location = new Point(690, 20);
            txtGhiChuTSV.Multiline = true;
            txtGhiChuTSV.Size = new Size(210, 67);
            txtGhiChuTSV.Name = "txtGhiChuTSV";

            btnThemTSV.Text = "Cấp Thẻ SV";
            btnThemTSV.Location = new Point(130, 110);
            btnThemTSV.Size = new Size(120, 35);
            btnThemTSV.Name = "btnThemTSV";
            btnThemTSV.Click += btnThemTSV_Click;

            btnSuaTSV.Text = "Cập nhật Thẻ";
            btnSuaTSV.Location = new Point(265, 110);
            btnSuaTSV.Size = new Size(120, 35);
            btnSuaTSV.Name = "btnSuaTSV";
            btnSuaTSV.Click += btnSuaTSV_Click;

            btnXoaTSV.Text = "Xóa Thẻ";
            btnXoaTSV.Location = new Point(400, 110);
            btnXoaTSV.Size = new Size(110, 35);
            btnXoaTSV.Name = "btnXoaTSV";
            btnXoaTSV.Click += btnXoaTSV_Click;

            dgvTheSV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTheSV.Location = new Point(15, 160);
            dgvTheSV.Name = "dgvTheSV";
            dgvTheSV.ReadOnly = true;
            dgvTheSV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTheSV.Size = new Size(910, 310);
            dgvTheSV.CellClick += dgvTheSV_CellClick;

            tabTheSV.Controls.Add(lblTSV1);
            tabTheSV.Controls.Add(txtMaTheSV);
            tabTheSV.Controls.Add(lblTSV2);
            tabTheSV.Controls.Add(cboMaSV_TSV);
            tabTheSV.Controls.Add(lblTSV3);
            tabTheSV.Controls.Add(dtpNgayCapTSV);
            tabTheSV.Controls.Add(lblTSV4);
            tabTheSV.Controls.Add(cboTinhTrangTSV);
            tabTheSV.Controls.Add(lblTSV5);
            tabTheSV.Controls.Add(txtGhiChuTSV);
            tabTheSV.Controls.Add(btnThemTSV);
            tabTheSV.Controls.Add(btnSuaTSV);
            tabTheSV.Controls.Add(btnXoaTSV);
            tabTheSV.Controls.Add(dgvTheSV);

            // 
            // tabTheTV
            // 
            tabTheTV.Text = "3. Cấp Thẻ Thư Viện";
            tabTheTV.Location = new Point(4, 29);
            tabTheTV.Name = "tabTheTV";
            tabTheTV.Size = new Size(942, 487);

            lblTTV1.Text = "Mã Thẻ TV:";
            lblTTV1.Location = new Point(20, 23);
            lblTTV1.AutoSize = true;
            txtMaTheTV.Location = new Point(130, 20);
            txtMaTheTV.Size = new Size(160, 27);
            txtMaTheTV.Name = "txtMaTheTV";

            lblTTV2.Text = "Chọn Mã SV:";
            lblTTV2.Location = new Point(20, 63);
            lblTTV2.AutoSize = true;
            cboMaSV_TTV.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMaSV_TTV.Location = new Point(130, 60);
            cboMaSV_TTV.Size = new Size(160, 28);
            cboMaSV_TTV.Name = "cboMaSV_TTV";

            lblTTV3.Text = "Ngày cấp:";
            lblTTV3.Location = new Point(320, 23);
            lblTTV3.AutoSize = true;
            dtpNgayCapTTV.Format = DateTimePickerFormat.Short;
            dtpNgayCapTTV.Location = new Point(430, 20);
            dtpNgayCapTTV.Size = new Size(160, 27);
            dtpNgayCapTTV.Name = "dtpNgayCapTTV";

            lblTTV4.Text = "Hạn sử dụng:";
            lblTTV4.Location = new Point(320, 63);
            lblTTV4.AutoSize = true;
            dtpHanSD_TTV.Format = DateTimePickerFormat.Short;
            dtpHanSD_TTV.Location = new Point(430, 60);
            dtpHanSD_TTV.Size = new Size(160, 27);
            dtpHanSD_TTV.Name = "dtpHanSD_TTV";

            lblTTV5.Text = "Tình trạng:";
            lblTTV5.Location = new Point(615, 23);
            lblTTV5.AutoSize = true;
            cboTinhTrangTTV.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTinhTrangTTV.Items.AddRange(new object[] { "Đang hoạt động", "Bị khóa do nợ sách", "Hết hạn", "Đã thu hồi" });
            cboTinhTrangTTV.Location = new Point(710, 20);
            cboTinhTrangTTV.Size = new Size(190, 28);
            cboTinhTrangTTV.Name = "cboTinhTrangTTV";

            btnThemTTV.Text = "Cấp Thẻ TV";
            btnThemTTV.Location = new Point(130, 110);
            btnThemTTV.Size = new Size(120, 35);
            btnThemTTV.Name = "btnThemTTV";
            btnThemTTV.Click += btnThemTTV_Click;

            btnSuaTTV.Text = "Cập nhật Thẻ";
            btnSuaTTV.Location = new Point(265, 110);
            btnSuaTTV.Size = new Size(120, 35);
            btnSuaTTV.Name = "btnSuaTTV";
            btnSuaTTV.Click += btnSuaTTV_Click;

            btnXoaTTV.Text = "Xóa Thẻ";
            btnXoaTTV.Location = new Point(400, 110);
            btnXoaTTV.Size = new Size(110, 35);
            btnXoaTTV.Name = "btnXoaTTV";
            btnXoaTTV.Click += btnXoaTTV_Click;

            dgvTheTV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTheTV.Location = new Point(15, 160);
            dgvTheTV.Name = "dgvTheTV";
            dgvTheTV.ReadOnly = true;
            dgvTheTV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTheTV.Size = new Size(910, 310);
            dgvTheTV.CellClick += dgvTheTV_CellClick;

            tabTheTV.Controls.Add(lblTTV1);
            tabTheTV.Controls.Add(txtMaTheTV);
            tabTheTV.Controls.Add(lblTTV2);
            tabTheTV.Controls.Add(cboMaSV_TTV);
            tabTheTV.Controls.Add(lblTTV3);
            tabTheTV.Controls.Add(dtpNgayCapTTV);
            tabTheTV.Controls.Add(lblTTV4);
            tabTheTV.Controls.Add(dtpHanSD_TTV);
            tabTheTV.Controls.Add(lblTTV5);
            tabTheTV.Controls.Add(cboTinhTrangTTV);
            tabTheTV.Controls.Add(btnThemTTV);
            tabTheTV.Controls.Add(btnSuaTTV);
            tabTheTV.Controls.Add(btnXoaTTV);
            tabTheTV.Controls.Add(dgvTheTV);

            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(950, 520);
            Controls.Add(tabControl1);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hệ thống Quản lý Sinh Viên - Cấp Thẻ SV & Thẻ Thư Viện";
            Load += Form1_Load;

            tabControl1.ResumeLayout(false);
            tabSinhVien.ResumeLayout(false);
            tabSinhVien.PerformLayout();
            tabTheSV.ResumeLayout(false);
            tabTheSV.PerformLayout();
            tabTheTV.ResumeLayout(false);
            tabTheTV.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSinhVien).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvTheSV).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvTheTV).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabSinhVien;
        private TabPage tabTheSV;
        private TabPage tabTheTV;
        private DataGridView dgvSinhVien;
        private DataGridView dgvTheSV;
        private DataGridView dgvTheTV;
        private TextBox txtMaSV;
        private TextBox txtTen;
        private TextBox txtLop;
        private TextBox txtKhoa;
        private TextBox txtSDT;
        private TextBox txtEmail;
        private ComboBox GioiTinh;
        private DateTimePicker dtbNgaySinh;
        private Button button1;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Label lblSV1, lblSV2, lblSV3, lblSV4, lblSV5, lblSV6, lblSV7, lblSV8;

        private TextBox txtMaTheSV;
        private TextBox txtGhiChuTSV;
        private ComboBox cboMaSV_TSV;
        private ComboBox cboTinhTrangTSV;
        private DateTimePicker dtpNgayCapTSV;
        private Button btnThemTSV;
        private Button btnSuaTSV;
        private Button btnXoaTSV;
        private Label lblTSV1, lblTSV2, lblTSV3, lblTSV4, lblTSV5;

        private TextBox txtMaTheTV;
        private ComboBox cboMaSV_TTV;
        private ComboBox cboTinhTrangTTV;
        private DateTimePicker dtpNgayCapTTV;
        private DateTimePicker dtpHanSD_TTV;
        private Button btnThemTTV;
        private Button btnSuaTTV;
        private Button btnXoaTTV;
        private Label lblTTV1, lblTTV2, lblTTV3, lblTTV4, lblTTV5;
    }
}