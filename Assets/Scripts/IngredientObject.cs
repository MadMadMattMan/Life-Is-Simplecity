using UnityEngine;

public class IngredientObject : MonoBehaviour {
    [SerializeField] float lowerY = -1f;
    public Ingredient type;
    [SerializeField] IngredientManager manager;
    AudioSource splashAudio;
    [SerializeField] AudioClip[] splashClips;
    int lasti = -1;


    private void Start() {
        splashAudio = GetComponent<AudioSource>();
    }

    private void Update() {
        if (transform.localPosition.y <= lowerY)
            manager.RespawnObject(type);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Cauldron")) {
            manager.RespawnObject(type);
            other.GetComponentInParent<CauldronManager>().AddIngredient(type);

            int i = -1;
            while (i == -1 || i == lasti)
                i = Random.Range(0, splashClips.Length);
             
            AudioClip clip = splashClips[i];
            splashAudio.PlayOneShot(clip);
            Tutorial.UsedIngredients = true;
        }
    }
}
