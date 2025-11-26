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
using static SourceForDataBase.Form1;

namespace SourceForDataBase
{
    public partial class FormRegister : Form
    {
        public FormRegister()
        {
            InitializeComponent();
        }
        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirm = txtConfirm.Text.Trim();

            // Kiểm tra nhập trống
            if (username == "" || password == "" || confirm == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            // Kiểm tra nhập lại mật khẩu
            if (password != confirm)
            {
                MessageBox.Show("Mật khẩu nhập lại không khớp!");
                return;
            }

            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();

                // Kiểm tra tài khoản đã tồn tại
                string checkUser = "SELECT COUNT(*) FROM Users WHERE UserName=@u";
                SqlCommand cmdCheck = new SqlCommand(checkUser, conn);
                cmdCheck.Parameters.AddWithValue("@u", username);

                int exists = (int)cmdCheck.ExecuteScalar();
                if (exists > 0)
                {
                    MessageBox.Show("Tên tài khoản đã tồn tại! Hãy chọn tên khác.");
                    return;
                }

                // Thêm tài khoản mới
                string sql = "INSERT INTO Users(UserName, PasswordHash) VALUES (@u, @p)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@p", password);

                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Đăng ký thành công!");

                    // Quay lại form đăng nhập
                    Form1 login = new Form1();
                    login.Show();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }
        //nut quay lai form dang nhap
        private void btnBack_Click(object sender, EventArgs e)
        {
            Form1 login = new Form1();
            login.Show();
            this.Close();
        }
        //
        private void FormRegister_Load(object sender, EventArgs e)
        {

        }
    }
}
