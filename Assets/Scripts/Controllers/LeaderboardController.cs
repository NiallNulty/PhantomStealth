using System.Collections.Generic;

public class LeaderboardController
{
    private string name;
    private List<LeaderboardEntry> leaderboard;

    public string Name
    {
        get { return name; }
    }

    public LeaderboardController(string Name, List<LeaderboardEntry> Leaderboard)
    {
        name = Name;
        leaderboard = Leaderboard;
    }

    public LeaderboardEntry GetLeaderboardEntryAtIndex(int index)
    {
        if (index >= 0 && index < GetCount())
            return leaderboard[index];
        return null;
    }

    public int GetCount()
    {
        return leaderboard.Count;
    }
}