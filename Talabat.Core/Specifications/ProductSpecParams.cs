using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications
{
    public class ProductSpecParams
    {
        public string? SearchByName { get; set; }
        public string? sort { get; set; }
        public int? brandId { get; set; }
        public int? categoryId { get; set; }
        public int pageIndex { get; set; }
        private const int MaxPageSize = 10;
        private int pageSize;

        public int pagesize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }

    }
}
