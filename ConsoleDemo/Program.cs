using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nrkn2DLib;
using Nrkn2DLib.Extensions;
using Nrkn2DLib.Interfaces;

namespace ConsoleDemo {
  class Program {
    static void Main( string[] args ) {
      //generate a forest using some noise and put two rough paths through it, one running from top left to bottom right and one from bottom left to top right
      var command = String.Empty;
      var noisyGrid = new Grid<double>( 78, 23 );
      do {
        if( command == "W" ) File.WriteAllText( "screen.pgm", noisyGrid.ToPgm() );
        Console.Clear();
        noisyGrid = (Grid<double>) new Grid<double>( 78, 23 ).NoiseFill( 5 );

        noisyGrid.Cells = noisyGrid.Cells.Normalize();

        var path = new Grid<double>( 78, 23 );
        var line1 = new Line( new Point( 1, 1 ), new Point( 77, 22 ) );
        var line2 = new Line( new Point( 1, 22 ), new Point( 77, 1 ) );

        //guarantee some walkable paths
        var lines = new[] { line1, line2 };
        foreach( var point in lines.SelectMany( line => line.DrunkenWalk( 0.5, noisyGrid.Bounds ) ) ) {
          path[ point ] = 1;
        }

        var river = new Grid<double>( 78, 23 );
        foreach( var point in new Line( new Point( 18, 1 ), new Point( 58, 22 )).DrunkenWalk( 0.75, river.Bounds )) {
          river[ point ] = 1;
        }

        var walls = new Grid<double>( 78, 23 );
        var building = new Rectangle( new Size( 5, 6 ) );
        //place one in top left
        var buildingPoints = new List<IPoint>();
        foreach( var line in building.Lines ) {
          buildingPoints.AddRange( line.Bresenham() );
        }

        foreach( var line in building.Lines.Translate( new Point( 10, 7 )) ) {
          buildingPoints.AddRange( line.Bresenham() );  
        }

        var rotated1 = new Point( 50, 7 );
        foreach( var line in building.Lines.Translate( rotated1 ).Rotate( 45, rotated1 ) ) {
          buildingPoints.AddRange( line.Bresenham() );
        }
       
        var rotated3 = new Point( 35, 7 );
        foreach( var line in building.Lines.Translate( rotated3 ).Rotate( 90, rotated3 ) ) {
          buildingPoints.AddRange( line.Bresenham() );
        }

        foreach( var point in buildingPoints ) {
          walls[ point ] = 1;
        }

        noisyGrid.ForEach( ( value, point ) => {
          Console.BackgroundColor =
            walls[ point ] == 1 ? ConsoleColor.Gray
            : path[ point ] == 1 && river[ point] == 1 ? ConsoleColor.DarkYellow 
            : path[ point ] == 1 ? ConsoleColor.Green
            : river[ point ] == 1 ? ConsoleColor.DarkBlue
            : ConsoleColor.DarkGreen;

          var color = RandomHelper.Random.NextDouble();

          Console.ForegroundColor =
            walls[ point ] == 1 ? ConsoleColor.DarkGray
            : path[ point ] == 1 && river[ point] == 1 ? ConsoleColor.DarkRed
            : path[ point ] == 1 ? ConsoleColor.DarkGreen
            : river[ point ] == 1 ? ConsoleColor.Blue
            : color < 0.75 ? ConsoleColor.Green 
            : color < 0.96 ? ConsoleColor.Yellow 
            : color < 0.97 ? ConsoleColor.DarkRed 
            : color < 0.98 ? ConsoleColor.DarkMagenta 
            : ConsoleColor.DarkCyan;


          Console.Write( 
            walls[ point ] == 1 && path[ point ] == 1 ? "+"
            : walls[ point ] == 1 ? "#" 
            : path[ point ] == 1 && river[ point] == 1 ? "=" 
            : path[ point ] == 1 ? "." 
            : river[ point ] == 1 ? "~" 
            : DoubleToForestItem( value ) 
          );

          Console.BackgroundColor = ConsoleColor.Black;
          if( point.X == noisyGrid.Width - 1 ) Console.WriteLine();          
        } );

        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write( "Q to quit or enter to regenerate >" );
      } while( ( command = Console.ReadLine() ) != "Q" && command != "q" );
    }



    static string DoubleToForestItem( double value ) {
      return 
        RandomHelper.Random.NextDouble() > value ?
          value < 0.5 ? "♠" 
          : value < 0.6 ? "♣" 
          : value < 0.7 ? "T" 
          : "t" 
        : ".";
    }
  }
}
