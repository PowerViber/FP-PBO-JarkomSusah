using System;
using System.Drawing;
using System.Windows.Forms;
using Sepuluh_Nopember_s_Adventure;
using SepuluhNopemberAdventure;


namespace Sepuluh_Nopember_s_Adventure
{
    public class Coin
    {
        private PictureBox _coinPictureBox;
        private const int CoinWidth = 60;
        private const int CoinHeight = 60;

        public Coin(Point location)
        {
            using (MemoryStream ms = new MemoryStream(Resource.star)) // Convert byte[] to stream
            {
                _coinPictureBox = new PictureBox
                {
                    Size = new Size(CoinWidth, CoinHeight),
                    Location = location,
                    BackColor = Color.Transparent,
                    Image = new Bitmap(ms), // Create the Bitmap from MemoryStream
                    SizeMode = PictureBoxSizeMode.StretchImage
                };
            }
        }

        public PictureBox GetPictureBox() => _coinPictureBox;
        public void Collect()
        {
            _coinPictureBox.Visible = false;
        }

        public bool CheckCollision(PictureBox playerBox)
        {
            return playerBox.Bounds.IntersectsWith(_coinPictureBox.Bounds);
        }
    }
}