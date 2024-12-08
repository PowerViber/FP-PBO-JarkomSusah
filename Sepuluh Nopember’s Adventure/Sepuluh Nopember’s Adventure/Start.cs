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

        //npc1
        private PictureBox npc1_pbox;
        private NPC npc1;
        private const int Npc1X = 120;
        private const int Npc1Y = 800;

        //npc2
        //private PictureBox npc2_pbox;
        //private NPC npc2;
        //private const int Npc2X = 120;
        //private const int Npc2Y = 800;

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
            npc1 = new NPC(2, 7, npc1_pbox);
            this.Controls.Add(npc1_pbox);

            //player
            _player = new Player(new Point(PlayerX, PlayerY));
            this.Controls.Add(_player.GetPictureBox());

            _animationTimer = new System.Windows.Forms.Timer { Interval = 16 };
            _animationTimer.Tick += (sender, e) => _player.Walk(this.ClientSize, npc1_pbox);
            _animationTimer.Start();

            this.KeyDown += (sender, e) => _player.KeyDown(e.KeyCode);
            this.KeyUp += (sender, e) => _player.KeyUp(e.KeyCode);

            //npc2
            //npc2_pbox = new PictureBox
            //{
            //    Location = new Point(Npc2X, Npc2Y),
            //    BackColor = Color.Transparent,
            //    BorderStyle = BorderStyle.None
            //};
            //npc2 = new NPC(2, 7, npc2_pbox);
            //this.Controls.Add(npc2_pbox);

            //npc3
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            _player.KeyDown(e.KeyCode);
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            _player.KeyUp(e.KeyCode);
        }
    }
}
