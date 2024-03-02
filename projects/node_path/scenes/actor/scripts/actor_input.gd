class_name ActorInput extends Node


var direction := Vector2.ZERO


#-- Callbacks
func _process(_delta: float) -> void:
	if direction.y == 0:
		direction.x = Input.get_axis("Left", "Right")

	if direction.x == 0:
		direction.y = Input.get_axis("Up", "Down")
