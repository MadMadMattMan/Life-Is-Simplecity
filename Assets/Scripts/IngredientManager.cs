using System;
using UnityEngine;

public class IngredientManager : MonoBehaviour {

    [SerializeField] float spawnDelay = 1f;
    [SerializeField] GameObject[] ingredientSpawns;
    [SerializeField] GameObject[] ingredients;


    public void RespawnObject(Ingredient objectToRespawn) {
        int i = (int)objectToRespawn;
        ingredients[i].GetComponent<Rigidbody>().position = ingredientSpawns[i].transform.position;
        ingredients[i].GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
    }
    
}

