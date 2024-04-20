using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Day02.Models;
using Day02.DTO;
using Swashbuckle.AspNetCore.Annotations;
using Day02.UnitOfWorks;
using Microsoft.AspNetCore.Authorization;


namespace Day02.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class StudentsController : ControllerBase
    {
        UnitOfWork unitOfWork;
        public StudentsController(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        /// <summary>
        /// Method to return students data with Department name
        /// </summary>
        /// <returns>
        /// List of StudentDTO 
        /// </returns>
        // GET: api/Students
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudents()
        {
            return unitOfWork.StudentRepository.GetAll().Select(sts => new StudentDTO
            {
                Id = sts.St_Id,
                FullName = sts.St_Fname + " " + sts.St_Lname,
                Address = sts.St_Address,
                Age = sts.St_Age,
                DepartmentName = sts?.Dept?.Dept_Name,
                SupervisorName = sts?.St_superNavigation?.St_Fname,
                // SupervisorName = _context.Students.FirstOrDefault(s=> s.St_Id == sts.St_super).St_Fname,
            }).ToList();
        }

        /// <summary>
        /// Method to make pagination of students data
        /// </summary>
        /// <param name="page">Page where from start pagenation</param>
        /// <param name="pageSize">The size of your page you want</param>
        /// <returns>
        /// List of StudentDTO 
        /// </returns>
        [HttpGet("{page:int},{pageSize:int}")]
        public ActionResult Get(int page = 1, int pageSize = 10)
        {
            var q = unitOfWork.StudentRepository.GetAll().Select(sts => new StudentDTO
            {
                Id = sts.St_Id,
                FullName = sts.St_Fname + " " + sts.St_Lname,
                Address = sts.St_Address,
                Age = sts.St_Age,
                DepartmentName = sts?.Dept?.Dept_Name,
                SupervisorName = sts?.St_superNavigation?.St_Fname,
                // SupervisorName = _context.Students.FirstOrDefault(s=> s.St_Id == sts.St_super).St_Fname,
            }).ToList();

            if(q == null)
                return NotFound();

            var totalCount = q.Count();
            var pageCount = (int)Math.Ceiling((double)totalCount/pageSize);

            q = q.Skip((page-1) * pageSize).Take(pageSize).ToList();

            return Ok(q);
        }

        [HttpGet("{page:int},{pageSize:int},{name:alpha}")]
        public ActionResult Search(int page = 1, int pageSize = 10,string name = null)
        {
            var q = unitOfWork.StudentRepository.GetByName(S=>S.St_Fname==name).Select(sts => new StudentDTO
            {
                Id = sts.St_Id,
                FullName = sts.St_Fname + " " + sts.St_Lname,
                Address = sts.St_Address,  
                Age = sts.St_Age,
                DepartmentName = sts?.Dept?.Dept_Name,
                SupervisorName = sts?.St_superNavigation?.St_Fname,
                // SupervisorName = _context.Students.FirstOrDefault(s=> s.St_Id == sts.St_super).St_Fname,
            }).ToList();

            if (q == null)
                return NotFound();

            var totalCount = q.Count();
            var pageCount = (int)Math.Ceiling((double)totalCount / pageSize);

            q = q.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Ok(q);
        }

       
        // GET: api/Students/5
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(
            summary : "Method to get Student By Id",
            description : "Take Id then search for student"
            )]
        [SwaggerResponse(201,"Student Created",typeof(Student))]
        [SwaggerResponse(404,"Student Not found")]
        public async Task<ActionResult<StudentDTO>> GetStudent(int id)
        {
            var student = unitOfWork.StudentRepository.GetById(id);

            var studentDto = new StudentDTO()
            {
                Id = student.St_Id,
                FullName = student.St_Fname + " " + student.St_Fname,
                Address = student.St_Address,
                Age = student.St_Age,
                DepartmentName = student?.Dept?.Dept_Name,
                SupervisorName = student?.St_superNavigation?.St_Fname
            };
            
            if (student == null)
            {
                return NotFound();
            }


            return studentDto;
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.St_Id)
            {
                return BadRequest();
            }

            unitOfWork.StudentRepository.Update(student);

            try
            {
                unitOfWork.saveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw ;
            }

            return NoContent();
        }

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            unitOfWork.StudentRepository.add(student);
            try
            {
                unitOfWork.saveChanges();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return CreatedAtAction("GetStudent", new { id = student.St_Id }, student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = unitOfWork.StudentRepository.GetById(id);

            if (student == null)
            {
                return NotFound();
            }

            unitOfWork.StudentRepository.delete(id);

            unitOfWork.saveChanges();

            return NoContent();
        }

    }
}
