#if NETSTANDARD2_1
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Digbyswift.Web.Mvc
{
    public class ViewRenderer : IViewRenderer
    {
        private readonly IRazorViewEngine _viewEngine;

        public ViewRenderer(IRazorViewEngine viewEngine) => _viewEngine = viewEngine;

        public async Task<string> RenderAsStringAsync<TModel>(Controller controller, string viewName, TModel model)
        {
            var viewEngineResult = _viewEngine.FindView(controller.ControllerContext, viewName, false);
            if (!viewEngineResult.Success)
                throw new InvalidOperationException($"Could not find view: {viewName}");

            var view = viewEngineResult.View;
            controller.ViewData.Model = model;

            using var writer = new StringWriter();
            var viewContext = new ViewContext(
                controller.ControllerContext,
                view,
                controller.ViewData,
                controller.TempData,
                writer,
                new HtmlHelperOptions());

            await view.RenderAsync(viewContext);

            return new HtmlString(writer.ToString()).Value;
        }

        public async Task<string> RenderAsStringAsync<TModel>(ViewComponent component, string viewName, TModel model)
        {
            var viewEngineResult = _viewEngine.FindView(component.ViewContext, viewName, false);
            if (!viewEngineResult.Success)
                throw new InvalidOperationException($"Could not find view: {viewName}");

            var view = viewEngineResult.View;
            component.ViewData.Model = model;

            using var writer = new StringWriter();
            var viewContext = new ViewContext(
                component.ViewContext,
                view,
                component.ViewData,
                component.TempData,
                writer,
                new HtmlHelperOptions());

            await view.RenderAsync(viewContext);

            var output = writer.ToString();
            
            return new HtmlString(output).Value;
        }
    }
}
#endif