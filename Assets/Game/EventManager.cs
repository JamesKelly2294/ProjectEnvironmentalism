using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    public DateTime prevTime;
    public AlertManager alertManager;
    public GameObject alertPrefab;

    public Sprite truck, ship, derrick, rig, fire, leaf, happy, mad, oilEmpty, oilFull, oilReserveEmpty, oilReserveFull;
    public AudioClip normalSound, angerSound;


    // Start is called before the first frame update
    void Start()
    {
        prevTime = GameTime.Instance.GameDate();
        alertManager = GameObject.FindObjectOfType<AlertManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SummonAngryMobNotification() {
        GameObject notification = alertManager.SummonNotification(alertPrefab, "Angry Mob", "You have angered the people.", mad, Color.white, true, 60, AngryMobWasDismissed, angerSound);
    }

    public void AngryMobWasDismissed(bool byUser) {
        Debug.Log(byUser);
    }

    public void SummonDryExtractorNotification(OilExtractor extractor) {
        AlertWasDismissedDelegate showMore = wasUser => {
            extractor.gameObject.GetComponent<SelectableSprite>().TriggerBoxSelect();
        };

        if (extractor.ExtractedOilType == OilSlickType.Land) {
            GameObject notification = alertManager.SummonNotification(alertPrefab, "A Derrick has run dry", "You must dig deeper to get more oil from this extractor.", derrick, Color.white, true, 20,showMore , normalSound);
        } else {
            GameObject notification = alertManager.SummonNotification(alertPrefab, "A Rig has run dry", "You must dig deeper to get more oil from this extractor.", rig, Color.white, true, 20, showMore, normalSound);
        }
    }









    public void TimeDidChange(PubSubListenerEvent e) {
        DateTime currentTime = (DateTime) e.value;

        if (currentTime.DayOfYear != prevTime.DayOfYear) {
            DayDidChange(currentTime);
        }

        if (currentTime.Month != prevTime.Month) {
            MonthDidChange(currentTime);
        }
        
        if (currentTime.Year != prevTime.Year) {
            YearDidChange(currentTime);
        }

        prevTime = currentTime;
    }

    public void DayDidChange(DateTime time) {

    }

    public void MonthDidChange(DateTime time) {

    }

    public void YearDidChange(DateTime time) {

    }
}
