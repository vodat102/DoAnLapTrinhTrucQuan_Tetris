using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SourceForDataBase.Form1;

namespace SourceForDataBase
{
    public partial class GameForm : Form
    {
        private int userId;
        public GameForm(int userId)
        {
            InitializeComponent();
            this.userId = userId;
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            // Lấy thông tin user để hiển thị
            lblWelcome.Text = "Xin chào, người chơi #" + userId;

            // Tải dữ liệu save game từ SQL
            SavedGameData saved = LoadSavedGame(userId);

            if (saved != null)
            {
                // Có save → khôi phục trạng thái game
                RestoreGame(saved);
            }
            else
            {
                // Không có save → tạo game mới
                StartNewGame();
            }
        }
        public SavedGameData LoadSavedGame(int userId)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                string sql = @"SELECT BoardState, CurrentPiece, NextPiece, Score, Level
                       FROM SavedGame WHERE UserID = @uid";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@uid", userId);

                SqlDataReader rd = cmd.ExecuteReader();

                if (rd.Read())
                {
                    return new SavedGameData
                    {
                        BoardState = rd["BoardState"].ToString(),
                        CurrentPiece = rd["CurrentPiece"].ToString(),
                        NextPiece = rd["NextPiece"].ToString(),
                        Score = Convert.ToInt32(rd["Score"]),
                        Level = Convert.ToInt32(rd["Level"])
                    };
                }
            }
            return null;
        }
        public class SavedGameData
        {
            public string BoardState { get; set; }
            public string CurrentPiece { get; set; }
            public string NextPiece { get; set; }
            public int Score { get; set; }
            public int Level { get; set; }
        }
        private void StartNewGame()
        {
           // score = 0;
           // level = 1;

            // Board rỗng
           // CreateEmptyBoard();

            // Sinh các khối mới
           // SpawnNewPiece();
        }

        private void RestoreGame(SavedGameData data)
        {
            //score = data.Score;
            //level = data.Level;

            // Load board
            //LoadBoard(data.BoardState);

            // Load khối đang rơi
            //currentPiece = DeserializePiece(data.CurrentPiece);

            // Load khối tiếp theo
            //nextPiece = DeserializePiece(data.NextPiece);
        }
        /*
                private void btnSaveGame_Click(object sender, EventArgs e)
                {
                    SaveGameState(userId);
                    MessageBox.Show("Đã lưu trạng thái game!");
                }
                private void SaveGameState(int userId)
                {
                    using (SqlConnection conn = Database.GetConnection())
                    {
                        conn.Open();

                        string board = SerializeBoard();
                        string current = SerializePiece(currentPiece);
                        string next = SerializePiece(nextPiece);

                        string sqlCheck = "SELECT SaveID FROM SavedGame WHERE UserID=@uid";
                        SqlCommand checkCmd = new SqlCommand(sqlCheck, conn);
                        checkCmd.Parameters.AddWithValue("@uid", userId);

                        bool exists = checkCmd.ExecuteScalar() != null;

                        string sql;

                        if (!exists)
                        {
                            sql = @"INSERT INTO SavedGame(UserID, BoardState, CurrentPiece, NextPiece, Score, Level)
                            VALUES (@uid, @board, @cur, @next, @score, @level)";
                        }
                        else
                        {
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
                        cmd.Parameters.AddWithValue("@cur", current);
                        cmd.Parameters.AddWithValue("@next", next);
                        cmd.Parameters.AddWithValue("@score", score);
                        cmd.Parameters.AddWithValue("@level", level);

                        cmd.ExecuteNonQuery();
                    }
                }
        */
        private void btnLogout_Click(object sender, EventArgs e)
        {
            Form1 login = new Form1();
            login.Show();
            this.Close();
        }

    }
}
