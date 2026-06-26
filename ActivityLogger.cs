using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotPart2
{
    public class ActivityLogger
    {
        private List<string> activities =
            new List<string>();

        public void Add(string action)
        {
            activities.Add(
                DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                + " - "
                + action);
        }

        public List<string> GetRecent()
        {
            return activities
        .Skip(Math.Max(0, activities.Count - 10))
        .ToList();
        }

        public List<string> GetAll()
        {
            return activities;
        }
    }
}