class_name ActorMove extends Node


@export_category("Parent")
@export var parent: Actor

@export_category("Components")
@export var input: ActorInput

@export_category("Speed")
@export_range(100.0, 400.0, 1.0) var max_speed: float
@export_range(0.0, 1.0, 0.1) var scaler: float

var _is_moveing := false

var _current_direction := Vector2.ZERO
var _target_direction := Vector2.ZERO
var _target_position := Vector2.ZERO
var _target_speed := 1.0 # some nodes can alter speed

var board: Board


#-- Callbacks
func _process(delta: float) -> void:
	_update_target()
	_update_position(delta)


#--
func _update_target() -> void:
	if _is_moveing:
		return
	
	# NEW DIRECTION

	if input.direction != Vector2.ZERO:
		_target_direction = input.direction
		
		var target := board.find_node(parent.position + _target_direction * board.NODE_DISTANCE)
		if target != null:

			var current := board.find_node(parent.position)
			if current.find_neighbour(target.position) != null:
			
				if current.allowd_to_move_to(target.position, board.MASK_VALUE_DEFAULT):
					
					_current_direction = _target_direction
					
					_target_position = target.position
					_target_speed = current.value_to(target.position)
					_is_moveing = true

	# ELSE KEEP SAME DIRECTION

	if not _is_moveing:
		if _current_direction != Vector2.ZERO:
			_target_direction = _current_direction

			var target := board.find_node(parent.position + _target_direction * board.NODE_DISTANCE)
			if target != null:

				var current := board.find_node(parent.position)
				if current.find_neighbour(target.position) != null:

					if current.allowd_to_move_to(target.position, board.MASK_VALUE_DEFAULT):
						_target_position = target.position
						_target_speed = current.value_to(target.position)
						_is_moveing = true
		

func _update_position(delta: float) -> void:
	if not _is_moveing:
		return

	# UPDATE POSITION

	var distance := (_target_position - parent.position).abs()
	var velocity := _target_direction * (max_speed * scaler * _target_speed) * delta

	if abs(velocity.x) > distance.x:
		velocity.x = distance.x * _target_direction.x
		_is_moveing = false

	if abs(velocity.y) > distance.y:
		velocity.y = distance.y * _target_direction.y
		_is_moveing = false

	parent.position += velocity