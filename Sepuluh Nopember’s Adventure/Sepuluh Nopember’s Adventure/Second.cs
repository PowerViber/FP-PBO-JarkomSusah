using SepuluhNopemberAdventure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepuluh_Nopember_s_Adventure
{
    public class Second : Form
    {
        //wins
        //private int win1 = 0;
        //private int win2 = 0;

        //player
        private const int PlayerX = 365;
        private const int PlayerY = 900;
        private Player _player;
        private System.Windows.Forms.Timer _animationTimer;

        //npc list
        private List<PictureBox> _npcPictureBoxes;
        private List<Stage_2NPC> _npcs;

        //npc1
        private PictureBox npc1_pbox;
        private Stage_2NPC npc1;
        private const int Npc1X = 120;
        private const int Npc1Y = 800;

        //boundaries
        private List<PictureBox> _boundaries;

        //top area -> invis player
        private List<PictureBox> _topPictureBoxes;

        // Doors
        private PictureBox door1;
        private PictureBox door2;
        private PictureBox door3;
        private List<PictureBox> _doors;

        private PictureBox winningDoor; // Store the current winning door
        private Random random = new Random(); // Random number generator
        private bool doorInteractionTriggered = false; // Flag to prevent multiple triggers


        public Second()
        {
            Init();
        }

        private void Init()
        {
            //map
            this.Size = new Size(800, 1000);
            using (MemoryStream ms = new MemoryStream(Resource.stage_2))
            {
                this.BackgroundImage = Image.FromStream(ms);
            }
            this.BackgroundImageLayout = ImageLayout.Stretch;

            //-------- BOUNDARIES LIST ----------//
            _boundaries = new List<PictureBox>();

            //wall jembatan
            AddBoundary(new Rectangle(65, 500, 10, 350)); //bawah kiri
            AddBoundary(new Rectangle(490, 0, 10, 490)); //atas kiri
            AddBoundary(new Rectangle(660, 0, 10, 640)); //atas kanan
            AddBoundary(new Rectangle(65, 500, 435, 10)); //tengah atas
            AddBoundary(new Rectangle(250, 650, 435, 10)); //tengah bawah
            AddBoundary(new Rectangle(240, 650, 10, 180)); //bawah kanan

            //wall log
            AddBoundary(new Rectangle(240, 830, 550, 10)); //bawah kanan

            //pojok kiri bawah
            AddBoundary(new Rectangle(0, 775, 60, 10));

            //pembedaan pintu
            AddBoundary(new Rectangle(547, 0, 2, 90)); //kiri
            AddBoundary(new Rectangle(607, 0, 2, 90)); //kanan


            //------------------------------------//

            //------------------- TOP LIST --------------------//
            _topPictureBoxes = new List<PictureBox>();

            AddTop(new Rectangle(490, 0, 180, 70));

            foreach (var top in _topPictureBoxes)
            {
                this.Controls.SetChildIndex(top, 1);
            }
            //-------------------------------------------------//

            //npc1
            npc1_pbox = new PictureBox
            {
                Location = new Point(Npc1X, Npc1Y),
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.None
            };
            npc1 = new Stage_2NPC(2, 7, npc1_pbox)
            {
                InteractionAction = () =>
                {
                    npc1.ProvideQuest();
                }
            };
            this.Controls.Add(npc1_pbox);

            //-------- NPC LIST ----------//
            _npcs = new List<Stage_2NPC>
            {
                npc1
            };

            _npcPictureBoxes = new List<PictureBox>
            {
                npc1_pbox
            };

            foreach (var npcPbox in _npcPictureBoxes)
            {
                this.Controls.Add(npcPbox);
            }
            //---------------------------//

            // Doors Setup
            door1 = CreateDoor(new Point(508, 0)); // Left Door
            door2 = CreateDoor(new Point(568, 0)); // Middle Door
            door3 = CreateDoor(new Point(627, 0)); // Right Door

            _doors = new List<PictureBox> { door1, door2, door3 };

            // Add doors to form
            this.Controls.Add(door1);
            this.Controls.Add(door2);
            this.Controls.Add(door3);

            // Randomize winning door
            RandomizeWinningDoor();

            //player
            _player = new Player(new Point(PlayerX, PlayerY));
            this.Controls.Add(_player.GetPictureBox());

            _animationTimer = new System.Windows.Forms.Timer { Interval = 16 };
            _animationTimer.Tick += (sender, e) =>
            {
                // Boundaries final area
                Rectangle restrictedArea = new Rectangle(0, 0, 0, 0);
                bool canPassBoundary = true;

                _player.Walk(this.ClientSize, _npcPictureBoxes, restrictedArea, canPassBoundary, _boundaries);
                CheckDoorCollision();
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

        private void AddBoundary(Rectangle bounds)
        {
            PictureBox boundary = new PictureBox
            {
                Location = new Point(bounds.X, bounds.Y),
                Size = new Size(bounds.Width, bounds.Height),
                BackColor = Color.Transparent, 
            };
            this.Controls.Add(boundary);
            _boundaries.Add(boundary);
        }

        private void AddTop(Rectangle topBounds)
        {
            PictureBox top = new PictureBox
            {
                Location = new Point(topBounds.X, topBounds.Y),
                Size = new Size(topBounds.Width, topBounds.Height),
                BackColor = Color.Transparent,
            };

            top.Image = new Bitmap(topBounds.Width, topBounds.Height);
            using (Graphics g = Graphics.FromImage(top.Image))
            {
                g.Clear(Color.Transparent); //ganti wanra buat debug
            }

            this.Controls.Add(top);
            _topPictureBoxes.Add(top);
        }

        private PictureBox CreateDoor(Point location)
        {
            return new PictureBox
            {
                Location = location,
                Size = new Size(25, 40),
                BackColor = Color.Transparent, 
                BorderStyle = BorderStyle.Fixed3D
            };
        }

        private void RandomizeWinningDoor()
        {
            int randomIndex = random.Next(0, _doors.Count);
            winningDoor = _doors[randomIndex];
            //winningDoor.BackColor = Color.Green; 
        }

        private void CheckDoorCollision()
        {
            if (doorInteractionTriggered) return; 

            Rectangle playerBounds = _player.GetPictureBox().Bounds;

            foreach (var door in _doors)
            {
                if (playerBounds.IntersectsWith(door.Bounds))
                {
                    doorInteractionTriggered = true; 

                    if (door == winningDoor)
                    {
                        ShowWinMessage();
                    }
                    else
                    {
                        ShowLoseMessage();
                    }
                    break; 
                }
            }
        }


        private void ShowLoseMessage()
        {
            MessageBox.Show("Oops! Wrong door. Try again.", "You Lost!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            ResetGame();
        }

        private void ShowWinMessage()
        {
            MessageBox.Show("Congratulations! You've chosen the correct door!", "You Win!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void ResetGame()
        {
            _player.GetPictureBox().Location = new Point(568, 300);
            RandomizeWinningDoor();
            doorInteractionTriggered = false;
        }
    }
}
