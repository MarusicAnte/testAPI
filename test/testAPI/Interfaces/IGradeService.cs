using testAPI.Models.DTO.GradeDtos;
using testAPI.Query;

namespace testAPI.Interfaces
{
    public interface IGradeService
    {
        public Task<List<GradeDto>> GetAllGrades(GradeQuery gradeQuery);
        public Task<GradeDto> GetGradeById(int id);
        public Task<GradeDto> CreateGrade(CreateGradeDto createGradeDto);
        public Task<GradeDto> UpdateGradeById(int id, UpdateGradeDto updateGradeDto);
        public Task<GradeDto> DeleteGradeById(int id);
    }
}
