using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAdminPortal.API.DomainModels;
using StudentAdminPortal.API.Repositeries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAdminPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("angularApplications")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;

        public StudentsController(IStudentRepository studentRepository, IMapper mapper)
        {
            this.studentRepository = studentRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await studentRepository.GetStudentsAsync();
            return Ok(mapper.Map<List<StudentDTO>>(students));
        }
        [HttpGet("{studentId:guid}"),ActionName("GetStudentAsync")]
        public async Task<IActionResult> GetStudentAsync([FromRoute] Guid studentId)
        {
            // Fetch Single Student Detail
            var student = await studentRepository.GetStudentAsync(studentId);

            // Return Student
            if (student != null)
            {
                return Ok(mapper.Map<StudentDTO>(student));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddStudentAsync([FromBody] AddStudentRequest request)
        {
            var student= await studentRepository.AddStudentAsync(mapper.Map<DataModels.Student>(request));
            return CreatedAtAction(nameof(GetStudentAsync),new { studentId=student.Id },
                mapper.Map<StudentDTO>(student));
        }


        [HttpPut("{studentId:guid}")]
        public async Task<IActionResult> UpdateStudentAsync([FromRoute] Guid studentId, [FromBody] UpdateStudentRequest request)
        {
            if (await studentRepository.Exists(studentId))
            {
                // update Details
                var updatedStudents = await studentRepository.UpdateStudent(studentId, mapper.Map<DataModels.Student>(request));
                if (updatedStudents != null)
                {
                    return Ok(mapper.Map<StudentDTO>(updatedStudents));
                }
            }

            return NotFound();

        }
        [HttpDelete("{studentId:guid}")]
        public async Task<IActionResult> DeleteStudentAsync([FromRoute] Guid studentId)
        {
            if (await studentRepository.Exists(studentId))
            {
                // Delete the Student
                var student = await studentRepository.DeleteStudentAsync(studentId);
                return Ok(mapper.Map<StudentDTO>(student));
            }
            return NotFound();
        }

    }
}
