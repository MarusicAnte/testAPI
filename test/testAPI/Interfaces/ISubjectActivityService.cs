using testAPI.Models.DTO.SubjectActivityDtos;
using testAPI.Query;

namespace testAPI.Interfaces
{
    public interface ISubjectActivityService
    {
        public Task<List<ActivityDto>> GetAllSubjectActivities(SubjectActivityQuery subjectActivityQuery);
        public Task<ActivityDto> GetSubjectActivityById(int id);
        public Task<ActivityDto> CreateSubjectActivity(CreateSubjectActivityDto createSubjectActivityDto);
        public Task<ActivityDto> UpdateSubjectActivityById(int id, UpdateSubjectActivityDto updateSubjectActivityDto);
        public Task<ActivityDto> DeleteSubjectActivityById(int id);
    }
}
