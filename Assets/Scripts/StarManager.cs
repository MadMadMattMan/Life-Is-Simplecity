using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StarManager : MonoBehaviour
{
    [Range(0, 5)] public float StarRating;
    public GameObject Stars;


    private void FixedUpdate() {
        DisplayStars();
    }

    public void DisplayStars() {
        Stars.GetComponent<Image>().fillAmount = Mathf.Lerp(0.05f, 0.95f, StarRating / 5);
    }

    public void CauldronOrderToStars(CauldronManager cauldron, Order order) {
        float stars = 0;
        // pour, 1 star
        //cauldron.baseColor;
        //order.Liquid;

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

        // brew, 3 stars
        float penalty = 0f;
        penalty += cauldron.brewAmount;
        penalty += cauldron.brewQuality;
        penalty = Mathf.Clamp01(penalty / 5f); // 5 seconds of decline
        stars += -3*penalty + 3;
        

        print(stars);
        UpdateStars(stars);
    }

    public void UpdateStars(float newStars) {
        StarRating = newStars;
        DisplayStars();
    }


}
