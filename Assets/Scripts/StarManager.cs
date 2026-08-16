using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StarManager : MonoBehaviour
{
    [Range(0, 5)] public float StarRating;
    public GameObject Stars;
    public float customersToFiveStars = 15f;
    public StarManager storeStars;
    public GameManager gm;

    void FixedUpdate() {
        GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
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
        // pour, 1 star
        float diff = (cauldron.baseColor - order.Liquid).magnitude;
        print(diff);
        stars += 1 - diff;

        print("stars 1: " + stars);

        // ingredient, 1 star
        if (cauldron.addedIngredients.Equals(order.Contents.ToList()))
            stars += 1;
        else {
            float pain = 0f;
            for (int i = 0; i < cauldron.addedIngredients.Count; i++) {
                if (i > order.Contents.Length)
                    pain += 0.75f;
                else if (cauldron.addedIngredients[i] != order.Contents[i])
                    pain += 0.5f;
            }
            stars += Mathf.Clamp01(1 - pain);
        }
        print("stars 2: " + stars);


        // brew, 3 stars
        float penalty = 0f;
        penalty += cauldron.brewAmount;
        penalty += cauldron.brewQuality;
        penalty = Mathf.Clamp01(penalty / 5f); // 5 seconds of decline
        stars += -3*penalty + 3;
        

        print("stars 3:" + stars);
        UpdateStars(stars);
    }

    public void UpdateStars(float newStars) {
        StarRating = newStars;
        DisplayStars();
    }

    public void UpdateStore(float newStars) {
        if (storeStars == null)
            return;

        gm.storeRating += newStars / customersToFiveStars;
        storeStars.UpdateStars(gm.storeRating);
    }
}
