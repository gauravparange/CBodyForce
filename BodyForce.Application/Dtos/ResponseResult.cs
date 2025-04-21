using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyForce
{
    public class ResponseResult
    {
        
        public ResponseResult(bool Success, List<string> ErrorMessages)
        {
            this.Success = Success;
            this.ErrorMessages = ErrorMessages;
        }
        public ResponseResult(bool Success)
        {
            this.Success = Success;
        }
        public bool Success { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();
    }
    public class ResponseResult<T> : ResponseResult
    {
        public ResponseResult(bool success, List<string> errorMessages, T? data = default)
            : base(success, errorMessages)
        {
            Data = data;
        }

        public ResponseResult(bool success, T? data = default)
            : base(success)
        {
            Data = data;
        }

        public T? Data { get; set; }
    }
}
