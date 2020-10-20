#ifndef _SPATIAL_VECTOR_H
#define _SPATIAL_VECTOR_H

#include "main.h"
#include "position.h"

class SpatialVector
{
    public:
    float direction = 0;
    float magnitude = 0;

    SpatialVector();
    SpatialVector(float dir, float mag);
    SpatialVector operator+(SpatialVector dest);
    SpatialVector operator-(SpatialVector dest);
    SpatialVector VectorBetween(Position& orig, Position& dest);
};

#endif