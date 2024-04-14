using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
	[SerializeField]
	private GameObject MainCanvas;

    [SerializeField]
    private GameObject LoginCanvas;

    [SerializeField]
    private GameObject RegisterCanvas;

    [SerializeField]
    private GameObject ControlsCanvas;

    [SerializeField]
    private GameObject LeaderboardCanvas;

    private void Start()
    {
        if (Network.sharedInstance.IsAuthenticated())
        {
            Network.sharedInstance.LogOut();
        }
    }

    public void HandleAuthentication()
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

    public void ShowRegisterCanvas()
    {
        try
        {
            RegisterCanvas.gameObject.SetActive(true);
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    public void HideRegisterCanvas()
    {
        try
        {
            RegisterCanvas.gameObject.SetActive(false);
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
