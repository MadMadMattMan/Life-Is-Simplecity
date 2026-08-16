using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public GameObject popupPrefab;
    public void CreatePopup(string text)
    {
        GameObject popup = Instantiate(popupPrefab, popupPrefab.transform.position, popupPrefab.transform.rotation);
        popup.GetComponent<Popup>().QueuePopup(text);
    }
    public static PopupManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
}
