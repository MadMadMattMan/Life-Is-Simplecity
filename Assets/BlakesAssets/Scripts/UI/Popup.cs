using UnityEngine;
using TMPro;
public class Popup : MonoBehaviour
{
    public void QueuePopup(string text)
    {
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = text;
        Destroy(gameObject, 5f);
    }
}
