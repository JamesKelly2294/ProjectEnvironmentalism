using SWS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
    Waypoint,
    City,
    OilSlick
}

[Serializable]
public class Node
{
    public NodeType Type;
    public GameObject Data;
    public GameObject Waypoint;

    [NonSerialized]
    public List<Node> Neighbors;

    [NonSerialized]
    public bool isLoop = false;

    public PathManager PathManager
    {
        get
        {
            return Waypoint.transform.parent.GetComponent<PathManager>();
        }
    }

    public override string ToString()
    {
        return Type + " Node at " + Waypoint + ", Data = " + Data + ", " + Neighbors.Count + " Neighbors";
    }

    //public override int GetHashCode()
    //{
    //    return Type.GetHashCode() +
    //        (Data != null ? Data.GetHashCode() : 0) +
    //        (Waypoint != null ? Waypoint.GetHashCode() : 0);
    //}

    //public override bool Equals(object obj)
    //{
    //    //Check for null and compare run-time types.
    //    if ((obj == null) || !this.GetType().Equals(obj.GetType()))
    //    {
    //        return false;
    //    }
    //    else
    //    {
    //        Node n = (Node)obj;
    //        return (Type == n.Type) && (Data = n.Data) && (Waypoint == n.Waypoint);
    //    }
    //}
}

[Serializable]
public struct GraphEdge
{
    public GameObject FirstWaypoint;
    public GameObject SecondWaypoint;
}

public class PathManagerEdge
{
    public PathManager PathManager;
    public GameObject EntryWaypoint;
    public GameObject ExitWaypoint;

    public PathManagerEdge PreviousEdge;
    public PathManagerEdge NextEdge;

    public override string ToString()
    {
        return "(" + PathManager + ") From " + EntryWaypoint + " to " + ExitWaypoint;
    }
}

public class RoadManager : MonoBehaviour
{
    public WaypointManager WaypointManager;

    public List<Node> SupplementalNodes;
    public List<GraphEdge> SupplementalGraphEdges;

    private Dictionary<City, Node> _cityNodes = new Dictionary<City, Node>();
    private Dictionary<OilSlick, Node> _oilSlickNodes = new Dictionary<OilSlick, Node>();

    // Start is called before the first frame update
    void Start()
    {
        List<PathManager> pathManagers = new List<PathManager>();
        // First, grab all of the path managers we want to know about
        for (int i = 0; i < WaypointManager.transform.childCount; i++)
        {
            var child = WaypointManager.transform.GetChild(i);
            var pathManager = child.GetComponent<PathManager>();
            if (pathManager != null)
            {
                pathManagers.Add(pathManager);
            }
        }
        
        Dictionary<GameObject, Node> waypointsToNodes = new Dictionary<GameObject, Node>();
        // Next, build the nodes
        foreach(var pathManager in pathManagers)
        {
            Debug.Log("Processing path manager " + pathManager);
            for (int i = 0; i < pathManager.transform.childCount; i++)
            {
                var waypoint = pathManager.transform.GetChild(i);
                waypointsToNodes[waypoint.gameObject] = new Node()
                {
                    Type = NodeType.Waypoint,
                    Data = null,
                    Waypoint = waypoint.gameObject,
                    Neighbors = new List<Node>()
                };
            }
        }

        // Then, build up the neighbors
        foreach (var pathManager in pathManagers)
        {
            for (int i = 0; i < pathManager.transform.childCount; i++)
            {
                var waypoint = pathManager.transform.GetChild(i);
                var neighbors = waypointsToNodes[waypoint.gameObject].Neighbors;
                if (i > 0)
                {
                    neighbors.Add(waypointsToNodes[pathManager.transform.GetChild(i - 1).gameObject]);
                }
                if (i < pathManager.transform.childCount - 1)
                {
                    neighbors.Add(waypointsToNodes[pathManager.transform.GetChild(i + 1).gameObject]);
                }
            }
        }

        // Then, patch in any supplemental nodes
        foreach (var supplementalNode in SupplementalNodes)
        {
            var existingNode = waypointsToNodes[supplementalNode.Waypoint];
            existingNode.Type = supplementalNode.Type;
            existingNode.Data = supplementalNode.Data;

            if (supplementalNode.Type == NodeType.City)
            {
                var city = existingNode.Data.GetComponent<City>();
                _cityNodes[city] = existingNode;
            }
            else if (supplementalNode.Type == NodeType.OilSlick)
            {
                var oilSlick = existingNode.Data.GetComponent<OilSlick>();
                _oilSlickNodes[oilSlick] = existingNode;
            }
            Debug.Log("Patched node: " + waypointsToNodes[supplementalNode.Waypoint]);
        }

        // Finally, patch in any supplemental edges
        foreach (var supplementalEdge in SupplementalGraphEdges)
        {
            var firstNode = waypointsToNodes[supplementalEdge.FirstWaypoint];
            var secondNode = waypointsToNodes[supplementalEdge.SecondWaypoint];

            firstNode.Neighbors.Add(secondNode);
            secondNode.Neighbors.Add(firstNode);
            Debug.Log("Patched node 1: " + waypointsToNodes[supplementalEdge.FirstWaypoint]);
            Debug.Log("Patched node 2: " + waypointsToNodes[supplementalEdge.SecondWaypoint]);

            // this is a fucking hack
            if (firstNode.PathManager == secondNode.PathManager)
            {
                firstNode.isLoop = true;
                secondNode.isLoop = true;
            }
        }
    }

