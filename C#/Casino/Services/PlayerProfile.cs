namespace Final_Task.Services
{
    public class PlayerProfile
    {
        public string Name { get; }
        public int Bank { get; set; }
        public int Wins { get; private set; }
        public int Losses { get; private set; }
        public int Draws { get; private set; }

        public PlayerProfile(string name, int initialBank)
        {
            Name = name;
            Bank = initialBank;
            Wins = 0;
            Losses = 0;
            Draws = 0;
        }

        public void AddWin() => Wins++;
        public void AddLoss() => Losses++;
        public void AddDraw() => Draws++;

        public override string ToString()
        {
            return $"{Name},{Bank},{Wins},{Losses},{Draws}";
        }

        public static PlayerProfile FromString(string data)
        {
            var parts = data.Split(',');
            return new PlayerProfile(parts[0], int.Parse(parts[1]))
            {
                Wins = int.Parse(parts[2]),
                Losses = int.Parse(parts[3]),
                Draws = int.Parse(parts[4])
            };
        }
    }
}
