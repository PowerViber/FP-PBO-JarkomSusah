using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Sepuluh_Nopember_s_Adventure
{
    public interface IInteractable
    {
        void Interact(); // Method to enforce interaction behavior
    }

    public abstract class BaseNPC : IInteractable
    {
        protected const int TotalFrames = 12;
        protected const int TotalRows = 8;
        protected const int NpcWidth = 32;
        protected const int NpcHeight = 48;

        protected PictureBox _npcPictureBox;
        protected Image _spriteSheet;
        protected int _currentFrame;
        protected int _currentRow;

        public abstract void Interact(); // Abstract method forces implementation

        public BaseNPC(int row, int column, PictureBox npcPictureBox, byte[] spriteResource)
        {
            _npcPictureBox = npcPictureBox;

            _npcPictureBox.Tag = new { Row = row, Column = column, Size = new Size(NpcWidth, NpcHeight) };
            _npcPictureBox.Size = new Size(NpcWidth, NpcHeight);

            using (MemoryStream ms = new MemoryStream(spriteResource))
            {
                _spriteSheet = Image.FromStream(ms);
            }

            _currentRow = row;
            _currentFrame = column;

            UpdateSprite();
        }

        public PictureBox GetPictureBox() => _npcPictureBox;

        protected void UpdateSprite()
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
    }

    public class Stage_1NPC : BaseNPC
    {
        public Action InteractionAction { get; set; }

        public Stage_1NPC(int row, int column, PictureBox npcPictureBox)
            : base(row, column, npcPictureBox, Resource.npc) // Pass sprite resource
        {
            InteractionAction = DefaultInteraction;
        }

        public override void Interact()
        {
            InteractionAction?.Invoke();
        }

        private void DefaultInteraction()
        {
            MessageBox.Show("This NPC has no specific interaction.");
        }
    }

    public class Stage_2NPC : BaseNPC
    {
        public Action InteractionAction { get; set; }

        public Stage_2NPC(int row, int column, PictureBox npcPictureBox)
            : base(row, column, npcPictureBox, Resource.npc) 
        {
            InteractionAction = DefaultInteraction;
        }

        public override void Interact()
        {
            InteractionAction?.Invoke();
        }

        private void DefaultInteraction()
        {
            MessageBox.Show("Welcome to Stage 2! Things are about to get challenging!",
                            "Stage 2 NPC Interaction", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void ProvideQuest()
        {
            MessageBox.Show("Choose the correct door to win",
                            "Stage 2 Quest", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
