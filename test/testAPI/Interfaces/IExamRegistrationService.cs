using testAPI.Models.DTO.ExamRegistrationDtos;
using testAPI.Query;

namespace testAPI.Interfaces
{
    public interface IExamRegistrationService
    {
        public Task<List<ExamRegistrationDto>> GetAllExamRegistrations(ExamRegistrationQuery examRegistrationQuery);
        public Task<ExamRegistrationDto> GetExamRegistrationById(int id);
        public Task<ExamRegistrationDto> CreateExamRegistration(CreateExamRegistrationDto createExamRegistrationDto);
        public Task<ExamRegistrationDto> UpdateExamRegistrationById(int id, UpdateExamRegistrationDto updateExamRegistrationDto);
        public Task<ExamRegistrationDto> DeleteExamRegistrationById(int id);
    }
}
