using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Sepuluh_Nopember_s_Adventure
{
    public class NPC
    {
        private const int TotalFrames = 12;
        private const int TotalRows = 8;
        private const int NpcWidth = 32;
        private const int NpcHeight = 48;

        private PictureBox _npcPictureBox;
        private Image _spriteSheet;
        private int _currentFrame;
        private int _currentRow;

        public NPC(int row, int column, PictureBox npcPictureBox)
        {
            _npcPictureBox = npcPictureBox;

            _npcPictureBox.Tag = new { Row = row, Column = column, Size = new Size(NpcWidth, NpcHeight) };
            _npcPictureBox.Size = new Size(NpcWidth, NpcHeight);

            using (MemoryStream ms = new MemoryStream(Resource.npc))
            {
                _spriteSheet = Image.FromStream(ms);
            }

            _currentRow = row;
            _currentFrame = column;

            UpdateSprite();
        }

        public PictureBox GetPictureBox() => _npcPictureBox;

        public void UpdateSprite()
        {
            int frameWidth = _spriteSheet.Width / TotalFrames; 
            int frameHeight = _spriteSheet.Height / TotalRows;  

            Rectangle srcRect = new Rectangle(
                _currentFrame * frameWidth,   
                _currentRow * frameHeight,   
                frameWidth,                   
                frameHeight                   
            );

            Bitmap resizedFrameImage = new Bitmap(NpcWidth, NpcHeight);

            using (Graphics g = Graphics.FromImage(resizedFrameImage))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                g.DrawImage(
                    _spriteSheet,                           
                    new Rectangle(0, 0, NpcWidth, NpcHeight),
                    srcRect,                                
                    GraphicsUnit.Pixel
                );
            }

            _npcPictureBox.Image = resizedFrameImage;
        }


        public void Interact(Keys key)
        {
            switch (key)
            {
                case Keys.E:
                    ShowDialog();
                    break;
            }
        }

        public void ShowDialog()
        {

        }
    }
}
