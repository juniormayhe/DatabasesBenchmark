using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseModel
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return $"Project: {Id}, {Name}";
        }
    }
}
