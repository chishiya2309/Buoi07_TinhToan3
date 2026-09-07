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
            btnThoat.CausesValidation = false;  //khi thoát không cần validate
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult dr;
            dr = MessageBox.Show("Bạn có thực sự muốn thoát không?",
                                 "Thông báo", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                this.AutoValidate = AutoValidate.Disable;   //bỏ qua validate
                this.Close();
            }
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
            //validate input
            if (!ValidateChildren())
                return;
            //lấy giá trị của 2 ô số
            string so1, so2, kq = "0";
            so1 = txtSo1.Text;
            so2 = txtSo2.Text;
            //Thực hiện phép tính dựa vào phép toán được chọn
            if (radCong.Checked) kq = Cong(so1, so2);
            else if (radTru.Checked) kq = Tru(so1, so2);
            else if (radNhan.Checked) kq = Nhan(so1, so2);
            else if (radChia.Checked && so2 != "0") kq = Chia(so1, so2);
            else if (radChia.Checked && so2 == "0")
            {
                MessageBox.Show("Lỗi: Không thể chia cho 0! Vui lòng nhập số khác.",
                                "Lỗi phép chia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSo2.Focus();
                txtSo2.SelectAll();
                return;
            }
            //Hiển thị kết quả lên trên ô kết quả
            txtKq.Text = kq;
        }

        // CỘNG
        static string Cong(string a, string b)
        {
            string kq = "";
            int i = a.Length - 1, j = b.Length - 1, nho = 0;

            while (i >= 0 || j >= 0 || nho > 0)
            {
                int x = i >= 0 ? a[i--] - '0' : 0;
                int y = j >= 0 ? b[j--] - '0' : 0;

                int tong = x + y + nho;

                kq = (tong % 10) + kq;
                nho = tong / 10;
            }

            return kq;
        }


        // TRỪ (giả sử a >= b)
        static string Tru(string a, string b)
        {
            string kq = "";
            int i = a.Length - 1;
            int j = b.Length - 1;
            int muon = 0;

            while (i >= 0)
            {
                int x = a[i--] - '0' - muon;
                int y = j >= 0 ? b[j--] - '0' : 0;

                if (x < y)
                {
                    x += 10;
                    muon = 1;
                }
                else
                {
                    muon = 0;
                }

                kq = (x - y) + kq;
            }

            return kq.TrimStart('0') == "" ? "0" : kq.TrimStart('0');
        }


        // NHÂN
        static string Nhan(string a, string b)
        {
            int[] kq = new int[a.Length + b.Length];

            for (int i = a.Length - 1; i >= 0; i--)
            {
                for (int j = b.Length - 1; j >= 0; j--)
                {
                    int tich = (a[i] - '0') * (b[j] - '0');

                    int viTri = i + j + 1;

                    kq[viTri] += tich;

                    kq[viTri - 1] += kq[viTri] / 10;
                    kq[viTri] %= 10;
                }
            }

            string s = "";

            foreach (int x in kq)
                s += x;

            s = s.TrimStart('0');

            return s == "" ? "0" : s;
        }


        // CHIA
        static string Chia(string a, string b)
        {
            string kq = "";
            string tam = "";

            foreach (char c in a)
            {
                tam += c;
                tam = tam.TrimStart('0');

                if (tam == "")
                    tam = "0";
                int dem = 0;

                // So sánh tam >= b
                while (
                    tam.Length > b.Length ||
                    (tam.Length == b.Length &&
                     string.Compare(tam, b) >= 0)
                )
                {
                    tam = Tru(tam, b);
                    dem++;
                }

                kq += dem;
            }

            kq = kq.TrimStart('0');

            return kq == "" ? "0" : kq;
        }
    }
}
