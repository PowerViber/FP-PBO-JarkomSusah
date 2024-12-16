using SepuluhNopemberAdventure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Sepuluh_Nopember_s_Adventure
{
    public class Start : Form
    {
        private int win1 = 0;
        private int win2 = 0;

        //player
        private const int PlayerX = 365;
        private const int PlayerY = 800;
        private Player _player;
        private System.Windows.Forms.Timer _animationTimer;

        // Coin
        private List<Coin> _coins;

        private List<PictureBox> _npcPictureBoxes;
        private List<NPC> _npcs;

        //npc1
        private PictureBox npc1_pbox;
        private NPC npc1;
        private const int Npc1X = 120;
        private const int Npc1Y = 800;

        //npc2
        private PictureBox npc2_pbox;
        private NPC npc2;
        private const int Npc2X = 700;
        private const int Npc2Y = 500;

        //npc3
        private PictureBox npc3_pbox;
        private NPC npc3;
        private const int Npc3X = 362;
        private const int Npc3Y = 50;

        //boundaries
        private List<PictureBox> _boundaries;

        //top area -> invis player
        private List<PictureBox> _topPictureBoxes;


        public Start()
        {
            Init();
        }

        private void Init()
        {
            //map
            this.Size = new Size(800, 1000);
            using (MemoryStream ms = new MemoryStream(Resource.stage_normal))
            {
                this.BackgroundImage = Image.FromStream(ms);
            }
            this.BackgroundImageLayout = ImageLayout.Stretch;

            //-------- BOUNDARIES LIST ----------//
            _boundaries = new List<PictureBox>();
            
            // water
            AddBoundary(new Rectangle(0, 315, 320, 55)); // akiri
            AddBoundary(new Rectangle(430, 315, 800, 55)); // akanan
            AddBoundary(new Rectangle(0, 620, 320, 80));  // bkiri
            AddBoundary(new Rectangle(430, 620, 800, 80)); // bkanan

            //jembatan
            AddBoundary(new Rectangle(305, 275, 15, 15)); // atas kiri
            AddBoundary(new Rectangle(430, 275, 15, 15)); // atas kanan

            //fountain
            AddBoundary(new Rectangle(321, 470, 100, 50));

            //chair
            AddBoundary(new Rectangle(125, 420, 100, 20)); //kiri
            AddBoundary(new Rectangle(556, 420, 100, 20)); //kanan

            //tower
            AddBoundary(new Rectangle(0, 841, 112, 40)); // kirib
            AddBoundary(new Rectangle(675, 841, 115, 40)); // kananb
            AddBoundary(new Rectangle(0, 183, 112, 40)); // kiria
            AddBoundary(new Rectangle(675, 183, 115, 40)); // kanana

            //------------------------------------//

            //------------------- TOP LIST --------------------//
            _topPictureBoxes = new List<PictureBox>();

            //tower
            AddTowerTop(new Rectangle(0, 700, 117, 130)); // kirib
            AddTowerTop(new Rectangle(664, 700, 117, 130)); // kananb
            AddTowerTop(new Rectangle(0, 43, 117, 130)); // kiria
            AddTowerTop(new Rectangle(664, 43, 117, 130)); // kanana



            foreach (var towerTop in _topPictureBoxes)
            {
                this.Controls.SetChildIndex(towerTop, 1); 
            }
            //-------------------------------------------------//

            //npc1
            npc1_pbox = new PictureBox
            {
                Location = new Point(Npc1X, Npc1Y),
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.None
            };
            int finalScore1 = 0;
            npc1 = new NPC(2, 7, npc1_pbox)
            {
                InteractionAction = () =>
                {
                    QuizGame quizGameForm = new QuizGame();
                    if (finalScore1 == 3)
                    {
                        if (win2 == 0)
                            quizGameForm.GameFinished();
                        else
                            quizGameForm.GameDone();
                        CheckWin();
                    }
                    else
                    {
                        quizGameForm.ShowDialog();
                        finalScore1 = quizGameForm.GetScore();
                        if (finalScore1 == 3)
                            win1 = 1;
                        CheckWin();
                    }
                }
            };
            this.Controls.Add(npc1_pbox);

            //npc2
            bool finalWin1 = false;
            npc2_pbox = new PictureBox
            {
                Location = new Point(Npc2X, Npc2Y),
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.None
            };
            npc2 = new NPC(5, 1, npc2_pbox)
            {
                InteractionAction = () =>
                {
                    PingPongGame pingPongGameForm = new PingPongGame();
                    if (finalWin1)
                    {
                        if(win1 == 0)
                            pingPongGameForm.GameFinished();
                        else
                            pingPongGameForm.GameDone();
                        CheckWin();
                    }
                    else
                    {
                        pingPongGameForm.ShowDialog();
                        finalWin1 = pingPongGameForm.CheckWin();
                        if(finalWin1)
                            win2 = 1;
                        CheckWin();
                    }
                }
            };
            this.Controls.Add(npc2_pbox);

            //npc3
            npc3_pbox = new PictureBox
            {
                Location = new Point(Npc3X, Npc3Y),
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.None
            };
            npc3 = new NPC(0, 10, npc3_pbox)
            {
                InteractionAction = () =>
                {
                    ShowCompletionMessage();
                }
            };
            this.Controls.Add(npc3_pbox);


            //-------- NPC LIST ----------//
            _npcs = new List<NPC>
            {
                npc1,
                npc2,
                npc3
            };

            _npcPictureBoxes = new List<PictureBox>
            {
                npc1_pbox,
                npc2_pbox,
                npc3_pbox
            };

            foreach (var npcPbox in _npcPictureBoxes)
            {
                this.Controls.Add(npcPbox);
            }
            //---------------------------//


            //player
            _player = new Player(new Point(PlayerX, PlayerY));
            this.Controls.Add(_player.GetPictureBox());

            //coins
            _coins = new List<Coin>(); 
            AddCoin(new Point(400, 500)); 
            AddCoin(new Point(200, 400)); 
            AddCoin(new Point(600, 700)); 

            _animationTimer = new System.Windows.Forms.Timer { Interval = 16 };
            _animationTimer.Tick += (sender, e) =>
            {
                // Boundaries final area
                Rectangle restrictedArea = new Rectangle(0, 360, 800, 10);
                bool canPassBoundary = (win1 == 1 && win2 == 1);

                _player.Walk(this.ClientSize, _npcPictureBoxes, restrictedArea, canPassBoundary, _boundaries);

                // collision coin
                for (int i = 0; i < _coins.Count; i++)
                {
                    if (_coins[i] != null && _coins[i].CheckCollision(_player.GetPictureBox()))
                    {
                        Console.WriteLine("Koin diambil!");
                        _player.IncreaseSpeed(); 
                        _coins[i].Collect(); 
                        this.Controls.Remove(_coins[i].GetPictureBox());
                        _coins[i] = null;
                    }
                }

                foreach (var coin in _coins)
                {
                    if (coin != null)
                    {
                        this.Controls.SetChildIndex(coin.GetPictureBox(), 0); // Bring coins to the top
                    }
                }
            };
            _animationTimer.Start();

            this.KeyDown += (sender, e) =>
            {
                _player.KeyDown(e.KeyCode);

                if (e.KeyCode == Keys.E)
                {
                    _player.Interact(_npcs);
                }
            };
            this.KeyUp += (sender, e) => _player.KeyUp(e.KeyCode);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            _player.KeyDown(e.KeyCode);

            if (e.KeyCode == Keys.E)
            {
                _player.Interact(_npcs); 
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            _player.KeyUp(e.KeyCode);
        }
        
        private void AddCoin(Point location)
        {
            Coin newCoin = new Coin(location);
            _coins.Add(newCoin);
            this.Controls.Add(newCoin.GetPictureBox());
        }

        private void AddBoundary(Rectangle bounds)
        {
            PictureBox boundary = new PictureBox
            {
                Location = new Point(bounds.X, bounds.Y),
                Size = new Size(bounds.Width, bounds.Height),
                BackColor = Color.Transparent, // Replace with transparent if using actual map graphics
            };
            this.Controls.Add(boundary);
            _boundaries.Add(boundary);
        }


        private void CheckWin()
        {
            if (win1 == 1 && win2 == 1)
            {
                // Change the map to Resource.map2
                using (MemoryStream ms = new MemoryStream(Resource.stage_final))
                {
                    this.BackgroundImage = Image.FromStream(ms);
                }
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }

        private void ShowCompletionMessage() 
        {
        MessageBox.Show("Selamat kamu telah menamatkan game! Terima kasih telah bermain!", "Game Selesai", MessageBoxButtons.OK, MessageBoxIcon.Information);
        Application.Exit();      
        }
        private void AddTowerTop(Rectangle topBounds)
        {
            PictureBox towerTop = new PictureBox
            {
                Location = new Point(topBounds.X, topBounds.Y),
                Size = new Size(topBounds.Width, topBounds.Height),
                BackColor = Color.Transparent,
            };

            towerTop.Image = new Bitmap(topBounds.Width, topBounds.Height);
            using (Graphics g = Graphics.FromImage(towerTop.Image))
            {
                g.Clear(Color.Transparent); //ganti wanra buat debug
            }

            this.Controls.Add(towerTop);
            _topPictureBoxes.Add(towerTop);
        }

    }
}
