using System;
using System.Collections.Generic;
using System.Text;

namespace NFL2K5Tool
{
    /// <summary>
    /// Validates player attributes, sort a team
    /// TODO: Split this up into 2 classes with a shared base
    /// </summary>
    public class PlayerValidator: PlayerParser
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="key">The attribute key.</param>
        public PlayerValidator(string key ):base(key) { }

        /// <summary>
        /// Processes the text looking for player attributes that seem incorrect.
        /// Adds warnings when something seems like it may be incorrect.
        /// </summary>
        /// <param name="text">The league/ team data</param>
        public string ValidatePlayers(string text)
        {
            StringBuilder builder = new StringBuilder(); 
            text = text.Replace("\r\n", "\n");
            char[] chars = "\n".ToCharArray();
            string[] lines = text.Split(chars);
            string line;
            for (int i = 0; i < lines.Length; i++)
            {
                line = lines[i].Trim();
                if (line.StartsWith("#") || line.StartsWith("Key="))
                {
                }
                else if (line.IndexOf(",") > -1 && !(  // don't check the following lines
                          line.StartsWith("Coach")
                       || line.StartsWith("SET")
                       || line.StartsWith("ApplyFormula")
                    ))
                {
                    try {                builder.Append(ValidatePlayer(line));                 } 
                    catch (Exception) { Console.WriteLine("# issue validating line: " + line); }
                }
            }
            if (builder.Length > 0)
            {
                builder.Insert(0, "LookupAndModify\n"+
                    "Key=Position,fname,lname,BodyType,Height,Weight\n"+
                    "#Team = FreeAgents  (this is a comment but allows player editor to function on this data)\n"
                    );
            }
            return builder.ToString();
        }

        /// <summary>
        /// Will add warnings if there is an issue with the player. 
        /// </summary>
        /// <param name="line">The player line</param>
        public string ValidatePlayer(string line)
        {
            List<string> playerParts = InputParser.ParsePlayerLine(line);
            PlayerValidationResult res = new PlayerValidationResult(Get(playerParts, "Position"), Get(playerParts, "fname"), Get(playerParts, "lname"));
            ValidateBodyType(playerParts, res);
            ValidateWeight(playerParts, res);
            if (res.Invalid)
            {
                res.Height = Get(playerParts, "Height");
                res.BodyType = Get(playerParts, "BodyType");
                res.Weight = Get(playerParts, "Weight");
                return String.Format("{0},{1},{2},{3},{4},{5}\n", res.Position, res.FirstName, res.LastName, res.BodyType, res.Height, res.Weight);
            }
            return "";
        }

        /// <summary>
        /// Processes the text looking for DevelopmentArchetype values retail would never generate
        /// for a player's position. Adds warnings when something seems incorrect. This is a separate
        /// check from ValidatePlayers (Height/Weight/BodyType) -- neither one changes any values.
        /// </summary>
        /// <param name="text">The league/team data</param>
        public string ValidatePlayersArchetype(string text)
        {
            StringBuilder builder = new StringBuilder();
            text = text.Replace("\r\n", "\n");
            char[] chars = "\n".ToCharArray();
            string[] lines = text.Split(chars);
            string line;
            for (int i = 0; i < lines.Length; i++)
            {
                line = lines[i].Trim();
                if (line.StartsWith("#") || line.StartsWith("Key="))
                {
                }
                else if (line.IndexOf(",") > -1 && !(  // don't check the following lines
                          line.StartsWith("Coach")
                       || line.StartsWith("SET")
                       || line.StartsWith("ApplyFormula")
                    ))
                {
                    try {                builder.Append(ValidatePlayerArchetype(line));                 } 
                    catch (Exception) { Console.WriteLine("# issue validating line: " + line); }
                }
            }
            if (builder.Length > 0)
            {
                builder.Insert(0, "LookupAndModify\n"+
                    "Key=Position,fname,lname,DevelopmentArchetype\n"+
                    "#Team = FreeAgents  (this is a comment but allows player editor to function on this data)\n"
                    );
            }
            return builder.ToString();
        }

        /// <summary>
        /// Will add a warning if this player's DevelopmentArchetype is one retail would never generate
        /// for their position. Friendly range is always 1-12 (see EnumDefinitions.PlayerOffsets.
        /// DevelopmentArchetype), but only the ten positions in sDualProfilePositions ever use 7-12
        /// (profile 1) in retail data; every other position should stay within 1-6 (profile 0).
        /// This only reports mismatches -- it never changes the player's value.
        /// </summary>
        /// <param name="line">The player line</param>
        public string ValidatePlayerArchetype(string line)
        {
            List<string> playerParts = InputParser.ParsePlayerLine(line);
            PlayerValidationResult res = new PlayerValidationResult(Get(playerParts, "Position"), Get(playerParts, "fname"), Get(playerParts, "lname"));
            ValidateDevelopmentArchetype(playerParts, res);
            if (res.Invalid)
            {
                res.DevelopmentArchetype = Get(playerParts, "DevelopmentArchetype");
                return String.Format("{0},{1},{2},{3}\n", res.Position, res.FirstName, res.LastName, res.DevelopmentArchetype);
            }
            return "";
        }

