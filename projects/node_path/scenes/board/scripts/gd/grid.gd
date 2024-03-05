class_name Grid extends Node

@export_category("Info")
@export var origin: Vector2
@export var size: Vector2
@export var cell: float = 20.0

@export_category("Color")
@export var line_color := Color(1.0, 0.74, 0.62, 0.1)
@export var background_color := Color(0.3, 0.3, 0.3, 0.8)


#-- Callbacks
func _ready() -> void:
	_draw_grid()


# -- Funcs
func _add_line(from: Vector2, to: Vector2) -> void:
	var line := Line2D.new()
	line.points = [from, to]
	line.default_color = line_color
	line.width = 1
	line.z_index = 0
	add_child(line)


func _draw_grid() -> void:
	var end := origin + size
	
	# LINES

	for i in range(origin.y, end.y + 1, cell):
		_add_line(Vector2(origin.x, i), Vector2(end.x, i))

	for i in range(origin.x, end.x + 1, cell):
		_add_line(Vector2(i, origin.y), Vector2(i, end.y))

	# BACKGROUND

	var poly := Polygon2D.new()
	poly.polygon = [
		origin,
		Vector2(origin.x, end.y),
		end,
		Vector2(end.x, origin.y),
	]
	poly.color = background_color
	poly.z_index = -1
	add_child(poly)
