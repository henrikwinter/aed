using System;

namespace Xapp.Models
{
    public class ErrorModel
    {
        public string ErrorTitle { get; set; }
        public Exception ExceptionDetail { get; set; }

        public ErrorModel()
        {

        }
    }
}