using Day02.Models;
using Day02.Repository;

namespace Day02.UnitOfWorks
{
    public class UnitOfWork
    {
        private readonly ITIContext context;

        GernericRepository<Student> studentRepository;
        GernericRepository<Department> departmentRepository;

        public UnitOfWork(ITIContext context)
        {
            this.context = context;
        }

        public GernericRepository<Student> StudentRepository
        {
            get
            {
                if (studentRepository == null)
                    studentRepository = new GernericRepository<Student>(context);

                return studentRepository;
            }
        }

        public GernericRepository<Department> DepartmentRepository
        {
            get
            {
                if(departmentRepository == null)
                    departmentRepository = new GernericRepository<Department> (context);

                return departmentRepository;
            }
        }

        public void saveChanges()
        {
            context.SaveChanges();
        }
    }
}
