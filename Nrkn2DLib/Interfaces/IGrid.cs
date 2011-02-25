using System;
using System.Collections.Generic;

namespace Nrkn2DLib.Interfaces {
  public interface IGrid {
    int Width { get; }
    int Height { get; }
    ISize Size { get; }
    IRectangle Bounds { get; }
  }

  public interface IGrid<T> : IGrid {
    string ToString( Func<T, string> converter );
    T this[ IPoint point ] { get; set; }
    T this[ int x, int y ] { get; set; }
    void ForEach( Action<T> action );
    void ForEach( Action<T, int, int> action );
    void ForEach( Action<int, int> action );
    void ForEach( Action<T, IPoint> action );
    void ForEach( Action<IPoint> action );
    void SetEach( Func<T> func );
    void SetEach( Func<T, int, int, T> func );
    void SetEach( Func<T, T> func );
    IEnumerable<T> Cells { get; set; }
  }
}