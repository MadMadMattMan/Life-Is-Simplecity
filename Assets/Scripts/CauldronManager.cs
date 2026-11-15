using UnityEngine;
using System.Collections.Generic;

public class CauldronManager : MonoBehaviour {


    // for comparison for grade
    public Vector3 colorAdditions = Vector3.zero;
    // for comparison for grade
    public List<Ingredient> addedIngredients = new List<Ingredient>();


    // how bad the brew was (higher number is bad)
    public float brewAmount = 0f;
    public float brewQuality = 1f;

    private void Start() {
        colorAdditions = Vector3.zero;
        addedIngredients.Clear();
        brewAmount = 0f;
        brewQuality = 1f;
    }

    public Vector3 GetAdditions() {
        float r = colorAdditions.x == 0 ? 0f : colorAdditions.x / GetColorSum();
        float g = colorAdditions.y == 0 ? 0f : colorAdditions.y / GetColorSum();
        float b = colorAdditions.z == 0 ? 0f : colorAdditions.z / GetColorSum();

        return new Vector3(r, g, b);
    }
    public float GetColorSum() {
        return (colorAdditions.x + colorAdditions.y + colorAdditions.z);
    }

    public void AddIngredient(Ingredient io) {
        addedIngredients.Add(io);
    }
}
