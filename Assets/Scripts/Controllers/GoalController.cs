using BrainCloud.LitJson;
using System.Collections;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private string playerPathString = string.Empty;

    private UserData userData;

    public string entityId = string.Empty;



    [SerializeField]
    private GameObject btnStorePath;

    [SerializeField]
    private GameObject btnRegister;

    [SerializeField]
    private GameObject loginCanvas;

    private void Start()
    {
        if (!Globals.isAnonymous)
        {
            //Network.sharedInstance.RequestUserEntityData();
            StartCoroutine(WaitToGetEntity());

        }
        else
        {
            Globals.isNewUser = true;
        }

        if (string.IsNullOrEmpty(Globals.EntityID))
        {
            //Network.sharedInstance.CreateUserEntityData();
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

    public void EnableLoginCanvas()
    {
        loginCanvas.SetActive(true);
    }

    public void DisableLoginCanvas()
    {
        loginCanvas.SetActive(false);
        Network.sharedInstance.CreateUserEntityData();
        StartCoroutine(WaitToGetEntity());
        btnRegister.SetActive(false);
    }

    private IEnumerator WaitToGetEntity()
    {
        yield return new WaitForSeconds(1f);
        Network.sharedInstance.RequestUserEntityData();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameManager.gameState == GameManager.GameState.LevelComplete)
        {
            return;
        }

        if (collision.gameObject == player)
        {
            gameManager.ShowLevelCompleteScreen();
            player.GetComponent<BoxCollider2D>().enabled = false;
            player.GetComponent<PlayerController>().isFinished = true;
            string jsonData = JsonUtility.ToJson(player.GetComponent<PlayerController>().playerPath).ToString();
            playerPathString = jsonData;
        }
    }

    public UserData GetUserData()
    {
        return userData;
    }

    public void PostPathToUserEntity()
    {
        Network.sharedInstance.UpdateUserEntityData(Globals.EntityID, "userProgress", playerPathString);
    }

    private void OnUserEntityDataRequestCompleted(UserData data)
    {
        if (data != null)
        {
            userData = data;
        }
        else
        {
            Network.sharedInstance.CreateUserEntityData();
        }
    }

}
