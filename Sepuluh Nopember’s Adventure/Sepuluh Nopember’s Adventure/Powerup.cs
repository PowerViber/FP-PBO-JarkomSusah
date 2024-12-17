using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sepuluh_Nopember_s_Adventure
{
    public abstract class Powerup
    {
        protected PictureBox _powerupPictureBox;
        protected const int PowerupWidth = 60;
        protected const int PowerupHeight = 60;

        public Powerup(Point location)
        {
            _powerupPictureBox = new PictureBox
            {
                Size = new Size(PowerupWidth, PowerupHeight),
                Location = location,
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.StretchImage
            };
        }

        public PictureBox GetPictureBox() => _powerupPictureBox;

        public abstract void Collect();
        public abstract bool CheckCollision(PictureBox playerBox);

        // Memindahkan logika penghapusan powerup
        public void RemoveFromGame(Control parentControl)
        {
            parentControl.Controls.Remove(_powerupPictureBox);
            _powerupPictureBox.Visible = false; // Menghapus tampilan gambar powerup
        }
    }
}
