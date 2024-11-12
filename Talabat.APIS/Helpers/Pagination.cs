using Talabat.APIS.DTOs;

namespace Talabat.APIS.Helpers
{
    public class Pagination <T>
    {
      
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }
        public Pagination(int pagesize, int pageIndex,int count, IReadOnlyList<T> data)
        {
            PageSize = pagesize;
            PageIndex = pageIndex;
            Count = count;
            Data = data;
        }

    }
}
