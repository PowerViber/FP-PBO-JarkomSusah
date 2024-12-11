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
        private int win = 0;
        private const int winNeed = 2;

        // Barrier Coordinates for rivers (example)
        private PictureBox _waterPictureBox1;
        private PictureBox _waterPictureBox2;
        private Rectangle bridge1 = new Rectangle(200, 320, 400, 50); // Jembatan pertama di sungai pertama

        //public PictureBox GetPictureBox() => _waterPictureBox1;

        //player
        private const int PlayerX = 365;
        private const int PlayerY = 800;
        private Player _player;
        private System.Windows.Forms.Timer _animationTimer;

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
                        quizGameForm.GameDone();
                        win++;
                        CheckWin();
                    }
                    else
                    {
                        quizGameForm.ShowDialog();
                        finalScore1 = quizGameForm.GetScore();
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
                        pingPongGameForm.GameDone();
                        win++;
                        CheckWin();
                    }
                    else
                    {
                        pingPongGameForm.ShowDialog();
                        finalWin1 = pingPongGameForm.CheckWin();
                    }
                }
            };
            this.Controls.Add(npc2_pbox);


            //-------- NPC LIST ----------//
            _npcs = new List<NPC>
            {
                npc1,
                npc2
            };

            _npcPictureBoxes = new List<PictureBox>
            {
                npc1_pbox,
                npc2_pbox
            };

            foreach (var npcPbox in _npcPictureBoxes)
            {
                this.Controls.Add(npcPbox);
            }
            //---------------------------//


            //player
            _player = new Player(new Point(PlayerX, PlayerY));
            this.Controls.Add(_player.GetPictureBox());

            _animationTimer = new System.Windows.Forms.Timer { Interval = 16 };
            _animationTimer.Tick += (sender, e) => _player.Walk(this.ClientSize, _npcPictureBoxes);
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

        private void CheckWin()
        {
            if (win >= 2)
            {
                // Change the map to Resource.map2
                using (MemoryStream ms = new MemoryStream(Resource.stage_final))
                {
                    this.BackgroundImage = Image.FromStream(ms);
                }
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }

    }
}
