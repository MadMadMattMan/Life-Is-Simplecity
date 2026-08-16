using UnityEngine;
using System.Collections.Generic;

public class CauldronManager : MonoBehaviour {


    // for comparison for grade
    public Vector3 baseColor = Vector3.one;


    // for comparison for grade
    public List<Ingredient> addedIngredients = new List<Ingredient>();


    // how bad the brew was (higher number is bad)
    public float brewAmount = 0f;
    public float brewQuality = 1f;


    public void SetColor(Vector3 c) {
        baseColor = c;
    }
    public void AddIngredient(Ingredient io) {
        addedIngredients.Add(io);
    }
}
