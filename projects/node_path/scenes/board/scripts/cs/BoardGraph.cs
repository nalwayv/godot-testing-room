using Godot;
using System.Collections;
using System.Collections.Generic;

public partial class BoardGraph
{
	private const int _notFound = -1;

	private List<BoardNode> _nodes = new();


	public BoardNode FindNode(Vector2 origin) 
	{
		for (int i = 0; i < _nodes.Count; i++)
		{
			if (_nodes[i].Position == origin)
			{
				return _nodes[i];
			}
		}

		return null;
	}


	public bool IsEmpty()
	{
		return _nodes.Count == 0;
	}


	public BoardNode Peek()
	{
		if(IsEmpty())
		{
			return null;
		}

		return _nodes[0];
	}


	public int FindIdx(Vector2 origin)
	{
		for (int i = 0; i < _nodes.Count; i++)
		{
			if(_nodes[i].Position == origin)
			{
				return i;
			}
		}

		return _notFound;
	}


	public bool AddNode(Vector2 origin)
	{
		int at = FindIdx(origin);

		if (at != _notFound)
		{
			return false;
		}

		_nodes.Add(new() {Position = origin});

		return true;
	}


	public bool AddEdge(Vector2 origin, Vector2 target, float value, int mask)
	{
		var nodeA = FindNode(origin);
		var nodeB = FindNode(target);

		if (nodeA == null || nodeB == null)
		{
			return false;
		}

		nodeA.AddNeighbour(nodeB, value, mask);
		nodeB.AddNeighbour(nodeA, value, mask);

		return true;
	}


	public bool RemoveEdge(Vector2 origin)
	{
		int at = FindIdx(origin);
		if (at == _notFound)
		{
			return false;
		}

		_nodes.RemoveAt(at);

		for (int i = 0; i < _nodes.Count; i++)
		{
			_nodes[i].RemoveNeighbour(origin);
		}

		return true;
	}
}
