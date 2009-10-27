﻿namespace GoogleWaveNotifier
{
    partial class PreferencesForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
            this.applyButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.emailLabel = new System.Windows.Forms.Label();
            this.emailBox = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.passwordBox = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pollingIntervalBox = new System.Windows.Forms.NumericUpDown();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.accountTab = new System.Windows.Forms.TabPage();
            this.behaviourTab = new System.Windows.Forms.TabPage();
            this.autoStartBox = new System.Windows.Forms.CheckBox();
            this.aboutTab = new System.Windows.Forms.TabPage();
            this.growlForWindowsLink = new System.Windows.Forms.LinkLabel();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.authorLabel = new System.Windows.Forms.Label();
            this.websiteLink = new System.Windows.Forms.LinkLabel();
            this.titleLabel = new System.Windows.Forms.Label();
            this.iconBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pollingIntervalBox)).BeginInit();
            this.tabControl.SuspendLayout();
            this.accountTab.SuspendLayout();
            this.behaviourTab.SuspendLayout();
            this.aboutTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).BeginInit();
            this.SuspendLayout();
            // 
            // applyButton
            // 
            this.applyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.applyButton.Location = new System.Drawing.Point(235, 143);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(75, 23);
            this.applyButton.TabIndex = 3;
            this.applyButton.Text = "Apply";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.ApplyButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(154, 143);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(73, 143);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // emailLabel
            // 
            this.emailLabel.AutoSize = true;
            this.emailLabel.Location = new System.Drawing.Point(6, 9);
            this.emailLabel.Name = "emailLabel";
            this.emailLabel.Size = new System.Drawing.Size(32, 13);
            this.emailLabel.TabIndex = 8;
            this.emailLabel.Text = "Email";
            // 
            // emailBox
            // 
            this.emailBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.emailBox.Location = new System.Drawing.Point(67, 6);
            this.emailBox.Name = "emailBox";
            this.emailBox.Size = new System.Drawing.Size(223, 20);
            this.emailBox.TabIndex = 0;
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(6, 35);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(53, 13);
            this.passwordLabel.TabIndex = 10;
            this.passwordLabel.Text = "Password";
            // 
            // passwordBox
            // 
            this.passwordBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.passwordBox.Location = new System.Drawing.Point(67, 32);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.Size = new System.Drawing.Size(223, 20);
            this.passwordBox.TabIndex = 1;
            this.passwordBox.UseSystemPasswordChar = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(191, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "seconds";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Polling interval";
            // 
            // pollingIntervalBox
            // 
            this.pollingIntervalBox.Location = new System.Drawing.Point(88, 30);
            this.pollingIntervalBox.Maximum = new decimal(new int[] {
            86400,
            0,
            0,
            0});
            this.pollingIntervalBox.Name = "pollingIntervalBox";
            this.pollingIntervalBox.Size = new System.Drawing.Size(97, 20);
            this.pollingIntervalBox.TabIndex = 1;
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.accountTab);
            this.tabControl.Controls.Add(this.behaviourTab);
            this.tabControl.Controls.Add(this.aboutTab);
            this.tabControl.Location = new System.Drawing.Point(6, 7);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(304, 130);
            this.tabControl.TabIndex = 0;
            // 
            // accountTab
            // 
            this.accountTab.Controls.Add(this.emailLabel);
            this.accountTab.Controls.Add(this.emailBox);
            this.accountTab.Controls.Add(this.passwordBox);
            this.accountTab.Controls.Add(this.passwordLabel);
            this.accountTab.Location = new System.Drawing.Point(4, 22);
            this.accountTab.Name = "accountTab";
            this.accountTab.Padding = new System.Windows.Forms.Padding(3);
            this.accountTab.Size = new System.Drawing.Size(296, 104);
            this.accountTab.TabIndex = 0;
            this.accountTab.Text = "Account";
            this.accountTab.UseVisualStyleBackColor = true;
            // 
            // behaviourTab
            // 
            this.behaviourTab.Controls.Add(this.autoStartBox);
            this.behaviourTab.Controls.Add(this.label2);
            this.behaviourTab.Controls.Add(this.label1);
            this.behaviourTab.Controls.Add(this.pollingIntervalBox);
            this.behaviourTab.Location = new System.Drawing.Point(4, 22);
            this.behaviourTab.Name = "behaviourTab";
            this.behaviourTab.Padding = new System.Windows.Forms.Padding(3);
            this.behaviourTab.Size = new System.Drawing.Size(296, 143);
            this.behaviourTab.TabIndex = 1;
            this.behaviourTab.Text = "Behaviour";
            this.behaviourTab.UseVisualStyleBackColor = true;
            // 
            // autoStartBox
            // 
            this.autoStartBox.AutoSize = true;
            this.autoStartBox.Location = new System.Drawing.Point(10, 7);
            this.autoStartBox.Name = "autoStartBox";
            this.autoStartBox.Size = new System.Drawing.Size(180, 17);
            this.autoStartBox.TabIndex = 0;
            this.autoStartBox.Text = "Automatically start with Windows";
            this.autoStartBox.UseVisualStyleBackColor = true;
            // 
            // aboutTab
            // 
            this.aboutTab.Controls.Add(this.growlForWindowsLink);
            this.aboutTab.Controls.Add(this.descriptionLabel);
            this.aboutTab.Controls.Add(this.authorLabel);
            this.aboutTab.Controls.Add(this.websiteLink);
            this.aboutTab.Controls.Add(this.titleLabel);
            this.aboutTab.Controls.Add(this.iconBox);
            this.aboutTab.Location = new System.Drawing.Point(4, 22);
            this.aboutTab.Name = "aboutTab";
            this.aboutTab.Size = new System.Drawing.Size(296, 104);
            this.aboutTab.TabIndex = 2;
            this.aboutTab.Text = "About";
            this.aboutTab.UseVisualStyleBackColor = true;
            // 
            // growlForWindowsLink
            // 
            this.growlForWindowsLink.AutoSize = true;
            this.growlForWindowsLink.Location = new System.Drawing.Point(3, 85);
            this.growlForWindowsLink.Name = "growlForWindowsLink";
            this.growlForWindowsLink.Size = new System.Drawing.Size(96, 13);
            this.growlForWindowsLink.TabIndex = 1;
            this.growlForWindowsLink.TabStop = true;
            this.growlForWindowsLink.Text = "Growl for Windows";
            this.growlForWindowsLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.growlForWindowsLinkClicked);
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.Location = new System.Drawing.Point(3, 38);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(278, 34);
            this.descriptionLabel.TabIndex = 5;
            this.descriptionLabel.Text = "{DESCRIPTION}";
            // 
            // authorLabel
            // 
            this.authorLabel.AutoSize = true;
            this.authorLabel.Location = new System.Drawing.Point(41, 22);
            this.authorLabel.Name = "authorLabel";
            this.authorLabel.Size = new System.Drawing.Size(75, 13);
            this.authorLabel.TabIndex = 4;
            this.authorLabel.Text = "by {AUTHOR}";
            // 
            // websiteLink
            // 
            this.websiteLink.AutoSize = true;
            this.websiteLink.Location = new System.Drawing.Point(3, 72);
            this.websiteLink.Name = "websiteLink";
            this.websiteLink.Size = new System.Drawing.Size(46, 13);
            this.websiteLink.TabIndex = 0;
            this.websiteLink.TabStop = true;
            this.websiteLink.Text = "Website";
            this.websiteLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.WebsiteLinkClicked);
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.Location = new System.Drawing.Point(41, 9);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(121, 13);
            this.titleLabel.TabIndex = 1;
            this.titleLabel.Text = "{TITLE} {VERSION}";
            // 
            // iconBox
            // 
            this.iconBox.BackColor = System.Drawing.Color.Transparent;
            this.iconBox.Location = new System.Drawing.Point(3, 3);
            this.iconBox.Name = "iconBox";
            this.iconBox.Size = new System.Drawing.Size(32, 32);
            this.iconBox.TabIndex = 0;
            this.iconBox.TabStop = false;
            // 
            // PreferencesForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(316, 173);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.applyButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PreferencesForm";
            ((System.ComponentModel.ISupportInitialize)(this.pollingIntervalBox)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.accountTab.ResumeLayout(false);
            this.accountTab.PerformLayout();
            this.behaviourTab.ResumeLayout(false);
            this.behaviourTab.PerformLayout();
            this.aboutTab.ResumeLayout(false);
            this.aboutTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label emailLabel;
        private System.Windows.Forms.TextBox emailBox;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.MaskedTextBox passwordBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown pollingIntervalBox;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage accountTab;
        private System.Windows.Forms.TabPage behaviourTab;
        private System.Windows.Forms.TabPage aboutTab;
        private System.Windows.Forms.CheckBox autoStartBox;
        private System.Windows.Forms.PictureBox iconBox;
        private System.Windows.Forms.LinkLabel websiteLink;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label authorLabel;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.LinkLabel growlForWindowsLink;
    }
}