using System;

namespace NetworkFramework.Data
{
    public class TaskResult
    {
        public bool Success { get; }
        public Exception Exception { get; }

        public TaskResult(bool success, Exception exception = null)
        {
            Success = success;
            Exception = exception;
        }

        public static readonly TaskResult Ok = new(true);
    }
}