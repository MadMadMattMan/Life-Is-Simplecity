using UnityEngine;
using System.Collections.Generic;
public enum Ingredient 
{
    CrowFeather,
    GrowMushroom,
    Eyeball,
    MoonFlower, 
    ToenailClippings,
    Count
}

public class Order : MonoBehaviour
{
    public static int OrderCounter = 0;
    public int OrderNumber = 0;
    public bool OrderComplete;
    public float timeElapsed;
    public int numberOfItems;
    public Color32 Liquid;
    public Ingredient[] Contents;
    public float BrewTime;
    public float Tempreture;
    public void FixedUpdate()
    {
        if (OrderComplete) return;
        timeElapsed += Time.fixedDeltaTime;
    }
    public void InitilizeOrder(Color32 Liquid, Ingredient[] Contents, float BrewTime, float Tempreture)
    {
        OrderCounter++;
        OrderNumber = OrderCounter;
        numberOfItems = 3 + Contents.Length;
        this.Liquid = Liquid;
        this.Contents = Contents;
        this.BrewTime = BrewTime;
        this.Tempreture = Tempreture;
        GameManager.Instance.activeOrders.Add(this);
        //printOrder();
    }
    public void printOrder()
    {
        List<string> temp = new List<string>();
        int i = 0;
        foreach (Ingredient ingredient in Contents)
        {
            temp.Add("Ingredient " + ++i + ": " + ingredient.ToString());
        }
        Debug.Log("=============Order Debug==============" + "\n" +
            "Completed: " + OrderComplete + "\n" +
            "Time elapsed since NPC spawn: " + timeElapsed + "\n" +
            "Colour: r:" + Liquid.r + " g: " + Liquid.g + " b: " + Liquid.b + "\n" +
            "Ingredients:\n" + string.Join("\n", temp) + "\n" +
            "Brew Time: " + BrewTime + "\n" +
            "Tempreture: " + Tempreture + "\n" +
            "===========Order Debug End============"
        );
    }
}
