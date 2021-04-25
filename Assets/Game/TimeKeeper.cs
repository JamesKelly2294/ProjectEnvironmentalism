using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class GameTime
{
    private static GameTime _instance;
    public static GameTime Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }
            else
            {
                _instance = new GameTime();
                return _instance;
            }
        }
    }

    float _elapsedTime;
    DateTime _startDate = new DateTime(2021, 4, 26, 12, 00, 0);

    private GameTime() { }

    public void SetElapsedTime(float elapsedTime)
    {
        _elapsedTime = elapsedTime;
    }

    public DateTime GameDate()
    {
        DateTime currentDate = _startDate.AddSeconds(_elapsedTime * 1752222.22222);
        return currentDate;
    }
}

[RequireComponent(typeof(PubSubSender))]
public class TimeKeeper : MonoBehaviour
{
    float elapsedTime;

    PubSubSender _sender;
    private void Awake()
    {
        _sender = GetComponent<PubSubSender>();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        GameTime.Instance.SetElapsedTime(elapsedTime);
        _sender.Publish("game.date", GameTime.Instance.GameDate());
    }
}
