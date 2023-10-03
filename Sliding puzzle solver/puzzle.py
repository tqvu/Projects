from __future__ import division
from __future__ import print_function

import sys
import math
import time
import queue as Q


class PuzzleState(object):
    """
        The PuzzleState stores a board configuration and implements
        movement instructions to generate valid children.
    """

    def __init__(self, config, n, parent=None, action="Initial", cost=0):

        if n * n != len(config) or n < 2:
            raise Exception("The length of config is not correct!")
        if set(config) != set(range(n * n)):
            raise Exception("Config contains invalid/duplicate entries : ", config)

        self.n = n
        self.cost = cost
        self.parent = parent
        self.action = action
        self.config = config
        self.children = []
        self.manhattan_dist = 0

        # Get the index and (row, col) of empty block
        self.blank_index = self.config.index(0)

    def display(self):
        """ Display this Puzzle state as a n*n board """
        for i in range(self.n):
            print(self.config[3 * i: 3 * (i + 1)])

    def move_up(self):
        blank = self.blank_index
        array = []
        for i in range(self.n):
            array.append(i)

        if blank not in array:
            child = PuzzleState(self.config[:], self.n, self, "Up", cost = self.cost + 1)
            child.config[blank], child.config[blank - child.n] = child.config[blank - child.n], child.config[blank]
            child.blank_index = child.config.index(0)
            return child
        else:
            return None
        pass

    def move_down(self):
        blank = self.blank_index
        array = []
        for i in range(self.n):
            array.append(len(self.config) - 1 - i)

        if blank not in array:
            child = PuzzleState(self.config[:], self.n, self, "Down", cost=self.cost + 1)
            child.config[blank], child.config[blank + child.n] = child.config[blank + child.n], child.config[blank]
            child.blank_index = child.config.index(0)
            return child
        else:
            return  None
        pass

    def move_left(self):
        blank = self.blank_index
        array = []
        for i in range(self.n):
            array.append(i * self.n)

        if blank not in array:
            child = PuzzleState(self.config[:], self.n, self, "Left", cost=self.cost + 1)
            child.config[blank], child.config[blank - 1] = child.config[blank - 1], child.config[blank]
            child.blank_index = child.config.index(0)
            return child
        else:
            return None
        pass

    def move_right(self):
        blank = self.blank_index
        array = []
        for i in range(self.n):
            array.append(len(self.config) - 1 - i * self.n)

        if blank not in array:
            child = PuzzleState(self.config[:], self.n, self, "Right", cost=self.cost + 1)
            child.config[blank], child.config[blank + 1] = child.config[blank + 1], child.config[blank]
            child.blank_index = child.config.index(0)
            return child
        else:
            return None
        pass

    def expand(self):
        """ Generate the child nodes of this node """

        # Node has already been expanded
        if len(self.children) != 0:
            return self.children

        # Add child nodes in order of UDLR
        children = [
            self.move_up(),
            self.move_down(),
            self.move_left(),
            self.move_right()]

        # Compose self.children of all non-None children states
        self.children = [state for state in children if state is not None]
        return self.children

    def __lt__(self, other):
        return (self.manhattan_dist < other.manhattan_dist)


def writeOutput(state, cost_of_path, nodes_expanded, search_depth, max_search_depth, running_time):
    path = []
    while state.parent:
        path.insert(0,state.action)
        state = state.parent
    f = open("../output.txt", "w")
    f.write("path_to_goal: " + str(path) + "\n")
    f.write("cost_of_path: " + str(cost_of_path) + "\n")
    f.write("nodes_expanded: " + str(nodes_expanded) + "\n")
    f.write("search_depth: " + str(search_depth) + "\n")
    f.write("max_search_depth: " + str(max_search_depth) + "\n")
    f.write("running_time: " + str("%.8f" % running_time) + "\n")
    f.close()

    pass


