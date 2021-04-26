using DG.Tweening;
using SWS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeRoutePath : MonoBehaviour
{
    public List<PathManagerEdge> PathManagerEdges;

    [Range(0, 25.0f)]
    public float VehicleSpeed = 6f;
    
    [Range(0, 25.0f)]
    public float VehicleAcceleration = 0.75f;

    private bool _accelerating;
    private float _currentSpeed;

    bool _isPaused;

    TradeRoute _tradeRoute;
    GameObject _oilVehicle;

    PathManagerEdge _currentPathManagerEdge;

    splineMove _currentSplineMove;

    bool firstPathInitialized = false;
    bool reversed = false;

    // Start is called before the first frame update
    void Start()
    {
        _tradeRoute = transform.GetComponent<TradeRoute>();
        _tradeRoute.TradeRoutePath = this;
        _oilVehicle = _tradeRoute.OilVehicle;

        _currentPathManagerEdge = PathManagerEdges[0];
        _currentSpeed = 0.01f;
        _accelerating = true;
        _oilVehicle.SetActive(true);
        _isPaused = true;
        _oilVehicle.transform.position = _currentPathManagerEdge.PathManager.waypoints[_currentPathManagerEdge.EntryWaypoint.transform.GetSiblingIndex()].transform.position;
        _tradeRoute.BeginLoadingOil();
    }

    public void ResumePath()
    {
        if(!_isPaused)
        {
            return;
        }
        
        _isPaused = false;
        MoveToEndOfPathManagerSegment();
    }
    
    void Update()
    {
        if(_currentSplineMove == null)
        {
            return;
        }
        if(_accelerating)
        {
            _currentSpeed += (VehicleAcceleration * Time.deltaTime);
            if (_currentSpeed > VehicleSpeed)
            {
                _currentSpeed = VehicleSpeed;
                _accelerating = false;
            }
            _currentSplineMove.ChangeSpeed(_currentSpeed);
        }
    }

    void MoveToEndOfPathManagerSegment()
    {
        if (_currentSplineMove)
        {
            _currentSplineMove.Stop();
            Destroy(_currentSplineMove);
        }

        _currentSplineMove = _oilVehicle.AddComponent<splineMove>();
        _currentSplineMove.pathContainer = _currentPathManagerEdge.PathManager;
        _currentSplineMove.pathMode = DG.Tweening.PathMode.TopDown2D;
        _currentSplineMove.onStart = true;
        _currentSplineMove.moveToPath = false;
        _currentSplineMove.loopType = splineMove.LoopType.none;
        _currentSplineMove.speed = _currentSpeed;
        _currentSplineMove.easeType = DG.Tweening.Ease.Linear;
        
        var startIndex = _currentPathManagerEdge.EntryWaypoint.transform.GetSiblingIndex();
        var endIndex = _currentPathManagerEdge.ExitWaypoint.transform.GetSiblingIndex();
        if (reversed)
        {
            var temp = endIndex;
            endIndex = startIndex;
            startIndex = temp;
        }
        _currentSplineMove.reverse = startIndex > endIndex;
        var bezierPathManager = _currentPathManagerEdge.PathManager as BezierPathManager;
        var defaultPoints = 10;
        var startIndexMultiplier = (bezierPathManager != null ? Mathf.CeilToInt(bezierPathManager.pathDetail * defaultPoints) : 1);
        _currentSplineMove.startPoint = startIndex * startIndexMultiplier + (bezierPathManager != null ? 0 : 0);

        _oilVehicle.transform.position = _currentPathManagerEdge.PathManager.waypoints[startIndex].transform.position;

        StartCoroutine(WaitForSplineInitialization());
    }

    void PathManagerSegmentComplete()
    {
        //Output message to the console

        if ((!reversed && _currentPathManagerEdge.NextEdge == null) || (reversed && _currentPathManagerEdge.PreviousEdge == null))
        {
            reversed = !reversed;
            _currentSpeed = 0.01f;
            _accelerating = true;

            if(reversed)
            {
                _tradeRoute.BeginUnloadingOil();
            }
            else
            {
                _tradeRoute.BeginLoadingOil();
            }
            
            _isPaused = true;
            _currentSplineMove.Pause();
            return;
        }
        else
        {
            if (!reversed)
            {
                _currentPathManagerEdge = _currentPathManagerEdge.NextEdge;
            }
            else
            {
                _currentPathManagerEdge = _currentPathManagerEdge.PreviousEdge;
            }
        }
        MoveToEndOfPathManagerSegment();
    }
    
    public IEnumerator WaitForSplineInitialization()
    {
        var numberOfFrames = firstPathInitialized ? 1 : 2;
        while(numberOfFrames > 0)
        {
            yield return new WaitForEndOfFrame();
            numberOfFrames -= 1;
        }
        firstPathInitialized = true;
        var index = reversed ? _currentPathManagerEdge.EntryWaypoint.transform.GetSiblingIndex() : _currentPathManagerEdge.ExitWaypoint.transform.GetSiblingIndex();
        _currentSplineMove.events[index].AddListener(PathManagerSegmentComplete);

        yield return null;
    }
}
