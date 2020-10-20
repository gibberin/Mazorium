    #include "cell.h"

    Cell::Cell()
    {
        position.x = 0;
        position.y = 0;
        position.z = 0;
    }

    Cell::Cell(int x, int y, int z)
    {
        position.x = x;
        position.y = y;
        position.z = z;
    }

    vector<Cell*> Cell::GetConnections()
    {
        return _connections;
    }

    vector<Cell*> Cell::GetAdjancentCells() 
    {
         return _adjacents; 
    }

    void Cell::AddNeighbor(Cell* cell)
    {
        _adjacents.push_back(cell);
    }

    void Cell::AddConnectionTo(Cell* cell)
    {
        _connections.push_back(cell);
    }

    bool Cell::IsConnected()
    {
        return _connections.size() > 0;
    }

    bool Cell::IsAdjacentTo(Cell* cell)
    {
        // TODO: Search adjacent cell list for the cell provided
        for(Cell* adj_cell : _adjacents)
        {
            if(adj_cell == cell) return true;
        }

        return false;
    }

    bool Cell::IsConnectedTo(Cell* cell)
    {
        // TODO: Search connections for cell provided
        for(Cell* cnx_cell : _connections)
        {
            if(cnx_cell == cell) return true;
        }

        return false;
    }

    SpatialVector Cell::VectorTo(Cell* dest)
    {
        SpatialVector result;

        // TODO: Calculate spatial vector from this cell to dest cell
        // Direction


        // Magnitude
        result.magnitude = sqrt(pow(dest->position.x, 2) + pow(dest->position.y, 2) + pow(dest->position.z, 2));

        return result;
    }

