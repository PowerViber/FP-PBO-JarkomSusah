﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Sepuluh_Nopember_s_Adventure;

public class Player
{
    private const int PlayerWidth = 32;
    private const int PlayerHeight = 48;
    private const int TotalFrames = 8;
    private int movementSpeed = 3;
    private int defaultSpeed = 3;
    private System.Windows.Forms.Timer speedTimer;

    private PictureBox _playerPictureBox;
    private Image _spriteSheet;
    private int _currentFrame;
    private int _currentRow;
    private bool _isMoving;

    public Dictionary<Keys, bool> KeyStates = new Dictionary<Keys, bool>();

    public Player(Point startPosition)
    {
        using (MemoryStream ms = new MemoryStream(Resource.user_male))
        {
            _spriteSheet = Image.FromStream(ms);
        }

        _currentFrame = 0;
        _currentRow = 0;

        _playerPictureBox = new PictureBox
        {
            Size = new Size(PlayerWidth, PlayerHeight),
            Location = startPosition,
            BackColor = Color.Transparent
        };

        KeyStates[Keys.W] = false;
        KeyStates[Keys.A] = false;
        KeyStates[Keys.S] = false;
        KeyStates[Keys.D] = false;

        UpdateSprite();
        speedTimer = new System.Windows.Forms.Timer();
        speedTimer.Interval = 3000; 
        speedTimer.Tick += (sender, e) => ResetSpeed();
    }

    public PictureBox GetPictureBox() => _playerPictureBox;

    public void Walk(Size boundary, List<PictureBox> npcBox)
    {
        _isMoving = false;
        Point tempPosition = _playerPictureBox.Location;

        if (KeyStates[Keys.S]) // Down
        {
            _currentRow = 0;
            tempPosition.Y += movementSpeed;
            if (tempPosition.Y + _playerPictureBox.Height <= boundary.Height
                && (npcBox == null || !CheckCollision(npcBox, new Rectangle(tempPosition, _playerPictureBox.Size))))
            {
                _playerPictureBox.Top += movementSpeed;
                _isMoving = true;
            }
        }
        if (KeyStates[Keys.W]) // Up
        {
            _currentRow = 1;
            tempPosition.Y -= movementSpeed;
            if (tempPosition.Y >= 0
                && (npcBox == null || !CheckCollision(npcBox, new Rectangle(tempPosition, _playerPictureBox.Size))))
            {
                _playerPictureBox.Top -= movementSpeed;
                _isMoving = true;
            }
        }
        if (KeyStates[Keys.A]) // Left
        {
            _currentRow = 2;
            tempPosition.X -= movementSpeed;
            if (tempPosition.X >= 0
                && (npcBox == null || !CheckCollision(npcBox, new Rectangle(tempPosition, _playerPictureBox.Size))))
            {
                _playerPictureBox.Left -= movementSpeed;
                _isMoving = true;
            }
        }
        if (KeyStates[Keys.D]) // Right
        {
            _currentRow = 3;
            tempPosition.X += movementSpeed;
            if (tempPosition.X + _playerPictureBox.Width <= boundary.Width
                && (npcBox == null || !CheckCollision(npcBox, new Rectangle(tempPosition, _playerPictureBox.Size))))
            {
                _playerPictureBox.Left += movementSpeed;
                _isMoving = true;
            }
        }

        if (_isMoving)
            Animate();
        else
            StopWalking();
    }

    public void IncreaseSpeed()
    {
        movementSpeed += 5;
        Console.WriteLine("Kecepatan sekarang: " + movementSpeed);
        speedTimer.Start(); 
    }

    private void ResetSpeed()
    {
        movementSpeed = defaultSpeed;
        Console.WriteLine("Kecepatan kembali ke: " + movementSpeed);
        speedTimer.Stop(); 
    }

    public void KeyDown(Keys key)
    {
        if (KeyStates.ContainsKey(key))
            KeyStates[key] = true; // Mark key as pressed
    }

    public void KeyUp(Keys key)
    {
        if (KeyStates.ContainsKey(key))
            KeyStates[key] = false; // Mark key as released
    }

    public void StopWalking()
    {
        _isMoving = false;
        _currentFrame = 0;
        UpdateSprite();
    }

    public void Animate()
    {
        _currentFrame = (_currentFrame + 1) % TotalFrames;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        int frameWidth = _spriteSheet.Width / TotalFrames;
        int frameHeight = _spriteSheet.Height / 4;

        Rectangle srcRect = new Rectangle(_currentFrame * frameWidth, _currentRow * frameHeight, frameWidth, frameHeight);
        Bitmap currentFrameImage = new Bitmap(frameWidth, frameHeight);

        using (Graphics g = Graphics.FromImage(currentFrameImage))
        {
            g.DrawImage(_spriteSheet, new Rectangle(0, 0, frameWidth, frameHeight), srcRect, GraphicsUnit.Pixel);
        }

        _playerPictureBox.Image = currentFrameImage;
    }

    public bool CheckCollision(List<PictureBox> npcBoxes, Rectangle nextPosition)
    {
        foreach (var npcBox in npcBoxes)
        {
            if (nextPosition.IntersectsWith(npcBox.Bounds))
                return true;
        }
        return false;
    }

    public void Interact(List<NPC> npcs)
    {
        int interactionMargin = 20; // Defines interaction range

        Rectangle expandedBounds = new Rectangle(
            _playerPictureBox.Left - interactionMargin,
            _playerPictureBox.Top - interactionMargin,
            _playerPictureBox.Width + (interactionMargin * 2),
            _playerPictureBox.Height + (interactionMargin * 2)
        );

        foreach (var npc in npcs)
        {
            if (expandedBounds.IntersectsWith(npc.GetPictureBox().Bounds))
            {
                npc.Interact();
                break; // Interact with the first NPC in range
            }
        }
    }
}
