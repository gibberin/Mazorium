#include "SpatialVector.h"

    SpatialVector::SpatialVector()
    {
        direction = 0;
        magnitude = 0;
    }

    SpatialVector::SpatialVector(float dir, float mag)
    {
        direction = dir;
        magnitude = mag;
    }

    SpatialVector SpatialVector::operator+(SpatialVector dest)
    {
        // TODO: Add vector

        return *this;
    }

    SpatialVector SpatialVector::operator-(SpatialVector dest)
    {
        // TODO: Subtract vector

        return *this;
    }

    SpatialVector SpatialVector::VectorBetween(Position& orig, Position& dest)
    {
        SpatialVector vectorTo;

        // TODO: Calculate vector to destination
        
        return vectorTo;
    }
