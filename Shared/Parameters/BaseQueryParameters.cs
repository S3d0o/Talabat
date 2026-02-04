using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Parameters
{
    public class BaseQueryParameters<TSortEnum> where TSortEnum : Enum
    {
        public string? Search { get; set; }
        public TSortEnum? Sort { get; set; }
        public int PageIndex { get; set; } = 1;

        private const int MaxPageSize = 10;
        private const int DefaultPageSize = 5;
        private int _pageSize = DefaultPageSize;

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }

        // Generic filter bag (optional) - enhancement: allow typed filters
        public Dictionary<string, string?>? Filters { get; set; }
    }
}
