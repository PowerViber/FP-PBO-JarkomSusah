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
            // Logika untuk pengambilan koin, misalnya meningkatkan skor atau kecepatan pemain
            Console.WriteLine("Coin collected!");
            // Menghapus powerup dari game setelah dikumpulkan
            RemoveFromGame(_powerupPictureBox.Parent);
        }

        public override bool CheckCollision(PictureBox playerBox)
        {
            return _powerupPictureBox.Bounds.IntersectsWith(playerBox.Bounds);
        }
    }
}
