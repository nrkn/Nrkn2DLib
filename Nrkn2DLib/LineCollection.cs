using System.Collections.Generic;
using Nrkn2DLib.Interfaces;

namespace Nrkn2DLib {
  public class LineCollection : ILineCollection {
    public LineCollection() {
      Points = new List<IPoint>();
      Lines = new List<ILine>();
    }
    public IEnumerable<IPoint> Points { get; private set; }
    public IEnumerable<ILine> Lines { get; set; }
  }
}
