namespace NFL2K5Tool
{
    partial class TeamDataEditForm
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
            this.labelTeam = new System.Windows.Forms.Label();
            this.m_TeamsComboBox = new System.Windows.Forms.ComboBox();
            this.labelNickname = new System.Windows.Forms.Label();
            this.Nickname = new System.Windows.Forms.TextBox();
            this.labelAbbrev = new System.Windows.Forms.Label();
            this.Abbrev = new System.Windows.Forms.TextBox();
            this.labelCity = new System.Windows.Forms.Label();
            this.City = new System.Windows.Forms.TextBox();
            this.labelAbbrAlt = new System.Windows.Forms.Label();
            this.AbbrAlt = new System.Windows.Forms.TextBox();
            this.labelStadium = new System.Windows.Forms.Label();
            this.Stadium = new NFL2K5Tool.StringSelectionControl();
            this.labelPlaybook = new System.Windows.Forms.Label();
            this.Playbook = new NFL2K5Tool.StringSelectionControl();
            this.labelLogo = new System.Windows.Forms.Label();
            this.Logo = new System.Windows.Forms.TextBox();
            this.labelDefaultJersey = new System.Windows.Forms.Label();
            this.DefaultJersey = new System.Windows.Forms.TextBox();
            this.labelDefScheme = new System.Windows.Forms.Label();
            this.DefScheme = new NFL2K5Tool.StringSelectionControl();
            this.mOkButton = new System.Windows.Forms.Button();
            this.mCancelButton = new System.Windows.Forms.Button();
            this.mPrevButton = new System.Windows.Forms.Button();
            this.mNextButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // labelTeam
            //
            this.labelTeam.AutoSize = true;
            this.labelTeam.Location = new System.Drawing.Point(340, 12);
            this.labelTeam.Name = "labelTeam";
            this.labelTeam.Size = new System.Drawing.Size(33, 13);
            this.labelTeam.TabIndex = 0;
            this.labelTeam.Text = "Team";
            //
            // m_TeamsComboBox
            //
            this.m_TeamsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_TeamsComboBox.FormattingEnabled = true;
            this.m_TeamsComboBox.Location = new System.Drawing.Point(379, 9);
            this.m_TeamsComboBox.Name = "m_TeamsComboBox";
            this.m_TeamsComboBox.Size = new System.Drawing.Size(112, 21);
            this.m_TeamsComboBox.TabIndex = 1;
            this.m_TeamsComboBox.SelectedIndexChanged += new System.EventHandler(this.m_TeamsComboBox_SelectedIndexChanged);
            //
            // labelNickname
            //
            this.labelNickname.AutoSize = true;
            this.labelNickname.Location = new System.Drawing.Point(12, 15);
            this.labelNickname.Name = "labelNickname";
            this.labelNickname.Size = new System.Drawing.Size(52, 13);
            this.labelNickname.TabIndex = 2;
            this.labelNickname.Text = "Nickname";
            //
            // Nickname
            //
            this.Nickname.Location = new System.Drawing.Point(90, 12);
            this.Nickname.Name = "Nickname";
            this.Nickname.Size = new System.Drawing.Size(150, 20);
            this.Nickname.TabIndex = 3;
            this.Nickname.Leave += new System.EventHandler(this.ValueChanged);
            //
            // labelAbbrev
            //
            this.labelAbbrev.AutoSize = true;
            this.labelAbbrev.Location = new System.Drawing.Point(12, 45);
            this.labelAbbrev.Name = "labelAbbrev";
            this.labelAbbrev.Size = new System.Drawing.Size(41, 13);
            this.labelAbbrev.TabIndex = 4;
            this.labelAbbrev.Text = "Abbrev";
            //
            // Abbrev
            //
            this.Abbrev.Location = new System.Drawing.Point(90, 42);
            this.Abbrev.Name = "Abbrev";
            this.Abbrev.Size = new System.Drawing.Size(60, 20);
            this.Abbrev.TabIndex = 5;
            this.Abbrev.Leave += new System.EventHandler(this.ValueChanged);
            //
            // labelCity
            //
            this.labelCity.AutoSize = true;
            this.labelCity.Location = new System.Drawing.Point(160, 45);
            this.labelCity.Name = "labelCity";
            this.labelCity.Size = new System.Drawing.Size(25, 13);
            this.labelCity.TabIndex = 6;
            this.labelCity.Text = "City";
            //
            // City
            //
            this.City.Location = new System.Drawing.Point(190, 42);
            this.City.Name = "City";
            this.City.Size = new System.Drawing.Size(150, 20);
            this.City.TabIndex = 7;
            this.City.Leave += new System.EventHandler(this.ValueChanged);
            //
            // labelAbbrAlt
            //
            this.labelAbbrAlt.AutoSize = true;
            this.labelAbbrAlt.Location = new System.Drawing.Point(350, 45);
            this.labelAbbrAlt.Name = "labelAbbrAlt";
            this.labelAbbrAlt.Size = new System.Drawing.Size(51, 13);
            this.labelAbbrAlt.TabIndex = 8;
            this.labelAbbrAlt.Text = "AbbrAlt";
            //
            // AbbrAlt
            //
            this.AbbrAlt.Location = new System.Drawing.Point(410, 42);
            this.AbbrAlt.Name = "AbbrAlt";
            this.AbbrAlt.Size = new System.Drawing.Size(60, 20);
            this.AbbrAlt.TabIndex = 9;
            this.AbbrAlt.Leave += new System.EventHandler(this.ValueChanged);
            //
            // labelStadium
            //
            this.labelStadium.AutoSize = true;
            this.labelStadium.Location = new System.Drawing.Point(12, 78);
            this.labelStadium.Name = "labelStadium";
            this.labelStadium.Size = new System.Drawing.Size(44, 13);
            this.labelStadium.TabIndex = 10;
            this.labelStadium.Text = "Stadium";
            //
            // Stadium
            //
            this.Stadium.BackColor = System.Drawing.Color.Moccasin;
            this.Stadium.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Stadium.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Stadium.Location = new System.Drawing.Point(90, 75);
            this.Stadium.Name = "Stadium";
            this.Stadium.RepresentedValue = typeof(string);
            this.Stadium.Size = new System.Drawing.Size(250, 26);
            this.Stadium.TabIndex = 11;
            this.Stadium.Value = "";
            this.Stadium.ValueChanged += new System.EventHandler(this.ValueChanged);
            //
            // labelPlaybook
            //
            this.labelPlaybook.AutoSize = true;
            this.labelPlaybook.Location = new System.Drawing.Point(12, 112);
            this.labelPlaybook.Name = "labelPlaybook";
            this.labelPlaybook.Size = new System.Drawing.Size(47, 13);
            this.labelPlaybook.TabIndex = 12;
            this.labelPlaybook.Text = "Playbook";
            //
            // Playbook
            //
            this.Playbook.BackColor = System.Drawing.Color.Moccasin;
            this.Playbook.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Playbook.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Playbook.Location = new System.Drawing.Point(90, 109);
            this.Playbook.Name = "Playbook";
            this.Playbook.RepresentedValue = typeof(string);
            this.Playbook.Size = new System.Drawing.Size(180, 26);
            this.Playbook.TabIndex = 13;
            this.Playbook.Value = "";
            this.Playbook.ValueChanged += new System.EventHandler(this.ValueChanged);
            //
            // labelLogo
            //
            this.labelLogo.AutoSize = true;
            this.labelLogo.Location = new System.Drawing.Point(12, 146);
            this.labelLogo.Name = "labelLogo";
            this.labelLogo.Size = new System.Drawing.Size(29, 13);
            this.labelLogo.TabIndex = 14;
            this.labelLogo.Text = "Logo";
            //
            // Logo
            //
            this.Logo.Location = new System.Drawing.Point(90, 143);
            this.Logo.Name = "Logo";
            this.Logo.Size = new System.Drawing.Size(50, 20);
            this.Logo.TabIndex = 15;
            this.Logo.Leave += new System.EventHandler(this.ValueChanged);
            //
            // labelDefaultJersey
            //
            this.labelDefaultJersey.AutoSize = true;
            this.labelDefaultJersey.Location = new System.Drawing.Point(160, 146);
            this.labelDefaultJersey.Name = "labelDefaultJersey";
            this.labelDefaultJersey.Size = new System.Drawing.Size(74, 13);
            this.labelDefaultJersey.TabIndex = 16;
            this.labelDefaultJersey.Text = "DefaultJersey";
            //
            // DefaultJersey
            //
            this.DefaultJersey.Location = new System.Drawing.Point(240, 143);
            this.DefaultJersey.Name = "DefaultJersey";
            this.DefaultJersey.Size = new System.Drawing.Size(50, 20);
            this.DefaultJersey.TabIndex = 17;
            this.DefaultJersey.Leave += new System.EventHandler(this.ValueChanged);
            //
            // labelDefScheme
            //
            this.labelDefScheme.AutoSize = true;
            this.labelDefScheme.Location = new System.Drawing.Point(12, 180);
            this.labelDefScheme.Name = "labelDefScheme";
            this.labelDefScheme.Size = new System.Drawing.Size(63, 13);
            this.labelDefScheme.TabIndex = 18;
            this.labelDefScheme.Text = "DefScheme";
            //
            // DefScheme
            //
            this.DefScheme.BackColor = System.Drawing.Color.Moccasin;
            this.DefScheme.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DefScheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DefScheme.Location = new System.Drawing.Point(90, 177);
            this.DefScheme.Name = "DefScheme";
            this.DefScheme.RepresentedValue = typeof(NFL2K5Tool.DefensiveScheme);
            this.DefScheme.Size = new System.Drawing.Size(140, 26);
            this.DefScheme.TabIndex = 19;
            this.DefScheme.Value = "";
            this.DefScheme.ValueChanged += new System.EventHandler(this.ValueChanged);
            //
            // mOkButton
            //
            this.mOkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.mOkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.mOkButton.Location = new System.Drawing.Point(335, 220);
            this.mOkButton.Name = "mOkButton";
            this.mOkButton.Size = new System.Drawing.Size(75, 23);
            this.mOkButton.TabIndex = 20;
            this.mOkButton.Text = "&OK";
            this.mOkButton.UseVisualStyleBackColor = true;
            this.mOkButton.Click += new System.EventHandler(this.mOkButton_Click);
            //
            // mCancelButton
            //
            this.mCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.mCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.mCancelButton.Location = new System.Drawing.Point(416, 220);
            this.mCancelButton.Name = "mCancelButton";
            this.mCancelButton.Size = new System.Drawing.Size(75, 23);
            this.mCancelButton.TabIndex = 21;
            this.mCancelButton.Text = "&Cancel";
            this.mCancelButton.UseVisualStyleBackColor = true;
            //
            // mPrevButton
            //
            this.mPrevButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mPrevButton.Location = new System.Drawing.Point(12, 220);
            this.mPrevButton.Name = "mPrevButton";
            this.mPrevButton.Size = new System.Drawing.Size(56, 23);
            this.mPrevButton.TabIndex = 22;
            this.mPrevButton.Text = "Prev";
            this.mPrevButton.UseVisualStyleBackColor = true;
            this.mPrevButton.Click += new System.EventHandler(this.mPrevButton_Click);
            //
            // mNextButton
            //
            this.mNextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mNextButton.Location = new System.Drawing.Point(74, 220);
            this.mNextButton.Name = "mNextButton";
            this.mNextButton.Size = new System.Drawing.Size(56, 23);
            this.mNextButton.TabIndex = 23;
            this.mNextButton.Text = "Next";
            this.mNextButton.UseVisualStyleBackColor = true;
            this.mNextButton.Click += new System.EventHandler(this.mNextButton_Click);
            //
            // TeamDataEditForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.mCancelButton;
            this.ClientSize = new System.Drawing.Size(503, 255);
            this.Controls.Add(this.mNextButton);
            this.Controls.Add(this.mPrevButton);
            this.Controls.Add(this.mOkButton);
            this.Controls.Add(this.mCancelButton);
            this.Controls.Add(this.DefScheme);
            this.Controls.Add(this.labelDefScheme);
            this.Controls.Add(this.DefaultJersey);
            this.Controls.Add(this.labelDefaultJersey);
            this.Controls.Add(this.Logo);
            this.Controls.Add(this.labelLogo);
            this.Controls.Add(this.Playbook);
            this.Controls.Add(this.labelPlaybook);
            this.Controls.Add(this.Stadium);
            this.Controls.Add(this.labelStadium);
            this.Controls.Add(this.AbbrAlt);
            this.Controls.Add(this.labelAbbrAlt);
            this.Controls.Add(this.City);
            this.Controls.Add(this.labelCity);
            this.Controls.Add(this.Abbrev);
            this.Controls.Add(this.labelAbbrev);
            this.Controls.Add(this.Nickname);
            this.Controls.Add(this.labelNickname);
            this.Controls.Add(this.m_TeamsComboBox);
            this.Controls.Add(this.labelTeam);
            this.Name = "TeamDataEditForm";
            this.Text = "TeamDataEditForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelTeam;
        private System.Windows.Forms.ComboBox m_TeamsComboBox;
        private System.Windows.Forms.Label labelNickname;
        private System.Windows.Forms.TextBox Nickname;
        private System.Windows.Forms.Label labelAbbrev;
        private System.Windows.Forms.TextBox Abbrev;
        private System.Windows.Forms.Label labelCity;
        private System.Windows.Forms.TextBox City;
        private System.Windows.Forms.Label labelAbbrAlt;
        private System.Windows.Forms.TextBox AbbrAlt;
        private System.Windows.Forms.Label labelStadium;
        private StringSelectionControl Stadium;
        private System.Windows.Forms.Label labelPlaybook;
        private StringSelectionControl Playbook;
        private System.Windows.Forms.Label labelLogo;
        private System.Windows.Forms.TextBox Logo;
        private System.Windows.Forms.Label labelDefaultJersey;
        private System.Windows.Forms.TextBox DefaultJersey;
        private System.Windows.Forms.Label labelDefScheme;
        private StringSelectionControl DefScheme;
        private System.Windows.Forms.Button mOkButton;
        private System.Windows.Forms.Button mCancelButton;
        private System.Windows.Forms.Button mPrevButton;
        private System.Windows.Forms.Button mNextButton;
    }
}
