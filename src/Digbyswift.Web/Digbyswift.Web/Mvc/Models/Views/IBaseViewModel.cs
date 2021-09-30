namespace Digbyswift.Web.Mvc.Models.Views
{
    public interface IBaseViewModel<out T>
    {
        T Content { get; }
    }
}