using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SepuluhNopemberAdventure
{
    public class PingPongGame : Form
    {
        private Panel gameField;
        private Panel playerPaddle;
        private Panel npcPaddle;
        private Panel ball;
        private System.Windows.Forms.Timer gameTimer;
        private Label scoreLabel;

        private int ballSpeedX;
        private int ballSpeedY;
        private int paddleSpeed = 10;
        private int npcPaddleSpeed = 5;

        private int playerScore = 0;
        private int npcScore = 0;

        private Random random = new Random();

        private Dictionary<Keys, bool> keyStates = new Dictionary<Keys, bool>
        {
            { Keys.W, false },
            { Keys.S, false },
            { Keys.A, false },
            { Keys.D, false }
        };

        private int npcHorizontalDirection = 1; // 1 for right, -1 for left
        private int npcHorizontalSpeed = 5; // Speed for horizontal movement

        public PingPongGame()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            this.Text = "Ping Pong Game";
            this.Size = new Size(1000, 800); // Larger form size for zoom-out effect
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Game field setup (zoomed-out area)
            gameField = new Panel
            {
                Size = new Size(800, 600), // Smaller game field size
                Location = new Point(100, 100), // Margins (zoom-out effect)
                BackColor = Color.LightGray
            };
            this.Controls.Add(gameField);

            // Player paddle setup
            playerPaddle = new Panel
            {
                Size = new Size(15, 100),
                Location = new Point(30, gameField.Height / 2 - 50),
                BackColor = Color.Blue
            };
            gameField.Controls.Add(playerPaddle);

            // NPC paddle setup
            npcPaddle = new Panel
            {
                Size = new Size(15, 100),
                Location = new Point(gameField.Width - 45, gameField.Height / 2 - 50),
                BackColor = Color.Red
            };
            gameField.Controls.Add(npcPaddle);

            // Ball setup
            ball = new Panel
            {
                Size = new Size(15, 15),
                Location = new Point(gameField.Width / 2 - 7, gameField.Height / 2 - 7),
                BackColor = Color.Green
            };
            gameField.Controls.Add(ball);

            // Score label
            scoreLabel = new Label
            {
                Text = $"Player: {playerScore}  |  NPC: {npcScore}",
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(this.ClientSize.Width / 2 - 100, 20),
                AutoSize = true
            };
            this.Controls.Add(scoreLabel);

            // Game timer
            gameTimer = new System.Windows.Forms.Timer { Interval = 16 };
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();

            // Key events for player control
            this.KeyDown += PingPongGame_KeyDown;
            this.KeyUp += PingPongGame_KeyUp;

            ResetBallSpeed(true);
        }

        private void PingPongGame_KeyDown(object sender, KeyEventArgs e)
        {
            if (keyStates.ContainsKey(e.KeyCode))
            {
                keyStates[e.KeyCode] = true; // Mark key as pressed
            }
        }

        private void PingPongGame_KeyUp(object sender, KeyEventArgs e)
        {
            if (keyStates.ContainsKey(e.KeyCode))
            {
                keyStates[e.KeyCode] = false; // Mark key as released
            }
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            // Player paddle movement using keyStates
            if (keyStates[Keys.W] && playerPaddle.Top > 0)
            {
                playerPaddle.Top -= paddleSpeed;
            }
            if (keyStates[Keys.S] && playerPaddle.Bottom < gameField.Height)
            {
                playerPaddle.Top += paddleSpeed;
            }

            // Player paddle horizontal movement (A and D)
            if (keyStates[Keys.A] && playerPaddle.Left > 0)
            {
                playerPaddle.Left -= paddleSpeed;
            }
            if (keyStates[Keys.D] && playerPaddle.Right < gameField.Width / 2)
            {
                playerPaddle.Left += paddleSpeed;
            }

            // NPC paddle vertical movement
            if (ball.Top > npcPaddle.Top + npcPaddle.Height / 2 && npcPaddle.Bottom < gameField.Height)
            {
                npcPaddle.Top += npcPaddleSpeed;
            }
            else if (ball.Top < npcPaddle.Top + npcPaddle.Height / 2 && npcPaddle.Top > 0)
            {
                npcPaddle.Top -= npcPaddleSpeed;
            }

            // NPC paddle horizontal movement (randomized)
            npcPaddle.Left += npcHorizontalDirection * npcHorizontalSpeed;
            if (npcPaddle.Left <= gameField.Width / 2 || npcPaddle.Right >= gameField.Width)
            {
                npcHorizontalDirection *= -1; // Reverse direction
            }

            // Ball movement
            ball.Left += ballSpeedX;
            ball.Top += ballSpeedY;

            // Ball collision with top and bottom walls
            if (ball.Top <= 0)
            {
                ball.Top = 0;
                ballSpeedY = GetRandomizedSpeed(); // Reverse and randomize speed
            }
            else if (ball.Bottom >= gameField.Height)
            {
                ball.Top = gameField.Height - ball.Height; // Correct position at the bottom
                ballSpeedY = -GetRandomizedSpeed(); // Reverse and randomize speed
            }

            // Ball collision with player paddle
            if (ball.Bounds.IntersectsWith(playerPaddle.Bounds) && ballSpeedX < 0)
            {
                ball.Left = playerPaddle.Right; // Correct position
                ballSpeedX = GetRandomizedSpeed();
            }

            // Ball collision with NPC paddle
            if (ball.Bounds.IntersectsWith(npcPaddle.Bounds) && ballSpeedX > 0)
            {
                ball.Left = npcPaddle.Left - ball.Width; // Correct position
                ballSpeedX = -GetRandomizedSpeed();
            }

            // Ball out of bounds
            if (ball.Left <= 0)
            {
                npcScore++;
                ResetBall(false);
            }
            else if (ball.Right >= gameField.Width)
            {
                playerScore++;
                ResetBall(true);
            }

            // Update score label
            scoreLabel.Text = $"Player: {playerScore}  |  NPC: {npcScore}";

            // End game condition
            if (playerScore >= 5 && playerScore >= npcScore + 3)
            {
                gameTimer.Stop();
                MessageBox.Show("You win!", "Ping Pong Game", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else if (npcScore >= 5 && npcScore >= playerScore + 3)
            {
                gameTimer.Stop();
                MessageBox.Show("NPC wins!", "Ping Pong Game", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void ResetBall(bool playerScored)
        {
            // Reset ball position to the center
            ball.Location = new Point(gameField.Width / 2 - ball.Width / 2, gameField.Height / 2 - ball.Height / 2);

            // Reset ball speed with direction based on who scored
            ResetBallSpeed(playerScored);
        }

        private void ResetBallSpeed(bool playerScored)
        {
            int speedX = random.Next(3, 10);
            int speedY = random.Next(3, 10) * (random.Next(0, 2) == 0 ? 1 : -1); // Randomize Y direction

            // Set X direction: positive (right) if player scored, negative (left) if NPC scored
            ballSpeedX = playerScored ? speedX : -speedX;
            ballSpeedY = speedY;
        }


        private int GetRandomizedSpeed()
        {
            return random.Next(3, 10); // Random speed between 3 and 10
        }
    }
}
