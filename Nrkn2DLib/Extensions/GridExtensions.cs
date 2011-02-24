using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nrkn2DLib.Extensions {
  public static class GridExtensions {
    public static Grid<T> Interpolate<T>( this Grid<T> grid, Size size ) {
      var xRatio = grid.Width / (double) size.Width;
      var yRatio = grid.Height / (double) size.Height;
      var newGrid = new Grid<T>( size.Width, size.Height );

      newGrid.SetEach( ( c, x, y ) => {
        var pointX = Math.Floor( x * xRatio );
        var pointY = Math.Floor( y * yRatio );
        return grid.Cells.ToList()[ (int) ( ( pointY * grid.Width ) + pointX ) ];          
      } );

      return newGrid; 
    }

    public static Grid<double> Interpolate( this Grid<double> grid, Size size, bool wrap = true ) {
      var xRatio = grid.Width / (double) size.Width;
      var yRatio = grid.Height / (double) size.Height;
      var newGrid = new Grid<double>( size.Width, size.Height );

      newGrid.SetEach( ( c, x, y ) => {
        var pointX = (int) Math.Floor( x * xRatio );
        var pointY = (int) Math.Floor( y * yRatio );

        var ceilingX = pointX + 1;
        if( ceilingX >= grid.Width ) ceilingX = wrap ? 0 : pointX;
        var ceilingY = pointY + 1;
        if( ceilingY >= grid.Height ) ceilingY = wrap ? 0 : pointY;

        var fractionX = x * xRatio - pointX;
        var fractionY = y * yRatio - pointY;

        var oneLessX = 1.0 - fractionX;
        var oneLessY = 1.0 - fractionY;

        var c1 = grid[ pointX, pointY ];
        var c2 = grid[ ceilingX, pointY ];
        var c3 = grid[ pointX, ceilingY ];
        var c4 = grid[ ceilingX, ceilingY ];

        var b1 = oneLessX * c1 + fractionX * c2;
        var b2 = oneLessX * c3 + fractionX * c4;

        return oneLessY * b1 + fractionY * b2;      
      } );


      return newGrid;
    }

    public static Grid<int> Interpolate( this Grid<int> grid, Size size, bool wrap = true ) {
      var doubleGrid = new Grid<double>( grid.Width, grid.Height ) {
        Cells = grid.Cells.Select( g => (double) g )
      };

      var newGrid = doubleGrid.Interpolate( size );

      return new Grid<int>( newGrid.Width, newGrid.Height ) {
        Cells = newGrid.Cells.Select( g => (int) g )
      };
    }

    public static Grid<double> Average( this IEnumerable<Grid<double>> grids ) {
      var gridList = grids.ToList();
      var first = grids.First();
      var averageGrid = new Grid<double>( first.Width, first.Height ) { Cells = first.Cells };
      for( var i = 1; i < gridList.Count; i++ ) {
        var cells = averageGrid.Cells.ToList();
        var newCells = gridList[ i ].Cells.ToList();
        var newCellValues = new List<double>();
        for( var c = 0; c < newCells.Count(); c++ ) {
          newCellValues.Add( ( cells[ c ] + newCells[ c ] ) / 2 );
        }
        averageGrid.Cells = newCellValues;
      }
      return averageGrid;
    }

    public static string ToPgm( this Grid<double> grid ) {
      var builder = new StringBuilder();
      builder.AppendLine( "P2" );
      builder.AppendLine( String.Format( "{0} {1}", grid.Width, grid.Height ) );
      builder.AppendLine( "255" );
      
      grid.ForEach( ( c, x, y ) => {
        builder.Append( Math.Floor( grid[ x, y ] * 255 ) );
        if( x == grid.Width - 1 ) builder.AppendLine(); 
        else builder.Append( " " );
      } );

      return builder.ToString();
    }

    public static Grid<double> NoiseFill( this Grid<double> grid, int levels, bool normalize = false ) {
      var grids = new List<Grid<double>>();

      var currentWidth = grid.Width;
      var currentHeight = grid.Height;
      for( var i = 0; i < levels; i++ ) {
        var newGrid = new Grid<double>( currentWidth, currentHeight );
        newGrid.SetEach( c => RandomHelper.Random.NextDouble() );
        newGrid = newGrid.Interpolate( new Size( grid.Width, grid.Height ) );
        grids.Add( newGrid );
        currentWidth /= 2;
        currentHeight /= 2;
        currentWidth = currentWidth < 1 ? 1 : currentWidth;
        currentHeight = currentHeight < 1 ? 1 : currentHeight;
      }

      var noiseFilled = grids.Average();

      if( normalize ) {
        noiseFilled.Cells = noiseFilled.Cells.Normalize();
      }

      return noiseFilled;
    }
  }
}