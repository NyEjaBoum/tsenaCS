namespace tsenaFinal
{
    partial class Form1
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

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            panel2 = new Panel();
            button1 = new Button();
            datePaiementPicker = new DateTimePicker();
            label8 = new Label();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            montantText = new TextBox();
            idBoxText = new TextBox();
            idPersonText = new TextBox();
            label1 = new Label();
            panel3 = new Panel();
            label4 = new Label();
            label3 = new Label();
            verificationBtn = new Button();
            anneeVerifierText = new TextBox();
            moisVerifierText = new TextBox();
            label2 = new Label();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ControlLight;
            panel1.BorderStyle = BorderStyle.Fixed3D;
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(943, 758);
            panel1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.ControlLight;
            panel2.BorderStyle = BorderStyle.Fixed3D;
            panel2.Controls.Add(button1);
            panel2.Controls.Add(datePaiementPicker);
            panel2.Controls.Add(label8);
            panel2.Controls.Add(label7);
            panel2.Controls.Add(label6);
            panel2.Controls.Add(label5);
            panel2.Controls.Add(montantText);
            panel2.Controls.Add(idBoxText);
            panel2.Controls.Add(idPersonText);
            panel2.Controls.Add(label1);
            panel2.Location = new Point(1003, 12);
            panel2.Name = "panel2";
            panel2.Size = new Size(337, 445);
            panel2.TabIndex = 1;
            // 
            // button1
            // 
            button1.Font = new Font("Gloucester MT Extra Condensed", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button1.Location = new Point(124, 332);
            button1.Name = "button1";
            button1.Size = new Size(96, 37);
            button1.TabIndex = 11;
            button1.Text = "PAYER";
            button1.UseVisualStyleBackColor = true;
            // 
            // datePaiementPicker
            // 
            datePaiementPicker.Location = new Point(124, 258);
            datePaiementPicker.Name = "datePaiementPicker";
            datePaiementPicker.Size = new Size(179, 23);
            datePaiementPicker.TabIndex = 10;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Gloucester MT Extra Condensed", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label8.Location = new Point(26, 250);
            label8.Name = "label8";
            label8.Size = new Size(46, 32);
            label8.TabIndex = 9;
            label8.Text = "date";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Gloucester MT Extra Condensed", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.Location = new Point(26, 200);
            label7.Name = "label7";
            label7.Size = new Size(76, 32);
            label7.TabIndex = 8;
            label7.Text = "montant";
            label7.Click += label7_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Gloucester MT Extra Condensed", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(26, 143);
            label6.Name = "label6";
            label6.Size = new Size(56, 32);
            label6.TabIndex = 7;
            label6.Text = "idBox";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Gloucester MT Extra Condensed", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(26, 76);
            label5.Name = "label5";
            label5.Size = new Size(79, 32);
            label5.TabIndex = 6;
            label5.Text = "idPerson";
            // 
            // montantText
            // 
            montantText.Location = new Point(124, 211);
            montantText.Name = "montantText";
            montantText.Size = new Size(100, 23);
            montantText.TabIndex = 3;
            // 
            // idBoxText
            // 
            idBoxText.Location = new Point(124, 154);
            idBoxText.Name = "idBoxText";
            idBoxText.Size = new Size(100, 23);
            idBoxText.TabIndex = 2;
            // 
            // idPersonText
            // 
            idPersonText.Location = new Point(124, 85);
            idPersonText.Name = "idPersonText";
            idPersonText.Size = new Size(100, 23);
            idPersonText.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Bernard MT Condensed", 27.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(90, 14);
            label1.Name = "label1";
            label1.Size = new Size(155, 44);
            label1.TabIndex = 0;
            label1.Text = "Paiement";
            // 
            // panel3
            // 
            panel3.BackColor = SystemColors.ControlLight;
            panel3.BorderStyle = BorderStyle.Fixed3D;
            panel3.Controls.Add(label4);
            panel3.Controls.Add(label3);
            panel3.Controls.Add(verificationBtn);
            panel3.Controls.Add(anneeVerifierText);
            panel3.Controls.Add(moisVerifierText);
            panel3.Controls.Add(label2);
            panel3.Location = new Point(1003, 487);
            panel3.Name = "panel3";
            panel3.Size = new Size(337, 283);
            panel3.TabIndex = 2;
            panel3.Paint += panel3_Paint;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Gloucester MT Extra Condensed", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(36, 123);
            label4.Name = "label4";
            label4.Size = new Size(57, 32);
            label4.TabIndex = 6;
            label4.Text = "annee";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Gloucester MT Extra Condensed", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(36, 78);
            label3.Name = "label3";
            label3.Size = new Size(49, 32);
            label3.TabIndex = 5;
            label3.Text = "mois";
            // 
            // verificationBtn
            // 
            verificationBtn.Font = new Font("Gloucester MT Extra Condensed", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            verificationBtn.Location = new Point(128, 195);
            verificationBtn.Name = "verificationBtn";
            verificationBtn.Size = new Size(104, 37);
            verificationBtn.TabIndex = 4;
            verificationBtn.Text = "VERIFIER";
            verificationBtn.UseVisualStyleBackColor = true;
            verificationBtn.Click += verificationMois;
            // 
            // anneeVerifierText
            // 
            anneeVerifierText.Location = new Point(107, 132);
            anneeVerifierText.Name = "anneeVerifierText";
            anneeVerifierText.Size = new Size(138, 23);
            anneeVerifierText.TabIndex = 3;
            // 
            // moisVerifierText
            // 
            moisVerifierText.Location = new Point(107, 87);
            moisVerifierText.Name = "moisVerifierText";
            moisVerifierText.Size = new Size(138, 23);
            moisVerifierText.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Bernard MT Condensed", 27.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(90, 11);
            label2.Name = "label2";
            label2.Size = new Size(181, 44);
            label2.TabIndex = 1;
            label2.Text = "Verification";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1365, 840);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "Form1";
            Text = "Form1";
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Label label1;
        private Panel panel3;
        private Label label2;
        private Button verificationBtn;
        private TextBox anneeVerifierText;
        private TextBox moisVerifierText;
        private Label label4;
        private Label label3;
        private Label label8;
        private Label label7;
        private Label label6;
        private Label label5;
        private TextBox montantText;
        private TextBox idBoxText;
        private TextBox idPersonText;
        private Button button1;
        private DateTimePicker datePaiementPicker;
    }
}
