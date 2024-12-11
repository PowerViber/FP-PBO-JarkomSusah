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

        private const int defaultBallSpeed = 5; // Default ball speed
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

        private int npcHorizontalDirection = 1; 
        private int playerVerticalSpeed = 0; 
        private int playerHorizontalSpeed = 0; 
        private int npcVerticalSpeed = 0; 
        private int npcHorizontalSpeed = 5; 

        bool win = false;

        public PingPongGame()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            this.Text = "Ping Pong Game";
            this.Size = new Size(1000, 800); 
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
                keyStates[e.KeyCode] = true; 
            }
        }

        private void PingPongGame_KeyUp(object sender, KeyEventArgs e)
        {
            if (keyStates.ContainsKey(e.KeyCode))
            {
                keyStates[e.KeyCode] = false; 
            }
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            // Track player paddle vertical speed
            playerVerticalSpeed = 0;
            playerHorizontalSpeed = 0;

            // Player paddle movement using keyStates
            if (keyStates[Keys.W] && playerPaddle.Top > 0)
            {
                playerPaddle.Top -= paddleSpeed;
                playerVerticalSpeed = -paddleSpeed;
            }
            if (keyStates[Keys.S] && playerPaddle.Bottom < gameField.Height)
            {
                playerPaddle.Top += paddleSpeed;
                playerVerticalSpeed = paddleSpeed;
            }

            // Player paddle horizontal movement (A and D)
            if (keyStates[Keys.A] && playerPaddle.Left > 0)
            {
                playerPaddle.Left -= paddleSpeed;
                playerHorizontalSpeed = -paddleSpeed;
            }
            if (keyStates[Keys.D] && playerPaddle.Right < gameField.Width / 2)
            {
                playerPaddle.Left += paddleSpeed;
                playerHorizontalSpeed = paddleSpeed;
            }

            // NPC paddle vertical movement
            npcVerticalSpeed = 0;
            if (ball.Top > npcPaddle.Top + npcPaddle.Height / 2 && npcPaddle.Bottom < gameField.Height)
            {
                npcPaddle.Top += npcPaddleSpeed;
                npcVerticalSpeed = npcPaddleSpeed;
            }
            else if (ball.Top < npcPaddle.Top + npcPaddle.Height / 2 && npcPaddle.Top > 0)
            {
                npcPaddle.Top -= npcPaddleSpeed;
                npcVerticalSpeed = -npcPaddleSpeed;
            }

            // NPC paddle horizontal movement(randomized)
            npcPaddle.Left += npcHorizontalDirection * npcHorizontalSpeed;
            if (npcPaddle.Left <= gameField.Width / 2 || npcPaddle.Right >= gameField.Width)
            {
                npcHorizontalDirection *= -1; 
            }

            // Ball movement
            ball.Left += ballSpeedX;
            ball.Top += ballSpeedY;

            // Ball collision with top and bottom walls
            if (ball.Top <= 0)
            {
                ball.Top = 0;
                ballSpeedY = random.Next(3, 10);
                ballSpeedY = Math.Abs(ballSpeedY);
            }
            else if (ball.Bottom >= gameField.Height)
            {
                ball.Top = gameField.Height - ball.Height; // Correct position at the bottom
                ballSpeedY = random.Next(3, 10);
                ballSpeedY = -Math.Abs(ballSpeedY);
            }

            // Ball collision with player paddle
            if (ball.Bounds.IntersectsWith(playerPaddle.Bounds) && ballSpeedX < 0)
            {
                ball.Left = playerPaddle.Right; // Correct position to prevent overlap

                // Reverse Y direction based on collision position
                if (ball.Bottom > playerPaddle.Top && ball.Top < playerPaddle.Bottom)
                {
                    ballSpeedY = ball.Top < playerPaddle.Top ? Math.Abs(ballSpeedY) : -Math.Abs(ballSpeedY);
                }

                // Reset ball speed to default
                ballSpeedX = defaultBallSpeed;
                //if(ballSpeedY > 0)
                //    ballSpeedY = -ballSpeedY;
                //else
                //    ballSpeedY = ballSpeedY;

                // Calculate resultant speed using paddle movement
                double resultantSpeed = Math.Sqrt(playerHorizontalSpeed * playerHorizontalSpeed +
                                                  playerVerticalSpeed * playerVerticalSpeed);
                ballSpeedX += (int)Math.Ceiling(resultantSpeed);
                ballSpeedX = Math.Abs(ballSpeedX); // Ensure it moves toward the NPC
            }

            // Ball collision with NPC paddle
            if (ball.Bounds.IntersectsWith(npcPaddle.Bounds) && ballSpeedX > 0)
            {
                ball.Left = npcPaddle.Left - ball.Width; // Correct position to prevent overlap

                // Reverse Y direction based on collision position
                if (ball.Bottom > npcPaddle.Top && ball.Top < npcPaddle.Bottom)
                {
                    ballSpeedY = ball.Top < npcPaddle.Top ? Math.Abs(ballSpeedY) : -Math.Abs(ballSpeedY);
                }

                // Reset ball speed to default
                ballSpeedX = defaultBallSpeed;
                //if (ballSpeedY < 0)
                //    ballSpeedY = -ballSpeedY;
                //else
                //    ballSpeedY = ballSpeedY;

                // Calculate resultant speed using paddle movement
                double resultantSpeed = Math.Sqrt(npcVerticalSpeed * npcVerticalSpeed);
                ballSpeedX += (int)Math.Ceiling(resultantSpeed);
                ballSpeedX = -Math.Abs(ballSpeedX); // Ensure it moves toward the player
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
                win = true;
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
            // Reset the positions of both paddles and the ball
            ResetPositions();

            // Reset ball speed
            ResetBallSpeed(playerScored);
        }

        private void ResetBallSpeed(bool playerScored)
        {
            int speedX = random.Next(3, 10);
            int speedY = random.Next(3, 10) * (random.Next(0, 2) == 0 ? 1 : -1); 

            // Set X direction: positive (right) if player scored, negative (left) if NPC scored
            ballSpeedX = playerScored ? speedX : -speedX;
            ballSpeedY = speedY;
        }

        private void ResetPositions()
        {
            // Reset player paddle position to the left center
            playerPaddle.Location = new Point(30, gameField.Height / 2 - playerPaddle.Height / 2);

            // Reset NPC paddle position to the right center
            npcPaddle.Location = new Point(gameField.Width - 45, gameField.Height / 2 - npcPaddle.Height / 2);

            // Reset ball position to the center
            ball.Location = new Point(gameField.Width / 2 - ball.Width / 2, gameField.Height / 2 - ball.Height / 2);
        }
        
        public bool CheckWin()
        {
            return win;
        }

        public void GameDone()
        {
            MessageBox.Show("Sudah menamatkan MiniGame ini, silahkan pergi ke area terakhir",
                            "Quiz Selesai", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
