using System;
using System.ComponentModel.DataAnnotations;

namespace Source
{
    public class Student
    {
        public Guid Id { get; set; }
        [MaxLength(24)]
        public string Name { get; set; }
    }
}
