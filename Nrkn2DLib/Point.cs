using Nrkn2DLib.Interfaces;

namespace Nrkn2DLib {
  public struct Point : IPoint {
    public Point( int x, int y ) : this() {
      X = x;
      Y = y; 
    }

    public int X { get; set; }
    public int Y { get; set; }
  }
}