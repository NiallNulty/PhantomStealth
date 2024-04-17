using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject MainCanvas;

    [SerializeField] private GameObject LoginCanvas;

    [SerializeField] private GameObject UpdateUsernameCanvas;

    [SerializeField] private GameObject ControlsCanvas;

    [SerializeField] private GameObject LeaderboardCanvas;

    [SerializeField] private GameObject AuthenticatingCanvas;

    [SerializeField] private GameObject AuthenticatingFailedCanvas;

    [SerializeField] private TMP_InputField usernameInputField;

    [SerializeField] private TMP_InputField passwordInputField;

    [SerializeField] private TMP_InputField currentUsernameInputField;

    [SerializeField] private TMP_InputField currentPasswordInputField;

    [SerializeField] private TMP_InputField newUsernameInputField;

    [SerializeField] private TextMeshProUGUI[] scores = new TextMeshProUGUI[5];

    private Network.AuthenticationRequestCompleted AuthenticationRequestCompleted;
    private Network.AuthenticationRequestFailed AuthenticationRequestFailed;

    private Network.UpdateUsernameRequestCompleted UpdateUsernameRequestCompleted;
    private Network.UpdateUsernameRequestFailed UpdateUsernameRequestFailed;

    private void Start()
    {
        if (Network.sharedInstance.IsAuthenticated())
        {
            Network.sharedInstance.LogOut();
        }
    }

    public void ChangeGhostPathEnabledValue()
    {
        if (Globals.GhostPathEnabled)
        {
            DisableGhostPath();
        }
        else
        {
            EnableGhostPath();
        }
    }

    public void DisableGhostPath()
    {
        Globals.GhostPathEnabled = false;
    }

    public void EnableGhostPath()
    {
        Globals.GhostPathEnabled = true;
    }

    public void HandleAnonymousAuthentication()
    {
        AuthenticatingCanvas.SetActive(true);

        if (Network.sharedInstance.HasAuthenticatedPreviously())
        {
            Network.sharedInstance.Reconnect();
        }
        else
        {
            Network.sharedInstance.RequestAnonymousAuthentication();
        }

        StartCoroutine(WaitToLoadLevel());
    }

    public void HandleUniversalAuthentication()
    {
        Network.sharedInstance.RequestAuthenticationUniversal(usernameInputField.text, passwordInputField.text, true, AuthenticationRequestCompleted, AuthenticationRequestFailed);
        StartCoroutine(WaitToLoadLevel());
    }

    public void UpdateUsername()
    {
        Network.sharedInstance.LogOut();

        Network.sharedInstance.RequestAuthenticationUniversal(currentUsernameInputField.text, currentPasswordInputField.text, false, AuthenticationRequestCompleted, AuthenticationRequestFailed);

        StartCoroutine(WaitToUpdateUsername());

        Network.sharedInstance.LogOut();
    }

    //Try every second for 5 seconds to see if user is authenticated
    private IEnumerator WaitToLoadLevel()
    {
        AuthenticatingCanvas.SetActive(true);

        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSecondsRealtime(1f);

            if (Network.sharedInstance.IsAuthenticated())
            {
                LoadLevel1Scene();
            }
        }

        AuthenticatingCanvas.SetActive(false);
        AuthenticatingFailedCanvas.SetActive(true);
    }

    //Wait 5 seconds to ensure user credentials are correct before Updating Username
    private IEnumerator WaitToUpdateUsername()
    {
        AuthenticatingCanvas.SetActive(true);

        yield return new WaitForSecondsRealtime(5f);

        if (Network.sharedInstance.IsAuthenticated())
        {
            Network.sharedInstance.RequestUpdateUsername(newUsernameInputField.text, UpdateUsernameRequestCompleted, UpdateUsernameRequestFailed);
        }
        else
        {
            AuthenticatingFailedCanvas.SetActive(true);
        }

        AuthenticatingCanvas.SetActive(false);
    }


    private IEnumerator WaitToShowLeaderboard()
    {
        AuthenticatingCanvas.SetActive(true);

        Network.sharedInstance.RequestAnonymousAuthentication();
        yield return new WaitForSecondsRealtime(2f);

        if (Network.sharedInstance.IsAuthenticated())
        {
            Network.sharedInstance.RequestLeaderboard("Main", 0, 5, OnLeaderboardRequestCompleted);
            yield return new WaitForSecondsRealtime(2f);
        }
        else
        {
            AuthenticatingFailedCanvas.SetActive(true);
            ShowMainCanvas();
        }

        AuthenticatingCanvas.SetActive(false);

        LeaderboardController leaderboard = LeaderboardManager.sharedInstance.GetLeaderboardByName("Main");

        if (leaderboard != null)
        {
            for (int i = 0; i < 5; i++)
            {
                LeaderboardEntry leaderboardEntry = leaderboard.GetLeaderboardEntryAtIndex(i);
                scores[i].text = (i + 1).ToString() + ". Name: " + leaderboardEntry.Nickname.PadRight(20) + "Score: " + leaderboardEntry.Score;
            }
            
            LeaderboardCanvas.gameObject.SetActive(true);
        }

        Network.sharedInstance.LogOut();
    }

    public void LoadLevel1Scene()
    {
        try
        {
            SceneManager.LoadScene("Level1Scene");
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public void ShowMainCanvas()
    {
        MainCanvas.gameObject.SetActive(true);
    }

    public void HideMainCanvas()
    {
        MainCanvas.gameObject.SetActive(false);
    }

    public void ShowLoginCanvas()
    {
        LoginCanvas.gameObject.SetActive(true);
    }

    public void HideLoginCanvas()
    {
        LoginCanvas.gameObject.SetActive(false);
    }

    public void ShowUpdateUsernameCanvas()
    {
        UpdateUsernameCanvas.gameObject.SetActive(true);
    }

    public void HideUpdateUsernameCanvas()
    {
        UpdateUsernameCanvas.gameObject.SetActive(false);
    }

    public void ShowControlsCanvas()
    {
        ControlsCanvas.gameObject.SetActive(true);
    }

    public void HideControlsCanvas()
    {
        ControlsCanvas.gameObject.SetActive(false);
    }
    private void OnLeaderboardRequestCompleted(LeaderboardController leaderboard)
    {
        LeaderboardManager.sharedInstance.AddLeaderboard(leaderboard);
    }

    public void ShowLeaderboardCanvas()
    {
        try
        {
            Network.sharedInstance.RequestAnonymousAuthentication();

            StartCoroutine(WaitToShowLeaderboard());
        }
        catch (System.Exception ex)
        {
            LogManager.Log(ex.Message);
        }
    }

    public void HideLeaderboardCanvas()
    {
        LeaderboardCanvas.gameObject.SetActive(false);
    }

    public void CloseGame()
    {
        try
        {
            Application.Quit();
        }
        catch (System.Exception ex)
        {
            LogManager.Log(ex.Message);
        }
    }
}
