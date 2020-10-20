#include "main.h"

int main(int argc, const char* argv[])
{
    int input = 0;
    Maze theMaze; // Maze object to contain maze data and provide navigation

    cout << "Welcome to the Mazorium!" << endl << endl;

    // Gather maze parameters
    if(argc > 0)
    {
        // TODO: Parse command line parameters for maze details
    }
    else
    {
        // Prompt user for maze details
        cout << "Let's create a maze.  How many columns should it have?" << endl << "columns > ";
        cin >> input;
        while(input < 1 || input > 100)
        {
            cout << "Let's keep it between 1 and 100..." << endl << "columns >";
            cin >> input;
        }

        theMaze.SetWidth(input);

        cout << "Great, and how many rows?" << endl << "rows >";
        cin >> input;

        while(input < 1 || input > 100)
        {
            cout << "Let's keep it between 1 and 100..." << endl << "rows >";
            cin >> input;
        }

        theMaze.SetHeight(input);
    }

    // Create cell graph
    for(int row = 0; row < theMaze.Height(); row++)
    {
        for(int col = 0; col < theMaze.Width(); col++)
        {
            Cell* cell = theMaze.AddCell(col, row, 1);
            
            // Add cell above to adjacents
            if(0 < row)
            {
                theMaze.MakeNeighbors(cell, theMaze.CellAt(row - 1, col, 1));
            }

            // Add cell to the left
            if(0 < col)
            {

            }
        }
    }

    // Generate maze and cell graph
    theMaze.GeneratePaths();

    // TODO: Display maze

    // TODO: Enable navigation of maze
}