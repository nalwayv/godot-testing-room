class_name Board extends TileMap

const NOT_FOUND := -1
const TILE_NOT_EXIST := -1
const TILE_LAYER := 0
const PATH_VALUE_DEFAULT := 1.0
const MASK_VALUE_DEFAULT := 2
const NODE_DISTANCE := 20.0

@export_category("Grid Color")
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
func _add_line(from: Vector2, to: Vector2) -> void:
	var line := Line2D.new()
	line.points = [from, to]
	line.width = 1
	line.default_color = color
	add_child(line)


func _add_circle(origin: Vector2, radius: float) -> void:
	var pts := 6
	var pts_data: Array[Vector2] = []
	
	pts_data.resize(pts + 1)
	for i in range(pts + 1):
		var at: float = i * TAU / pts
		pts_data[i] = origin+ Vector2(cos(at), sin(at)) * radius

	var circle := Polygon2D.new()
	circle.color = color
	circle.polygon = pts_data
	add_child(circle)


func _setup_board() -> void:
	# GET COORDS FROM TILEMAP

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
			for neighbour in current.neighbours:

				# DRAW LINE
				_add_line(current.position, neighbour.position)
	
				# DRAW CIRCLE

				_add_circle(current.position, 5)

				# ADD TO VISITED

				if visited.find(neighbour.position) == -1:
					visited.append(neighbour.position)
					queue.append(neighbour)


func _setup_actor() -> void:
	var coord := Vector2i.ZERO
	var local := map_to_local(coord)
	var idx := _graph.find_idx(local)

	if idx != NOT_FOUND:
		_actor.setup(local, self)


func find_node(origin: Vector2) -> BoardNode:
	return _graph.find_node(origin)


func find_node_direction(origin: Vector2, direction: Vector2) -> BoardNode:
	if not direction.is_normalized():
		return _graph.find_node(origin + direction.normalized() * NODE_DISTANCE)
		
	return _graph.find_node(origin + direction * NODE_DISTANCE)
