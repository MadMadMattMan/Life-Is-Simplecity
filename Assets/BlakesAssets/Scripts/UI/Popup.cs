using UnityEngine;
using TMPro;
using System.Collections;
public class Popup : MonoBehaviour
{
    [SerializeField] Animator ani;

    public void QueuePopup(string text, float time = 4, bool persist=false)
    {
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = text;
        if (persist == false) {
            Destroy(gameObject, time);
            StartCoroutine(PopupAnim(time));
        }
    }

    IEnumerator PopupAnim(float time) {
        yield return new WaitForSeconds(time - 0.5f);
        ani.SetTrigger("down");
    }


}
