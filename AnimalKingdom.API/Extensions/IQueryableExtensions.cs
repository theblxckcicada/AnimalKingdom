using System.Linq.Expressions;
using System.Reflection;

namespace api.Extensions;
public static class IQueryableExtensions
{
    private static PropertyInfo GetPropertyInfo(Type objType, string name)
    {
        var matchedProperty = objType
            .GetProperties()
            .FirstOrDefault(p =>
                string.Equals(name, p.Name, StringComparison.InvariantCultureIgnoreCase)
            );
        if (matchedProperty == null)
        {
            throw new ArgumentException("Property not found", nameof(name));
        }

        return matchedProperty;
    }

    private static LambdaExpression GetOrderExpression(Type objType, PropertyInfo pi)
    {
        var paramExpr = Expression.Parameter(objType);
        var propAccess = Expression.PropertyOrField(paramExpr, pi.Name);
        var expr = Expression.Lambda(propAccess, paramExpr);
        return expr;
    }

    private static IOrderedQueryable<T> OrderByDirection<T>(
        IQueryable<T> query,
        string name,
        bool desc
    )
    {
        var propInfo = GetPropertyInfo(typeof(T), name);
        var expr = GetOrderExpression(typeof(T), propInfo);

        var methodName = desc ? "OrderByDescending" : "OrderBy";

        var method = typeof(Queryable)
            .GetMethods()
            .First(m => m.Name == methodName && m.GetParameters().Length == 2);
        var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);

        var result = (IOrderedQueryable<T>?)
            genericMethod.Invoke(null, new object[] { query, expr });
        if (result == null)
        {
            throw new InvalidOperationException($"Failed to call {methodName}");
        }
        return result;
    }

    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string name)
    {
        return OrderByDirection<T>(query, name, false);
    }

    public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string name)
    {
        return OrderByDirection<T>(query, name, true);
    }
}
