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
        private int npcPaddleSpeed = 2;
        private int originalPaddleSpeed;
        private int originalPaddleNpcSpeed;

        private int playerScore = 0;
        private int npcScore = 0;

        private Random random = new Random();
        private System.Windows.Forms.Timer speedBoostTimer;
        private System.Windows.Forms.Timer cooldownTimer;
        private bool isCooldownActive = false;
        private int cooldownTimeLeft = 10;
        private Label cooldownLabel;


        private Dictionary<Keys, bool> keyStates = new Dictionary<Keys, bool>
        {
            { Keys.W, false },
            { Keys.S, false },
            { Keys.A, false },
            { Keys.D, false },
            { Keys.F, false }
        };

        private int npcHorizontalDirection = 1; 
        private int playerVerticalSpeed = 0; 
        private int playerHorizontalSpeed = 0; 
        private int npcVerticalSpeed = 0; 
        private int npcHorizontalSpeed = 2; 

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

            // Game field setup 
            gameField = new Panel
            {
                Size = new Size(800, 600), 
                Location = new Point(100, 100), // Margins 
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

            // Make the ball round
            ball.Region = new Region(new System.Drawing.Drawing2D.GraphicsPath());
            using (var path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                path.AddEllipse(0, 0, ball.Width, ball.Height);
                ball.Region = new Region(path); 
            }

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

            // Cooldown label
            cooldownLabel = new Label
            {
                Text = "F",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.Green,
                Location = new Point(gameField.Left, gameField.Bottom + 10),
                AutoSize = true
            };
            this.Controls.Add(cooldownLabel);

            // Game timer
            gameTimer = new System.Windows.Forms.Timer { Interval = 16 };
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();

            // Speed boost timer
            speedBoostTimer = new System.Windows.Forms.Timer();
            speedBoostTimer.Interval = 2000; // 2 seconds
            speedBoostTimer.Tick += SpeedBoostTimer_Tick;

            // Cooldown timer
            cooldownTimer = new System.Windows.Forms.Timer();
            cooldownTimer.Interval = 1000; // 1-second intervals
            cooldownTimer.Tick += CooldownTimer_Tick;

            originalPaddleSpeed = paddleSpeed;
            originalPaddleNpcSpeed = npcPaddleSpeed;

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

            // F skill
            if (keyStates[Keys.F] &&
            playerPaddle.Right < gameField.Width / 2 &&
            ball.Left < gameField.Width / 2 &&
            !isCooldownActive)
            {
                // Swap the positions of the ball and the paddle
                Point tempBallPosition = ball.Location;
                Point tempPaddlePosition = playerPaddle.Location;

                // Set new positions
                ball.Location = new Point(tempPaddlePosition.X + playerPaddle.Width + 10, tempPaddlePosition.Y + (playerPaddle.Height / 2) - (ball.Height / 2));
                playerPaddle.Location = new Point(tempBallPosition.X - playerPaddle.Width - 10, tempBallPosition.Y - (playerPaddle.Height / 2) + (ball.Height / 2));

                // Boost player paddle speed
                paddleSpeed += 5;
                speedBoostTimer.Start();

                ballSpeedY = random.Next(-50, 50);
                if (ballSpeedX < 0)
                    ballSpeedX = random.Next(20, 50);
                else
                    ballSpeedX = random.Next(-50, -20);

                playerPaddle.BackColor = Color.Yellow;

                // Start cooldown
                isCooldownActive = true;
                cooldownTimeLeft = 10;
                cooldownLabel.Text = $"F: {cooldownTimeLeft}s";
                cooldownTimer.Start();
            }

            // Passive effect: Increase paddle size and speed if the score difference is 2
            if (npcScore >= playerScore + 2 || playerScore >= npcScore + 2)
            {
                // Apply passive effect for the player
                if (npcScore >= playerScore + 2)
                {
                    if (playerPaddle.Height != 150) 
                    {
                        playerPaddle.Height = 150;
                        paddleSpeed += 2;          
                    }
                }
                else // Apply passive effect for the NPC
                {
                    if (npcPaddle.Height != 150) 
                    {
                        npcPaddle.Height = 150;   
                        npcPaddleSpeed += 2;      
                    }
                }
            }
            else
            {
                if (playerPaddle.Height != 100)
                {
                    playerPaddle.Height = 100;
                    paddleSpeed = originalPaddleSpeed;
                }

                if (npcPaddle.Height != 100)
                {
                    npcPaddle.Height = 100;
                    npcPaddleSpeed = originalPaddleNpcSpeed;
                }
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
                ball.Top = gameField.Height - ball.Height; 
                ballSpeedY = random.Next(3, 10);
                ballSpeedY = -Math.Abs(ballSpeedY);
            }

            // Ball collision with player paddle
            if (ball.Bounds.IntersectsWith(playerPaddle.Bounds) && ballSpeedX < 0)
            {
                int temp;
                ball.Left = playerPaddle.Right; 

                // Reset ball speed to default
                ballSpeedX = defaultBallSpeed;
                temp = random.Next(1, 3);
                if (temp == 1)
                    ballSpeedY = -ballSpeedY;
                else
                    ballSpeedY = ballSpeedY;

                // Calculate resultant speed using paddle movement
                double resultantSpeed = Math.Sqrt(playerHorizontalSpeed * playerHorizontalSpeed +
                                                  playerVerticalSpeed * playerVerticalSpeed);
                ballSpeedX += (int)Math.Ceiling(resultantSpeed);
                ballSpeedX = Math.Abs(ballSpeedX);
            }

            // Ball collision with NPC paddle
            if (ball.Bounds.IntersectsWith(npcPaddle.Bounds) && ballSpeedX > 0)
            {
                int temp;
                ball.Left = npcPaddle.Left - ball.Width; 

                // Reset ball speed to default
                ballSpeedX = defaultBallSpeed;
                temp = random.Next(1, 3);
                if (temp == 1)
                    ballSpeedY = -ballSpeedY;
                else
                    ballSpeedY = ballSpeedY;


                // Calculate resultant speed using paddle movement
                double resultantSpeed = Math.Sqrt(npcVerticalSpeed * npcVerticalSpeed);
                ballSpeedX += (int)Math.Ceiling(resultantSpeed);
                ballSpeedX = -Math.Abs(ballSpeedX); 
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
            if (playerScore >= 5 && playerScore >= npcScore + 3 && !win)
            {
                win = true;
                gameTimer.Stop();
                MessageBox.Show("You win!", "Ping Pong Game", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else if (npcScore >= 5 && npcScore >= playerScore + 3 && !win)
            {
                gameTimer.Stop();
                MessageBox.Show("NPC wins!", "Ping Pong Game", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void ResetBall(bool playerScored)
        {
            ResetBallPositions();
            ResetPositions();
            ResetBallSpeed(playerScored);
        }

        private void ResetBallSpeed(bool playerScored)
        {
            int speedX = random.Next(3, 10);
            int speedY = random.Next(3, 10) * (random.Next(0, 2) == 0 ? 1 : -1); 
            ballSpeedX = playerScored ? speedX : -speedX;
            ballSpeedY = speedY;
        }

        private void ResetPositions()
        {
            // Reset player paddle position to the left center
            playerPaddle.Location = new Point(30, gameField.Height / 2 - playerPaddle.Height / 2);

            // Reset NPC paddle position to the right center
            npcPaddle.Location = new Point(gameField.Width - 45, gameField.Height / 2 - npcPaddle.Height / 2);
        }

        private void ResetBallPositions()
        {
            // Reset ball position to the center
            ball.Location = new Point(gameField.Width / 2 - ball.Width / 2, gameField.Height / 2 - ball.Height / 2);
        }

        // Timer tick: Reset the paddle speed after 2 seconds
        private void SpeedBoostTimer_Tick(object sender, EventArgs e)
        {
            playerPaddle.BackColor = Color.Blue;
            paddleSpeed = originalPaddleSpeed;
            speedBoostTimer.Stop();
        }

        // Cooldown Timer Tick
        private void CooldownTimer_Tick(object sender, EventArgs e)
        {
            cooldownTimeLeft--;

            if (cooldownTimeLeft > 0)
            {
                cooldownLabel.Text = $"F: {cooldownTimeLeft}s";
                cooldownLabel.ForeColor = Color.Red;
            }
            else
            {
                // Reset cooldown
                isCooldownActive = false;
                cooldownLabel.Text = "F";
                cooldownLabel.ForeColor = Color.Green;
                cooldownTimer.Stop();
            }
        }

        public bool CheckWin()
        {
            return win;
        }

        public void GameFinished()
        {
            MessageBox.Show("Sudah menamatkan MiniGame ini, silahkan lanjut ke Quiz selanjutnya.",
                            "MiniGame Selesai", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        public void GameDone()
        {
            MessageBox.Show("Sudah menamatkan MiniGame ini, silahkan pergi ke area terakhir",
                            "MiniGame Selesai", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
