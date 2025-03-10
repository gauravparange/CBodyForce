using System.Linq.Expressions;
using System.Reflection;

namespace BodyForce
{
        public class PaginationParams
        {
            public int pagenumber { get; set; }
            public int pagesize { get; set; }
            public string? SortColumn { get; set; }
            public bool IsSortAscending { get; set; }
            public Dictionary<string, string> Filters { get; set; }
            public string? SearchTerm { get; set; }
        }
        public static class PaginationExtensions
        {
            public static (IQueryable<T> Results, int TotalCount, int FilterCount, int StartingNo, int EndingNo)
                Paginate<T>(this IQueryable<T> query, PaginationParams model) where T : class
            {
                if (!string.IsNullOrEmpty(model.SortColumn))
                {
                    query = ApplySorting(query, model);
                }
                if (model.Filters != null && model.Filters.Count > 0)
                {
                    query = ApplyFiltering(query, model);
                }
                if (!string.IsNullOrEmpty(model.SearchTerm))
                {
                    query = ApplySearch(query, model);
                }
                // Store the filtered/searched/sorted query 
                var filteredQuery = query;

                // Get count before pagination
                var totalCount = filteredQuery.Count();

                var starting_no = (model.pagenumber - 1) * model.pagesize + 1;
                var paginatedData_count = filteredQuery.Skip((model.pagenumber - 1) * model.pagesize)
                             .Take(model.pagesize).Count();
                var ending_no = starting_no + paginatedData_count - 1;

                // Paginate  
                return (filteredQuery.Skip((model.pagenumber - 1) * model.pagesize)
                            .Take(model.pagesize),
                        totalCount,
                        filteredQuery.Count(),
                        starting_no,
                        ending_no); // Filtered count
            }

            private static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, PaginationParams model)
                where T : class
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var property = Expression.Property(parameter, model.SortColumn);
                var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), parameter);

                if (model.IsSortAscending)
                {
                    return query.OrderBy(lambda);
                }
                else
                {
                    return query.OrderByDescending(lambda);
                }
            }

            private static IQueryable<T> ApplyFiltering<T>(this IQueryable<T> query, PaginationParams model)
                where T : class
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var predicates = new List<Expression>();

                foreach (var filter in model.Filters)
                {
                    var property = Expression.Property(parameter, filter.Key);
                    var constant = Expression.Constant(filter.Value);
                    var predicate = Expression.Equal(property, constant);
                    predicates.Add(predicate);
                }

                var combinedPredicate = predicates.Aggregate(Expression.AndAlso);
                var lambda = Expression.Lambda<Func<T, bool>>(combinedPredicate, parameter);

                return query.Where(lambda);
            }

            private static IQueryable<T> ApplySearch<T>(this IQueryable<T> query, PaginationParams model)
              where T : class
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var predicates = new List<Expression>();

                var trimmedSearchTerm = model.SearchTerm?.Trim();

                if (!string.IsNullOrWhiteSpace(trimmedSearchTerm))
                {
                    var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                        .Where(p => p.PropertyType == typeof(string));

                    foreach (var property in properties)
                    {
                        var propertyExpression = Expression.Property(parameter, property);
                        var isNullExpression = Expression.Equal(propertyExpression, Expression.Constant(null, typeof(string)));
                        var containsExpression = GetContainsExpression(propertyExpression, trimmedSearchTerm);

                        var nullCaseExpression = Expression.AndAlso(isNullExpression, Expression.Constant(false));
                        var nonNullCaseExpression = Expression.AndAlso(Expression.Not(isNullExpression), containsExpression);

                        var combinedExpression = Expression.OrElse(nullCaseExpression, nonNullCaseExpression);

                        predicates.Add(combinedExpression);
                    }
                }

                if (predicates.Count == 0)
                {
                    return query;
                }

                var combinedPredicate = predicates.Aggregate(Expression.Or);
                var lambda = Expression.Lambda<Func<T, bool>>(combinedPredicate, parameter);

                var returnData = query.Where(lambda);
                return returnData;
            }

            private static Expression GetContainsExpression(Expression propertyExpression, string searchTerm)
            {
                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string), typeof(StringComparison) });
                var searchTermExpression = Expression.Constant(searchTerm);
                var comparisonType = Expression.Constant(StringComparison.OrdinalIgnoreCase);
                var containsExpression = Expression.Call(propertyExpression, containsMethod, searchTermExpression, comparisonType);

                return containsExpression;
            }
        }
    }
