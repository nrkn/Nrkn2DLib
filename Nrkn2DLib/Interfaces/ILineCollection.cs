using System.Collections.Generic;

namespace Nrkn2DLib.Interfaces {
  public interface ILineCollection : IPointCollection {
    IEnumerable<ILine> Lines { get; }
  }
}
