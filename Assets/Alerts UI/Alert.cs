using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Alert : MonoBehaviour, IPointerClickHandler
{
    
    public TextMeshProUGUI Title;

    public TextMeshProUGUI Description;

    public Image Image;

    public bool DoesAutoDismiss = true;
    public float AutoDismissTime = 5f;
    public float TimeLeft = -1f;
    public bool alive = true;
    public AlertWasDismissedDelegate wasDismissed;

    public AudioClip sound;

    // Start is called before the first frame update
    void Start()
    {
        TimeLeft = AutoDismissTime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData) {
        Dismiss(wasDismissedByUser: true);
    }

    public void Dismiss(bool wasDismissedByUser) {
        alive = false;
        if (wasDismissed != null) {
            wasDismissed(wasDismissedByUser);
        }
    }

    public void Configure(string title, string description, Sprite icon, Color iconTint, bool doesAutoDismiss, float autoDismissTime, AlertWasDismissedDelegate wasDismissed, AudioClip sound) {
        gameObject.name = title + " Alert";

        Title.SetText(title);
        Title.gameObject.SetActive(title.Length > 0);

        Description.SetText(description);
        Description.gameObject.SetActive(description.Length > 0);

        Image.sprite = icon;
        Image.color = iconTint;
        Image.gameObject.SetActive(icon != null);

        DoesAutoDismiss = doesAutoDismiss;
        AutoDismissTime = autoDismissTime;
        TimeLeft = AutoDismissTime;

        this.wasDismissed = wasDismissed;
        this.sound = sound;
    }
}

public delegate void AlertWasDismissedDelegate(bool wasDismissedByUser);