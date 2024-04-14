using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager sharedInstance;

    private List<LeaderboardController> leaderboards;
    private float userTime;

    private void Awake()
    {
        sharedInstance = this;
        leaderboards = new List<LeaderboardController>();
    }

    public void AddLeaderboard(LeaderboardController leaderboard)
    {
        if (userTime > 0.0f)
        {
            for (int i = 0; i < leaderboard.GetCount(); i++)
            {
                if (leaderboard.GetLeaderboardEntryAtIndex(i).Time == userTime)
                {
                    leaderboard.GetLeaderboardEntryAtIndex(i).IsUserScore = true;
                    break;
                }
            }
        }

        // Remove all existing leaderboards with the same name
        leaderboards.RemoveAll(p => p.Name == leaderboard.Name);

        // Add the Leaderboard object to the list
        leaderboards.Add(leaderboard);
    }

    public LeaderboardController GetLeaderboardByName(string name)
    {
        for (int i = 0; i < leaderboards.Count; i++)
        {
            if (leaderboards[i].Name == name)
                return leaderboards[i];
        }
        return null;
    }

    public int GetCount()
    {
        return leaderboards.Count;
    }

    public void SetUserTime(float userTime)
    {
        long ms = (long)(userTime * 1000.0f);
        userTime = (float)(ms) / 1000.0f;
    }
}