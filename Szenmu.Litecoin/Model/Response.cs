using System;
using System.Collections.Generic;
using System.Text;

namespace Szenmu.Litecoin.Model
{
    class Response
    {
        public Error Error { get; set; }
        public string Id { get; set; }
    }

    class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
