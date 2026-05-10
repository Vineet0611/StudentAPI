using Core.Base;
using Core.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Student : MetaFields
    {
        [Key]
        public Guid id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public int age { get; set; }
        public string course { get; set; }
        public EntityStatus status { get; set; }
    }
}
