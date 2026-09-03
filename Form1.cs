using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Buoi07_TinhToan3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtSo1.Text = txtSo2.Text = "0";
            radCong.Checked = true;             //đầu tiên chọn phép cộng
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult dr;
            dr = MessageBox.Show("Bạn có thực sự muốn thoát không?",
                                 "Thông báo", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
                this.Close();
        }

        private void txtSo1_Validating(object sender, CancelEventArgs e)
        {
            double result;
            if (!double.TryParse(txtSo1.Text, out result))
            {
                MessageBox.Show("Số thứ nhất không hợp lệ! Vui lòng nhập số.",
                                "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }
        private void txtSo2_Validating(object sender, CancelEventArgs e)
        {
            double result;
            if (!double.TryParse(txtSo2.Text, out result))
            {
                MessageBox.Show("Số thứ hai không hợp lệ! Vui lòng nhập số.",
                                "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        private void txtSo1_Enter(object sender, EventArgs e)
        {
            txtSo1.BeginInvoke(new Action(() => txtSo1.SelectAll()));
        }

        private void txtSo2_Enter(object sender, EventArgs e)
        {
            txtSo2.BeginInvoke(new Action(() => txtSo2.SelectAll()));
        }

        private void btnTinh_Click(object sender, EventArgs e)
        {
            //lấy giá trị của 2 ô số
            double so1, so2, kq = 0;
            so1 = double.Parse(txtSo1.Text);
            so2 = double.Parse(txtSo2.Text);
            //Thực hiện phép tính dựa vào phép toán được chọn
            if (radCong.Checked) kq = so1 + so2;
            else if (radTru.Checked) kq = so1 - so2;
            else if (radNhan.Checked) kq = so1 * so2;
            else if (radChia.Checked && so2 != 0) kq = so1 / so2;
            //Hiển thị kết quả lên trên ô kết quả
            txtKq.Text = kq.ToString();
        }
    }
}
