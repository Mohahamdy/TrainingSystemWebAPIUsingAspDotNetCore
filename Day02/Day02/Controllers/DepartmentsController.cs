using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Day02.Models;
using Day02.DTO;
using Day02.UnitOfWorks;

namespace Day02.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly UnitOfWork unitOfWork;

        public DepartmentsController(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDTO>>> GetDepartments()
        {
            return unitOfWork.DepartmentRepository.GetAll().Select(
                d => new DepartmentDTO
                {
                    Id = d.Dept_Id,
                    Name = d.Dept_Name,
                    Location = d.Dept_Location,
                    Description = d.Dept_Desc,
                    MangerId = d.Dept_Manager,
                    StudentsCount = d.Students.Count(),
                }
                ).ToList();
        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            var department = unitOfWork.DepartmentRepository.GetById(id);

            if (department == null)
            {
                return NotFound();
            }

            return department;
        }

        // PUT: api/Departments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment(int id, Department department)
        {
            if (id != department.Dept_Id)
            {
                return BadRequest();
            }

            unitOfWork.DepartmentRepository.Update(department);

            try
            {
                unitOfWork.saveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        // POST: api/Departments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Department>> PostDepartment(Department department)
        {
            unitOfWork.DepartmentRepository.add(department);
            try
            {
                unitOfWork.saveChanges();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return CreatedAtAction("GetDepartment", new { id = department.Dept_Id }, department);
        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = unitOfWork.DepartmentRepository.GetById(id);
            if (department == null)
            {
                return NotFound();
            }

            unitOfWork.DepartmentRepository.delete(id);
            unitOfWork.saveChanges();

            return NoContent();
        }
    }
}