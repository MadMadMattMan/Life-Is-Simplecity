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
    public bool OrderComplete;
    public float timeElapsed;
    public Color Liquid { get; private set; }
    public Ingredient[] Contents { get; private set; }
    public float BrewTime { get; private set; }
    public float Tempreture { get; private set; }
    public void FixedUpdate()
    {
        if (OrderComplete) return;
        timeElapsed += Time.fixedDeltaTime;
    }
    public void InitilizeOrder(Color Liquid, Ingredient[] Contents, float BrewTime, float Tempreture)
    {
        this.Liquid = Liquid;
        this.Contents = Contents;
        this.BrewTime = BrewTime;
        this.Tempreture = Tempreture;
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
