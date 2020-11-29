using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Bll.Validation
{
    public class ValidationMessage
    {
        public string Message { get; set; }
        public string PropertyName { get; set; }
        public ValidationMessageType Type { get; set; }
    }

    public enum ValidationMessageType
    {
        Info,
        Warning,
        Error
    }
}
