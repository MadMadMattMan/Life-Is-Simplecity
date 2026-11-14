using System;
using TMPro;
using UnityEngine;

public class BrewingManager : Interactable {

    [Header("Current Settings")]
    public Order attachedOrder;
    public float heatPercent = -1f;
    float lowerHeat, upperHeat;
    public float timerStart = -1f;
    public float brewTimeGrace = 5f;
    public float degradeGrace = 3f;
    public float tempFalloff = 15f;


    [Header("Tracking")]
    public float currentTemp = 0f;
    public float timerValue = -1f;
    public float degradeValue = 0f;
    public bool inZone = false;
    public bool belowZero;

    public bool doTimerCount = false;
    public CauldronManager attachedCauldron;


    [Header("Internal References")]
    public TextMeshPro TimerScreen;
    public Transform GreenBar;
    public Transform Indicator;


    [Header("Audio")]
    [SerializeField] AudioClip doneAlarm;
    [SerializeField] AudioSource brewingSound;
    [SerializeField] AudioClip burningSound;


    [Header("Testing")]
    public bool update = false;
    public bool heat = false;


    private void Awake() {
        UpdateStation();
    }

    private void Update() {
        if (update) {
            timerValue = timerStart;
            UpdateStation();
            update = false;
            doTimerCount = true;
        }
        if (heat) {
            heatFire();
            heat = false;
        }

        // Start count when in range
        if (currentTemp < upperHeat && currentTemp > lowerHeat && !doTimerCount && timerStart != -1) {
            brewingSound.Play();
            doTimerCount = true;
        }
            


        // Count timer down if active
        if (doTimerCount) {
            timerValue -= Time.deltaTime;
            UpdateScreen();

            // if out of range
            if (currentTemp > upperHeat || currentTemp < lowerHeat)
                    degradeValue += Time.deltaTime;

            // if done
            if (timerValue <= 0f && !belowZero) {
                belowZero = true;
                TimerHitZero();
            }
        }

        // cool flame
        if (currentTemp > 0) {
            currentTemp -= Time.deltaTime / tempFalloff;
            if (currentTemp < 0)
                currentTemp = 0;
            UpdateBar();
        }
    }

    public void heatFire() {
        currentTemp += 0.075f;
        UpdateBar();
    }

    public override void PlaceCauldron()
    {
        base.PlaceCauldron();
        try
        {
            GameObject t = GameObject.FindWithTag("MainTicket").GetComponent<TicketSlot>().currentTicket;
            Order o = t.GetComponent<Ticket>().order;
            addOrder(o, Cauldron.GetComponentInChildren<CauldronManager>());
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed to start brewing for " + e);
        }
    }

    public override void PickupCauldron()
    {
        base.PickupCauldron();
        removeOrder();
    }


    // called on place cauldron
    public void addOrder(Order newOrder, CauldronManager newCauldron) {
        attachedOrder = newOrder;
        heatPercent = attachedOrder.Tempreture;
        upperHeat = heatPercent + 0.1f;
        lowerHeat = heatPercent - 0.1f;
        doTimerCount = false;

        timerStart = attachedOrder.BrewTime;
        timerValue = timerStart;
        UpdateStation();

        attachedCauldron = newCauldron;
    }

    // called on remove cauldron
    public void removeOrder() {
        brewingSound.Stop();

        attachedOrder = null;
        heatPercent = -1;
        timerStart = -1;
        timerValue = -1;
        doTimerCount = false;
        UpdateStation();

        attachedCauldron.brewAmount = Mathf.Abs(timerValue) - brewTimeGrace;
        attachedCauldron.brewQuality = Mathf.Abs(degradeValue) - degradeGrace;

        attachedCauldron = null;
    }


    void TimerHitZero() {
        audioSource.PlayOneShot(doneAlarm);
    }

    public void UpdateStation() {
        UpdateScreen();
        UpdateTemp();
    }

    void UpdateScreen() {
        string text = ((int)timerValue).ToString();
        if (timerValue < 0)
            text = "--";
        TimerScreen.text = text + "s";
    }

    void UpdateTemp() {
        if (heatPercent < 0f || heatPercent > 1f)
            GreenBar.localPosition = new Vector3(0, Mathf.Lerp(0.03f, -0.03f, heatPercent), -100);
        else
            GreenBar.localPosition = new Vector3(0, Mathf.Lerp(0.03f, -0.03f, heatPercent), 0);
    }

    void UpdateBar() {
        Indicator.localPosition = new Vector3(0.02562021f, Mathf.Lerp(-0.01097f, 0.01236f, currentTemp), 0.0103f);
    }
}