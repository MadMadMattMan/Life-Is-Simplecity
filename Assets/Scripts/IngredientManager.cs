using UnityEngine;

public class IngredientManager : Interactable {

    [SerializeField] GameObject[] ingredientSpawns;
    [SerializeField] GameObject[] ingredients;
    [SerializeField] AudioClip pop;
    AudioSource aSource;

    void Start(){
        aSource = GetComponent<AudioSource>(); 
        if (!aSource)
            aSource = gameObject.AddComponent<AudioSource>();
    }

    public void RespawnObject(Ingredient objectToRespawn) {
        int i = (int)objectToRespawn;
        ingredients[i].GetComponent<Rigidbody>().position = ingredientSpawns[i].transform.position;
        ingredients[i].GetComponent<Rigidbody>().rotation = ingredientSpawns[i].transform.rotation;
        ingredients[i].GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        aSource.PlayOneShot(pop, 0.025f);
    }
    public override void PlaceCauldron()
    {
        base.PlaceCauldron();
        Tutorial.InteractedWithIngredientCounter = true;
    }
}