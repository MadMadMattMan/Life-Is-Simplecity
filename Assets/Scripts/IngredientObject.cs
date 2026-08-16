using UnityEngine;

public class IngredientObject : MonoBehaviour {
    [SerializeField] float lowerY = -1f;
    [SerializeField] Ingredient type;
    [SerializeField] IngredientManager manager;

    private void Update() {
        if (transform.position.y <= lowerY)
            manager.RespawnObject(type);

    }
}
