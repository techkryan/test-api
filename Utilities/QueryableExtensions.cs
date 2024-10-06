using System.Linq.Expressions;

namespace MusicCatalog.Api.Utilities;


public static class QueryableExtensions 
{ 
    public static IOrderedQueryable<TSource> OrderByDirected<TSource,TKey>( 
        this IQueryable<TSource> source, 
        Expression<Func<TSource, TKey>> keySelector, 
        bool isDescending = false 
    ) 
    { 
        return isDescending ? source.OrderByDescending(keySelector) 
            : source.OrderBy(keySelector); 
    } 
}  
