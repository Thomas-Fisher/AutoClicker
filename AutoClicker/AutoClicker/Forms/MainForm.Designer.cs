namespace AutoClicker
{
    partial class MainForm
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            label1 = new System.Windows.Forms.Label();
            ctrlRadioButton = new System.Windows.Forms.RadioButton();
            altRadioButton = new System.Windows.Forms.RadioButton();
            shiftRadioButton = new System.Windows.Forms.RadioButton();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            mousePositionLabel = new System.Windows.Forms.Label();
            startStopKeyComboBox = new System.Windows.Forms.ComboBox();
            intervalNumericUpDown = new System.Windows.Forms.NumericUpDown();
            randomDelayNumericUpDown = new System.Windows.Forms.NumericUpDown();
            currentCursorPositionRadioButton = new System.Windows.Forms.RadioButton();
            specificPositionRadioButton = new System.Windows.Forms.RadioButton();
            xNumericUpDown = new System.Windows.Forms.NumericUpDown();
            yNumericUpDown = new System.Windows.Forms.NumericUpDown();
            jitterRadiusNumericUpDown = new System.Windows.Forms.NumericUpDown();
            notifyIcon1 = new System.Windows.Forms.NotifyIcon(components);
            groupBox1 = new System.Windows.Forms.GroupBox();
            displayOverlayCheckBox = new System.Windows.Forms.CheckBox();
            groupBox2 = new System.Windows.Forms.GroupBox();
            mouseButtonComboBox = new System.Windows.Forms.ComboBox();
            label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)intervalNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)randomDelayNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)xNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)yNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)jitterRadiusNumericUpDown).BeginInit();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(45, 15);
            label1.TabIndex = 0;
            label1.Text = "Hotkey:";
            //
            // ctrlRadioButton
            //
            ctrlRadioButton.AutoSize = true;
            ctrlRadioButton.Location = new System.Drawing.Point(64, 8);
            ctrlRadioButton.Name = "ctrlRadioButton";
            ctrlRadioButton.Size = new System.Drawing.Size(47, 19);
            ctrlRadioButton.TabIndex = 22;
            ctrlRadioButton.Text = "Ctrl";
            ctrlRadioButton.UseVisualStyleBackColor = true;
            //
            // altRadioButton
            //
            altRadioButton.AutoSize = true;
            altRadioButton.Location = new System.Drawing.Point(114, 8);
            altRadioButton.Name = "altRadioButton";
            altRadioButton.Size = new System.Drawing.Size(43, 19);
            altRadioButton.TabIndex = 23;
            altRadioButton.Text = "Alt";
            altRadioButton.UseVisualStyleBackColor = true;
            //
            // shiftRadioButton
            //
            shiftRadioButton.AutoSize = true;
            shiftRadioButton.Location = new System.Drawing.Point(160, 8);
            shiftRadioButton.Name = "shiftRadioButton";
            shiftRadioButton.Size = new System.Drawing.Size(54, 19);
            shiftRadioButton.TabIndex = 24;
            shiftRadioButton.Text = "Shift";
            shiftRadioButton.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 72);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(152, 15);
            label2.TabIndex = 1;
            label2.Text = "Click Interval (milliseconds)";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(5, 24);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(161, 15);
            label3.TabIndex = 2;
            label3.Text = "Random Delay (milliseconds)";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(11, 163);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(128, 15);
            label4.TabIndex = 3;
            label4.Text = "Mouse Click Position X";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(11, 192);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(128, 15);
            label5.TabIndex = 4;
            label5.Text = "Mouse Click Position Y";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(4, 53);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(172, 15);
            label6.TabIndex = 5;
            label6.Text = "Click Jitter (Radius Around X/Y)";
            // 
            // mousePositionLabel
            // 
            mousePositionLabel.AutoSize = true;
            mousePositionLabel.Location = new System.Drawing.Point(7, 321);
            mousePositionLabel.Name = "mousePositionLabel";
            mousePositionLabel.Size = new System.Drawing.Size(114, 15);
            mousePositionLabel.TabIndex = 6;
            mousePositionLabel.Text = "mousePositionLabel";
            mousePositionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // startStopKeyComboBox
            // 
            startStopKeyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            startStopKeyComboBox.FormattingEnabled = true;
            startStopKeyComboBox.Location = new System.Drawing.Point(217, 6);
            startStopKeyComboBox.Name = "startStopKeyComboBox";
            startStopKeyComboBox.Size = new System.Drawing.Size(121, 23);
            startStopKeyComboBox.TabIndex = 7;
            // 
            // intervalNumericUpDown
            // 
            intervalNumericUpDown.Location = new System.Drawing.Point(218, 64);
            intervalNumericUpDown.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            intervalNumericUpDown.Name = "intervalNumericUpDown";
            intervalNumericUpDown.Size = new System.Drawing.Size(120, 23);
            intervalNumericUpDown.TabIndex = 8;
            // 
            // randomDelayNumericUpDown
            // 
            randomDelayNumericUpDown.Location = new System.Drawing.Point(210, 22);
            randomDelayNumericUpDown.Maximum = new decimal(new int[] { 32767, 0, 0, 0 });
            randomDelayNumericUpDown.Name = "randomDelayNumericUpDown";
            randomDelayNumericUpDown.Size = new System.Drawing.Size(120, 23);
            randomDelayNumericUpDown.TabIndex = 9;
            // 
            // currentCursorPositionRadioButton
            // 
            currentCursorPositionRadioButton.AutoSize = true;
            currentCursorPositionRadioButton.Location = new System.Drawing.Point(6, 14);
            currentCursorPositionRadioButton.Name = "currentCursorPositionRadioButton";
            currentCursorPositionRadioButton.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            currentCursorPositionRadioButton.Size = new System.Drawing.Size(149, 19);
            currentCursorPositionRadioButton.TabIndex = 10;
            currentCursorPositionRadioButton.TabStop = true;
            currentCursorPositionRadioButton.Text = "Click at Mouse position";
            currentCursorPositionRadioButton.UseVisualStyleBackColor = true;
            // 
            // specificPositionRadioButton
            // 
            specificPositionRadioButton.AutoSize = true;
            specificPositionRadioButton.Location = new System.Drawing.Point(166, 14);
            specificPositionRadioButton.Name = "specificPositionRadioButton";
            specificPositionRadioButton.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            specificPositionRadioButton.Size = new System.Drawing.Size(160, 19);
            specificPositionRadioButton.TabIndex = 11;
            specificPositionRadioButton.TabStop = true;
            specificPositionRadioButton.Text = "Click at Set X / Y Location";
            specificPositionRadioButton.UseVisualStyleBackColor = true;
            // 
            // xNumericUpDown
            // 
            xNumericUpDown.Location = new System.Drawing.Point(213, 161);
            xNumericUpDown.Maximum = new decimal(new int[] { 32767, 0, 0, 0 });
            xNumericUpDown.Minimum = new decimal(new int[] { 32767, 0, 0, -2147483648 });
            xNumericUpDown.Name = "xNumericUpDown";
            xNumericUpDown.Size = new System.Drawing.Size(120, 23);
            xNumericUpDown.TabIndex = 19;
            // 
            // yNumericUpDown
            // 
            yNumericUpDown.Location = new System.Drawing.Point(213, 190);
            yNumericUpDown.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            yNumericUpDown.Minimum = new decimal(new int[] { 32767, 0, 0, -2147483648 });
            yNumericUpDown.Name = "yNumericUpDown";
            yNumericUpDown.Size = new System.Drawing.Size(120, 23);
            yNumericUpDown.TabIndex = 18;
            // 
            // jitterRadiusNumericUpDown
            // 
            jitterRadiusNumericUpDown.Location = new System.Drawing.Point(210, 51);
            jitterRadiusNumericUpDown.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            jitterRadiusNumericUpDown.Name = "jitterRadiusNumericUpDown";
            jitterRadiusNumericUpDown.Size = new System.Drawing.Size(120, 23);
            jitterRadiusNumericUpDown.TabIndex = 14;
            // 
            // notifyIcon1
            // 
            notifyIcon1.Text = "notifyIcon1";
            notifyIcon1.Visible = true;
            notifyIcon1.MouseDoubleClick += NotifyIcon1_MouseDoubleClick;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(currentCursorPositionRadioButton);
            groupBox1.Controls.Add(specificPositionRadioButton);
            groupBox1.Location = new System.Drawing.Point(12, 115);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(350, 40);
            groupBox1.TabIndex = 15;
            groupBox1.TabStop = false;
            // 
            // displayOverlayCheckBox
            // 
            displayOverlayCheckBox.AutoSize = true;
            displayOverlayCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            displayOverlayCheckBox.Location = new System.Drawing.Point(217, 90);
            displayOverlayCheckBox.Name = "displayOverlayCheckBox";
            displayOverlayCheckBox.Size = new System.Drawing.Size(107, 19);
            displayOverlayCheckBox.TabIndex = 16;
            displayOverlayCheckBox.Text = "Display Overlay";
            displayOverlayCheckBox.UseVisualStyleBackColor = true;
            displayOverlayCheckBox.CheckedChanged += DisplayOverlayCheckBox_CheckedChanged;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(jitterRadiusNumericUpDown);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(randomDelayNumericUpDown);
            groupBox2.Controls.Add(label3);
            groupBox2.Location = new System.Drawing.Point(3, 220);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(339, 84);
            groupBox2.TabIndex = 17;
            groupBox2.TabStop = false;
            groupBox2.Text = "Basic Anti Cheat";
            // 
            // mouseButtonComboBox
            // 
            mouseButtonComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            mouseButtonComboBox.FormattingEnabled = true;
            mouseButtonComboBox.Location = new System.Drawing.Point(217, 35);
            mouseButtonComboBox.Name = "mouseButtonComboBox";
            mouseButtonComboBox.Size = new System.Drawing.Size(121, 23);
            mouseButtonComboBox.TabIndex = 21;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(12, 38);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(82, 15);
            label7.TabIndex = 20;
            label7.Text = "Mouse Button";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(365, 370);
            Controls.Add(label7);
            Controls.Add(mouseButtonComboBox);
            Controls.Add(groupBox2);
            Controls.Add(displayOverlayCheckBox);
            Controls.Add(groupBox1);
            Controls.Add(yNumericUpDown);
            Controls.Add(xNumericUpDown);
            Controls.Add(intervalNumericUpDown);
            Controls.Add(startStopKeyComboBox);
            Controls.Add(mousePositionLabel);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(ctrlRadioButton);
            Controls.Add(altRadioButton);
            Controls.Add(shiftRadioButton);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Text = "AutoClicker";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            Resize += MainForm_Resize;
            ((System.ComponentModel.ISupportInitialize)intervalNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)randomDelayNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)xNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)yNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)jitterRadiusNumericUpDown).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
        

        #endregion

        private Label label1;
        private System.Windows.Forms.RadioButton ctrlRadioButton;
        private System.Windows.Forms.RadioButton altRadioButton;
        private System.Windows.Forms.RadioButton shiftRadioButton;
        private System.Windows.Forms.Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private System.Windows.Forms.Label mousePositionLabel;
        private System.Windows.Forms.ComboBox startStopKeyComboBox;
        private System.Windows.Forms.NumericUpDown intervalNumericUpDown;
        private System.Windows.Forms.NumericUpDown randomDelayNumericUpDown;
        private System.Windows.Forms.RadioButton currentCursorPositionRadioButton;
        private RadioButton specificPositionRadioButton;
        private System.Windows.Forms.NumericUpDown xNumericUpDown;
        private System.Windows.Forms.NumericUpDown yNumericUpDown;
        private System.Windows.Forms.NumericUpDown jitterRadiusNumericUpDown;
        private NotifyIcon notifyIcon1;
        private System.Windows.Forms.GroupBox groupBox1;
        private CheckBox displayOverlayCheckBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox mouseButtonComboBox;
        private System.Windows.Forms.Label label7;
    }
}