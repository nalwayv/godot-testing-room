using Godot;
using System.Collections.Generic;

public partial class Board : TileMap
{
	private const int _notFound = -1;
	private const int _tileNotExist = -1;
	private const int _tileLayer = 0;
	public const float DefaultPathValue = 1f;
	public const int DefaultMaskValue = 2;
	public const float NodeDistance = 20f;

	[ExportCategory("Graph Color")]
	[Export]
	public Color GraphColor { get; set; } = new(0f, 0f, 0f);

	private readonly List<Vector2I> _coords = new();
	private BoardGraph _boardGraph;
	private Actor _actor;


	public override void _Ready()
	{
		_boardGraph = new();
		_actor = GetNode<Actor>("Actor");

		SetupBoard();
		SetupActors();
	}


	private void AddLine(Vector2 from, Vector2 to)
	{
		Line2D line = new()
		{
			Points = new[] {from, to},
			DefaultColor = GraphColor,
			Width = 1.0f,
			ZIndex = 0
		};

		AddChild(line);
	}


	private void AddCircle(Vector2 position, float radius)
	{
		Vector2[] pts = new Vector2[6];

		for (int i = 0; i < pts.Length; i++)
		{
			float at = i * Mathf.Tau / pts.Length;
			pts[i] = position + new Vector2(Mathf.Cos(at), Mathf.Sin(at)) * radius;
		}

		Polygon2D polygon2D = new()
		{
			Color = GraphColor,
			Polygon = pts
		};

		AddChild(polygon2D);
	}


	private void SetupBoard()
	{
		// GET COORDS FROM TILEMAP

		foreach (var coord in GetUsedCells(_tileLayer))
		{
			var cell = GetCellSourceId(_tileLayer, coord);

			if (cell != _tileNotExist)
			{
				_boardGraph.AddNode(MapToLocal(coord));
				_coords.Add(coord);

				SetCell(_tileLayer, coord);
			}
		}

		// ADD EDGES

		Vector2I[] directions = new[]{
			Vector2I.Up,
			Vector2I.Down,
			Vector2I.Left,
			Vector2I.Right,
		};

		int n = _coords.Count;
		for (int i = 0; i < n - 1; i++)
		{
			var from = _coords[i];

			for (int j = i + 1; j < n; j++)
			{
				var to = _coords[j];

				foreach (Vector2I dir in directions)
				{
					if ((from + dir) == to)
					{
						_boardGraph.AddEdge(
							MapToLocal(from),
							MapToLocal(to),
							DefaultPathValue,
							DefaultMaskValue
						);
					}
				}
			}
		}

		// DRAW

		if (!_boardGraph.IsEmpty())
		{
			BoardNode first = _boardGraph.Peek();

			List<Vector2> visited = new();

			Queue<BoardNode> que = new();
			que.Enqueue(first);

			while (que.Count > 0)
			{
				BoardNode current = que.Dequeue();

				foreach (BoardNode neighbour in current)
				{
					// DRAW LINE

					AddLine(current.Position, neighbour.Position);

					// DRAW CIRCLE

					AddCircle(current.Position, 4f);
	

					if (!visited.Contains(neighbour.Position))
					{
						visited.Add(neighbour.Position);
						que.Enqueue(neighbour);
					}
				}
			}
		}
	}


	private void SetupActors()
	{
		Vector2I coord = Vector2I.Zero;
		Vector2 local = MapToLocal(coord);

		int at = _boardGraph.FindIdx(local);
		if (at != _notFound)
		{
			_actor.Setup(MapToLocal(coord), this);
		}
	}


	public BoardNode FindNode(Vector2 origin)
	{
		return _boardGraph.FindNode(origin);
	}


	public BoardNode FindNodeDirection(Vector2 origin, Vector2 direction)
	{
		if(!direction.IsNormalized())
		{
			return _boardGraph.FindNode(origin + direction.Normalized() * NodeDistance);
		}

		return _boardGraph.FindNode(origin + direction * NodeDistance);
	}
}
