using UnityEngine;
using UnityEngine.Events;
using System.Collections;
public class Tutorial : MonoBehaviour
{
    public TutorialState State;
    public enum TutorialState
    {
        Started,
        TakenOrder,
        TakenCauldron,
        UsedBase,
        UsedFlame,
        UsedIngredients,
        CompletedOrder,
        FinishingNotes
    }
    public static bool InteractedWithOrderCounter;
    public static bool InteractedWithCustomer;
    public static bool TakenCauldron;
    public static bool PlacedCauldronOnPourStation;
    public static bool UsedBase;
    public static bool PlacedCauldronOnBrewStation;
    public static bool UsedFlame;
    public static bool InteractedWithIngredientCounter;
    public static bool UsedIngredients;
    public static bool InteractedWithPickupCounter;
    public static bool CompletedOrder;
    void Start()
    {
        StartCoroutine(nameof(TutorialSequence));
    }
    IEnumerator TutorialSequence()
    {
        while (State != TutorialState.FinishingNotes)
        {
            switch (State)
            {
                case TutorialState.Started:
                    yield return new WaitForSeconds(1);
                    PopupManager.Instance.CreatePopup("Welcome to mama's brewery");
                    yield return new WaitForSeconds(4.25f);
                    PopupManager.Instance.CreatePopup("Let's step you through how to play the game");
                    yield return new WaitForSeconds(4.25f);
                    PopupManager.Instance.CreatePopup("First off, use 'e' to interact with workdesks");
                    yield return new WaitForSeconds(4.25f);
                    PopupManager.Instance.CreatePopup("And 'f' to interact with cauldrons");
                    yield return new WaitForSeconds(4.25f);
                    PopupManager.Instance.CreatePopup("Now, let's go grab an order!");
                    yield return new WaitForSeconds(4.25f);
                    State = TutorialState.TakenOrder;
                    break;
                case TutorialState.TakenOrder:
                    yield return new WaitForSeconds(1);
                    PopupManager.Instance.CreatePopup("Interact with the order counter");
                    while (!InteractedWithOrderCounter) yield return null;
                    PopupManager.Instance.CreatePopup("Take orders by clicking on the speech bubbles");
                    while (!InteractedWithCustomer) yield return null;
                    PopupManager.Instance.CreatePopup("Great, you have your first order!");
                    yield return new WaitForSeconds(4.25f);
                    PopupManager.Instance.CreatePopup("TIP: When using a workdesk, you can move tickets by dragging");
                    yield return new WaitForSeconds(4.25f);
                    State = TutorialState.TakenCauldron;
                    break;
                case TutorialState.TakenCauldron:
                    yield return new WaitForSeconds(1);
                    PopupManager.Instance.CreatePopup("Now pick up a cauldron from the stack");
                    while (!TakenCauldron) yield return null;
                    PopupManager.Instance.CreatePopup("Good, now you can start brewing!");
                    yield return new WaitForSeconds(4.25f);
                    State = TutorialState.UsedBase;
                    break;
                case TutorialState.UsedBase:
                    yield return new WaitForSeconds(1);
                    PopupManager.Instance.CreatePopup("Go ahead and place the cauldron on a pour station");
                    while (!PlacedCauldronOnPourStation) yield return null;
                    PopupManager.Instance.CreatePopup("Now, try and pour out the colour you see on the ticket");
                    while (!UsedBase) yield return null;
                    PopupManager.Instance.CreatePopup("Great, you're a natural!");
                    yield return new WaitForSeconds(4.25f);
                    State = TutorialState.UsedFlame;
                    break;
                case TutorialState.UsedFlame:
                    yield return new WaitForSeconds(1);
                    PopupManager.Instance.CreatePopup("Now you have to boil the liquid");
                    yield return new WaitForSeconds(4.25f);
                    PopupManager.Instance.CreatePopup("Go to a brewing station with your cauldron");
                    while (!PlacedCauldronOnBrewStation) yield return null;
                    PopupManager.Instance.CreatePopup("Press the red button until the arrow is in the green range");
                    yield return new WaitForSeconds(4.25f);
                    PopupManager.Instance.CreatePopup("You have to keep the arrow in the green range until the timer is up");
                    while (!UsedFlame) yield return null;
                    PopupManager.Instance.CreatePopup("This is starting to look good");
                    State = TutorialState.UsedIngredients;
                    break;
                case TutorialState.UsedIngredients:
                    yield return new WaitForSeconds(4.25f);
                    PopupManager.Instance.CreatePopup("Now it's time for the most important part");
                    yield return new WaitForSeconds(4.25f);
                    PopupManager.Instance.CreatePopup("Find an ingredient station and place your cauldron at it");
                    while (!InteractedWithIngredientCounter) yield return null;
                    PopupManager.Instance.CreatePopup("Now just drop in the ingredients on the ticket");
                    while (!UsedIngredients) yield return null;
                    PopupManager.Instance.CreatePopup("Boom, now all that's left to do is give it to a customer");
                    yield return new WaitForSeconds(4.25f);
                    State = TutorialState.CompletedOrder;
                    break;
                case TutorialState.CompletedOrder:
                    yield return new WaitForSeconds(1);
                    PopupManager.Instance.CreatePopup("Go over to the pickup counter to give the customer your order");
                    while (!InteractedWithPickupCounter) yield return null;
                    PopupManager.Instance.CreatePopup("Now drag your ticket onto the cauldron to complete the order");
                    while (!CompletedOrder) yield return null;
                    PopupManager.Instance.CreatePopup("Well done, a few more notes before you start though");
                    yield return new WaitForSeconds(4.25f);
                    State = TutorialState.FinishingNotes;
                    break;
            }
        }
        yield return new WaitForSeconds(1);
        PopupManager.Instance.CreatePopup("Press 'Esc' for options at any time");
        yield return new WaitForSeconds(4.25f);
        PopupManager.Instance.CreatePopup("Customers rate higher if you do a better job");
        yield return new WaitForSeconds(4.25f);
        PopupManager.Instance.CreatePopup("The more store rating you have, the more customers you get");
        yield return new WaitForSeconds(4.25f);
        PopupManager.Instance.CreatePopup("If you mess up an order you can throw it in the trash");
        yield return new WaitForSeconds(4.25f);
        PopupManager.Instance.CreatePopup("You also don't have a wage or breaks. Good luck!");
        Destroy(gameObject);
    }
}
