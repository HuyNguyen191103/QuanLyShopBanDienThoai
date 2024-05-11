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
    public partial class FrmSanPham : Form
    {
        SqlConnection conn;
        SqlDataAdapter ad = new SqlDataAdapter();
        DataTable tb = new DataTable();
        SqlCommand com;
        public FrmSanPham()
        {
            InitializeComponent();
            conn = new SqlConnection("Data Source=LAPTOP-IK9U00KH\\TEST2;Initial Catalog =QLSHOPDT; Integrated Security = True");

        }
        public void Clear()
        {
            txtMaSP.Clear();
            txtTenSP.Clear();
            txtSL.Clear();
            txtPic.Clear();
            txtGiaBan.Clear();
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
            com.CommandText = "SELECT * FROM SanPham";
            com.ExecuteNonQuery();
            ad.SelectCommand = com;
            ad.Fill(tb);
            dataGridView1.DataSource = tb;
        }
        public void LoadMaLoaiSanPham()
        {
            //Kiem tra ket noi truoc khi mo
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            string query = "SELECT DISTINCT MaSP FROM LoaiSanPham";
            SqlCommand cmd = new SqlCommand(query, conn);
            try
            {
                
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cboLoaiSanPham.Items.Add(reader["MaSP"].ToString());
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void btnOpenLink_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Bitmap(*.bmp) |*.bmp|JPEG(*.jpg) |*.jpg|GIF(*.gif) |*.gif|All files(*.*) |*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.Title = "Chọn ảnh minh họa";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(openFileDialog.FileName);
                txtPic.Text = openFileDialog.FileName;
            }
        }

        private void FrmSanPham_Load(object sender, EventArgs e)
        {
            conn.Open();
            Hienthi();
            LoadMaLoaiSanPham();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            //Kiem tra ket noi truoc khi mo
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            string insertString;
            insertString = "insert into SanPham values(N'" + txtMaSP.Text + "',N'" + txtTenSP.Text + "'," + txtGiaBan.Text + ",'" + txtPic.Text + "'," + txtSL.Text + ",N'" +cboLoaiSanPham.Text+ "')";
            //Khai bao commamnd moi
            SqlCommand cmd = new SqlCommand(insertString, conn);
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
                MessageBox.Show("That bai" +ex.Message);
            }

        }

        private void cboLoaiSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Hienthi();
            btnThem.Enabled = true;
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
            deleteString = "delete from SanPham where MaSanPham = '" + txtMaSP.Text + "'";
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
                MessageBox.Show("That bai, "+ex.Message);   
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaSP.ReadOnly = true;
            int i;
            i = dataGridView1.CurrentRow.Index;
            txtMaSP.Text = dataGridView1.Rows[i].Cells[0].Value.ToString();
            txtTenSP.Text = dataGridView1.Rows[i].Cells[1].Value.ToString();
            txtGiaBan.Text = dataGridView1.Rows[i].Cells[2].Value.ToString();
            txtPic.Text = dataGridView1.Rows[i].Cells[3].Value.ToString();
            pictureBox1.Image = Image.FromFile(txtPic.Text);
            txtSL.Text = dataGridView1.Rows[i].Cells[4].Value.ToString();
            cboLoaiSanPham.Text = dataGridView1.Rows[i].Cells[5].Value.ToString();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //Kiem tra ket noi truoc khi mo
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            string insertSearch;
            insertSearch = "SELECT * FROM SanPham WHERE TenSanPham LIKE @keyword";
            //Khai bao commamnd moi
            SqlCommand cmd = new SqlCommand(insertSearch, conn);
            cmd.Parameters.AddWithValue("@keyword", "%" + txtTenSP.Text + "%");
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
                MessageBox.Show("That bai, "+ex.Message);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            txtMaSP.ReadOnly = true;
            btnThem.Enabled = false;
            //Kiem tra ket noi truoc khi mo
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            //Xac dinh chuoi truy van
            string updateString;
            updateString = "update SanPham set TenSanPham =N'" + txtTenSP.Text + "',GiaBan='" + txtGiaBan.Text + "',Anh=N'" + txtPic.Text + "',SoLuong=" + txtSL.Text + ",MaSP=N'" +cboLoaiSanPham.Text+"' where MaSanPham = '" + txtMaSP.Text + "'";
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
