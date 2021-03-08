using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertWrapper : MonoBehaviour
{

    public GameObject Alert;
    public AlertManager AlertManager;

    public AlertWrapperState State = AlertWrapperState.starting;

    public float time = 0f;
    public float animateInTime = 0.5f;
    public AnimationCurve animateInCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    public float animateOutTime = 0.5f;
    public AnimationCurve animateOutCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    public float spacing = 5f;

    public float GetAlertHeight() {
        RectTransform alertRect = Alert.transform.GetComponent<RectTransform>();
        return alertRect.sizeDelta.y * alertRect.localScale.y;
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<LayoutElement>().preferredHeight = 0;
        GetComponent<CanvasGroup>().alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Alert alert = Alert.GetComponent<Alert>();
        if (State == AlertWrapperState.starting) {
            State = AlertWrapperState.animatingIn;

            if (alert.sound != null && AlertManager.AudioSource != null) {
                AlertManager.AudioSource.PlayOneShot(alert.sound);
            }

            // Never recurses more than once.
            Update();

        } else if (State == AlertWrapperState.animatingIn) {
            time += Time.deltaTime;
            time = Mathf.Max(time, 0.000001f);
            time = Mathf.Min(time, animateInTime);

            if (time == animateInTime) {
                State = AlertWrapperState.waiting;
                time = 0;

                float desiredHeight = GetAlertHeight();
                GetComponent<LayoutElement>().preferredHeight = desiredHeight + spacing;
                GetComponent<CanvasGroup>().alpha = 1;
            } else if (!alert.alive) {
                State = AlertWrapperState.animatingOut;
            } else {
                AnimateIn();
            }

        } else if (State == AlertWrapperState.waiting) {
            if (alert.DoesAutoDismiss) {
                alert.TimeLeft -= Time.deltaTime;
                alert.TimeLeft = Mathf.Max(alert.TimeLeft, 0f);
                if (alert.TimeLeft <= 0) {
                    alert.Dismiss(false);
                }
            }

            if (alert.alive == false) {
                State = AlertWrapperState.animatingOut;
                time = 0;
            }

        } else {
            time += Time.deltaTime;
            time = Mathf.Max(time, 0.000001f);
            time = Mathf.Min(time, animateOutTime);

            if (time >= animateOutTime) {
                Destroy(gameObject);
            } else {
                AnimateOut();
            }
        }
    }
 
    void AnimateIn() {
        float desiredHeight = GetAlertHeight() + spacing;
        float percent = animateInCurve.Evaluate(time / animateInTime);
        GetComponent<CanvasGroup>().alpha = percent;
        GetComponent<LayoutElement>().preferredHeight = ((1 - percent) * 40f) + desiredHeight;
    }

    void AnimateOut() {
        float desiredHeight = GetAlertHeight() + spacing;
        float percent = animateOutCurve.Evaluate(1f - (time / animateOutTime));
        GetComponent<LayoutElement>().preferredHeight = percent * desiredHeight;
        GetComponent<CanvasGroup>().alpha = percent;
    }
}

public enum AlertWrapperState {
    starting,
    animatingIn,
    waiting,
    animatingOut
}