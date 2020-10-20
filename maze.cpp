    #include "maze.h"

    void Maze::GeneratePaths()
    {
        // Make a list of all the unconnected cells in the maze
        set<Cell*> unconnectedCells;

        for(vector<vector<Cell*>> row : _cells)
        {
            for(vector<Cell*> col : row)
            {
                for(Cell* cell : col)
                {
                    if(!cell->IsConnected())
                    {
                        unconnectedCells.emplace(cell);
                    }
                }
            }
        }

        // Seed RNG
        srand(time(NULL));

        // While unconnected cells remain pick a random one and connect it to an adjacent cell
        while(0 < unconnectedCells.size())
        {
            // Pick a random cell
            int idx = rand() % (unconnectedCells.size());

            // TODO: Assert 0 <= idx <= unconnectedCells.size()

            set<Cell*>::iterator unCnxIterator = unconnectedCells.begin();
            for(int i = 1; i < idx; unCnxIterator++) {}
            Cell* currentCell = *unCnxIterator;

            // Connect to a random neighbor... recursively
            vector<Cell*> newPath;
            newPath.push_back(currentCell);
            bool extended = ExtendPath(currentCell, newPath);

            // Remove cells in new path from unconnected cells list
            for(Cell* pathCell : newPath)
            {
                for(int idx = 0; idx < unconnectedCells.size(); idx++)
                {
                    unconnectedCells.erase(pathCell);
                }
            }
        }
    }

    bool Maze::ExtendPath(Cell* startCell, vector<Cell*>& path)
    {
        bool pathExtended = false;

        // Pick a random adjacent cell and make a connection
        vector<Cell*> adjacentCells = startCell->GetAdjancentCells();
        Cell* nextCell = adjacentCells.at(rand() % adjacentCells.size());
        path.push_back(nextCell);

        if(!nextCell->IsConnected())
        {
            pathExtended = ExtendPath(nextCell, path);
        }

        ConnectCells(startCell, nextCell);

        return pathExtended;
    }

    // Check if there is a path between two cells
    bool Maze::CanNavigate(Cell& origCell, Cell& destCell)
    {
        return origCell.IsConnectedTo(&destCell);
    }

    // Make two cells neighbors
    bool Maze::MakeNeighbors(Cell* cell1, Cell* cell2)
    {
        cell1->AddNeighbor(cell2);
        cell2->AddNeighbor(cell1);

        return true;
    }

    void Maze::ConnectCells(Cell* cell1, Cell* cell2)
    {
        cell1->AddConnectionTo(cell2);
        cell2->AddConnectionTo(cell1);
    }

    Cell* Maze::AddCell(unsigned int x, unsigned int y, unsigned int z)
    {
        Cell* newCell = new Cell(x, y, z);
        _cells[y][x][z] = newCell;

        return newCell;
    }

    Cell* Maze::CellAt(unsigned int x, unsigned int y, unsigned int z)
    {
        if(Height() <= x || Width() <= y || Depth() <= z)
        {
            return nullptr;
        }

        return _cells[y][x][z];
    }