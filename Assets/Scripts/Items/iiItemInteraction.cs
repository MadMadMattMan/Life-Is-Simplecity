using UnityEngine;

public interface iItemInteraction {
    public void click(); // on raycast hit
    public void release(); // on raycast release
    public void drag(Vector3 pointingDir); // raycast hit and mouse move
}