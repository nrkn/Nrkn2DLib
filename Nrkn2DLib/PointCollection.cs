using System.Collections.Generic;
using Nrkn2DLib.Interfaces;

namespace Nrkn2DLib {
  public class PointCollection : IPointCollection {
    public PointCollection() {
      Points = new List<IPoint>();
    }

    public IEnumerable<IPoint> Points { get;set; }
  }
}