    public bool TestFindPath(City city, OilSlick oilSlick)
    {
        return false;
    }

    public void GenerateTradeRoutePath(TradeRoute tradeRoute)
    {
        var city = tradeRoute.City;
        var oilSlick = tradeRoute.OilExtractor.ExtractedOilSlick;
        Debug.Log("(" + gameObject.tag + ") Find path between " + city + " <-> " + oilSlick);

        Node startNode = _cityNodes[city];
        Node endNode = _oilSlickNodes[oilSlick];

        Queue<Node> candidateNodes = new Queue<Node>();
        Dictionary<Node, Node> visitedFrom = new Dictionary<Node, Node>
        {
            [startNode] = null
        };

        candidateNodes.Enqueue(startNode);
        bool foundPath = false;
        int currentIterations = 0;
        while (candidateNodes.Count > 0)
        {
            currentIterations += 1;
            Node currentNode = candidateNodes.Dequeue();
            if (currentNode.Equals(endNode))
            {
                foundPath = true;
                break;
            }
            foreach (var n in currentNode.Neighbors)
            {
                if (!visitedFrom.ContainsKey(n))
                {
                    visitedFrom[n] = currentNode;
                    candidateNodes.Enqueue(n);
                }
            }
        }

        if (!foundPath)
        {
            return;
        }

        // build the path
        List<Node> path = new List<Node>();
        Node currentPathNode = endNode;
        currentIterations = 0;
        while (currentPathNode != null)
        {
            currentIterations += 1;
            path.Add(currentPathNode);
            currentPathNode = visitedFrom[currentPathNode];
            Debug.Log(currentPathNode);
        }
        Debug.Log("Found path!");

        List<PathManagerEdge> pathManagerEdges = new List<PathManagerEdge>();
        GameObject entryWaypoint = null;
        for(int i = 0; i < path.Count; i++)
        {
            var curPathNode = path[i];
            var nextPathNode = i < path.Count - 1 ? path[i + 1] : null;

            if(entryWaypoint == null)
            {
                entryWaypoint = curPathNode.Waypoint;
            }

            if (nextPathNode == null || curPathNode.PathManager != nextPathNode.PathManager || (curPathNode.PathManager == nextPathNode.PathManager && curPathNode.isLoop && nextPathNode.isLoop))
            {
                pathManagerEdges.Add(new PathManagerEdge()
                {
                    PathManager = curPathNode.PathManager,
                    EntryWaypoint = entryWaypoint,
                    ExitWaypoint = curPathNode.Waypoint
                });
                entryWaypoint = null;
                Debug.Log(pathManagerEdges[pathManagerEdges.Count - 1]);
            }
        }

        for(int i = 0; i < pathManagerEdges.Count; i++)
        {
            var currentEdge = pathManagerEdges[i];
            if (i > 0)
            {
                currentEdge.PreviousEdge = pathManagerEdges[i - 1];
            }
            if (i < pathManagerEdges.Count - 1)
            {
                currentEdge.NextEdge = pathManagerEdges[i + 1];
            }
        }

        var tradeRoutePath = tradeRoute.gameObject.AddComponent<TradeRoutePath>();
        tradeRoutePath.PathManagerEdges = pathManagerEdges;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
