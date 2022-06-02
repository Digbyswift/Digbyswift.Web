namespace Digbyswift.Web.Net4.Mvc.Models.Views
{
    public interface IBaseViewModel<out T>
    {
        T Content { get; }
    }
}