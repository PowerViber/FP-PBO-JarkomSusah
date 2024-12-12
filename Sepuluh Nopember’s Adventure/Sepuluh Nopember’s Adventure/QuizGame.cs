using System;
using System.Windows.Forms;

namespace SepuluhNopemberAdventure
{
    public partial class QuizGame : Form
    {
        private int currentQuestionIndex = 0;
        private int score = 0;

        private string[] questions = new string[]
        {
            "Dimana letak Institut Teknologi Sepuluh Nopember?",
            "Salah satu nama fakultas di ITS",
            "Pada tahun berapa ITS didirikan?"
        };
         
        private string[,] answers = new string[,]
        {
            { "Surabaya", "Kota Bandung", "Cirebon", "Sidoarjo" },
            { "Fakultas Teknologi Elektro Informatika Cerdas", "Fakultas Kedokteran", "Fakultas Hukum", "Fakultas Ilmu Sosial Ilmu Politik" },
            { "1927", "1957", "2001", "1990" }
        };

        private string[] correctAnswers = new string[]
        {
            "Surabaya",
            "Fakultas Teknologi Elektro Informatika Cerdas",
            "1957"
        };

        public QuizGame()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Resize += new EventHandler(QuizGame_Resize);
            ShowQuestion();
        }

        private void QuizGame_Resize(object sender, EventArgs e)
        {
            lblQuestion.Size = new System.Drawing.Size(this.Width - 400, 100);
            lblQuestion.Location = new System.Drawing.Point(this.Width / 2 - lblQuestion.Width / 2, 100);

            btnOption1.Size = new System.Drawing.Size(this.Width / 2, 60);
            btnOption2.Size = new System.Drawing.Size(this.Width / 2, 60);
            btnOption3.Size = new System.Drawing.Size(this.Width / 2, 60);
            btnOption4.Size = new System.Drawing.Size(this.Width / 2, 60);

            btnOption1.Location = new System.Drawing.Point(this.Width / 2 - btnOption1.Width / 2, 300);
            btnOption2.Location = new System.Drawing.Point(this.Width / 2 - btnOption2.Width / 2, 380);
            btnOption3.Location = new System.Drawing.Point(this.Width / 2 - btnOption3.Width / 2, 460);
            btnOption4.Location = new System.Drawing.Point(this.Width / 2 - btnOption4.Width / 2, 540);
        }

        private void ShowQuestion()
        {
            if (currentQuestionIndex < questions.Length)
            {
                lblQuestion.Text = questions[currentQuestionIndex];
                btnOption1.Text = answers[currentQuestionIndex, 0];
                btnOption2.Text = answers[currentQuestionIndex, 1];
                btnOption3.Text = answers[currentQuestionIndex, 2];
                btnOption4.Text = answers[currentQuestionIndex, 3];
            }
            else
            {
                EndQuiz();
            }
        }

        private void btnOption_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton.Text == correctAnswers[currentQuestionIndex])
            {
                score++;
                MessageBox.Show("Betul! Kamu jawab dengan benar!");
            }
            else
            {
                MessageBox.Show("Salah!");
            }
            currentQuestionIndex++;
            ShowQuestion();
        }

        private void EndQuiz()
        {
            MessageBox.Show($"Selamat! Berikut adalah skor kamu: {score}", "Quiz Selesai", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        public void GameFinished()
        {
            MessageBox.Show("Sudah menamatkan Quiz ini, silahkan lanjut ke MiniGame selanjutnya.",
                            "Quiz Selesai", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        public void GameDone()
        {
            MessageBox.Show("Sudah menamatkan Quiz ini, silahkan pergi ke area terakhir",
                            "Quiz Selesai", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        public int GetScore()
        {
            return score;
        }
    }
}