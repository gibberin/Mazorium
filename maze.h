#ifndef MAZE_H
#define MAZE_H

#include "main.h"
#include "cell.h"
#include "spatialvector.h"
#include "time.h"
#include <vector>
#include <set>

class Maze
{
    public:
    int Width() { return _width; }
    void SetWidth(int width) { _width = width; }
    int Height() { return _height; }
    void SetHeight(int height) { _height = height; }
    int Depth() { return _depth; }
    void GeneratePaths();
    bool ExtendPath(Cell* startCell, vector<Cell*>& path);
    bool MakeNeighbors(Cell* cell1, Cell* cell2);
    void ConnectCells(Cell* cell1, Cell* cell2);
    bool CanNavigate(Cell& origCell, Cell& destCell);
    Cell* AddCell(unsigned int x, unsigned int y, unsigned int z);
    Cell* CellAt(unsigned int x, unsigned int y, unsigned int z);

    private:
    int _width = 1;
    int _height = 1;
    int _depth = 1;
    vector<vector<vector<Cell*>>> _cells;
};

#endif