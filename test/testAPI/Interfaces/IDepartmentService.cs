using testAPI.Models.DTO.DepartmentDtos;

namespace testAPI.Interfaces
{
    public interface IDepartmentService
    {
        public Task<List<DepartmentDto>> GetAllCollegeDepartments();
        public Task<DepartmentDto> GetCollegeDepartmentById(int id);
        public Task<DepartmentDto> CreateCollegeDepartment(CreateDepartmentDto createDepartmentDto);
        public Task<DepartmentDto> UpdateCollegeDepartmentById(int id, UpdateDepartmentDto updateDepartmentDto);
        public Task<DepartmentDto> DeleteCollegeDepartmentById(int id);
    }
}
