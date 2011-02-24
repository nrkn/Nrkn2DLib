using System;
using System.Linq;
using Nrkn2DLib;
using Nrkn2DLib.Extensions;

namespace ConsoleDemo {
  class Program {
    static void Main( string[] args ) {
      //generate a forest using some noise and put two rough paths through it, one running from top left to bottom right and one from bottom left to top right
      string command;
      do {
        Console.Clear();
        var noisyGrid = new Grid<double>( 78, 23 ).NoiseFill( 6 );

        var line1 = new Line( new Point( 1, 1 ), new Point( 77, 22 ) );
        var line2 = new Line( new Point( 1, 22 ), new Point( 77, 1 ) );

        //guarantee some walkable paths
        var lines = new[] { line1, line2 };
        foreach( var point in lines.SelectMany( line => line.DrunkenWalk( 0.8, noisyGrid.Bounds ) ) ) {
          noisyGrid[ point ] = 1;
        }

        Console.WriteLine( 
          noisyGrid.ToString( DoubleToForestItem ) 
        );

        Console.Write( "Q to quit or enter to regenerate >" );
      } while( ( command = Console.ReadLine() ) != "Q" && command != "q" );
    }

    static string DoubleToForestItem( double value ) {
      return RandomHelper.Random.NextDouble() > value ?
        value < 0.5 ?
          "T" : "t"
      : ".";
    }
  }
}
