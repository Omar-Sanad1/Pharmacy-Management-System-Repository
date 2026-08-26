using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    public class UnAuthorizedException : Exception
    {
        public UnAuthorizedException(string Message) : base(Message) { }
    }
}
