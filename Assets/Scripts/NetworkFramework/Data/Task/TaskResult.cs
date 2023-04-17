using System;

namespace NetworkFramework.Data
{
    /// <summary>
    /// A class for storing the state of a completed task
    /// </summary>
    public class TaskResult
    {
        /// <summary>
        /// The task was completed successfully
        /// </summary>
        public bool Success { get; }
        /// <summary>
        /// The error that prevented the task from being completed
        /// </summary>
        public Exception Exception { get; }
        
        /// <param name="success">Whether the task is completed successfully</param>
        /// <param name="exception">Optional parameter. Error that prevented the task from being performed</param>
        public TaskResult(bool success, Exception exception = null)
        {
            Success = success;
            Exception = exception;
        }

        /// <summary>
        /// A sample for a successfully completed task
        /// </summary>
        public static readonly TaskResult Ok = new(true);
    }
}