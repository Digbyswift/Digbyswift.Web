using System;

namespace Digbyswift.Web.Mvc.Models.Forms
{
    [Serializable]
    public class FormResult
    {
        public FormResultState Status { get; set; }
        public string Message { get; set; }
        public bool HasMessage => !String.IsNullOrWhiteSpace(Message);
        public bool IsSuccess => Status == FormResultState.Success;

        public static FormResult Success(string message = null) => new FormResult { Message = message };
        public static FormResult Invalid(string message) => new FormResult { Message = message, Status = FormResultState.Invalid };
        public static FormResult Error(string message) => new FormResult { Message = message, Status = FormResultState.Error};
    }

    public enum FormResultState
    {
        Success,
        Invalid,
        Error
    }
}