class_name Actor extends Node2D

@export_category("Color")
@export var color: Color

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