using System.Collections.Generic;
using X.PagedList;

namespace Digbyswift.Web.Net4.Mvc.Models.Views
{
    public class BaseListingViewModel<TContent, TResult> : BaseViewModel<TContent>
    {
        internal static readonly IReadOnlyList<TResult> EmptyResultSet = new List<TResult>();
        
        public IPagedList<TResult> Results { get; set; }

        public BaseListingViewModel()
        {
            Results = new StaticPagedList<TResult>(EmptyResultSet, 1, 10, 0);
        }

        public BaseListingViewModel(TContent content) : base(content)
        {
            Results = new StaticPagedList<TResult>(EmptyResultSet, 1, 10, 0);
        }
    }

    public class BaseListingViewModel<TContent, TResult, TForm> : BaseListingViewModel<TContent, TResult> where TForm : new()
    {
        public TForm FormModel { get; set; } = new TForm();

        public BaseListingViewModel()
        {
            Results = new StaticPagedList<TResult>(EmptyResultSet, 1, 10, 0);
        }

        public BaseListingViewModel(TContent content) : base(content)
        {
            Results = new StaticPagedList<TResult>(EmptyResultSet, 1, 10, 0);
        }
    }
}