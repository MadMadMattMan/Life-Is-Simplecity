using UnityEngine;

public class PourButtonInteraction : iItemInteraction {

    [SerializeField] PourManager manager;
    [Range(0, 3)][SerializeField] int colorInt = 0;

    public void click() {
        manager.AddColor(colorInt);
    }

    public void release() { return; }

    public void drag(Vector3 pointingDir) { return; }
}
