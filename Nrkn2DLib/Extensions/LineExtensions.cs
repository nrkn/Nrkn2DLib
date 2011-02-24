using System.Collections.Generic;

namespace Nrkn2DLib.Extensions {
  public static class LineExtensions {
    public static IEnumerable<Point> Bresenham( this Line line ) {
      var deltaX = line.End.X.Delta( line.Start.X );
      var deltaY = line.End.Y.Delta( line.Start.Y );
      var stepX = line.Start.X.Step( line.End.X );
      var stepY = line.Start.Y.Step( line.End.Y );
      var error = deltaX - deltaY;
      var current = line.Start;
      var points = new List<Point>();

      while( true ) {
        points.Add( current );
        if( current.Equals( line.End ) ) break;

        var error2 = 2 * error;

        if( error2 > -deltaY ) {
          error -= deltaY;
          current.X += stepX;
        }

        if( error2 >= deltaX ) continue;

        error += deltaX;
        current.Y += stepY;
      }
      return points;
    }

    public static IEnumerable<Point> DrunkenWalk( this Line line, double drunkenness, Rectangle bounds = new Rectangle() ) {
      var current = line.Start;
      var points = new List<Point> { current };
      while( !current.Equals( line.End ) ) {
        var oldLocation = new Point( current.X, current.Y );

        //are we drunk? go in a random direction.
        if( RandomHelper.Random.NextDouble() < drunkenness ) {
          if( RandomHelper.Random.NextDouble() > 0.5 ) {
            current.X += RandomHelper.Random.Next( 2 ) == 1 ? 1 : -1;
          }
          else {
            current.Y += RandomHelper.Random.Next( 2 ) == 1 ? 1 : -1;
          }
        }
        //no? we still stagger randomly but only in a direction we need to go in anyway
        else {
          var deltaX = current.X.Delta( line.End.X );
          var deltaY = current.Y.Delta( line.End.Y );

          if( RandomHelper.Random.NextDouble() < ( 1.0 / ( deltaX + deltaY ) ) * deltaX ) {
            current.X = current.X + current.X.Step( line.End.X );
          }
          else {
            current.Y = current.Y + current.Y.Step( line.End.Y );
          }
        }

        if( !bounds.IsEmpty ) {
          if( current.X < bounds.Left || current.X > bounds.Right || current.Y < bounds.Top || current.Y > bounds.Bottom ) {
            current = oldLocation;
            continue;
          }
        }

        points.Add( current );
      }
      return points;
    }
  }
}