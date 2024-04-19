using System.Collections;
using TMPro;
using UnityEngine;

public class HintsCanvasController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtHint;

    private IEnumerator GetHint(string hint)
    {
        Network.sharedInstance.GetHints(hint);

        yield return new WaitForSeconds(2f);

        txtHint.text = Globals.hint;
    }

    public void GetHint1()
    {
        StartCoroutine(GetHint("1"));
    }

    public void GetHint2()
    {
        StartCoroutine(GetHint("2"));
    }

    public void GetHint3()
    {
        StartCoroutine(GetHint("3"));
    }

}
