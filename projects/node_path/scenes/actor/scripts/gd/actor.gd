class_name Actor extends Node2D


@export_category("Actor Color")
@export var color := Color(0.88, 0.49, 0.12, 1)

var _polygon: Polygon2D


#-- Callbacks
func _ready() -> void:
	_polygon = get_node("Polygon2D") as Polygon2D
	_polygon.color = color


#-- Funcs
func setup(origin: Vector2, board: Board) -> void:
	position = origin

	for child in get_children():
		if child is ActorMove:
			(child as ActorMove).board = board