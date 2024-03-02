class_name Grid extends Node

@export_category("Grid Color")
@export var line_color: Color
@export var bg_color: Color

@export_category("Screen")
@export var width: int
@export var height: int

@export_category("Cell")
@export var size: int


#-- Callbacks
func _ready() -> void:
	_draw_grid()


#-- Funcs
func _draw_grid() -> void:

	# LINES

	for i in range(0, height, size):
		var line := Line2D.new()
		line.points = [Vector2(0, i), Vector2(width - 1, i)]
		line.default_color = line_color
		line.width = 1
		line.z_index = 0
		add_child(line)

	for i in range(0, width, size):
		var line := Line2D.new()
		line.points = [Vector2(i, 0), Vector2(i, height - 1)]
		line.default_color = line_color
		line.width = 1
		line.z_index = 0
		add_child(line)

	# BACKGROUND

	var poly := Polygon2D.new()
	poly.polygon = [
		Vector2(0, 0),
		Vector2(width, 0),
		Vector2(width, height),
		Vector2(0, height),
	]
	poly.color = bg_color
	poly.z_index = -1
	add_child(poly)