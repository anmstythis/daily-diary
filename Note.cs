using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace daily_diary
{
    internal class Note
    {
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Note (DateTime date, string title, string description)
        {
            Date = date;
            Title = title;
            Description = description;
        }
    }
}
