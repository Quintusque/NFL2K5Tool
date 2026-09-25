// ===================================================================================
// MainForm.designer.cs / MainForm.cs additions for Team Data editing.
// Mirrors the existing Coach wiring (coachOptionsToolStripMenuItem / listCoachesToolStripMenuItem1
// / EditCoach / CoachEditForm) exactly.
// ===================================================================================

// ─────────────────────────────────────────────────────────────────────────────────
// MainForm.designer.cs
// ─────────────────────────────────────────────────────────────────────────────────

// 1. In InitializeComponent(), add the field initializer near the other View-menu items:
//        this.listTeamDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();

// 2. Add it to the View menu's DropDownItems array, right after coachOptionsToolStripMenuItem:
//
//    this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
//        this.findToolStripMenuItem, this.debugDialogMenuItem, this.listTeamsToolStripMenuItem,
//        this.listApperanceToolStripMenuItem, this.listContractDetailsToolStripMenuItem,
//        this.listAttributesToolStripMenuItem, this.listSpecialTeamsToolStripMenuItem,
//        this.listScheduleToolStripMenuItem, this.listFreeAgentsToolStripMenuItem,
//        this.listDraftClassToolStripMenuItem, this.coachOptionsToolStripMenuItem,
//        this.listTeamDataToolStripMenuItem,                                          // NEW
//        this.playerControlledTeamsToolStripMenuItem});
//
// 3. Configure the new item, same block shape as listCoachesToolStripMenuItem1:
//
//    this.listTeamDataToolStripMenuItem.Name = "listTeamDataToolStripMenuItem";
//    this.listTeamDataToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
//    this.listTeamDataToolStripMenuItem.Text = "List Team Data";
//    this.listTeamDataToolStripMenuItem.Click += new System.EventHandler(this.listTeamDataToolStripMenuItemClick);
//
// 4. Add the field declaration alongside the other ToolStripMenuItem fields:
//        private System.Windows.Forms.ToolStripMenuItem listTeamDataToolStripMenuItem;


// ─────────────────────────────────────────────────────────────────────────────────
// MainForm.cs
// ─────────────────────────────────────────────────────────────────────────────────

// 1. In ListContents(), add right next to the existing Coach block:
//
//        if (listCoachesToolStripMenuItem1.Checked) builder.Append(mTool.GetCoachData());
//        if (listTeamDataToolStripMenuItem.Checked) builder.Append(mTool.GetTeamDataAll());   // NEW
//
// 2. Add the toggle click handler, identical shape to every other list* checkbox:
//
//        private void listTeamDataToolStripMenuItemClick(object sender, EventArgs e)
//        {
//            listTeamDataToolStripMenuItem.Checked = !listTeamDataToolStripMenuItem.Checked;
//        }
//
// 3. Extend DoubleClicked() to recognize TeamData, rows the same way it recognizes Coach, rows:
//
//        private void DoubleClicked()
//        {
//            string line = InputParser.GetLine(mTextBox.SelectionStart, mTextBox.Text);
//            if (!String.IsNullOrEmpty(line) && line.StartsWith("Coach", StringComparison.InvariantCultureIgnoreCase))
//                EditCoach();
//            else if (!String.IsNullOrEmpty(line) && line.StartsWith("TeamData", StringComparison.InvariantCultureIgnoreCase))  // NEW
//                EditTeamData();                                                                                                 // NEW
//            else if (!String.IsNullOrEmpty(line) && InputParser.ParsePlayerLine(line).Count > 2)
//                EditPlayer();
//        }
//
// 4. Add EditTeamData(), mirroring EditCoach() exactly:
//
//        private void EditTeamData()
//        {
//            TeamDataEditForm form = new TeamDataEditForm();
//            form.Tool = mTool;                      // gives the form access to GetJerseyName/lookup data
//            form.Data = mTextBox.Text;
//            form.SelectionStart = mTextBox.SelectionStart;
//            if (form.ShowDialog(this) == DialogResult.OK)
//            {
//                SetText(form.Data);
//                mTextBox.SelectionStart = form.SelectionStart;
//                mTextBox.ScrollToCaret();
//            }
//            form.Dispose();
//        }
//
// 5. Add StaticUtils.ShowWarnings(false) alongside every existing StaticUtils.ShowErrors(false)
//    call, so DefaultJersey's out-of-range warning (added a few commits back) actually reaches
//    the user instead of accumulating silently. There are three call sites in the current file:
//
//    a) LoadSaveFile(string filename), right after the existing line:
//           StaticUtils.ShowErrors(false);
//       becomes:
//           StaticUtils.ShowWarnings(false);   // NEW
//           StaticUtils.ShowErrors(false);
//
//    b) mSaveButtonClick(...) / ApplyTextToSave(), right after:
//           StaticUtils.ShowErrors(false);
//       same addition, same order (warnings first, matching BadAL's Program.dart which calls
//       StaticUtils.ShowWarnings() immediately before StaticUtils.ShowErrors()).
