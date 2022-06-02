using System.Web.Mvc;
using Digbyswift.Web.Net4.Mvc.Attributes;
using Digbyswift.Web.Net4.Mvc.ModelBinders;

namespace Digbyswift.Web.Net4.Mvc.Models.Forms
{
    [ModelBinder(typeof(AliasModelBinder))]
    public class BaseSearchModel
    {
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
            get => _pageSize < 1 ? 1 : _pageSize;
            set => _pageSize = value;
        }
    }
}
