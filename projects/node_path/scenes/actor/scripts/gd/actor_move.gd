class_name ActorMove extends Node


@export_category("Actor")
@export var actor: Actor

@export_category("Components")
@export var input: ActorInput

@export_category("Speed")
@export_range(100.0, 300.0, 1.0) var max_speed: float
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
	_update_parent_position(delta)


#-- Funcs
func _update_target() -> void:
	if _is_moveing:
		return

	# NEW DIRECTION

	if input.direction != Vector2.ZERO:
		_target_direction = input.direction
		
		var current = board.find_node(actor.position)
		var target := current.find_neighbour_in_direction_of(_target_direction)

		if target != null and current.can_move_to(target.position, board.MASK_VALUE_DEFAULT):
				_current_direction = _target_direction
				
				_target_position = target.position
				_target_speed = current.value_to(target.position)
				_is_moveing = true

	# ELSE TRY KEEP SAME DIRECTION

	if not _is_moveing:
		if _current_direction != Vector2.ZERO:
			_target_direction = _current_direction

			var current = board.find_node(actor.position)
			var target := current.find_neighbour_in_direction_of(_target_direction)

			if target != null and current.can_move_to(target.position, board.MASK_VALUE_DEFAULT):
					
					_target_position = target.position
					_target_speed = current.value_to(target.position)
					_is_moveing = true


func _update_parent_position(delta: float) -> void:
	if not _is_moveing:
		return

	# DISTANCE REMAINING TO TARGET

	var distance := actor.position.distance_to(_target_position)

	# CURRENT VELOCITY

	var velocity := _target_direction * (max_speed * scaler * _target_speed) * delta


	# HAS VELOCITY OVERTAKEN DISTANCE ?

	if abs(velocity.x) > distance:
		# _target_direction.x is ever + or -
		velocity.x = distance * _target_direction.x
		_is_moveing = false

	if abs(velocity.y) > distance:
		# _target_direction.y is ever + or -
		velocity.y = distance * _target_direction.y
		_is_moveing = false

	actor.position += velocity