        // Positions whose retail archetype rows use both profiles (friendly 1-12).
        // Every other position (K,P,FS,SS,C,G,T) only ever has profile 0 rows (friendly 1-6) in retail data.
        private static readonly string[] sDualProfilePositions = new string[] {
            "QB", "WR", "CB", "RB", "FB", "TE", "OLB", "ILB", "DT", "DE"
        };

        /// <summary>
        /// Flags a DevelopmentArchetype value retail would never generate for this position.
        /// This only reports mismatches -- it never changes the player's value.
        /// </summary>
        private void ValidateDevelopmentArchetype(List<string> playerParts, PlayerValidationResult res)
        {
            string archetypeStr = Get(playerParts, "DevelopmentArchetype");
            if (String.IsNullOrEmpty(archetypeStr))
                return; // field not present in this key; nothing to check

            int archetype;
            if (!Int32.TryParse(archetypeStr, out archetype))
                return;

            string pos = Get(playerParts, "Position");
            bool dualProfile = Array.IndexOf(sDualProfilePositions, pos) > -1;
            int max = dualProfile ? 12 : 6;

            if (archetype < 1 || archetype > max)
            {
                res.Invalid = true;
            }
        }

        private void ValidateWeight(List<string> playerParts, PlayerValidationResult res)
        {
            string pos = Get(playerParts, "Position");
            string[] possibilities = null;
            switch (pos)
            {
                case "SS":
                case "FS":
                case "QB":
                case "RB":
                case "WR":
                    possibilities = GetRange(170, 260);
                    break;
                case "CB":
                case "P":
                case "K":
                    possibilities = GetRange(150, 240);
                    break;
                case "ILB":
                case "OLB":
                case "FB":
                    possibilities = GetRange(210, 280);
                    break;
                case "TE":
                    possibilities = GetRange(220, 280);
                    break;
                case "G":
                case "T":
                    possibilities = GetRange(260, 390);
                    break;
                case "C":
                    possibilities = GetRange(240, 380);
                    break;
                case "DE":
                    possibilities = GetRange(220, 320);
                    break;
                case "DT":
                    possibilities = GetRange(260, 390);
                    break;
            }
            if (!ValidateAttribute("Weight", possibilities, playerParts))
            {
                res.Invalid = true;
            }
        }

        // Skinny = 0, Normal, Large, ExtraLarge
        private void ValidateBodyType(List<string> playerParts, PlayerValidationResult res)
        {
            string pos = Get(playerParts, "Position");
            string[] possibilities=null;
            switch (pos)
            {
                case "CB":
                case "SS":
                case "FS":
                case "QB":
                case "RB":
                case "P":
                case "K":
                case "WR":
                    possibilities = new string[] {"Skinny", "Normal", /*"Large", "ExtraLarge"*/};
                    break;
                case "ILB":
                case "OLB":
                case "FB":
                case "TE":
                    possibilities = new string[] { /*"Skinny",*/ "Normal", "Large", /*"ExtraLarge"*/ };
                    break;
                case "G":
                case "T":
                case "C":
                    possibilities = new string[] { /*"Skinny",*/ "Normal", "Large", "ExtraLarge" };
                    break;
                case "DE":
                    possibilities = new string[] { /*"Skinny",*/ "Normal", "Large", "ExtraLarge" };
                    break;
                case "DT":
                    possibilities = new string[] { /*"Skinny",*/ "Normal", "Large", "ExtraLarge" };
                    break;
            }
            if (!ValidateAttribute("BodyType", possibilities, playerParts))
            {
                res.Invalid = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute">the attribute to validate (BodyType, Weight...)</param>
        /// <param name="validValues"></param>
        /// <param name="playerParts"></param>
        private bool ValidateAttribute(string attribute, string[] validValues, List<string> playerParts)
        {
            string val = Get(playerParts, attribute);
            int index = -1;
            for (int i = 0; i < validValues.Length; i++)
            {
                if( val.Equals(validValues[i]))
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
                return false;
            
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>an array of strings like [4,5,6,7,8...]</returns>
        private string[] GetRange(int start, int end)
        {
            int length = end - start + 1;
            string[] retVal = new string[length];
            int j =0;
            
            for (int i = start; i <= end; i++)
                retVal[j++] = i.ToString();

            return retVal;
        }

        /// <summary>
        /// returns a range of equiptment attributes
        /// </summary>
        /// <param name="start">usually 0 or 1</param>
        /// <param name="end"></param>
        /// <param name="eqpt">a string like "FaceMask"</param>
        /// <returns>an array like [FaceMask1, FaceMask2 ...]</returns>
        private string[] GetEqptRange(int start, int end, string eqpt)
        {
            int length = end - start;
            string[] retVal = new string[length];
            int j = 0;

            for (int i = start; i <= end; i++)
                retVal[j++] = eqpt + i.ToString();

            return retVal;
        }

    }

    public class PlayerValidationResult
    {
        public PlayerValidationResult(string position, string firstName, string lastName)
        {
            this.Position = position;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Invalid = false;
        }

        public string Position  { get; set; }
        public string FirstName { get; set; }
        public string LastName  { get; set; }

        public String BodyType  { get; set; }
        public String Height    { get; set; }
        public String Weight    { get; set; }
        public String DevelopmentArchetype { get; set; }

        public bool Invalid { get; set; }
    }
}
