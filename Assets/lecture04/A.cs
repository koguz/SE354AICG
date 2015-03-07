using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Node
{
	public Node(Vector3 d)
	{
		x = (int)d.x; z = (int)d.z;
	}
	public int x;
	public int z;
	public Vector3 getValue()
	{
		return new Vector3(x, 0, z);
	}
}

public class Edge
{
	public Node from;
	public Node to;
	public float getCost()
	{
		return (from.getValue() - to.getValue()).magnitude;
	}
}

public class NodeRecord
{
	public Node node;
	public Edge connection;
	public float costSoFar;
	public float estimatedTotalCost;
}

public class Heuristic
{
	public Heuristic(Vector3 target)
	{
		this.target = target;
	}
	private Vector3 target;
	public float estimate(Vector3 t)
	{
		return (target - t).magnitude;
	}
	public float estimate(Node t)
	{
		return (target - t.getValue()).magnitude;
	}
}

public class A : MonoBehaviour {
	/* What does A* need? A source, a target
	 * and a graph to work on. 
	 */
	public bool dur = true;
	private bool cal = false;
	public Vector3 source;
	public Vector3 target;
	public Map map; // graph comes from the map
	public ArrayList targets;

	// Use this for initialization
	void Start () {
		targets = new ArrayList();
		source = transform.position;
		target = transform.position;
	}

	List<Edge> getConnections(Node node)
	{
		List<Edge> connections = new List<Edge>();
		
		int px = node.x; int pz = node.z;
		for (int i = px - 1; i <= px + 1; i++)
		{
			for (int j = pz - 1; j <= pz + 1; j++)
			{
				if (i == px && j == pz) continue;
				if (i >= 0 && i < 30 && j >= 0 && j < 30 &&  map.alan[i, j] != 1)
				{
					Edge edge = new Edge();
					edge.from = node;
					edge.to = new Node(new Vector3(i, 0, j));
					connections.Add(edge);
				}
			}
		}
		return connections;
	}

	NodeRecord findSmallest(List<NodeRecord> list) 
	{
		if(list.Count == 1) return list[0];
		int index = 0;
		float s = list[0].estimatedTotalCost;
		for(int i=1;i<list.Count;i++)
		{
			if(list[i].estimatedTotalCost < s) 
			{
				index = i; 
				s = list[i].estimatedTotalCost;
			}
		}
		return list[index];
	}
	
	NodeRecord FindRecordInList(List<NodeRecord> list, Node n)
	{
		foreach (NodeRecord r in list)
		{
			if (r.node.getValue() == n.getValue()) return r;
		}
		return null;
	}

	List<Edge> aStar(Vector3 start, Vector3 end)
	{
		Heuristic heuristic = new Heuristic(end);
		NodeRecord startRecord = new NodeRecord();
		startRecord.node = new Node(start);
		startRecord.connection = null;
		startRecord.costSoFar = 0;
		startRecord.estimatedTotalCost = heuristic.estimate(start);
		
		List<NodeRecord> open = new List<NodeRecord>();
		open.Add(startRecord);
		List<NodeRecord> closed = new List<NodeRecord>();
		NodeRecord current = null;
		while (open.Count > 0)
		{
			current = findSmallest(open);
			// target is the closest node on the list
			// break out of the while loop
			if (current.node.getValue() == end) break;
		
			List<Edge> connections = getConnections(current.node);
			foreach (Edge connection in connections)
			{
				Node endNode = connection.to;
				float endNodeCost = current.costSoFar + connection.getCost();
				float endNodeHeuristic = float.MaxValue;
				NodeRecord endNodeRecord = FindRecordInList(closed, endNode);
				if (endNodeRecord != null)
				{
					if (endNodeRecord.costSoFar <= endNodeCost)
						continue;
					closed.Remove(endNodeRecord);
					endNodeHeuristic = endNodeRecord.estimatedTotalCost - endNodeRecord.costSoFar;
				}
				else if (FindRecordInList(open, endNode) != null)
				{
					endNodeRecord = FindRecordInList(open, endNode);
					if (endNodeRecord.costSoFar <= endNodeCost)
						continue;
					endNodeHeuristic = endNodeRecord.estimatedTotalCost - endNodeRecord.costSoFar;
				}
				else
				{
					endNodeRecord = new NodeRecord();
					endNodeRecord.node = endNode;
					endNodeHeuristic = heuristic.estimate(endNode);
				}
				endNodeRecord.costSoFar = endNodeCost;
				endNodeRecord.connection = connection;
				endNodeRecord.estimatedTotalCost = endNodeCost + endNodeHeuristic;
				
				if (FindRecordInList(open, endNode) == null)
				{
					open.Add(endNodeRecord);
				}
			}
			open.Remove(current);
			closed.Add(current);
		}
		if (current.node.getValue() != end) return null; // null;
		else
		{
			List<Edge> path = new List<Edge>();
			while (current.node.getValue() != start)
			{
				path.Add(current.connection);
				current = FindRecordInList(closed, current.connection.from);
			}
			path.Reverse();
			return path;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (dur) return;
		if (!cal)
		{
			//getPath(source, target);
			List<Edge> edges = aStar(source, target);
			if(edges == null) 
			{
				cal = true;
				return;
			}
			foreach (Edge edge in edges)
			{
				GameObject.Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), edge.to.getValue(), Quaternion.identity);
			}
			cal = true;
		}
	}
}
