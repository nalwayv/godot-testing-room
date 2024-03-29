using Godot;

public partial class ActorMove : Node
{
	[ExportCategory("Actor")]
	[Export]
	public Actor Actor { get; set; }

	[ExportCategory("Components")]
	[Export]
	public ActorInput ActorInput { get; set; }

	[ExportCategory("Speed")]
	[Export(PropertyHint.Range, "0.0, 300.0, 1.0")]
	public float Speed { get; set; }
	[Export(PropertyHint.Range, "0.0, 1.0, 0.1")]
	public float Scale { get; set; }

	public Board Board { get; set; }

	private bool _isMoveing = false;
	private Vector2 _currentDirection = Vector2.Zero;
	private Vector2 _targetDirection = Vector2.Zero;
	private Vector2 _targetPosition = Vector2.Zero;
	private float _targetSpeed = 1f;


	public override void _Process(double delta)
	{
		UpdateTarget();
		UpdateParentPosition(delta);
	}


	private void UpdateTarget()
	{
		if (_isMoveing)
		{
			return;
		}

		// NEW DIRECTION

		if (ActorInput.Direction != Vector2.Zero)
		{
			_targetDirection = ActorInput.Direction;

			BoardNode current = Board.FindNode(Actor.Position);
			BoardNode target = current.FindNeighbourInDirectionOf(_targetDirection);

			if (target != null && current.CanMoveTo(target.Position, Board.DefaultMaskValue))
			{
				if (current.CanMoveTo(target.Position, Board.DefaultMaskValue))
				{
					_currentDirection = _targetDirection;
					_targetPosition = target.Position;
					_targetSpeed = current.ValueTo(target.Position);
					_isMoveing = true;
				}
			}
		}

		// ELSE TRY AND KEEP SAME DIRECTION

		if (!_isMoveing)
		{
			if (_currentDirection != Vector2.Zero)
			{
				_targetDirection = _currentDirection;

				BoardNode current = Board.FindNode(Actor.Position);
				BoardNode target = current.FindNeighbourInDirectionOf(_targetDirection);

				if (target != null && current.CanMoveTo(target.Position, Board.DefaultMaskValue))
				{
					if (current.CanMoveTo(target.Position, Board.DefaultMaskValue))
					{
						_targetPosition = target.Position;
						_targetSpeed = current.ValueTo(target.Position);
						_isMoveing = true;
					}
				}
			}
		}
	}


	private void UpdateParentPosition(double delta)
	{
		if (!_isMoveing)
		{
			return;
		}

		// DISTANCE REMAINING TO TARGET

		float distance = Actor.Position.DistanceTo(_targetPosition);

		// CURRENT VELOCITY

		Vector2 velocity = _targetDirection * (Speed * Scale * _targetSpeed) * ((float)delta);

		// HAS VELOCITY OVERTAKEN DISTANCE ?

		if (Mathf.Abs(velocity.X) > distance)
		{
			// _targetDirection.X is ever +1 or -1
			velocity.X = distance * _targetDirection.X;
			_isMoveing = false;
		}

		if (Mathf.Abs(velocity.Y) > distance)
		{
			// _targetDirection.Y is ever +1 or -1
			velocity.Y = distance * _targetDirection.Y;
			_isMoveing = false;
		}

		Actor.Position += velocity;
	}
}
