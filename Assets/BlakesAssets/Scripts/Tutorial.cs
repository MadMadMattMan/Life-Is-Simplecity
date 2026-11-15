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
    public static bool TicketDragged;
    public static bool UsedFlame;
    public static bool DoneBrewing;
    public static bool InteractedWithIngredientCounter;
    public static bool UsedIngredients;
    public static bool InteractedWithPickupCounter;
    public static bool CompletedOrder;

    float delayTime = 2.75f;
    float longDelay = 3.75f;
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
                    yield return new WaitForSeconds(delayTime);
                    PopupManager.Instance.CreatePopup("Welcome to mama's brewery", delayTime);
                    yield return new WaitForSeconds(delayTime);
                    PopupManager.Instance.CreatePopup("Let's step you through how to play the game", delayTime);
                    yield return new WaitForSeconds(delayTime);
                    PopupManager.Instance.CreatePopup("First off, use 'e' to interact with workdesks", delayTime);
                    yield return new WaitForSeconds(delayTime);
                    PopupManager.Instance.CreatePopup("And 'f' to interact with cauldrons", delayTime);
                    yield return new WaitForSeconds(delayTime);
                    PopupManager.Instance.CreatePopup("Now, let's go grab an order!", delayTime);
                    yield return new WaitForSeconds(delayTime);
                    PopupManager.Instance.CreatePopup("Use e to interact with the order counter", delayTime, true);
                    while (!InteractedWithOrderCounter) yield return null;
                    PopupManager.Instance.CreatePopup("Take orders by clicking on the speech bubbles", delayTime, true);
                    while (!InteractedWithCustomer) yield return null;
                    PopupManager.Instance.CreatePopup("Great, you have your first order!", delayTime);
                    yield return new WaitForSeconds(delayTime);
                    PopupManager.Instance.CreatePopup("TIP: When using a workdesk, you can move tickets by dragging", delayTime);
                    yield return new WaitForSeconds(delayTime);
                    State = TutorialState.TakenCauldron;
                    break;
                case TutorialState.TakenCauldron:
                    PopupManager.Instance.CreatePopup("Now pick up a cauldron from the stack", delayTime, true);
                    while (!TakenCauldron) yield return null;
                    PopupManager.Instance.CreatePopup("Good, now you can start brewing!", delayTime);
                    yield return new WaitForSeconds(delayTime);
                    State = TutorialState.UsedBase;
                    break;
                case TutorialState.UsedBase:
                    PopupManager.Instance.CreatePopup("Go ahead and place the cauldron on a pour station", delayTime, true);
                    while (!PlacedCauldronOnPourStation) yield return null;
                    PopupManager.Instance.CreatePopup("Now, try and pour out the colour you see on the ticket", delayTime, true);
                    while (!UsedBase) yield return null;
                    PopupManager.Instance.CreatePopup("Great, you're a natural!", delayTime);
                    yield return new WaitForSeconds(delayTime);
                    State = TutorialState.UsedFlame;
                    break;
                case TutorialState.UsedFlame:
                    PopupManager.Instance.CreatePopup("Now you have to boil the liquid", delayTime);
                    yield return new WaitForSeconds(delayTime);
                    PopupManager.Instance.CreatePopup("Go to a brewing station with your cauldron", delayTime, true);
                    while (!PlacedCauldronOnBrewStation) yield return null;
                    PopupManager.Instance.CreatePopup("Drag your order ticket onto the brewing station to start", delayTime, true);
                    while (!TicketDragged) yield return null;
                    PopupManager.Instance.CreatePopup("Press the red button until the arrow is in the green range", delayTime, true);
                    while (!UsedFlame) yield return null; 
                    PopupManager.Instance.CreatePopup("You have to keep the arrow in the green range until the timer is up", delayTime, true);
                    while (!DoneBrewing) yield return null;
                    PopupManager.Instance.CreatePopup("This is starting to look good", delayTime);
                    State = TutorialState.UsedIngredients;
                    break;
                case TutorialState.UsedIngredients:
                    yield return new WaitForSeconds(delayTime);
                    PopupManager.Instance.CreatePopup("Now it's time for the most important part", delayTime);
                    yield return new WaitForSeconds(delayTime);
                    PopupManager.Instance.CreatePopup("Find an ingredient station and place your cauldron at it", delayTime, true);
                    while (!InteractedWithIngredientCounter) yield return null;
                    PopupManager.Instance.CreatePopup("Now just drag and drop in the ingredients to match the ticket", delayTime, true);
                    while (!UsedIngredients) yield return null;
                    PopupManager.Instance.CreatePopup("Boom, now all that's left to do is give it to a customer", delayTime);
                    yield return new WaitForSeconds(delayTime);
                    State = TutorialState.CompletedOrder;
                    break;
                case TutorialState.CompletedOrder:
                    PopupManager.Instance.CreatePopup("Go over to the pickup counter to give the customer your order", delayTime, true);
                    while (!InteractedWithPickupCounter) yield return null;
                    PopupManager.Instance.CreatePopup("Now drag your ticket onto the cauldron to complete the order", delayTime, true);
                    while (!CompletedOrder) yield return null;
                    PopupManager.Instance.CreatePopup("OH NO! A GHOST HAS CHANGED YOUR KEYBINDS!!!", longDelay);
                    yield return new WaitForSeconds(longDelay);
                    PopupManager.Instance.CreatePopup("Try and find your new keybinds after completing each customer", longDelay);
                    yield return new WaitForSeconds(longDelay);
                    PopupManager.Instance.CreatePopup("Well done, just a few more notes before you start though", longDelay);
                    State = TutorialState.FinishingNotes;
                    break;
            }
        }
        yield return new WaitForSeconds(longDelay);
        PopupManager.Instance.CreatePopup("Press 'Esc' for options at any time", longDelay);
        yield return new WaitForSeconds(longDelay);
        PopupManager.Instance.CreatePopup("If you feel possessed and begin floating, be sure to use the holy water button in the pause menu", delayTime);
        yield return new WaitForSeconds(longDelay);
        PopupManager.Instance.CreatePopup("Customers rate higher if you do a better job", longDelay);
        yield return new WaitForSeconds(longDelay);
        PopupManager.Instance.CreatePopup("The more store rating you have, the more customers you get", longDelay);
        yield return new WaitForSeconds(longDelay);
        PopupManager.Instance.CreatePopup("If you mess up an order you can always throw it in the trash and restart", longDelay);
        yield return new WaitForSeconds(longDelay);
        PopupManager.Instance.CreatePopup("You can refer to the posters on the walls if your stuck", longDelay);
        yield return new WaitForSeconds(longDelay);
        PopupManager.Instance.CreatePopup("You also don't have a wage or any breaks", longDelay);
        yield return new WaitForSeconds(longDelay);
        PopupManager.Instance.CreatePopup("Good luck!", longDelay);
        Destroy(gameObject);
    }
}