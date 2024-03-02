class_name Board extends TileMap

const NOT_FOUND := -1
const TILE_NOT_EXIST := -1
const TILE_LAYER := 0
const PATH_VALUE_DEFAULT := 1.0
const MASK_VALUE_DEFAULT := 2
const NODE_DISTANCE := 20.0

@export_category("Color")
@export var color: Color

var _coords: Array[Vector2i] = []
var _graph: BoardGhraph
var _actor: Actor

#-- Callbacks
func _ready() -> void:
	_graph = BoardGhraph.new()
	_actor = get_node("Actor") as Actor

	_setup_board()
	_setup_actor()


#-- Funcs
func _setup_board() -> void:
	# USE TILEMAP TO FOR NODE PLACEMENT

	for cell_coord in get_used_cells(TILE_LAYER):
		var cell := get_cell_source_id(TILE_LAYER, cell_coord)

		if cell != TILE_NOT_EXIST:
			var cell_local := map_to_local(cell_coord)

			_graph.add_node(cell_local)
			_coords.append(cell_coord)

			set_cell(TILE_LAYER, cell_coord)

	# CONNECT EDGES

	var n := _coords.size()

	for i in range(0, n - 1):

		var coord_a := _coords[i]
		var local_a := map_to_local(coord_a)

		for j in range(i + 1, n):
			var coord_b := _coords[j]
			var local_b := map_to_local(coord_b)

			for dir in [Vector2i.UP, Vector2i.DOWN, Vector2i.LEFT, Vector2i.RIGHT]:
				if (coord_a + dir) == coord_b:
					_graph.add_edge(local_a, local_b, PATH_VALUE_DEFAULT, MASK_VALUE_DEFAULT)

	# DRAW

	if _graph.board_nodes.size() > 0:
		var first := _graph.board_nodes[0]

		var visited: Array[Vector2] = []
		var queue: Array[BoardNode] = [first]

		while not queue.is_empty():
			var current: BoardNode = queue.pop_front()
			var current_p: Vector2 = current.position

			for neighbour in current.neighbours:
				var neighbour_p: Vector2 = neighbour.position

				# DRAW LINE

				var line := Line2D.new()
				line.points = [current_p, neighbour_p]
				line.width = 1
				line.default_color = color
				add_child(line)

				# DRAW CIRCLE

				var radius := 4.0
				var pts := 8
				var pts_data: Array[Vector2] = []
				
				pts_data.resize(pts + 1)
				for i in range(pts + 1):
					var at: float = i * TAU / pts
					pts_data[i] = current_p + Vector2(cos(at), sin(at)) * radius

				var circle := Polygon2D.new()
				circle.color = color
				circle.polygon = pts_data
				add_child(circle)

				# ADD TO VISITED

				if visited.find(neighbour_p) == -1:
					visited.append(neighbour_p)
					queue.append(neighbour)


func _setup_actor() -> void:
	var coord := Vector2i.ZERO
	var local := map_to_local(coord)
	var idx := _graph.find_idx(local)

	if idx != NOT_FOUND:
		_actor.setup(local, self)


# Find node based on its origin position
func find_node(origin: Vector2) -> BoardNode:
	return _graph.find_node(origin)