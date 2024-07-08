using eStudent.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using testAPI.Interfaces;
using testAPI.Models.DTO.ClassroomDtos;

namespace testAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassroomsController : BaseController
    {
        private readonly IClassroomService _classroomService;

        public ClassroomsController(IClassroomService classroomService)
        {
            _classroomService = classroomService;
        }


        [HttpGet]
        public async Task<List<ClassroomDto>> GetAll()
        {
            return await _classroomService.GetAllClassrooms();
        }


        [HttpGet("{id}")]
        public async Task<ClassroomDto> GetById(int id)
        {
            return await _classroomService.GetClassroomById(id);
        }


        [HttpPost]
        public async Task<ClassroomDto> Create([FromBody] CreateClassroomDto createClassroomDto)
        {
            return await _classroomService.CreateClassroom(createClassroomDto);
        }


        [HttpPatch("{id}")]
        public async Task<ClassroomDto> Update([FromRoute] int id, [FromBody] UpdateClassroomDto updateClassroomDto)
        {
            return await _classroomService.UpdateClassroomById(id,updateClassroomDto);
        }


        [HttpDelete("{id}")]
        public async Task<ClassroomDto> DeleteById([FromRoute] int id)
        {
            return await _classroomService.DeleteClassroomById(id);
        }
    }
}
