using System.Collections;
using TMPro;
using UnityEngine;

public class LevelCanvasController : MonoBehaviour
{
    [SerializeField] private GameObject player;

    [SerializeField] private GameObject RegisterCanvas;

    [SerializeField] private TMP_InputField usernameInputField;

    [SerializeField] private TMP_InputField passwordInputField;

    [SerializeField] private GameObject btnStorePath;

    [SerializeField] private GameObject btnRegister;

    [SerializeField] private GameObject AuthenticatingCanvas;

    [SerializeField] private GameObject AuthenticatingFailedCanvas;

    [SerializeField] private GameObject gameManager;

    // Start is called before the first frame update
    void Start()
    {
        if (!Globals.isAnonymous)
        {
            StartCoroutine(WaitToGetEntity());
        }
        else
        {
            Globals.isNewUser = true;
        }

        if (Globals.isAnonymous)
        {
            btnRegister.SetActive(true);
        }
        else
        {
            btnStorePath.SetActive(true);
        }
    }


    public void EnableRegisterCanvas()
    {
        RegisterCanvas.SetActive(true);
    }

    public void DisableRegisterCanvas()
    {
        RegisterCanvas.SetActive(false);
        Network.sharedInstance.CreateUserEntityData();
        StartCoroutine(WaitToGetEntity());

        if (Globals.isAnonymous)
        {
            btnRegister.SetActive(true);
        }
        else
        {
            btnRegister.SetActive(false);
        }
    }

    private IEnumerator WaitToGetEntity()
    {
        yield return new WaitForSeconds(1f);
        Network.sharedInstance.RequestUserEntityData();
    }

    public void PostPathToUserEntity()
    {
        Network.sharedInstance.UpdateUserEntityData(Globals.EntityID, "userProgress", player.GetComponent<PlayerController>().playerPathString);
        StartCoroutine(WaitToShowAuthenticationFailed());
    }

    public void RegisterUser()
    {
        Network.AuthenticationRequestCompleted authenticationRequestCompleted = null;
        Network.AuthenticationRequestFailed authenticationRequestFailed = null;
        Network.sharedInstance.RequestAuthenticationUniversal(usernameInputField.text, passwordInputField.text, true, authenticationRequestCompleted, authenticationRequestFailed);
        StartCoroutine(WaitToShowAuthenticationFailed());
    }

    private IEnumerator WaitToShowAuthenticationFailed()
    {
        AuthenticatingCanvas.SetActive(true);

        yield return new WaitForSeconds(3f);

        if (Globals.isAnonymous)
        {
            AuthenticatingFailedCanvas.SetActive(true);
            btnRegister.SetActive(true);
        }
        else
        {
            Network.PostScoreRequestCompleted postScoreRequestCompleted = null;
            Network.PostScoreRequestFailed postScoreRequestFailed = null;
            Network.sharedInstance.PostScoreToLeaderboard("Main", gameManager.GetComponent<GameManager>().totalPoints, usernameInputField.text, postScoreRequestCompleted, postScoreRequestFailed);
            DisableRegisterCanvas();
        }

        AuthenticatingCanvas.SetActive(false);
    }
}
