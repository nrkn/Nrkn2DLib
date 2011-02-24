using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nrkn2DLib {
  public class Grid<T> {
    public Grid( int width = 0, int height = 0 ) {
      _grid = new List<List<T>>();
      _width = width;
      _height = height;

      if( Width == 0 || Height == 0 ) return;

      Initialize(); 
    }

    public int Width {
      get { return _width; }
    }

    public int Height {
      get { return _height; }
    }

    public Rectangle Bounds {
      get {
        return new Rectangle {
          Top = 0,
          Left = 0,
          Right = Width - 1,
          Bottom = Height - 1
        }; 
      }
    }

    private void Initialize() {
      _grid.Clear();
      for( var y = 0; y < Height; y++ ) {
        _grid.Add( new List<T>( Width ) );
        for( var x = 0; x < Width; x++ ) {
          _grid[ y ].Add( default( T ) );
        }
      }
    }

    private readonly int _width;
    private readonly int _height;
    private readonly List<List<T>> _grid;

    public override string ToString() {
      var builder = new StringBuilder();
      foreach( var row in _grid ) {
        foreach( var cell in row ) {
          builder.Append( cell.ToString() );
        }
        builder.AppendLine();
      }
      return builder.ToString();
    }

    public string ToString( Func<T,string> converter ) {
      var builder = new StringBuilder();
      foreach( var row in _grid ) {
        foreach( var cell in row ) {
          builder.Append( converter( cell ) );
        }
        builder.AppendLine();
      }
      return builder.ToString();
    }

    public void SetEach( Func<T,T> action ) {
      for( var y = 0; y < Height; y++ ) {
        for( var x = 0; x < Width; x++ ) {
          _grid[ y ][ x ] = action( _grid[ y ][ x ] );
        }
      }     
    }

    public void SetEach( Func<T, int, int, T> action ) {
      for( var y = 0; y < Height; y++ ) {
        for( var x = 0; x < Width; x++ ) {
          _grid[ y ][ x ] = action( _grid[ y ][ x ], x, y );
        }
      }
    }

    public void ForEach( Action<T> action ) {
      for( var y = 0; y < Height; y++ ) {
        for( var x = 0; x < Width; x++ ) {
          action( _grid[ y ][ x ] );
        }
      }
    }

    public void ForEach( Action<T, int, int> action ) {
      for( var y = 0; y < Height; y++ ) {
        for( var x = 0; x < Width; x++ ) {
          action( _grid[ y ][ x ], x, y );
        }
      }
    }

    public IEnumerable<T> Cells {
      get {
        var cells = new List<T>();
        ForEach( cells.Add );
        return cells;
      }
      set {
        var stack = new Stack<T>( value.Reverse() );
        SetEach( c => stack.Count > 0 ? stack.Pop() : default( T ) );
      }
    }

    public T this[ Point point ] {
      get { return this[ point.X, point.Y ]; }
      set { this[ point.X, point.Y ] = value; }
    }

    public T this[ int x, int y ] {
      get{
        return _grid[ y ][ x ];
      }
      set {
        _grid[ y ][ x ] = value;
      }
    }
  }
}