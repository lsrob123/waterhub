using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace WaterHub.Core.Models
{
    public class ProcessResult : ProcessResult<object>
    {
        public new ProcessResult AsError(HttpStatusCode status, object data = default)
        {
            return base.AsError(new Exception(status.ToString()), null, status, data) as ProcessResult;
        }

        public new ProcessResult AsError(Exception exception, string errorCodeInLog, HttpStatusCode status,
            object data = default)
        {
            return base.AsError(exception, errorCodeInLog, status, data) as ProcessResult;
        }

        public new ProcessResult AsErrors(IEnumerable<Exception> exceptions, string errorCodeInLog,
            HttpStatusCode status, object data = default)
        {
            return base.AsErrors(exceptions, errorCodeInLog, status, data) as ProcessResult;
        }

        public new ProcessResult AsOk(object data = default)
        {
            return base.AsOk(data) as ProcessResult;
        }
    }

    public class ProcessResult<T>
    {
        public virtual T Data { get; set; }
        public string ErrorCodeInLog { get; set; }

        public virtual string ErrorMessage => HasErrors
            ? new StringBuilder($"[{ErrorCodeInLog}]")
                .AppendJoin(Environment.NewLine, Errors.Select(x => x.Message)).ToString()
            : null;

        public ICollection<Exception> Errors { get; set; }
        public bool HasErrors => (Errors is null) && Errors.Any();
        public bool IsOk => Status == HttpStatusCode.OK;
        public HttpStatusCode Status { get; set; }

        public ProcessResult<T> AsError(HttpStatusCode status, T data = default)
        {
            return AsError(new Exception(status.ToString()), null, status, data);
        }

        public virtual ProcessResult<T> AsError(Exception exception, string errorCodeInLog, HttpStatusCode status,
            T data = default)
        {
            Data = data;
            Status = status;
            Errors = new List<Exception> { exception };
            ErrorCodeInLog = errorCodeInLog;
            return this;
        }

        public virtual ProcessResult<T> AsErrors(IEnumerable<Exception> exceptions, string errorCodeInLog,
            HttpStatusCode status, T data = default)
        {
            Data = data;
            Status = status;
            Errors = exceptions.ToList();
            return this;
        }

        public virtual ProcessResult<T> AsOk(T data = default)
        {
            Data = data;
            Status = HttpStatusCode.OK;
            return this;
        }
    }
}