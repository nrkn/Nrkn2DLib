using System.Collections.Generic;
using System.Linq;

namespace Nrkn2DLib {
  public struct Line {
    public Line( Point start, Point end ) {
      Start = start;
      End = end; 
    }

    public Point Start;
    public Point End;

    public IEnumerable<int> Verticals {
      get { return new[] {Start.X, End.X}; }
    }

    public IEnumerable<int> Horizontals {
      get { return new[] {Start.Y, End.Y}; }
    }

    public Line Normalized {
      get {
        return new Line( 
          new Point( Verticals.Min(), Horizontals.Min()), 
          new Point( Verticals.Max(), Horizontals.Max()) 
        );
      }
    }
  }
}