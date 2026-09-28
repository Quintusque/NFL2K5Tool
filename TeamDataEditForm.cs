using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NFL2K5Tool
{
    // NOTE: hand-written skeleton -- create TeamDataEditForm in the VS Designer (so
    // TeamDataEditForm.Designer.cs + .resx get generated normally) and drop this logic
    // in, adjusting control names to match what you lay out. Control names below
    // (Nickname, Abbrev, City, AbbrAlt, DefaultJersey as TextBox; Stadium, Playbook,
    // Logo, DefScheme as StringSelectionControl; m_TeamsComboBox as ComboBox; mOkButton/
    // mCancelButton/mPrevButton/mNextButton as Button) must match exactly, same
    // convention as CoachEditForm.
    public partial class TeamDataEditForm : Form
    {
        private bool mInitializing = false;
        private string mKeyString = "";
        private string[] mKeyParts = null;

        /// <summary>
        /// The loaded GamesaveTool -- needed for dropdown population (stadium names,
        /// playbook names, jersey names all come from the tool's lookup tables, which
        /// are built at load time and specific to the save currently open, not static
        /// lists). Set by MainForm.EditTeamData() before Data is assigned.
        /// </summary>
        public GamesaveTool Tool { get; set; }

        public TeamDataEditForm()
        {
            InitializeComponent();

            DefScheme.RepresentedValue = typeof(DefensiveScheme);
        }

        private string mData = "";
        public string Data
        {
            get { return mData; }
            set { mData = value; SetupGui(); }
        }

        private void SetupGui()
        {
            if (SetupKey())
            {
                List<string> rows = InputParser.GetTeamDataRows(Data);
                m_TeamsComboBox.Items.Clear();
                if (rows != null)
                {
                    foreach (string line in rows)
                        m_TeamsComboBox.Items.Add(GetAttribute(line, "Team"));
                }

                // Stadium/Playbook dropdowns are populated once per Data assignment
                // from the currently loaded save -- not static, since a modded save
                // could in principle have different stadium/playbook entries than
                // another save. Values come from GamesaveTool's own lookup tables,
                // already built at LoadSaveFile time.
                if (Tool != null)
                {
                    Stadium.SetItems(ParseBracketedNames(Tool.GetStadiumNamesList()));
                    Playbook.SetItems(ParsePlaybookTokens(Tool.GetPlaybookNamesList()));
                }
            }
        }

        // GetStadiumNamesList() returns lines like "  25: [San Francisco Park]" --
        // extract just the bracketed name for the dropdown's item list.
        private static string[] ParseBracketedNames(string listText)
        {
            List<string> names = new List<string>();
            Regex r = new Regex(@"\[(.*?)\]");
            foreach (Match m in r.Matches(listText))
                names.Add(m.Groups[1].Value);
            return names.ToArray();
        }

        // GetPlaybookNamesList() returns lines like "  PB_49ers" -- one token per line.
        private static string[] ParsePlaybookTokens(string listText)
        {
            List<string> tokens = new List<string>();
            string[] lines = listText.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                string trimmed = line.Trim();
                if (trimmed.StartsWith("PB_"))
                    tokens.Add(trimmed);
            }
            return tokens.ToArray();
        }

        private bool SetupKey()
        {
            bool retVal = false;
            Regex keyReg = new Regex("TeamDataKEY=(.*)", RegexOptions.IgnoreCase);
            Match m = keyReg.Match(Data);
            if (m != Match.Empty)
            {
                if (mKeyString != m.Groups[1].Value)
                {
                    mKeyString = m.Groups[1].Value;
                    mKeyParts = mKeyString.Split(new char[] { ',' });
                    retVal = true;
                }
            }
            else
            {
                throw new InvalidOperationException("Please make sure the TeamData key is present in the text.");
            }
            return retVal;
        }

        private string GetAttribute(string line, string attr)
        {
            List<string> parts = InputParser.ParseTeamDataLine(line);
            int i = Array.IndexOf(mKeyParts, attr);
            if (i > -1 && i < parts.Count)
                return parts[i];
            return null;
        }

        private int mSelectionStart = 0;
        public int SelectionStart
        {
            get { return mSelectionStart; }
            set
            {
                if (mSelectionStart != value)
                {
                    mSelectionStart = value;
                    OnSelectionStartChanged();
                }
            }
        }

        private void OnSelectionStartChanged()
        {
            string line = InputParser.GetLine(mSelectionStart, Data);
            List<string> rows = InputParser.GetTeamDataRows(Data);
            if (rows != null)
            {
                for (int i = 0; i < rows.Count; i++)
                {
                    if (rows[i].StartsWith(line))
                    {
                        m_TeamsComboBox.SelectedIndex = i;
                        break;
                    }
                }
            }
            mInitializing = true;
            SetCurrentTeamData();
            mInitializing = false;
        }

        private void SetCurrentTeamData()
        {
            List<string> rows = InputParser.GetTeamDataRows(Data);
            if (rows == null || m_TeamsComboBox.SelectedItem == null) return;
            string team = m_TeamsComboBox.SelectedItem.ToString();
            string tmp = "TeamData," + team;
            foreach (string row in rows)
            {
                if (row.StartsWith(tmp))
                {
                    SetTeamData(row);
                    break;
                }
            }
        }

        private void SetTeamData(string current)
        {
            foreach (string attribute in mKeyParts)
            {
                string val = GetAttribute(current, attribute);
                if (val != null)
                    SetControlValue(attribute, val);
            }
        }

        private void m_TeamsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            mInitializing = true;
            SetCurrentTeamData();
            mInitializing = false;
        }

        // Wire this as the ValueChanged/Leave handler for every field control in the Designer.
        private void ValueChanged(object sender, EventArgs e)
        {
            if (!mInitializing)
                ReplaceTeamData();
        }

        private void ReplaceTeamData()
        {
            if (m_TeamsComboBox.SelectedItem == null) return;
            string team = m_TeamsComboBox.SelectedItem.ToString();
            string oldRow = GetTeamDataString(team);
            if (oldRow == null) return;

            string newRow = GetTeamDataString_UI();
            string replacement = string.Format("TeamData,{0},{1}", team, newRow);
            mData = Data.Replace(oldRow, replacement);
        }

        private string GetTeamDataString(string team)
        {
            Regex r = new Regex(String.Format("(TeamData,{0},.*)", team));
            Match m = r.Match(Data);
            return m != Match.Empty ? m.Groups[0].Value : null;
        }

        public string GetTeamDataString_UI()
        {
            StringBuilder sb = new StringBuilder(200);
            foreach (string key in mKeyParts)
            {
                if (key.Equals("Team", StringComparison.InvariantCultureIgnoreCase)
                    || key.Equals("TeamData", StringComparison.InvariantCultureIgnoreCase))
                    continue;
                string v = GetControlValue(key);
                if (!String.IsNullOrEmpty(v))
                {
                    // Stadium is wrapped in [brackets] on export, matching GetTeamData.
                    if (key.Equals("Stadium", StringComparison.InvariantCultureIgnoreCase))
                        sb.Append("[").Append(v).Append("]");
                    else
                        sb.Append(v);
                    sb.Append(",");
                }
            }
            if (sb.Length > 0) sb.Remove(sb.Length - 1, 1); // trailing comma
            return sb.ToString();
        }

        private string GetControlValue(string controlName)
        {
            Control c = FindControl(this, controlName);
            if (c == null) return null;

            TextBox tb = c as TextBox;
            StringSelectionControl ssc = c as StringSelectionControl;

            if (ssc != null)
            {
                if (controlName.Equals("DefScheme", StringComparison.InvariantCultureIgnoreCase))
                    return ((int)Enum.Parse(typeof(DefensiveScheme), ssc.Value)).ToString();
                return ssc.Value;
            }
            if (tb != null)
            {
                if (controlName.Equals("Logo", StringComparison.InvariantCultureIgnoreCase)
                    || controlName.Equals("DefaultJersey", StringComparison.InvariantCultureIgnoreCase))
                {
                    // Let GamesaveTool.SetTeamString do the actual int validation/error
                    // reporting -- this form just passes the raw text through.
                    return tb.Text;
                }
                return tb.Text;
            }
            return null;
        }

        private void SetControlValue(string controlName, string val)
        {
            Control c = FindControl(this, controlName);
            TextBox tb = c as TextBox;
            StringSelectionControl ssc = c as StringSelectionControl;

            if (ssc != null)
            {
                if (controlName.Equals("DefScheme", StringComparison.InvariantCultureIgnoreCase))
                {
                    int schemeVal;
                    ssc.Value = Int32.TryParse(val, out schemeVal)
                        ? ((DefensiveScheme)schemeVal).ToString()
                        : val;
                }
                else
                {
                    ssc.Value = val;
                }
            }
            else if (tb != null)
            {
                tb.Text = val;
            }
        }

        private static Control FindControl(Control parent, string name)
        {
            if (parent.Name == name) return parent;
            foreach (Control c in parent.Controls)
            {
                if (c.Name == name) return c;
                Control found = FindControl(c, name);
                if (found != null) return found;
            }
            return null;
        }

        private void mPrevButton_Click(object sender, EventArgs e)
        {
            if (m_TeamsComboBox.Items.Count == 0) return;
            m_TeamsComboBox.SelectedIndex = m_TeamsComboBox.SelectedIndex != 0
                ? m_TeamsComboBox.SelectedIndex - 1
                : m_TeamsComboBox.Items.Count - 1;
        }

        private void mNextButton_Click(object sender, EventArgs e)
        {
            if (m_TeamsComboBox.Items.Count == 0) return;
            m_TeamsComboBox.SelectedIndex = m_TeamsComboBox.SelectedIndex == m_TeamsComboBox.Items.Count - 1
                ? 0
                : m_TeamsComboBox.SelectedIndex + 1;
        }

        private void mOkButton_Click(object sender, EventArgs e)
        {
            ReplaceTeamData();
        }
    }
}
