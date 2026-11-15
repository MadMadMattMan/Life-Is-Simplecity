using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class StarManager : MonoBehaviour
{
    [Range(0, 5)] public float StarRating;
    public GameObject Stars;
    public float customersToFiveStars = 10f;
    public StarManager storeStars;
    GameManager gm;
    AudioSource audioSource;
    [SerializeField] AudioClip goodSFX;
    [SerializeField] AudioClip badSFX;

    void Awake() {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();
        if (gm == null)
            Debug.LogWarning("Failed to get GameManager for stars");

        if (storeStars == null)
            StarRating = gm.storeRating;

        DisplayStars();
    }

    public void DisplayStars() {
        Stars.GetComponent<Image>().fillAmount = Mathf.Lerp(0f, 1f, StarRating / 5);
    }

    public void CauldronOrderToStars(CauldronManager cauldron, Order order) {
        float stars = 0;
        // pour, 1.5 stars
        stars = PourStationRating(cauldron, order) * 1.5f;

        // ingredient, 1 star
        stars += IngredientStationRating(cauldron, order) * 1f;

        // brew, 2.5 stars
        stars += BrewingStationRating(cauldron, order) * 2.5f;

        // round to snap to nearest .5 star
        stars = Mathf.Round(stars * 2f) / 2f;

        UpdateStore(stars);
        UpdateStars(stars);
    }

    // returns the ranking of the pour station between 0 and 1
    float PourStationRating(CauldronManager cauldron, Order order) {
        float ranking = 1-(cauldron.colorAdditions - order.Liquid).magnitude;

        print("stars 1: | r: " + ranking + " | s: " + ranking * 1.5);
        return Mathf.Clamp01(ranking);
    }
    // returns the ranking of the pour station between 0 and 1
    float IngredientStationRating(CauldronManager cauldron, Order order) {
        float ranking = 1;
        List<Ingredient> ingredients = order.Contents.ToList();
        if (cauldron.addedIngredients.Equals(ingredients))
            return 1f;
        else {
            int amount = order.Contents.Length;
            foreach (Ingredient i in cauldron.addedIngredients) {
                if (ingredients.Contains(i))
                    ingredients.Remove(i);
                else
                    ranking -= 0.3f;
            }
            ranking -= ingredients.Count / amount;
        }

        print("stars 2: | r: " + ranking + " | s: " + ranking*1);
        return Mathf.Clamp01(ranking);
    }

    [SerializeField] float brewPunishment = 15f; // seconds before failed brew
    // returns the ranking of the pour station between 0 and 1
    float BrewingStationRating(CauldronManager cauldron, Order order) {
        float ranking = 1f;

        float oc  = Mathf.Abs(cauldron.brewAmount/ brewPunishment);  // overcooking time
        float ins = Mathf.Abs(cauldron.brewQuality/ brewPunishment); // inaccuracy score 
        ranking -= ins;
        ranking -= oc;

        print("Stars 3: oc=" + oc + " ins=" + ins);
        print("stars 3: | r: " + ranking + " | s: " + ranking * 2.5);
        return Mathf.Clamp01(ranking);
    }

    public void UpdateStars(float newStars) {
        StarRating = newStars;
        if (StarRating >= 3)
            audioSource.PlayOneShot(goodSFX);
        else
            audioSource.PlayOneShot(badSFX);
        DisplayStars();
    }

    public void UpdateStore(float newStars) {
        if (storeStars == null)
            return;

        // scale stars so after n 5 stars have store reach 5 stars
        float diff = (newStars - 3f)/(customersToFiveStars/2.5f);


        gm.storeRating += diff;
        storeStars.UpdateStars(gm.storeRating);
    }
}
