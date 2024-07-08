using testAPI.Models.DTO.SubjectDtos;
using testAPI.Query;

namespace testAPI.Interfaces
{
    public interface ISubjectService
    {
        Task<List<SubjectDto>> GetAllSubjects(SubjectQuery subjectQuery);
        Task<SubjectDto> GetSubjectById(int id);
        Task<SubjectDto> CreateSubject(CreateSubjectDto createSubjectDto);
        Task<SubjectDto> UpdateSubjectById(int id, UpdateSubjectDto updateSubjectDto);
        Task<SubjectDto> DeleteSubjectById(int id);
    }
}
