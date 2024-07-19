using testAPI.Models.DTO.ExamDtos;
using testAPI.Query;

namespace testAPI.Interfaces
{
    public interface IExamService
    {
        public Task<List<ExamDto>> GetAllExams(ExamQuery examQuery);
        public Task<ExamDto> GetExamById(int id);
        public Task<ExamDto> CreateExam(CreateExamDto createExamDto);
        public Task<ExamDto> UpdateExamById(int id, UpdateExamDto updateExamDto);
        public Task<ExamDto> DeleteExamById(int id);
    }
}
