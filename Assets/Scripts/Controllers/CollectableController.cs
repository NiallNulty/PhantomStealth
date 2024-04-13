using UnityEngine;

public class ColectableController : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    private void Update()
    {
        this.transform.Rotate(0, 0, -1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HideCollectable();
    }

    private void HideCollectable()
    {
        try
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;

            foreach (Transform t in this.gameObject.transform)
            {
                t.gameObject.SetActive(false);
            }

            gameManager.collactableFound = true;
            gameManager.ShowCollectableNotification();
        }
        catch (System.Exception)
        {
            throw;
        }
    }
}
