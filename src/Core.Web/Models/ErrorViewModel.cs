using System;
using System.Collections.Generic;

namespace Core.Web.Models
{
    public class ErrorViewModel
    {
        public int ErroCode { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }

    public class ResponseResult
    {
        public ResponseResult()
        {
            Errors = new ResponseErrorMessages();
        }

        public string Title { get; set; }
        public int Status { get; set; }
        public ResponseErrorMessages Errors { get; set; }
    }

    public class ResponseErrorMessages
    {
        public ResponseErrorMessages()
        {
            Messages = new List<string>();
        }

        public List<string> Messages { get; set; }
    }

    public class DeleteResponseMessage
    {
        public string message { get; set; }
        public bool success { get; set; }
    }
}
