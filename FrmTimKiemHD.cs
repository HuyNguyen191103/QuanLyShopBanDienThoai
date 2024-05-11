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
    public partial class FrmTimKiemHD : Form
    {
        SqlConnection conn;
        SqlDataAdapter ad = new SqlDataAdapter();
        DataTable tb = new DataTable();
        SqlCommand com;

        public void Hienthi()
        {
            //Kiem tra ket noi truoc khi mo
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            tb.Clear();
            com = conn.CreateCommand();
            com.CommandText = "SELECT * FROM HoaDon";
            com.ExecuteNonQuery();
            ad.SelectCommand = com;
            ad.Fill(tb);
            dataGridView1.DataSource = tb;
        }
        public void Clear()
        {
            txtMaHD.Clear();
            txtTongTien.Clear();
        }

        public void LoadMaNhanVien()
        {
            string query = "SELECT DISTINCT MaNV FROM NhanVien";
            SqlCommand cmd = new SqlCommand(query, conn);
            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    cboMaNV.Items.Add(reader["MaNV"].ToString());
                }
                reader.Close();
                //Kiem tra ket noi truoc khi dong
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            } catch(Exception ex)
            {
                MessageBox.Show("Thất bại" +ex.Message);
            }
        }

        public void LoadMaKhachHang()
        {
            string query = "SELECT DISTINCT MaKH FROM KhachHang";
            SqlCommand cmd = new SqlCommand(query, conn);
            try
            {
                
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cboMaKH.Items.Add(reader["MaKH"].ToString());
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

        public FrmTimKiemHD()
        {
            InitializeComponent();
            conn = new SqlConnection("Data Source=LAPTOP-IK9U00KH\\TEST2;Initial Catalog =QLSHOPDT; Integrated Security = True");

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void FrmTimKiemHD_Load(object sender, EventArgs e)
        {
            conn.Open();
            Hienthi();
            LoadMaKhachHang();
            LoadMaNhanVien();
        }

        private void txtMaHD_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtTongTien_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            //Kiem tra ket noi truoc khi mo
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            string insertSearch;
            insertSearch = "SELECT * FROM HoaDon WHERE MaHD LIKE @keyword";
            //Khai bao commamnd moi
            SqlCommand cmd = new SqlCommand(insertSearch, conn);
            cmd.Parameters.AddWithValue("@keyword","%" +txtMaHD.Text+ "%");
            //Goi ExecuteNonQuery de gui command
            try
            {
                ad = new SqlDataAdapter(cmd);
                tb = new DataTable();
                ad.Fill(tb);
                dataGridView1.DataSource = tb;
            }
            catch (Exception ex)
            {
                MessageBox.Show("That bai");
            }
        }

        private void txtReset_Click(object sender, EventArgs e)
        {
            Hienthi();
            btnThem.Enabled = true;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
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
            insertString = "insert into HoaDon values(N'" + txtMaHD.Text + "','" + ngay + "'," + txtTongTien.Text + ",N'" + cboMaNV.Text + "',N'" + cboMaKH.Text + "')";
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

        private void btnXoa_Click(object sender, EventArgs e)
        {
            //Kiem tra ket noi truoc khi mo
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            //Xac dinh chuoi truy van
            string deleteString;
            deleteString = "delete from HoaDon where MaHD = '" + txtMaHD.Text + "'";
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaHD.ReadOnly = true;
            btnThem.Enabled = false;
            int i;
            i = dataGridView1.CurrentRow.Index;
            txtMaHD.Text = dataGridView1.Rows[i].Cells[0].Value.ToString();
            dateTimePicker1.Text = dataGridView1.Rows[i].Cells[1].Value.ToString();
            txtTongTien.Text = dataGridView1.Rows[i].Cells[2].Value.ToString();
            cboMaNV.Text = dataGridView1.Rows[i].Cells[3].Value.ToString();
            cboMaKH.Text = dataGridView1.Rows[i].Cells[4].Value.ToString();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            txtMaHD.ReadOnly = true;
            btnThem.Enabled = false;
            //Kiem tra ket noi truoc khi mo
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            //Xac dinh chuoi truy van
            string ngay = dateTimePicker1.Value.ToString("yyyy/MM/dd");
            string updateString;
            updateString = "update HoaDon set NgayBan ='" + ngay + "',TongTien=" + txtTongTien.Text + ",MaNV=N'" + cboMaNV.Text + "',MaKH=N'" + cboMaKH.Text + "' where MaHD = '" + txtMaHD.Text + "'";
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
                MessageBox.Show("That bai, "+ex.Message);
            }
        }
    }
}
