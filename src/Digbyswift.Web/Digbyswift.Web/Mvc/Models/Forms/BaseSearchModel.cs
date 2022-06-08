#if NETSTANDARD2_1    
using Microsoft.AspNetCore.Mvc;
#else
using System.Web.Mvc;
using Digbyswift.Web.Mvc.Attributes;
using Digbyswift.Web.Mvc.ModelBinders;
#endif

namespace Digbyswift.Web.Mvc.Models.Forms
{
#if NETSTANDARD2_1    
    public class BaseSearchModel
    {
        private const string PageQueryAlias = "p";
        private const string PageSizeAlias = "s";
        
        private int _page;
        [FromQuery(Name = PageQueryAlias)]
        public int Page
        {
            get => _page < 1 ? 1 : _page;
            set => _page = value;
        }

        private int _pageSize;
        [FromQuery(Name = PageSizeAlias)]
        public int PageSize
        {
            get => _pageSize < 1 ? 1 : _pageSize;
            set => _pageSize = value;
        }
    }
#else
    [ModelBinder(typeof(AliasModelBinder))]
    public class BaseSearchModel
    {
        private const string PageQueryAlias = "p";
        private const string PageSizeAlias = "s";

        private int _page;
        [BindAlias(PageQueryAlias)]
        public int Page
        {
            get => _page < 1 ? 1 : _page;
            set => _page = value;
        }

        private int _pageSize;
        [BindAlias(PageSizeAlias)]
        public int PageSize
        {
            get => _pageSize < 1 ? 1 : _pageSize;
            set => _pageSize = value;
        }
    }
#endif
}
