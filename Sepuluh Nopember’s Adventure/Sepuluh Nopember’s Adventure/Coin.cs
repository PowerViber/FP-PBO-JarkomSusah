using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sepuluh_Nopember_s_Adventure
{
    public class Coin : Powerup
    {
        public Coin(Point location) : base(location)
        {
            using (MemoryStream ms = new MemoryStream(Resource.star))
            {
                _powerupPictureBox.Image = new Bitmap(ms);
            }
        }

        public override void Collect()
        {
            Console.WriteLine("Coin collected!");
            RemoveFromGame(_powerupPictureBox.Parent);
        }

        public override bool CheckCollision(PictureBox playerBox)
        {
            return _powerupPictureBox.Bounds.IntersectsWith(playerBox.Bounds);
        }
    }
}
