using UnityEngine;
using System.Collections.Generic;

public class CauldronManager : MonoBehaviour {

    public Vector3 baseColor = Vector3.one;
    public List<Ingredient> addedIngredients = new List<Ingredient>();


    public void SetColor(Vector3 c) {
        baseColor = c;
    }
    public void AddIngredient(Ingredient io) {
        addedIngredients.Add(io);
    }
}
