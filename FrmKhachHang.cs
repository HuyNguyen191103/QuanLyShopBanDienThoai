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
    public partial class FrmKhachHang : Form
    {
        SqlConnection conn;
        SqlDataAdapter ad = new SqlDataAdapter();
        DataTable tb = new DataTable();
        SqlCommand com;
        public FrmKhachHang()
        {
            InitializeComponent();
            conn = new SqlConnection("Data Source=LAPTOP-IK9U00KH\\TEST2;Initial Catalog =QLSHOPDT; Integrated Security = True");

        }
        public void Clear()
        {
            txtCMND.Clear();
            txtDiaChi.Clear();
            txtHoTen.Clear();
            txtSDT.Clear();
        }

        public void Hienthi()
        {
            //Kiem tra ket noi truoc khi mo
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            tb.Clear();
            com = conn.CreateCommand();
            com.CommandText = "SELECT * FROM KhachHang";
            com.ExecuteNonQuery();
            ad.SelectCommand = com;
            ad.Fill(tb);
            dataGridView1.DataSource = tb;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Hienthi();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            //Kiem tra ket noi truoc khi mo
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            string insertString;
            string ngay = dateTimePicker1.Value.ToString("yyyy/MM/dd");
            insertString = "insert into KhachHang values(N'" + txtCMND.Text + "',N'" + txtHoTen.Text + "',N'" + txtDiaChi.Text + "',N'" + txtSDT.Text + "','" + ngay + "')";
            //Khai bao commamnd moi
            SqlCommand cmd = new SqlCommand(insertString, conn);
            //Goi ExecuteNonQuery de gui command
            try
            {
                cmd.ExecuteNonQuery();
                Hienthi();
                Clear();
                //Kiem tra ket noi truoc khi dong
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                //Hien thi thong bao
                MessageBox.Show("Thanh cong");
            }
            catch (Exception ex)
            {
                MessageBox.Show("That bai");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtCMND.ReadOnly = true;
            int i;
            i = dataGridView1.CurrentRow.Index;
            txtCMND.Text = dataGridView1.Rows[i].Cells[0].Value.ToString();
            txtHoTen.Text = dataGridView1.Rows[i].Cells[1].Value.ToString();
            txtDiaChi.Text = dataGridView1.Rows[i].Cells[2].Value.ToString();
            txtSDT.Text = dataGridView1.Rows[i].Cells[3].Value.ToString();
            dateTimePicker1.Text = dataGridView1.Rows[i].Cells[4].Value.ToString();
        }

        private void FrmKhachHang_Load(object sender, EventArgs e)
        {
            conn.Open();
            Hienthi();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            //Kiem tra ket noi truoc khi mo
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            //Xac dinh chuoi truy van
            string deleteString;
            deleteString = "delete from KhachHang where MaKH = '" + txtCMND.Text + "'";
            //Khai bao commamnd moi
            SqlCommand cmd = new SqlCommand(deleteString, conn);
            //Goi ExecuteNonQuery de gui command
            try
            {
                cmd.ExecuteNonQuery();
                Clear();
                //Kiem tra ket noi truoc khi dong
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                //Hien thi thong bao
                MessageBox.Show("Thanh cong");
            }
            catch (Exception ex)
            {
                MessageBox.Show("That bai");
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {

            this.Close();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            txtCMND.ReadOnly = true;
            btnThem.Enabled = false;
            //Kiem tra ket noi truoc khi mo
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            //Xac dinh chuoi truy van
            string ngay = dateTimePicker1.Value.ToString("yyyy/MM/dd");
            string updateString;
            updateString = "update KhachHang set TenKH =N'" + txtHoTen.Text + "',DiaChi=N'" +txtDiaChi.Text+"',SDT='" +txtSDT.Text+"',NgaySinh='" +ngay+"' where MaKH = '" + txtCMND.Text + "'";
            //Khai bao commamnd moi
            SqlCommand cmd = new SqlCommand(updateString, conn);
            //Goi ExecuteNonQuery de gui command
            try
            {
                cmd.ExecuteNonQuery();
                Clear();
                //Kiem tra ket noi truoc khi dong
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                //Hien thi thong bao
                MessageBox.Show("Thanh cong");
            }
            catch (Exception ex)
            {
                MessageBox.Show("That bai");
            }
        }
    }
}
