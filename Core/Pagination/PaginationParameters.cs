using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Pagination
{
    public class PaginationParameters
    {
        private const int maxSize = 10;
        public int PageNumber { get; set; } = 1;
        private int pageSize;
        public int PageSize 
        {
            get => pageSize; 
            set => pageSize = value > maxSize ? maxSize : value; 
        }
    }
}
