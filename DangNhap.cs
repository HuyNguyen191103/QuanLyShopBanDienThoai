using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyShopDienThoai
{
    public partial class DangNhap : Form
    {
        SqlConnection conn;
        public DangNhap()
        {
            InitializeComponent();
            conn = new SqlConnection("Data Source=LAPTOP-IK9U00KH\\TEST2;Initial Catalog =QLSHOPDT; Integrated Security = True");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sdt = txtSDT.Text;
            string password = txtMatKhau.Text;
            //Kiem tra ket noi truoc khi mo
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            string query = "SELECT COUNT(*) FROM NhanVien WHERE SDT = @SDT AND MatKhau = @Password";
            SqlCommand cmd = new SqlCommand(query, conn);
            try
            {
                cmd.Parameters.AddWithValue("@SDT", sdt);
                cmd.Parameters.AddWithValue("@Password", password);
                // Thực thi truy vấn và kiểm tra kết quả
                int count = (int)cmd.ExecuteScalar();
                if (count > 0)
                {
                    MessageBox.Show("Đăng nhập thành công!");
                    Form1 frm = new Form1();
                    frm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Số điện thoại hoặc mật khẩu không đúng!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
