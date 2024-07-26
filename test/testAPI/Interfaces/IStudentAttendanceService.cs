using testAPI.Models.DTO.StudentAttendanceDtos;
using testAPI.Query;

namespace testAPI.Interfaces
{
    public interface IStudentAttendanceService
    {
        public Task<List<StudentAttendanceDto>> GetAllStudentAttendances(StudentAttendanceQuery studentAttendanceQuery);
        public Task<StudentAttendanceDto> GetStudentAttendanceById(int id);
        public Task<StudentAttendanceDto> CreateStudentAttendance(CreateStudentAttendanceDto createStudentAttendanceDto);
        public Task<StudentAttendanceDto> UpdateStudentAttendance(int id, UpdateStudentAttendanceDto updateStudentAttendanceDto);
        public Task<StudentAttendanceDto> DeleteStudentAttendanceById(int id);
    }
}
