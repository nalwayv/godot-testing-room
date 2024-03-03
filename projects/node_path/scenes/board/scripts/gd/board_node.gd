class_name BoardNode

const NOT_FOUND := -1

var neighbours: Array[BoardNode] = []
var values: Array[float] = []
var masks: Array[int] = []
var position := Vector2.ZERO


#-- Funcs
func find_neighbour(origin: Vector2) -> BoardNode:
    for node in neighbours:
        if node.position == origin:
            return node

    return null


## Find nodes idx location within neighbours array
func _find_idx(origin: Vector2) -> int:
    var i := 0

    for node in neighbours:
        if node.position == origin:
            return i
        i += 1

    return NOT_FOUND


## Return value to this node
func value_to(origin: Vector2) -> float:
    var at := _find_idx(origin)

    if at == NOT_FOUND:
        return 1.0
    
    return values[at]


## Check if allowed to travel
func allowd_to_move_to(origin: Vector2, mask: int) -> bool:
    var at := _find_idx(origin)

    if at == NOT_FOUND:
        return false

    var value := masks[at]
    return (value & mask) != 0


## Add neighbour to list of neighbours and its value
func add_neighbour(node: BoardNode, value: float, mask: int) -> void:
    neighbours.append(node)
    values.append(value)
    masks.append(mask)


## Remove node from neighbours
func remove_neighbour(origin: Vector2) -> bool:
    var at := _find_idx(origin)
    
    if at == NOT_FOUND:
        return false
    
    neighbours.remove_at(at)
    values.remove_at(at)
    masks.remove_at(at)
    
    return true