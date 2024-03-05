using Godot;
using System.Collections;
using System.Collections.Generic;

public partial class BoardNode : IEnumerable<BoardNode>
{
	private const int _notFound = -1;
	private const float _defaultValue = 1f;

	private readonly List<BoardNode> _neighbours = new();
	private readonly List<int> _masks = new();
	private readonly List<float> _values = new();

	public Vector2 Position { get; set; }


	public int FindIdx(Vector2 origin)
	{
		for (int i = 0; i < _neighbours.Count; i++)
		{
			if (_neighbours[i].Position == origin) 
			{
				return i;
			}
		}

		return _notFound;
	}


	public BoardNode FindNeighbour(Vector2 origin)
	{
		for (int i = 0; i < _neighbours.Count; i++)
		{
			if (_neighbours[i].Position == origin) 
			{
				return _neighbours[i];
			}
		}

		return null;
	}


	public BoardNode FindNeighbourInDirectionOf(Vector2 direction)
	{
		Vector2 dir = direction.IsNormalized() ? direction : direction.Normalized();

		for (int i = 0; i < _neighbours.Count; i++)
		{
			Vector2 directionTo = Position.DirectionTo(_neighbours[i].Position);
			if(directionTo == dir)
			{
				return _neighbours[i];
			}
		}
		return null;
	}


	public float ValueTo(Vector2 origin)
	{
		int at = FindIdx(origin);
		
		if (at != _notFound)
		{
			return _values[at];
		}

		return _defaultValue;
	}


	public bool CanMoveTo(Vector2 targetOrigin, int mask)
	{
		int at = FindIdx(targetOrigin);

		if (at != _notFound)
		{
			return (_masks[at] & mask) != 0;
		}

		return false;
	}
	

	public void AddNeighbour(BoardNode node, float value, int mask)
	{
		_neighbours.Add(node);
		_values.Add(value);
		_masks.Add(mask);
	}


	public bool RemoveNeighbour(Vector2 origin)
	{
		int at = FindIdx(origin);

		if (at == _notFound)
		{
			return false;
		}

		_neighbours.RemoveAt(at);
		_values.RemoveAt(at);
		_masks.RemoveAt(at);

		return true;
	}


    public IEnumerator<BoardNode> GetEnumerator()
    {
		for (int i = 0; i < _neighbours.Count; i++)
		{
			yield return _neighbours[i];
		}
    }


    IEnumerator IEnumerable.GetEnumerator()
    {
		return GetEnumerator();
    }
}
