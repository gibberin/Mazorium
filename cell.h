#ifndef CELL_H
#define CELL_H

#include "main.h"
#include "position.h"
#include "spatialvector.h"
#include <vector>

class Cell
{
    public:
    Cell();
    Cell(int x, int y, int z = 0);
    bool visited = false;
    Position position;
    vector<Cell*> GetConnections();
    vector<Cell*> GetAdjancentCells();
    void AddNeighbor(Cell* cell);
    void AddConnectionTo(Cell* cell);
    bool IsConnected();
    bool IsAdjacentTo(Cell* cell);
    bool IsConnectedTo(Cell* cell);
    SpatialVector VectorTo(Cell* dest);

    private:
    vector<Cell*> _adjacents;
    vector<Cell*> _connections;
};

#endif