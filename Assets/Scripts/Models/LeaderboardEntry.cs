public class LeaderboardEntry
{
    private string nickname;
    private int rank;
    private float time;
    private bool isUserScore;
    private long score;


    public LeaderboardEntry(string Nickname, int Rank, float Time, long Score)
    {
        nickname = Nickname;
        rank = Rank;
        time = Time;
        score = Score;
    }

    public string Nickname
    {
        get { return nickname; }
        set { nickname = value; }
    }

    public float Time
    {
        get { return time; }
        set { time = value; }
    }

    public int Rank
    {
        get { return rank; }
        set { rank = value; }
    }

    public bool IsUserScore
    {
        get { return isUserScore; }
        set { isUserScore = value; }
    }

    public long Score
    {
        get { return score; }
        set { score = value; }
    }
}