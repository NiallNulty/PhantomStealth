using System.Collections;
using UnityEngine;

public class AuthenticatingFailedCanvasController : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(DisableSelf());
    }

    private IEnumerator DisableSelf()
    {
        yield return new WaitForSeconds(3f);
        this.gameObject.SetActive(false);
    }
}
