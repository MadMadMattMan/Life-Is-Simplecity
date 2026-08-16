using UnityEngine;

public class BrewButtonInteraction : MonoBehaviour, iItemInteraction {

    [SerializeField] BrewingManager manager;


    public void click() {
        manager.heatFire();
    }

    public void release() { return; }

    public void drag(Vector3 pointingDir) { return; }
}
