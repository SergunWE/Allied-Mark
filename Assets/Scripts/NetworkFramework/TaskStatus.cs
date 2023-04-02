using System;

namespace NetworkFramework
{
    public class TaskStatus
    {
        public bool Success { get; }
        public Exception Exception { get; }

        public TaskStatus(bool success, Exception exception = null)
        {
            Success = success;
            Exception = exception;
        }
    }
}