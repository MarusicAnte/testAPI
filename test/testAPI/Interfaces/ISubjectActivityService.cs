using testAPI.Models.DTO.SubjectActivityDtos;

namespace testAPI.Interfaces
{
    public interface ISubjectActivityService
    {
        public Task<List<SubjectActivityDto>> GetAllSubjectActivities();
        public Task<SubjectActivityDto> GetSubjectActivityById(int id);
        public Task<SubjectActivityDto> CreateSubjectActivity(CreateSubjectActivityDto createSubjectActivityDto);
        public Task<SubjectActivityDto> UpdateSubjectActivityById(int id, UpdateSubjectActivityDto updateSubjectActivityDto);
        public Task<SubjectActivityDto> DeleteSubjectActivityById(int id);
    }
}
