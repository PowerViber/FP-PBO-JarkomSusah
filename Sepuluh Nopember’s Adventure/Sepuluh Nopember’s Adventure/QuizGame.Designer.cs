namespace SepuluhNopemberAdventure
{
    partial class QuizGame
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblQuestion = new Label();
            btnOption1 = new Button();
            btnOption2 = new Button();
            btnOption3 = new Button();
            btnOption4 = new Button();
            SuspendLayout();
            // 
            // lblQuestion
            // 
            lblQuestion.Font = new Font("Microsoft Sans Serif", 20F, FontStyle.Regular, GraphicsUnit.Point);
            lblQuestion.Location = new Point(50, 50);
            lblQuestion.Name = "lblQuestion";
            lblQuestion.Size = new Size(600, 100);
            lblQuestion.TabIndex = 0;
            lblQuestion.Text = "Pertanyaan akan muncul disini";
            lblQuestion.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnOption1
            // 
            btnOption1.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point);
            btnOption1.Location = new Point(50, 300);
            btnOption1.Name = "btnOption1";
            btnOption1.Size = new Size(400, 60);
            btnOption1.TabIndex = 1;
            btnOption1.Text = "Opsi 1";
            btnOption1.UseVisualStyleBackColor = true;
            btnOption1.Click += btnOption_Click;
            // 
            // btnOption2
            // 
            btnOption2.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point);
            btnOption2.Location = new Point(50, 380);
            btnOption2.Name = "btnOption2";
            btnOption2.Size = new Size(400, 60);
            btnOption2.TabIndex = 2;
            btnOption2.Text = "Opsi 2";
            btnOption2.UseVisualStyleBackColor = true;
            btnOption2.Click += btnOption_Click;
            // 
            // btnOption3
            // 
            btnOption3.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point);
            btnOption3.Location = new Point(50, 460);
            btnOption3.Name = "btnOption3";
            btnOption3.Size = new Size(400, 60);
            btnOption3.TabIndex = 3;
            btnOption3.Text = "Opsi 3";
            btnOption3.UseVisualStyleBackColor = true;
            btnOption3.Click += btnOption_Click;
            // 
            // btnOption4
            // 
            btnOption4.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point);
            btnOption4.Location = new Point(50, 540);
            btnOption4.Name = "btnOption4";
            btnOption4.Size = new Size(400, 60);
            btnOption4.TabIndex = 4;
            btnOption4.Text = "Opsi 4";
            btnOption4.UseVisualStyleBackColor = true;
            btnOption4.Click += btnOption_Click;
            // 
            // QuizGame
            // 
            ClientSize = new Size(800, 600);
            Controls.Add(btnOption4);
            Controls.Add(btnOption3);
            Controls.Add(btnOption2);
            Controls.Add(btnOption1);
            Controls.Add(lblQuestion);
            Name = "QuizGame";
            Text = "Quiz Game";
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblQuestion;
        private System.Windows.Forms.Button btnOption1;
        private System.Windows.Forms.Button btnOption2;
        private System.Windows.Forms.Button btnOption3;
        private System.Windows.Forms.Button btnOption4;
    }
}
