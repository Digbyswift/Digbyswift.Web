namespace Digbyswift.Web.Mvc.Models.Views
{
    public class BaseViewModel<T> : IBaseViewModel<T>
    {
        public T Content { get; set; }

        public BaseViewModel()
        {
        }
        
        public BaseViewModel(T content)
        {
            Content = content;
        }

    }
}