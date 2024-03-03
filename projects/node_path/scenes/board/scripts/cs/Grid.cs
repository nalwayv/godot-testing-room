using Godot;

public partial class Grid : Node
{
	[ExportCategory("Size")]
	[Export]
	public int Width { get; set; }
	[Export]
	public int Height { get; set; }
	[Export]
	public int Cell { get; set; }

	private Color _lineColor = new(1f, 0.74f, 0.62f, 0.1f);
	private Color _bgColor = new(0.3f, 0.3f, 0.3f, 1f);


	public override void _Ready()
	{
		DrawGrid();
	}


	private void DrawGrid()
	{
		// LINES

		for (int i = 0; i < Height; i+=Cell)
		{
			Line2D line = new()
			{
				Points = new[]
				{
					new Vector2(0, i),
					new Vector2(Width - 1, i)
				},
				DefaultColor = _lineColor,
				Width = 1.0f,
				ZIndex = 0
			};
			AddChild(line);
		}

		for (int i = 0; i < Width; i+=Cell)
		{
			Line2D line = new()
			{
				Points = new[] {
					new Vector2(i, 0),
					new Vector2(i, Height - 1)
				},
				DefaultColor = _lineColor,
				Width = 1.0f,
				ZIndex = 0
			};
			AddChild(line);
		}

        // BACKGROUND

        Polygon2D polygon2D = new()
        {
            Polygon = new[] 
			{
				new Vector2(0f, 0f),
				new Vector2(Width, 0f),
				new Vector2(Width, Height),
				new Vector2(0f, Height),
        	},
            Color = _bgColor,
            ZIndex = -1
        };
        AddChild(polygon2D);
	}
}
