using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
	[SerializeField]
	private GameObject MainCanvas;

    [SerializeField]
    private GameObject LoginCanvas;

    [SerializeField]
    private GameObject UpdateUsernameCanvas;

    [SerializeField]
    private GameObject ControlsCanvas;

    [SerializeField]
    private GameObject LeaderboardCanvas;

    [SerializeField]
    private TMP_InputField usernameInputField;

    [SerializeField]
    private TMP_InputField passwordInputField;

    [SerializeField]
    private TMP_InputField currentUsernameInputField;

    [SerializeField]
    private TMP_InputField currentPasswordInputField;

    [SerializeField]
    private TMP_InputField newUsernameInputField;

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

    public void Set(Network.AuthenticationRequestCompleted authenticationRequestCompleted, Network.AuthenticationRequestFailed authenticationRequestFailed)
    {
        AuthenticationRequestCompleted = authenticationRequestCompleted;
        AuthenticationRequestFailed = authenticationRequestFailed;
    }

    public void HandleAnonymousAuthentication()
    {
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
        if (Network.sharedInstance.HasAuthenticatedPreviously())
        {
            Network.sharedInstance.Reconnect();
        }
        else
        {
            Network.sharedInstance.RequestAuthenticationUniversal(usernameInputField.text, passwordInputField.text, true, AuthenticationRequestCompleted, AuthenticationRequestFailed);
        }

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
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSecondsRealtime(1f);

            if (Network.sharedInstance.IsAuthenticated())
            {
                LoadLevel1Scene();
            }
        }
    }

    //Wait 5 seconds to ensure user credentials are correct before Updating Username
    private IEnumerator WaitToUpdateUsername()
    {
        yield return new WaitForSecondsRealtime(5f);

        if (Network.sharedInstance.IsAuthenticated())
        {
            Network.sharedInstance.RequestUpdateUsername(newUsernameInputField.text, UpdateUsernameRequestCompleted, UpdateUsernameRequestFailed);
        }
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
        try
        {
            MainCanvas.gameObject.SetActive(true);
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public void HideMainCanvas()
    {
        try
        {
            MainCanvas.gameObject.SetActive(false);
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    public void ShowLoginCanvas()
	{
		try
		{
			LoginCanvas.gameObject.SetActive(true);
		}
		catch (System.Exception)
		{

			throw;
		}
	}

    public void HideLoginCanvas()
    {
        try
        {
            LoginCanvas.gameObject.SetActive(false);
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    public void ShowUpdateUsernameCanvas()
    {
        try
        {
            UpdateUsernameCanvas.gameObject.SetActive(true);
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    public void HideUpdateUsernameCanvas()
    {
        try
        {
            UpdateUsernameCanvas.gameObject.SetActive(false);
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    public void ShowControlsCanvas()
    {
        try
        {
            ControlsCanvas.gameObject.SetActive(true);
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public void HideControlsCanvas()
    {
        try
        {
            ControlsCanvas.gameObject.SetActive(false);
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    public void ShowLeaderboardCanvas()
    {
        try
        {
            LeaderboardCanvas.gameObject.SetActive(true);
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public void HideLeaderboardCanvas()
    {
        try
        {
            LeaderboardCanvas.gameObject.SetActive(false);
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    public void CloseGame()
    {
		try
		{
			Application.Quit();
		}
		catch (System.Exception)
		{

			throw;
		}
    }
}
