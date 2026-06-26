using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotPart2
{
    public class CyberTask
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime ReminderDate { get; set; }

        public bool Completed { get; set; }

        public override string ToString()
        {
            return $"{Title} - {Description}";
        }
    }
}