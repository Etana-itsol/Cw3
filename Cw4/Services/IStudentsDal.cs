using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wyk4.Models;

namespace Wyk4.Services
{
    public interface IStudentsDal
    {
        public IEnumerable<Student> GetStudents();
    }
}
