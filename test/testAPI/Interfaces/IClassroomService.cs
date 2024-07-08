using testAPI.Models.DTO.ClassroomDtos;

namespace testAPI.Interfaces
{
    public interface IClassroomService
    {
        public Task<List<ClassroomDto>> GetAllClassrooms();
        public Task<ClassroomDto> GetClassroomById(int id);
        public Task<ClassroomDto> CreateClassroom(CreateClassroomDto createClassroomDto);
        public Task<ClassroomDto> UpdateClassroomById(int id, UpdateClassroomDto updateClassroomDto);
        public Task<ClassroomDto> DeleteClassroomById(int id);
    }
}
