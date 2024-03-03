class_name BoardGhraph


const NOT_FOUND := -1

var board_nodes: Array[BoardNode] = []


#-- Funcs
## Find node based on its unique position else return null
func find_node(origin: Vector2) -> BoardNode:
    for node in board_nodes:
        if node.position == origin:
            return node
    return null


## Find idx of node within nodes else return -1 not found
func find_idx(origin: Vector2) -> int:
    var idx := 0
    
    for node in board_nodes:
        if node.position == origin:
            return idx
        idx += 1
    
    return NOT_FOUND


## Add a new node to nodes
func add_node(origin: Vector2) -> bool:
    if find_idx(origin) != NOT_FOUND:
        return false

    var node = BoardNode.new()
    node.position = origin
    board_nodes.append(node)
    
    return true


## Add a two way direction edge
func add_edge(origin: Vector2, destination: Vector2, value: float = 1.0, mask: int = 0) -> void:
    var node_a := find_node(origin)
    var node_b := find_node(destination)

    if node_a == null or node_b == null:
        return

    node_a.add_neighbour(node_b, value, mask)
    node_b.add_neighbour(node_a, value, mask)


## Remove node and its edges to and from other nodes
func remove_edge(origin: Vector2) -> bool:
    var idx := find_idx(origin)
    if idx == NOT_FOUND:
        return false

    board_nodes.remove_at(idx)
    
    for node in board_nodes:
        node.remove_neighbour(origin)

    return true