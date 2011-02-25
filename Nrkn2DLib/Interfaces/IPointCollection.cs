using System.Collections.Generic;

namespace Nrkn2DLib.Interfaces {
  public interface IPointCollection {
    IEnumerable<IPoint> Points { get; }
  }
}