using BrainCloud.LitJson;
using System.Collections;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameManager gameManager;

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
            player.GetComponent<PlayerController>().SetPlayerPathString();
        }
    }
}
