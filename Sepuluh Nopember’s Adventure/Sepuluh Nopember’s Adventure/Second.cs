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

            //AddBoundary(new Rectangle(0, 620, 320, 80));

            //------------------------------------//

            //------------------- TOP LIST --------------------//
            _topPictureBoxes = new List<PictureBox>();

            //AddTop(new Rectangle(664, 700, 117, 130));

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
                    MessageBox.Show("halo");
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
                BackColor = Color.Transparent, // Replace with transparent if using actual map graphics
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
    }
}
