using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Digbyswift.Web.Mvc.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString CheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression, bool singleInput = true, object htmlAttributes = null)
        {
            if (!singleInput)
                return htmlHelper.CheckBoxFor(expression, htmlAttributes);

            string checkBoxWithHidden = htmlHelper.CheckBoxFor(expression, htmlAttributes).ToHtmlString().Trim();
            int hiddenInputPosition = checkBoxWithHidden.IndexOf("<input", 1, StringComparison.CurrentCultureIgnoreCase);
            string pureCheckBox = checkBoxWithHidden.Substring(0, hiddenInputPosition);
            return new MvcHtmlString(pureCheckBox);
        }

        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, IEnumerable<TProperty>>> expression, IEnumerable<SelectListItem> multiSelectList, Object htmlAttributes = null)
        {
            // Derive property name for checkbox name
            var body = expression.Body as MemberExpression;
            if (body == null)
                return null;

            String propertyName = body.Member.Name;

            // Get currently select values from the ViewData model
            IEnumerable<TProperty> list = expression.Compile().Invoke(htmlHelper.ViewData.Model);

            // Convert selected value list to a List<String> for easy manipulation
            var selectedValues = new List<String>();

            if (list != null)
                selectedValues = new List<TProperty>(list).ConvertAll(i => i.ToString());

            // Create div
            var ulTag = new TagBuilder("ul");
            ulTag.AddCssClass("checkBoxList");
            ulTag.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);

            if (multiSelectList != null)
            {
                // Add checkboxes
                foreach (var item in multiSelectList)
                {
                    var isChecked = selectedValues.Contains(item.Value);
                    ulTag.InnerHtml += $"<li><input type=\"checkbox\" name=\"{propertyName}\" id=\"{propertyName}_{item.Value}\" value=\"{item.Value}\" {(isChecked ? "checked" : "")} /><label for=\"{propertyName}_{item.Value}\">{item.Text}</label></li>";
                }
            }

            return MvcHtmlString.Create(ulTag.ToString());
        }

    }
}