def bfs_search(initial_state):
    """BFS search"""
    start_time = time.time()
    nodes_expanded = -1
    max_search_depth = 0

    #create queue
    frontier = Q.Queue()
    frontier.put(initial_state)
    explored = set()

    while frontier:
        #pop from queue
        state = frontier.get()
        nodes_expanded += 1
        explored.add(tuple(state.config))

        #if state is goal state
        if test_goal(state):
            writeOutput(state, state.cost, nodes_expanded, state.cost, max_search_depth, time.time() - start_time)
            return

        #expand state in UDLR
        for child in state.expand():
            if tuple(child.config) not in explored:
                explored.add(tuple(child.config))
                max_search_depth = max(child.cost, max_search_depth)
                frontier.put(child)
    pass


def dfs_search(initial_state):
    """DFS search"""
    start_time = time.time()
    nodes_expanded = -1
    max_search_depth = 0

    #create stack
    frontier = Q.LifoQueue()
    frontier.put(initial_state)
    explored = set()

    while frontier:
        #pop from stack
        state = frontier.get()
        nodes_expanded += 1
        explored.add(tuple(state.config))

        #if state is goal state
        if test_goal(state):
            writeOutput(state, state.cost, nodes_expanded, state.cost, max_search_depth, time.time() - start_time)
            return

        #expand state in RLDU
        for child in state.expand()[::-1]:
            if tuple(child.config) not in explored:
                explored.add(tuple(child.config))
                max_search_depth = max(child.cost, max_search_depth)
                frontier.put(child)
    pass


def A_star_search(initial_state):
    """A * search"""
    start_time = time.time()
    nodes_expanded = -1
    max_search_depth = 0

    #create priority queue
    frontier = Q.PriorityQueue()
    frontier.put((0, initial_state))
    explored = set()

    while frontier.qsize() > 0:
        #pop from priority queue
        state = frontier.get()
        nodes_expanded += 1
        explored.add(tuple(state[1].config))

        #if state is goal state
        if test_goal(state[1]):
            writeOutput(state[1], state[1].cost, nodes_expanded, state[1].cost, max_search_depth, time.time() - start_time)
            return

        #expand state in UDLR
        for child in state[1].expand():
            if tuple(child.config) not in explored:
                explored.add(tuple(child.config))
                max_search_depth = max(child.cost, max_search_depth)
                # calculate manhattan distance
                distance = 0
                for i in range(1, child.n ** 2):
                    distance += calculate_manhattan_dist(i, child.config.index(i), child.n)
                child.manhattan_dist = distance
                frontier.put(((child.manhattan_dist + child.cost), child))
    pass

def calculate_total_cost(state):
    """calculate the total estimated cost of a state"""
    count = 0
    while state:
        count += 1
        state = state.parent
    return count
    pass

def calculate_manhattan_dist(idx, value, n):
    return (abs(value%n - idx%n) + abs(value//n - idx//n))
    pass

def test_goal(puzzle_state):
    """test the state is the goal state or not"""
    goal_state = [0, 1, 2, 3, 4, 5, 6, 7, 8]
    return puzzle_state.config == goal_state
    pass

def main():
    #get puzle information
    size = input("Size of puzzle: ")
    board_size = int(math.sqrt(int(size)))
    puzzle = []
    for i in range(board_size):
        for j in range(board_size):
            puzzle.append(int(input("Enter number in position [" + str(i + 1) + "][" + str(j + 1) + "]: " )))

    search_mode = input('Type "bfs" for Breadth-First Search, "dfs" for Depth-First Search, or "ast for A-Star Search: ')

    hard_state = PuzzleState(puzzle, board_size)
    start_time = time.time()

    if search_mode == "bfs":
        bfs_search(hard_state)
    elif search_mode == "dfs":
        dfs_search(hard_state)
    elif search_mode == "ast":
        A_star_search(hard_state)
    else:
        print("Enter valid command arguments !")

    end_time = time.time()
    print("Program completed in %.3f second(s)" % (end_time - start_time))


if __name__ == '__main__':
    main()


