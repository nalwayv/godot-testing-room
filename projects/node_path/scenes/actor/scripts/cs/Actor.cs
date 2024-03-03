using Godot;

public partial class Actor : Node2D
{
	private Color _color = new(0.88f, 0.49f, 0.12f, 1f);
	private Polygon2D _polygon2D;


	public override void _Ready()
	{
		_polygon2D = GetNode<Polygon2D>("Polygon2D");
		_polygon2D.Color = _color;
	}


	public void Setup(Vector2 origin, Board board)
	{

		Position = origin;

		foreach(var child in GetChildren())
		{
			if(child is ActorMove actorMove)
			{
				actorMove.Board = board;
			}
		}
	}
}
