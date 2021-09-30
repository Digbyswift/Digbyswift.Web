using System.Web.Mvc;
using Digbyswift.Web.Mvc.Attributes;
using Digbyswift.Web.Mvc.ModelBinders;

namespace Digbyswift.Web.Mvc.Models.Forms
{
    [ModelBinder(typeof(AliasModelBinder))]
    public class BaseSearchModel
    {
        public static int DefaultPageSize = 10;
        
        private int _page;
        [BindAlias("p")]
        public int Page
        {
            get => _page < 1 ? 1 : _page;
            set => _page = value;
        }

        private int _pageSize;
        [BindAlias("s")]
        public int PageSize
        {
            get => _pageSize < DefaultPageSize ? DefaultPageSize : _pageSize;
            set => _pageSize = value;
        }
    }
}
