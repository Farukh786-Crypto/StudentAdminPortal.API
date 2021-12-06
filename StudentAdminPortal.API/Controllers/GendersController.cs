using AutoMapper;
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
    public class GendersController : ControllerBase
    {
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;

        public GendersController(IStudentRepository _studentRepository,IMapper _mapper)
        {
            studentRepository = _studentRepository;
            mapper = _mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllGenders()
        {
            var genderList = await studentRepository.GetGenderAsync(); 
            if(genderList==null || !genderList.Any())
            {
                return NotFound();
            }
            return Ok(mapper.Map<List<GenderDTO>>(genderList));
        }
       
    }
}
