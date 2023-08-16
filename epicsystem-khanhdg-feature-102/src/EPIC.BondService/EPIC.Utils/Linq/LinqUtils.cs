using DocumentFormat.OpenXml.Spreadsheet;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace EPIC.Utils.Linq
{
    public static class LinqUtils
    {
        /// <summary>
        /// sắp xếp linh động
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public static IQueryable<TEntity> OrderDynamic<TEntity>(this IQueryable<TEntity> query, IList<string> sort) where TEntity : class
        {
            var param = Expression.Parameter(typeof(TEntity), "o");
            var propertyId = typeof(TEntity).GetProperty("Id");
            if (propertyId != null)
            {
                query = query.OrderByDescending(Expression.Lambda<Func<TEntity, object>>(
                    Expression.Convert(Expression.Property(param, propertyId), typeof(object)), param));
            }

            if (sort == null)
            {
                return query;
            }
            
            for (var i = 0; i < sort.Count; i++)
            {
                var item = sort[i].Split('-');

                if (item.Length != 2)
                {
                    continue;
                }

                var order = item[1].Trim();

                Expression propertyAccess = param;

                foreach (string property in item[0].Split('.'))
                {
                    propertyAccess = Expression.PropertyOrField(propertyAccess, property);
                }

                var lambda = Expression.Lambda<Func<TEntity, object>>(Expression.Convert(propertyAccess, typeof(object)), param);

                if (i == 0 && order == OrderByTypes.ASC)
                {
                    query = query.OrderBy(lambda);
                }
                else if (i == 0 && order == OrderByTypes.DESC)
                {
                    query = query.OrderByDescending(lambda);
                }
                else if (order == OrderByTypes.ASC)
                {
                    query = ((IOrderedQueryable<TEntity>)query).ThenBy(lambda);
                }
                else
                {
                    query = ((IOrderedQueryable<TEntity>)query).ThenByDescending(lambda);
                }
            }
            return query;
        }
    }
}
