using System;
using System.Collections.Generic;
using System.Linq;
using WorkTimeManager.Api.Models;

namespace WorkTimeManager.Api.Data
{
    public interface ITaskRepository
    {
        bool SaveChanges();

        bool DeleteTask(int taskId, Guid userId);

        IEnumerable<Task> GetAllTasks(Guid userId);

        IEnumerable<Task> GetTask(int taskId, Guid userId);

        Task UpdateTask(Task updatedTask, Guid userId);

        void CreateTask(Task[] tasks, Guid userId);
    }
}
