namespace Nrkn2DLib {
  public struct Rectangle {
    public Rectangle( Line line ) {
      _topLeft = line.Normalized.Start;
      _bottomRight = line.Normalized.End;
    }

    private Point _topLeft;
    private Point _bottomRight;

    public int Top { 
      get {
        return _topLeft.Y;
      }
      set {
        _topLeft.Y = value;
      }
    }

    public int Bottom {
      get {
        return _bottomRight.Y;
      }
      set {
        _bottomRight.Y = value;
      }
    }

    public int Left {
      get {
        return _topLeft.X;
      }
      set {
        _topLeft.X = value;
      }
    }

    public int Right {
      get {
        return _bottomRight.X;
      }
      set {
        _bottomRight.X = value;
      }
    }

    public int Width {
      get { return Right - Left; }
    }

    public int Height {
      get { return Bottom - Top; }
    }

    public bool IsEmpty {
      get {
        return _topLeft.Equals( new Point() ) && _topLeft.Equals( _bottomRight );
      }
    }
  }

  public struct Size {
    public Size( int width, int height ) {
      Width = width;
      Height = height;
    }

    public int Width;
    public int Height;
  }
}