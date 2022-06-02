namespace Digbyswift.Web.Net4.Mvc.Models.Views
{
    public class BaseFormViewModel<TContent, TForm> : BaseViewModel<TContent> where TForm : new()
    {
        public TForm FormModel { get; set; } = new TForm();

        public BaseFormViewModel(TContent content) : base(content)
        {
        }
    }
}