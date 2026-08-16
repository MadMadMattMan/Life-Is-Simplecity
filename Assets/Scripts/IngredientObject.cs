using UnityEngine;

public class IngredientObject : MonoBehaviour {
    [SerializeField] float lowerY = -1f;
    public Ingredient type;
    [SerializeField] IngredientManager manager;

    private void Update() {
        if (transform.position.y <= lowerY)
            manager.RespawnObject(type);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Cauldron")) {
            manager.RespawnObject(type);
            other.GetComponentInParent<CauldronManager>().AddIngredient(type);
        }
    }
}
