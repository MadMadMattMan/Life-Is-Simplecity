using UnityEngine;

public class IngredientManager : Interactable {

    [SerializeField] GameObject[] ingredientSpawns;
    [SerializeField] GameObject[] ingredients;
    [SerializeField] AudioClip pop;
    AudioSource audio;

    void Start(){
        audio = GetComponent<AudioSource>();    
    }

    public void RespawnObject(Ingredient objectToRespawn) {
        int i = (int)objectToRespawn;
        ingredients[i].GetComponent<Rigidbody>().position = ingredientSpawns[i].transform.position;
        ingredients[i].GetComponent<Rigidbody>().rotation = ingredientSpawns[i].transform.rotation;
        ingredients[i].GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        audio.PlayOneShot(pop, 0.025f);
    }
    public override void PlaceCauldron()
    {
        base.PlaceCauldron();
        Tutorial.InteractedWithIngredientCounter = true;
    }
}