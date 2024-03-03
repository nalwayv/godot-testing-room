using Godot;

public partial class ActorInput : Node
{
	public Vector2 Direction { get; set; } = Vector2.Zero;


	public override void _Process(double delta)
	{
		float directionX = (Direction.Y == 0) ? Input.GetAxis("Left", "Right") : Direction.X;
		float directionY = (Direction.X == 0) ? Input.GetAxis("Up", "Down") : Direction.Y;

		Direction = new Vector2(directionX, directionY);
	}
}
