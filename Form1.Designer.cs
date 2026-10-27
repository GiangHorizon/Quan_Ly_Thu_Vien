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
            button1 = new Button();
            dgvSinhVien = new DataGridView();
            txtMaSV = new TextBox();
            txtTen = new TextBox();
            txtLop = new TextBox();
            GioiTinh = new ComboBox();
            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            dtbNgaySinh = new DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)dgvSinhVien).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(107, 193);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 0;
            button1.Text = "Hiển thị sinh viên ";
            button1.UseVisualStyleBackColor = true;
            button1.Click += btnHienThi_Click;
            // 
            // dgvSinhVien
            // 
            dgvSinhVien.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSinhVien.Location = new Point(12, 12);
            dgvSinhVien.Name = "dgvSinhVien";
            dgvSinhVien.RowHeadersWidth = 51;
            dgvSinhVien.Size = new Size(287, 175);
            dgvSinhVien.TabIndex = 1;
            dgvSinhVien.CellContentClick += dgvSinhVien_CellContentClick;
            // 
            // txtMaSV
            // 
            txtMaSV.Location = new Point(365, 111);
            txtMaSV.Name = "txtMaSV";
            txtMaSV.Size = new Size(125, 27);
            txtMaSV.TabIndex = 2;
            txtMaSV.Text = "MaSV";
            // 
            // txtTen
            // 
            txtTen.Location = new Point(513, 111);
            txtTen.Name = "txtTen";
            txtTen.Size = new Size(125, 27);
            txtTen.TabIndex = 3;
            txtTen.Text = "Ten";
            txtTen.TextChanged += txtTen_TextChanged;
            // 
            // txtLop
            // 
            txtLop.Location = new Point(663, 111);
            txtLop.Name = "txtLop";
            txtLop.Size = new Size(125, 27);
            txtLop.TabIndex = 4;
            txtLop.Text = "Lop";
            txtLop.TextChanged += txtLop_TextChanged;
            // 
            // GioiTinh
            // 
            GioiTinh.DisplayMember = "Nam";
            GioiTinh.FormattingEnabled = true;
            GioiTinh.Items.AddRange(new object[] { "Nam", "Nu" });
            GioiTinh.Location = new Point(365, 172);
            GioiTinh.Name = "GioiTinh";
            GioiTinh.Size = new Size(151, 28);
            GioiTinh.TabIndex = 5;
            GioiTinh.Text = "Gioi Tinh";
            GioiTinh.SelectedIndexChanged += GioiTinh_SelectedIndexChanged;
            // 
            // btnThem
            // 
            btnThem.Location = new Point(396, 241);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(94, 29);
            btnThem.TabIndex = 6;
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = true;
            btnThem.Click += btnThem_Click;
            // 
            // btnSua
            // 
            btnSua.Location = new Point(525, 241);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(94, 29);
            btnSua.TabIndex = 7;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = true;
            btnSua.Click += btnSua_Click;
            // 
            // btnXoa
            // 
            btnXoa.Location = new Point(663, 241);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(94, 29);
            btnXoa.TabIndex = 8;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = true;
            btnXoa.Click += btnXoa_Click;
            // 
            // dtbNgaySinh
            // 
            dtbNgaySinh.Location = new Point(538, 173);
            dtbNgaySinh.Name = "dtbNgaySinh";
            dtbNgaySinh.Size = new Size(250, 27);
            dtbNgaySinh.TabIndex = 9;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(875, 456);
            Controls.Add(dtbNgaySinh);
            Controls.Add(btnXoa);
            Controls.Add(btnSua);
            Controls.Add(btnThem);
            Controls.Add(GioiTinh);
            Controls.Add(txtLop);
            Controls.Add(txtTen);
            Controls.Add(txtMaSV);
            Controls.Add(dgvSinhVien);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dgvSinhVien).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private DataGridView dgvSinhVien;
        private TextBox txtMaSV;
        private TextBox txtTen;
        private TextBox txtLop;
        private ComboBox GioiTinh;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private DateTimePicker dtbNgaySinh;
    }
}
