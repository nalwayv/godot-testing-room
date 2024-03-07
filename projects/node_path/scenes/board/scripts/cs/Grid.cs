using Godot;

public partial class Grid : Node
{
	[ExportCategory("Info")]
	[Export]
	public Vector2 Origin { get; set; }
	[Export]
	public Vector2 Size { get; set; }
	[Export]
	public float Cell { get; set; } = 20;

	[ExportCategory("Color")]
	[Export]
	public Color LineColor { get; set; } = new Color(1f, 0.74f, 0.62f, 0.1f);
	[Export]
	public Color BackgroundColor { get; set; } = new Color(0.3f, 0.3f, 0.3f, 0.8f);


	public override void _Ready()
	{
		DrawGrid();
	}


	private void AddLine(Vector2 from, Vector2 to)
	{
		Line2D line = new()
		{
			Points = new[] {from, to},
			DefaultColor = LineColor,
			Width = 1.0f,
			ZIndex = 0
		};
		AddChild(line);
	}


	private void DrawGrid()
	{
		Vector2 end = Origin + Size;

		// GRID LINES

		for (var i = Origin.Y + Cell; i < end.Y; i += Cell)
		{
			AddLine(new Vector2(Origin.X, i), new Vector2(end.X, i));
		}

		for (var i = Origin.X + Cell; i < end.Y; i += Cell)
		{
			AddLine(new Vector2(i, Origin.Y), new Vector2(i, end.Y));
		}

		// GRID OUTLINE
		//	top
		//	bottom
		//	left
		//	right
		AddLine(Origin, new Vector2(Origin.X + Size.X, Origin.Y));
		AddLine(new Vector2(Origin.X, Origin.Y + Size.Y), Origin + Size);
		AddLine(Origin, new Vector2(Origin.X, Origin.Y + Size.Y));
		AddLine(new Vector2(Origin.X + Size.X, Origin.Y), Origin + Size);

		// BACKGROUND

		Polygon2D polygon2D = new()
		{
			Polygon = new[]
			{
				Origin,
				new Vector2(Origin.X, end.Y),
				end,
				new Vector2(end.X, Origin.Y),
			},
			Color = BackgroundColor,
			ZIndex = -1
		};
		AddChild(polygon2D);
	}
}
