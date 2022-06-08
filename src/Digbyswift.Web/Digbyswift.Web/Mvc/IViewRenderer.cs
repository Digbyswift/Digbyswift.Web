#if NETSTANDARD2_1
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Digbyswift.Web.Mvc
{
    public interface IViewRenderer
    {
        Task<string> RenderAsStringAsync<TModel>(Controller controller, string name, TModel model);
        Task<string> RenderAsStringAsync<TModel>(ViewComponent component, string viewName, TModel model);
    }
}
#endif