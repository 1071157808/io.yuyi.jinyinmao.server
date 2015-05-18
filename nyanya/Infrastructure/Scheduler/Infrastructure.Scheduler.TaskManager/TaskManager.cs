// FileInformation: nyanya/Infrastructure.Scheduler.TaskManager/TaskManager.cs
// CreatedTime: 2014/04/25   5:37 PM
// LastUpdatedTime: 2014/05/04   10:48 AM

using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Scheduler.Interface;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Scheduler.TaskManager
{
    public class TaskManager
    {
        private readonly Dictionary<string, ISchedulerTask> tasks;

        public TaskManager()
        {
            this.tasks = new Dictionary<string, ISchedulerTask>();
        }

        public IEnumerable<string> TaskNameList
        {
            get { return this.tasks.Keys; }
        }

        public bool AddTask(string taskName, ISchedulerTask task)
        {
            if (this.tasks.ContainsKey(taskName))
            {
                return false;
            }
            this.tasks.Add(taskName, task);
            return true;
        }

        public bool Cancel(string taskName)
        {
            ISchedulerTask value;
            if (this.tasks.TryGetValue(taskName, out value) && value.CanCancel)
            {
                Task.Run(() => value.Cancel());
                return true;
            }

            return false;
        }

        public JObject GetTaskStatus(string taskName)
        {
            JObject jObject = new JObject();
            ISchedulerTask value;
            if (this.tasks.TryGetValue(taskName, out value))
            {
                jObject = JObject.FromObject(value);
            }
            return jObject;
        }

        public bool RunTask(string taskName)
        {
            ISchedulerTask value;
            if (this.tasks.TryGetValue(taskName, out value) && value.CanStart)
            {
                Task.Run(() => value.Run());
                return true;
            }
            return false;
        }
    }
}