﻿using System;
using System.Collections.Generic;
using System.Linq;
using WorkTimeManager.Api.Models;

namespace WorkTimeManager.Api.Data
{
    public class TaskRepository : ITaskRepository
    {
        private ApplicationContext _appContext;

        public TaskRepository(ApplicationContext applicationContext)
        {
            _appContext = applicationContext;
        }

        public void CreateTask(Task[] tasks, Guid userId)
        {
            if (!(tasks.Length > 0) || tasks == null)
                throw new ArgumentNullException(nameof(tasks));

            List<Task> newTasks = new List<Task>();

            foreach(var task in tasks)
            {
                _appContext.Add(task);
            }

        }

        public bool DeleteTask(int taskId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Task> GetAllTasks(Guid userId)
        {
            var task = _appContext.Tasks.Where(x => x.UserId == userId).ToList();

            if (task != null)
                return task;

            return null;
        }

        public IEnumerable<Task> GetTask(int taskId, Guid userId)
        {
            var tasks = _appContext.Tasks.Where(x => x.Id == taskId && x.UserId == userId).ToList();

            if (tasks != null)
                return tasks;

            return null;
        }

        public bool SaveChanges()
        {
            return (_appContext.SaveChanges() >= 0);
        }

        public Task UpdateTask(Task updatedTask, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
