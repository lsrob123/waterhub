using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace WaterHub.Core.Models
{
    public class ProcessResult : ProcessResult<object>
    {
        public ProcessResult AsError(HttpStatusCode status, object data = default, string message = null,
            string errorCodeInLog = null)
        {
            return Extensions.AsError(this, new Exception(message), errorCodeInLog, status, data) as ProcessResult;
        }

        public ProcessResult AsError(Exception exception, string errorCodeInLog, HttpStatusCode status,
            object data = default)
        {
            return Extensions.AsError(this, exception, errorCodeInLog, status, data) as ProcessResult;
        }

        public ProcessResult AsErrors(IEnumerable<Exception> exceptions, string errorCodeInLog,
            HttpStatusCode status, object data = default)
        {
            return Extensions.AsErrors(this, exceptions, errorCodeInLog, status, data) as ProcessResult;
        }

        public ProcessResult AsOk(object data = default)
        {
            return Extensions.AsOk(this, data);
        }
    }

    public class ProcessResult<T>
    {
        public virtual T Data { get; set; }
        public string ErrorCodeInLog { get; set; }

        public virtual string ErrorMessage
        {
            get
            {
                var errorCode = string.IsNullOrWhiteSpace(ErrorCodeInLog) ? null : $"[{ErrorCodeInLog}]{Environment.NewLine}";
                return HasErrors
                    ? new StringBuilder(errorCode)
                    .AppendJoin(Environment.NewLine, Errors.Select(x => x.Message)).ToString()
                : null;
            }
        }

        public ICollection<Exception> Errors { get; set; }
        public bool HasErrors => !(Errors is null) && Errors.Any();
        public bool IsOk => Status == HttpStatusCode.OK;
        public HttpStatusCode Status { get; set; }
    }
}