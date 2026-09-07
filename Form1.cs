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
            string text = txtSo1.Text.Trim();
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Số thứ nhất không được để trống! Vui lòng nhập số.",
                                "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
                return;
            }

            double result;
            if (!double.TryParse(text, out result))
            {
                MessageBox.Show("Số thứ nhất không hợp lệ! Vui lòng nhập số.",
                                "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
                return;
            }

            int digitCount = text.Count(char.IsDigit);
            if (digitCount > 30)
            {
                MessageBox.Show("Số thứ nhất không được vượt quá 30 chữ số!",
                                "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSo1.SelectAll();
                e.Cancel = true;
                return;
            }
        }
        private void txtSo2_Validating(object sender, CancelEventArgs e)
        {
            string text = txtSo2.Text.Trim();
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Số thứ hai không được để trống! Vui lòng nhập số.",
                                "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
                return;
            }

            double result;
            if (!double.TryParse(text, out result))
            {
                MessageBox.Show("Số thứ hai không hợp lệ! Vui lòng nhập số.",
                                "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
                return;
            }

            int digitCount = text.Count(char.IsDigit);
            if (digitCount > 30)
            {
                MessageBox.Show("Số thứ hai không được vượt quá 30 chữ số!",
                                "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSo2.SelectAll();
                e.Cancel = true;
                return;
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
            so1 = txtSo1.Text.Trim();
            so2 = txtSo2.Text.Trim();

            bool am1 = so1.StartsWith("-");
            bool am2 = so2.StartsWith("-");

            // Bỏ dấu âm để các hàm hiện tại chỉ xử lý chữ số
            if (am1)
                so1 = so1.Substring(1);

            if (am2)
                so2 = so2.Substring(1);
            //Thực hiện phép tính dựa vào phép toán được chọn
            if (radCong.Checked)
            {
                if (!am1 && !am2)
                    kq = Cong(so1, so2);            // 5 + 3

                else if (am1 && am2)
                    kq = "-" + Cong(so1, so2);      // -5 + -3

                else if (am1)
                    kq = Tru(so2, so1);             // -5 + 3 = 3 - 5

                else
                    kq = Tru(so1, so2);
            }
            else if (radTru.Checked)
            {
                if (!am1 && !am2)
                    kq = Tru(so1, so2);             // 5 - 3

                else if (am1 && am2)
                    kq = Tru(so2, so1);             // -5 - (-3) = 3 - 5

                else if (am1)
                    kq = "-" + Cong(so1, so2);      // -5 - 3 = -(5+3)

                else
                    kq = Cong(so1, so2);
            }
            else if (radNhan.Checked)
            {
                kq = Nhan(so1, so2);

                // Khác dấu → kết quả âm
                if (am1 != am2 && kq != "0")
                    kq = "-" + kq;
            }
            else if (radChia.Checked && so2 != "0")
            {
                kq = Chia(so1, so2);

                // Khác dấu → kết quả âm
                if (am1 != am2 && kq != "0")
                    kq = "-" + kq;
            }
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
            CanBangThapPhan(ref a, ref b, out int soLe);

            a = a.Replace(".", "");
            b = b.Replace(".", "");

            string kq = CongNguyen(a, b);

            return GanDauCham(kq, soLe);
        }


        // TRỪ
        static string Tru(string a, string b)
        {
            CanBangThapPhan(ref a, ref b, out int soLe);

            a = a.Replace(".", "");
            b = b.Replace(".", "");

            bool am = false;

            a = Xoa0Dau(a);
            b = Xoa0Dau(b);

            if (SoSanh(a, b) < 0)
            {
                string tam = a;
                a = b;
                b = tam;

                am = true;
            }

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

            kq = Xoa0Dau(kq);
            kq = GanDauCham(kq, soLe);

            if (am && kq != "0")
                kq = "-" + kq;

            return kq;
        }


        // NHÂN
        static string Nhan(string a, string b)
        {
            int leA = 0;
            int leB = 0;

            int vtA = a.IndexOf('.');
            int vtB = b.IndexOf('.');

            if (vtA >= 0)
                leA = a.Length - vtA - 1;

            if (vtB >= 0)
                leB = b.Length - vtB - 1;

            a = a.Replace(".", "");
            b = b.Replace(".", "");

            a = Xoa0Dau(a);
            b = Xoa0Dau(b);

            if (a == "0" || b == "0")
                return "0";

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

            s = Xoa0Dau(s);

            return GanDauCham(s, leA + leB);
        }


        // CHIA
        static string Chia(string a, string b)
        {
            int leA = 0;
            int leB = 0;

            int vtA = a.IndexOf('.');
            int vtB = b.IndexOf('.');

            if (vtA >= 0)
                leA = a.Length - vtA - 1;

            if (vtB >= 0)
                leB = b.Length - vtB - 1;

            a = a.Replace(".", "");
            b = b.Replace(".", "");

            a = Xoa0Dau(a);
            b = Xoa0Dau(b);

            // Cân bằng vị trí dấu thập phân
            if (leA > leB)
                b += new string('0', leA - leB);
            else if (leB > leA)
                a += new string('0', leB - leA);

            a = Xoa0Dau(a);
            b = Xoa0Dau(b);

            if (b == "0")
                throw new DivideByZeroException();

            string kq = "";
            string tam = "";

            // Phần nguyên
            foreach (char c in a)
            {
                tam += c;
                tam = Xoa0Dau(tam);

                int dem = 0;

                while (SoSanh(tam, b) >= 0)
                {
                    tam = TruNguyen(tam, b);
                    dem++;
                }

                kq += dem;
            }

            kq = Xoa0Dau(kq);

            if (tam == "0")
                return kq;

            kq += ".";

            // Tối đa 15 chữ số thập phân
            int soChuSo = 0;

            while (tam != "0" && soChuSo < 15)
            {
                tam += "0";
                tam = Xoa0Dau(tam);

                int dem = 0;

                while (SoSanh(tam, b) >= 0)
                {
                    tam = TruNguyen(tam, b);
                    dem++;
                }

                kq += dem;
                soChuSo++;
            }

            return ChuanHoa(kq);
        }

        // Cộng 2 số nguyên dương
        static string CongNguyen(string a, string b)
        {
            string kq = "";

            int i = a.Length - 1;
            int j = b.Length - 1;
            int nho = 0;

            while (i >= 0 || j >= 0 || nho > 0)
            {
                int x = i >= 0 ? a[i--] - '0' : 0;
                int y = j >= 0 ? b[j--] - '0' : 0;

                int tong = x + y + nho;

                kq = (tong % 10) + kq;
                nho = tong / 10;
            }

            return Xoa0Dau(kq);
        }


        // Trừ 2 số nguyên dương, a >= b
        static string TruNguyen(string a, string b)
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

            return Xoa0Dau(kq);
        }


        // So sánh 2 số nguyên dạng chuỗi
        static int SoSanh(string a, string b)
        {
            a = Xoa0Dau(a);
            b = Xoa0Dau(b);

            if (a.Length > b.Length)
                return 1;

            if (a.Length < b.Length)
                return -1;

            return string.CompareOrdinal(a, b);
        }


        // Xóa số 0 thừa đầu
        static string Xoa0Dau(string s)
        {
            s = s.TrimStart('0');

            return s == "" ? "0" : s;
        }


        // Cân bằng số chữ số sau dấu .
        static void CanBangThapPhan(ref string a, ref string b, out int soLe)
        {
            int leA = 0;
            int leB = 0;

            int vtA = a.IndexOf('.');
            int vtB = b.IndexOf('.');

            if (vtA >= 0)
                leA = a.Length - vtA - 1;

            if (vtB >= 0)
                leB = b.Length - vtB - 1;

            soLe = Math.Max(leA, leB);

            if (leA < soLe)
            {
                if (!a.Contains("."))
                    a += ".";

                a += new string('0', soLe - leA);
            }

            if (leB < soLe)
            {
                if (!b.Contains("."))
                    b += ".";

                b += new string('0', soLe - leB);
            }
        }


        // Gắn lại dấu thập phân
        static string GanDauCham(string s, int soLe)
        {
            s = Xoa0Dau(s);

            if (soLe == 0)
                return s;

            while (s.Length <= soLe)
                s = "0" + s;

            int viTri = s.Length - soLe;

            s = s.Insert(viTri, ".");

            return ChuanHoa(s);
        }


        // Chuẩn hóa kết quả
        static string ChuanHoa(string s)
        {
            if (s.Contains("."))
            {
                s = s.TrimEnd('0');
                s = s.TrimEnd('.');
            }

            if (s.StartsWith("."))
                s = "0" + s;

            return s == "" ? "0" : s;
        }
    }
}
