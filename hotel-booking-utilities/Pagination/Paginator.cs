using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_utilities.Pagination
{
    public static class Paginator
    {
        public static async Task<PageResult<IEnumerable<TDestination>>> PaginationAsync<TSource, TDestination>(this IQueryable<TSource> querable, int pageSize, int pageNumber, IMapper mapper)
            where TSource : class
            where TDestination : class
        {
            var count = querable.Count();
            var pageResult = new PageResult<IEnumerable<TDestination>>
            {
                Items = (pageSize > 10 || pageSize < 1) ? 10 : pageSize,
                Page = pageNumber > 1 ? pageNumber : 1,
                PreviousPage = pageNumber > 0 ? pageNumber - 1 : 0
            };
            pageResult.Count = count;
            pageResult.NumberOfPages = count % pageResult.Items != 0
                    ? count / pageResult.Items + 1
                    : count / pageResult.Items;
            var sourceList = await querable.Skip((pageResult.Page - 1) * pageResult.Items).Take(pageResult.Items).ToListAsync();
            var destinationList = mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(sourceList);
            pageResult.PageItems = destinationList;
            return pageResult;
        }
    }
}
