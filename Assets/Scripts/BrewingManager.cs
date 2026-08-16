using System.Collections;
using TMPro;
using UnityEngine;

public class BrewingManager : MonoBehaviour {

    [Header("Current Settings")]
    public Order attachedOrder;
    public float heatPercent = -1f;
    public float timerStart = -1f;
    public float brewTimeGrace = 3f;
    public float degradeGrace = 5f;


    [Header("Tracking")]
    public float currentTemp = 0f;
    public float timerValue = -1f;
    public float degradeValue = -1f;
    public bool inZone = false;
    public bool belowZero;

    public bool doTimerCount = false;
    public CauldronManager attachedCauldron;


    [Header("Internal References")]
    public TextMeshPro TimerScreen;
    public Transform GreenBar;


    [Header("Testing")]
    public bool u = false;


    private void Awake() {
        UpdateStation();
    }

    private void Update() {
        if (u) {
            timerValue = timerStart;
            UpdateStation();
            u = false;
            doTimerCount = true;
        }

        if (doTimerCount) {
            timerValue -= Time.deltaTime;
            if (currentTemp > upperHeat || currentTemp < lowerTemp)

            UpdateScreen();
            if (timerValue <= 0f && !belowZero) {
                belowZero = true;
                TimerHitZero();
            }
        }
    }

    public IEnumerator heatFire() {
        float f = 0;
        while (f < 1) {
            currentTemp .
            yield return new WaitForEndOfFrame();
            f += Time.deltaTime;
        }

        
    }

    // called on place cauldron
    public void addOrder(Order newOrder, CauldronManager newCauldron) {
        attachedOrder = newOrder;
        heatPercent = attachedOrder.Tempreture;
        timerStart = attachedOrder.BrewTime;
        timerValue = timerStart;
        UpdateStation();

        attachedCauldron = newCauldron;
    }

    // called on remove cauldron
    public void removeOrder() {
        attachedOrder = null;
        heatPercent = -1;
        timerStart = -1;
        timerValue = -1;
        UpdateStation();

        attachedCauldron.brewAmount = Mathf.Abs(timerValue) - brewTimeGrace;
        attachedCauldron.brewQuality = Mathf.Abs(degradeValue) - degradeGrace;

        attachedCauldron = null;
    }


    void TimerHitZero() {
        print("Hit Zero");
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

}