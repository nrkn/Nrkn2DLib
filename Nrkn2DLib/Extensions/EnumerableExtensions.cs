﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Nrkn2DLib.Extensions {
  public static class EnumerableExtensions {
    public static IEnumerable<Tuple<T, T>> Pairs<T>( this IEnumerable<T> values ) {
      var pairs = new List<Tuple<T, T>>();
      for( var i = 0; i < values.Count() - 1; i++ ) {
        pairs.Add( new Tuple<T, T>( values.ToList()[ i ], values.ToList()[ i + 1 ] ) );
      }
      return pairs; 
    }
    
    public static IEnumerable<T> Partition<T>( this IEnumerable<T> values, int size, int partition ) {
      if( size < 1 ) throw new ArgumentOutOfRangeException( "size" );

      var take = size < values.Count() ? size : values.Count();
      return values.Skip( size * partition ).Take( take );
    }

    public static IEnumerable<IEnumerable<T>> Partitions<T>( this IEnumerable<T> values, int size ) {
      if( size < 1 ) throw new ArgumentOutOfRangeException( "size" );

      var partitions = new List<List<T>>();
      var count = Math.Ceiling( (double) values.Count() / size );

      for( var page = 0; page < count; page++ ) {
        partitions.Add( values.Partition( size, page ).ToList() );
      }

      return partitions;
    }

    public static IEnumerable<double> Normalize( this IEnumerable<double> values ) {
      var min = values.Min();
      var max = values.Max();
      var range = max - min;
      var ratio = 1.0 / range;
      return values.Select( x => ( x - min ) * ratio );
    }
  }
}