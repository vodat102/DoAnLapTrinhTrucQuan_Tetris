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

namespace SourceForDataBase
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //tao chuoi ket noi den database

        public class Database
            {
            public static string connString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=GAMEDOAN;Integrated Security=True;";
            public static SqlConnection GetConnection()
                {
                    return new SqlConnection(connString);
                }
            }
        //dang ky tai khoan
        private void btnRegister_Click(object sender, EventArgs e)
        {
            FormRegister f = new FormRegister();
            f.Show();
            this.Hide();
        }
        //dang nhap tai khoan
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUser.Text;
            string password = txtPass.Text;

            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                string sql = "SELECT UserID FROM Users WHERE UserName=@u AND PasswordHash=@p";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@p", password);

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    int userId = Convert.ToInt32(result);
                    MessageBox.Show("Đăng nhập thành công!");

                    // Mở game form
                    GameForm game = new GameForm(userId);
                    game.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Sai tài khoản hoặc mật khẩu!");
                }
            }
        }
        //luu diem cao
        public void SaveHighScore(int userId, int score)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                string sql = "INSERT INTO HighScores(UserID, ScoreValue) VALUES (@uid, @score)";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@uid", userId);
                cmd.Parameters.AddWithValue("@score", score);

                cmd.ExecuteNonQuery();
            }
        }
        //luu trang thai game
        public void SaveGameState(int userId, string board, string currentPiece, string nextPiece, int score, int level)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();

                string sqlCheck =
                    "SELECT SaveID FROM SavedGame WHERE UserID=@uid";

                SqlCommand checkCmd = new SqlCommand(sqlCheck, conn);
                checkCmd.Parameters.AddWithValue("@uid", userId);

                object exists = checkCmd.ExecuteScalar();

                string sql;

                if (exists == null)
                {
                    // INSERT
                    sql = @"INSERT INTO SavedGame(UserID, BoardState, CurrentPiece, NextPiece, Score, Level)
                    VALUES(@uid, @board, @cur, @next, @score, @level)";
                }
                else
                {
                    // UPDATE
                    sql = @"UPDATE SavedGame SET 
                        BoardState=@board,
                        CurrentPiece=@cur,
                        NextPiece=@next,
                        Score=@score,
                        Level=@level,
                        SaveDate=GETDATE()
                    WHERE UserID=@uid";
                }

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@uid", userId);
                cmd.Parameters.AddWithValue("@board", board);
                cmd.Parameters.AddWithValue("@cur", currentPiece);
                cmd.Parameters.AddWithValue("@next", nextPiece);
                cmd.Parameters.AddWithValue("@score", score);
                cmd.Parameters.AddWithValue("@level", level);

                cmd.ExecuteNonQuery();
            }
        }
        //thuc hien tai tiep tuc game
        public SavedGameData LoadSavedGame(int userId)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();

                string sql = @"SELECT BoardState, CurrentPiece, NextPiece, Score, Level 
                       FROM SavedGame WHERE UserID=@uid";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@uid", userId);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new SavedGameData
                    {
                        BoardState = reader["BoardState"].ToString(),
                        CurrentPiece = reader["CurrentPiece"].ToString(),
                        NextPiece = reader["NextPiece"].ToString(),
                        Score = Convert.ToInt32(reader["Score"]),
                        Level = Convert.ToInt32(reader["Level"])
                    };
                }
                else
                    return null;
            }
        }

        public class SavedGameData
        {
            public string BoardState { get; set; }
            public string CurrentPiece { get; set; }
            public string NextPiece { get; set; }
            public int Score { get; set; }
            public int Level { get; set; }
        }
        // khi game khoi dong sau dang nhap

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
