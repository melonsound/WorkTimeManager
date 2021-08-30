using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WorkTimeManager.Api.Models;

namespace WorkTimeManager.Api.Services
{
    public interface ITaskService
    {
        Task Create(Task task, Guid userId);
        IEnumerable<Task> GetAll(string userId);
    }

    public class TaskService : ITaskService
    {
        private TaskContext _taskContext = null;
        private Task _task = null;


        public TaskService(TaskContext taskContext)
        {
            _taskContext = taskContext;
        }

        public Task Create(Task task, Guid userId)
        {
            if (task == null)
                return null;

            _task = new Task();
            _task.UserId = userId;
            _task.Title = task.Title;
            _task.Deadline = task.Deadline;
            _task.Description = task.Description;
            _task.Subtasks = task.Subtasks;

            _taskContext.Add(_task);
            _taskContext.SaveChanges();

            return _task;
        }

        public IEnumerable<Task> GetAll(string userId)
        {
            var tasks = _taskContext.Tasks.Include(x => x.Subtasks).Where(x => x.UserId.ToString().Equals(userId));


            if (tasks == null)
                return null;

            return tasks;
        }
    }
}
