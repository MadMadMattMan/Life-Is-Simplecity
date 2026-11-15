using Unity.VisualScripting;
using UnityEngine;

public class BrewButtonInteraction : MonoBehaviour, iItemInteraction {

    [SerializeField] BrewingManager manager;
    [SerializeField] AudioClip blow;
    AudioSource blowSource;

    void Awake()
    {
        blowSource = gameObject.AddComponent<AudioSource>();
    }


    public void click() {
        manager.heatFire();
        blowSource.PlayOneShot(blow, 0.4f);
    }

    public void release() { return; }

    public void drag(Vector3 pointingDir) { return; }
}
