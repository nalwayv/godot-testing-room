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


	private Color _graphColor = new (0f,0f,0f);
	private List<Vector2I> _coords = new();
	private BoardGraph _boardGraph;
	private Actor _actor;


	public override void _Ready()
	{
		_boardGraph = new();
		_actor = GetNode<Actor>("Actor");

		SetupBoard();
		SetupActors();
	}


	private void SetupBoard()
	{
		// GET COORDS FROM TILEMAP

		foreach(var coord in GetUsedCells(_tileLayer))
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

				for (int d = 0; d < directions.Length; d++)
				{
					if ((from + directions[d]) == to)
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

			while(que.Count > 0)
			{

				BoardNode current = que.Dequeue();

				foreach(BoardNode neighbour in current)
				{
					// DRAW LINE

                    Line2D line = new()
                    {
                        Points = new[]
						{
                        	current.Position,
							neighbour.Position
                    	},
                        Width = 1f,
                        DefaultColor = _graphColor
                    };
                    AddChild(line);

					// DRAW CIRCLE

					Vector2[] pts = new Vector2[8];
					float radius = 4f;
					
					for (int i = 0; i < pts.Length; i++)
					{	
						float at = i * Mathf.Tau / pts.Length;
						pts[i] = current.Position + new Vector2(Mathf.Cos(at), Mathf.Sin(at)) * radius;
					}

                    Polygon2D polygon2D = new()
                    {
                        Color = _graphColor,
                        Polygon = pts
                    };
                    AddChild(polygon2D);

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
		Vector2I coord  = Vector2I.Zero;
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
}
