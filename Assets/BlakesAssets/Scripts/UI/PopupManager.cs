using UnityEngine;
public class PopupManager : MonoBehaviour
{
    public GameObject popupPrefab;
    private GameObject currentPopup;
    public void CreatePopup(string text, float time=4, bool persist=false)
    {
        if (currentPopup != null) { Destroy(currentPopup); }
        GameObject popup = Instantiate(popupPrefab, popupPrefab.transform.position, popupPrefab.transform.rotation);
        currentPopup = popup;
        popup.GetComponent<Popup>().QueuePopup(text, time, persist);
    }
    public static PopupManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
}