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

    public partial class HoaDon : Form
    {
        SqlConnection conn;
        SqlDataAdapter ad = new SqlDataAdapter();
        DataTable tb = new DataTable();
        SqlCommand com;
        public HoaDon()
        {
            InitializeComponent();
            conn = new SqlConnection("Data Source=LAPTOP-IK9U00KH\\TEST2;Initial Catalog =QLSHOPDT; Integrated Security = True");

        }
        public void Hienthi()
        {
            txtTenSP.Enabled = false;
            txtGiaBan.Enabled = false;
            txtSLCon.Enabled = false;
            txtLinkAnh.Enabled = false;
            txtThanhTien.Enabled = false;
            //Kiem tra ket noi truoc khi mo
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            tb.Clear();
            com = conn.CreateCommand();
            com.CommandText = "SELECT * FROM ChiTietHoaDon";
            com.ExecuteNonQuery();
            ad.SelectCommand = com;
            ad.Fill(tb);
            dataGridView1.DataSource = tb;
        }
        public void Clear()
        {
            txtGiaBan.Clear();
            txtTenSP.Clear();
            txtGiamGia.Clear();
            txtGiaBan.Clear();
            txtLinkAnh.Clear();
            txtSLCon.Clear();
            txtSLMua.Clear();
            txtTenSP.Clear();
            txtThanhTien.Clear();
            cboMaHD.Items.Clear();
            cboMaSP.Items.Clear();
            pictureBox1.Image = null;
        }
        public void LoadMaHoaDon()
        {
            //Kiem tra ket noi truoc khi mo
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            string query = "SELECT DISTINCT MaHD FROM HoaDon";
            SqlCommand cmd = new SqlCommand(query, conn);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cboMaHD.Items.Add(reader["MaHD"].ToString());
                }
                reader.Close();
                //Kiem tra ket noi truoc khi dong
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thất bại" + ex.Message);
            }
        }
        public void LoadMaSP()
        {
            //Kiem tra ket noi truoc khi mo
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            string query = "SELECT DISTINCT MaSanPham FROM SanPham";
            SqlCommand cmd = new SqlCommand(query, conn);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cboMaSP.Items.Add(reader["MaSanPham"].ToString());
                }
                reader.Close();
                //Kiem tra ket noi truoc khi dong
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thất bại" + ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedID = cboMaSP.SelectedItem.ToString();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            string query = "SELECT TenSanPham,GiaBan,Anh,SoLuong FROM SanPham WHERE MaSanPham = @IDSanPham";
            com = new SqlCommand(query,conn);
            com.Parameters.AddWithValue("IDSanPham", selectedID);
            SqlDataReader reader2 = com.ExecuteReader();
            if (reader2.Read())
            {
                txtTenSP.Text = reader2["TenSanPham"].ToString();
                txtSLCon.Text = reader2["SoLuong"].ToString();
                txtGiaBan.Text = reader2["GiaBan"].ToString();
                txtLinkAnh.Text = reader2["Anh"].ToString();
                pictureBox1.Image = Image.FromFile(txtLinkAnh.Text);
            }
            reader2.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void HoaDon_Load(object sender, EventArgs e)
        {
            Hienthi();
            LoadMaHoaDon();
            LoadMaSP();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            //Kiem tra ket noi truoc khi mo
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            string IDSanPham = cboMaSP.SelectedItem.ToString();
            int SoluongMua;
            int thanhtien;
            if (!int.TryParse(txtThanhTien.Text, out thanhtien))
            {
                MessageBox.Show("Bạn chưa nhập số thanh toán");
                return;
            }

            if (!int.TryParse(txtSLMua.Text, out SoluongMua))
            {
                MessageBox.Show("Bạn chưa nhập số lượng mua");
                return;
            }
            int soLuongCon;
            if (!int.TryParse(txtSLCon.Text, out soLuongCon))
            {
                MessageBox.Show("Số lượng còn trong kho không hợp lệ.");
                return; // Thoát khỏi phương thức nếu số lượng không hợp lệ
            }
            if (SoluongMua > soLuongCon)
            {
                MessageBox.Show("Số lượng trong kho không đủ. Vui lòng nhập lại số lượng");
                txtSLMua.Clear();
                return;
            }
            else
            {


                string insertString;
                insertString = "insert into ChiTietHoaDon values(N'" + cboMaHD.Text + "',N'" + cboMaSP.Text + "'," + txtSLMua.Text + "," + txtGiaBan.Text + "," + txtThanhTien.Text + "," + txtGiamGia.Text + ")";
                string CapNhatSoLuong = "Update SanPham SET SoLuong = SoLuong - @SoluongMua WHERE MaSanPham = @ID";
                string query = "UPDATE HoaDon SET TongTien = TongTien + @ThanhTien FROM HOADON INNER JOIN ChiTietHoaDon ON HoaDon.MaHD = ChiTietHoaDon.MaHD";


                //Khai bao commamnd moi
                SqlCommand cmd2 = new SqlCommand(insertString, conn);
                SqlCommand cmd = new SqlCommand(CapNhatSoLuong, conn);
                SqlCommand cmd3 = new SqlCommand(query, conn);
                cmd3.Parameters.AddWithValue("@ThanhTien", thanhtien);
                cmd.Parameters.AddWithValue("@SoluongMua", SoluongMua);
                cmd.Parameters.AddWithValue("@ID", IDSanPham);
                //Goi ExecuteNonQuery de gui command
                try
                {
                    cmd.ExecuteNonQuery();
                    cmd2.ExecuteNonQuery();
                    cmd3.ExecuteNonQuery();
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
                    MessageBox.Show("That bai" + ex.Message);
                }
            }
        }
        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            Hienthi();
            Clear();
            LoadMaHoaDon();
            LoadMaSP();
            cboMaHD.Enabled = true;
            cboMaSP.Enabled = true;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            cboMaHD.Enabled = false;
            cboMaSP.Enabled = false;
            int i;
            i = dataGridView1.CurrentRow.Index;
            cboMaHD.Text = dataGridView1.Rows[i].Cells[0].Value.ToString();
            cboMaSP.Text = dataGridView1.Rows[i].Cells[1].Value.ToString();
            txtSLMua.Text = dataGridView1.Rows[i].Cells[2].Value.ToString();
            txtThanhTien.Text = dataGridView1.Rows[i].Cells[4].Value.ToString();

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            //Kiem tra ket noi truoc khi mo
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            string IDSanPham = cboMaSP.Text.ToString();
            int SoluongMua;
            if (!int.TryParse(txtSLMua.Text, out SoluongMua))
            {
                MessageBox.Show("Bạn chưa nhập số lượng mua");
                return;
            }
            int thanhtien;
            if (!int.TryParse(txtThanhTien.Text, out thanhtien))
            {
                MessageBox.Show("Bạn chưa nhập số thanh toán");
                return;
            }
            //Xac dinh chuoi truy van
            string CapNhatSoLuong = "Update SanPham SET SoLuong = SoLuong + @SoluongMua WHERE MaSanPham = @ID";
            string query = "UPDATE HoaDon SET TongTien = TongTien - @ThanhTien FROM HOADON INNER JOIN ChiTietHoaDon ON HoaDon.MaHD = ChiTietHoaDon.MaHD";

            string deleteString;
            deleteString = "delete from ChiTietHoaDon where MaHD = '" + cboMaHD.Text + "' AND MaSanPham = '" +cboMaSP.Text+"'";
            //Khai bao commamnd moi
            SqlCommand cmd2 = new SqlCommand(deleteString, conn);
            SqlCommand cmd = new SqlCommand(CapNhatSoLuong, conn);
            SqlCommand cmd3 = new SqlCommand(query, conn);
            cmd3.Parameters.AddWithValue("@ThanhTien", thanhtien);
            cmd.Parameters.AddWithValue("@SoluongMua", SoluongMua);
            cmd.Parameters.AddWithValue("@ID", IDSanPham);
            //Goi ExecuteNonQuery de gui command
            try
            {
                cmd.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                cmd3.ExecuteNonQuery();
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
                MessageBox.Show("That bai, " + ex.Message);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int Tongtien;
            if (!int.TryParse(txtGiaBan.Text, out int giaban))
            {
                MessageBox.Show("Giá bán không hợp lệ!");
                return;
            }

            if (!int.TryParse(txtGiamGia.Text, out int giamgia))
            {
                MessageBox.Show("Giảm giá không hợp lệ!");
                return;
            }

            if (!int.TryParse(txtSLMua.Text, out int soluong))
            {
                MessageBox.Show("Số lượng mua không hợp lệ!");
                return;
            }

            Tongtien = (giaban - giamgia) * soluong;
            txtThanhTien.Text = Tongtien.ToString();
        }
    }
}
