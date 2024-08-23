using eStudent.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using testAPI.Constants;
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


        [Authorize(Policy = RolesConstant.AnyUserRole)]
        [HttpGet]
        public async Task<List<ClassroomDto>> GetAll()
        {
            return await _classroomService.GetAllClassrooms();
        }


        [Authorize(Policy = RolesConstant.AnyUserRole)]
        [HttpGet("{id}")]
        public async Task<ClassroomDto> GetById(int id)
        {
            return await _classroomService.GetClassroomById(id);
        }


        [Authorize(Policy = RolesConstant.Administrator)]
        [HttpPost]
        public async Task<ClassroomDto> Create([FromBody] CreateClassroomDto createClassroomDto)
        {
            return await _classroomService.CreateClassroom(createClassroomDto);
        }


        [Authorize(Policy = RolesConstant.Administrator)]
        [HttpPatch("{id}")]
        public async Task<ClassroomDto> Update([FromRoute] int id, [FromBody] UpdateClassroomDto updateClassroomDto)
        {
            return await _classroomService.UpdateClassroomById(id,updateClassroomDto);
        }


        [Authorize(Policy = RolesConstant.Administrator)]
        [HttpDelete("{id}")]
        public async Task<ClassroomDto> DeleteById([FromRoute] int id)
        {
            return await _classroomService.DeleteClassroomById(id);
        }
    }
}
