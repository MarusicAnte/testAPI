using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using testAPI.Data;
using testAPI.Interfaces;
using testAPI.Logic;
using testAPI.Models.Domain;
using testAPI.Models.DTO.SubjectActivityDtos;
using testAPI.Query;

namespace testAPI.Services
{
    public class SubjectActivityService : ISubjectActivityService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly SubjectActivityLogic _subjectActivityLogic;
        public SubjectActivityService(ApplicationDBContext dbContext, SubjectActivityLogic subjectActivityLogic)
        {
            _dbContext = dbContext;
            _subjectActivityLogic = subjectActivityLogic;
        }


        public async Task<List<SubjectActivityDto>> GetAllSubjectActivities(SubjectActivityQuery subjectActivityQuery)
        {
            var subjectActivitiesDomain = await subjectActivityQuery.GetSubjectActivityQuery(_dbContext.SubjectActivities.Include(sa => sa.Subject)
                                                                                                                         .Include(sa => sa.ActivityType)
                                                                                                                         .Include(sa => sa.Classroom)
                                                                                                                         .Include(sa => sa.Instructor)
                                                                                            ).ToListAsync();

            if (subjectActivitiesDomain is null || subjectActivitiesDomain.Count == 0)
                throw new Exception("Subject Activities does not exist !");

            // Map Domains to DTOs
            return subjectActivitiesDomain.Select(subjectActivityDomain => new SubjectActivityDto()
            {
                Id = subjectActivityDomain.Id,
                Subject = subjectActivityDomain.Subject.Name,
                ActivityType = subjectActivityDomain.ActivityType.Name,
                Classroom = subjectActivityDomain.Classroom.Name,
                Instructor = $"{subjectActivityDomain.Instructor.FirstName} {subjectActivityDomain.Instructor.LastName}"
            }).ToList();
        }


        public async Task<SubjectActivityDto> GetSubjectActivityById(int id)
        {
            var subjectActivityDomain = await _dbContext.SubjectActivities.Include(sa => sa.Subject)
                                                                          .Include(sa => sa.ActivityType)
                                                                          .Include(sa => sa.Classroom)
                                                                          .Include(sa => sa.Instructor)
                                                                          .FirstOrDefaultAsync(x => x.Id == id);

            if (subjectActivityDomain is null)
                throw new Exception($"Subject Activity with id {id} does not exist !");

            var subjectActivityDto = new SubjectActivityDto()
            {
                Id = subjectActivityDomain.Id,
                Subject = subjectActivityDomain.Subject.Name,
                ActivityType= subjectActivityDomain.ActivityType.Name,
                Classroom = subjectActivityDomain.Classroom.Name,
                Instructor = $"{subjectActivityDomain.Instructor.FirstName} {subjectActivityDomain.Instructor.LastName}"
            };


            return subjectActivityDto;
        }


        public async Task<SubjectActivityDto> CreateSubjectActivity(CreateSubjectActivityDto createSubjectActivityDto)
        {
            await _subjectActivityLogic.ValidateInstructorAndSubject(createSubjectActivityDto.InstructorId, createSubjectActivityDto.SubjectId);

            await _subjectActivityLogic.ValidateActivityType(createSubjectActivityDto.ActivityTypeId);

            await _subjectActivityLogic.ValidateExistingSubjectActivity(createSubjectActivityDto.SubjectId,
                                                                        createSubjectActivityDto.ActivityTypeId,
                                                                        createSubjectActivityDto.InstructorId);

            var subjectActivityDomain = new SubjectActivity()
            {
                SubjectId = createSubjectActivityDto.SubjectId,
                ActivityTypeId = createSubjectActivityDto.ActivityTypeId,
                ClassroomId = createSubjectActivityDto.ClassroomId,
                InstructorId = createSubjectActivityDto.InstructorId,
            };

            _dbContext.SubjectActivities.Add(subjectActivityDomain);
            await _dbContext.SaveChangesAsync();

            // Get new created SubjectActivity
            subjectActivityDomain = await _dbContext.SubjectActivities.Include(sa => sa.Subject)
                                                                      .Include(sa => sa.ActivityType)
                                                                      .Include(sa => sa.Classroom)
                                                                      .Include(sa => sa.Instructor)
                                                                      .FirstOrDefaultAsync(x => x.Id == subjectActivityDomain.Id);

            if (subjectActivityDomain is null)
                throw new Exception("New created Subject Activity not found !");

            // Map Domain to DTO
            var subjectActivityDto = new SubjectActivityDto
            {
                Id = subjectActivityDomain.Id,
                Subject = subjectActivityDomain.Subject.Name,
                ActivityType = subjectActivityDomain.ActivityType.Name,
                Classroom = subjectActivityDomain.Classroom.Name,
                Instructor = $"{subjectActivityDomain.Instructor.FirstName} {subjectActivityDomain.Instructor.LastName}"
            };

            return subjectActivityDto;
        }


        public async Task<SubjectActivityDto> UpdateSubjectActivityById(int id, UpdateSubjectActivityDto updateSubjectActivityDto)
        {
            var subjectActivityDomain = await _dbContext.SubjectActivities.Include(sa => sa.Subject)
                                                              .Include(sa => sa.ActivityType)
                                                              .Include(sa => sa.Classroom)
                                                              .Include(sa => sa.Instructor)
                                                              .FirstOrDefaultAsync(x => x.Id == id);

            if (subjectActivityDomain is null)
                throw new Exception($"Subject Activity with id {id} does not exist !");

            subjectActivityDomain.SubjectId = updateSubjectActivityDto.SubjectId;
            subjectActivityDomain.ActivityTypeId = updateSubjectActivityDto.ActivityTypeId;
            subjectActivityDomain.ClassroomId = updateSubjectActivityDto.ClassroomId;
            subjectActivityDomain.InstructorId = updateSubjectActivityDto.InstructorId;

            await _subjectActivityLogic.ValidateInstructorAndSubject(updateSubjectActivityDto.InstructorId, 
                                                                     updateSubjectActivityDto.SubjectId);

            await _subjectActivityLogic.ValidateActivityType(updateSubjectActivityDto.ActivityTypeId);

            await _subjectActivityLogic.ValidateExistingSubjectActivity(updateSubjectActivityDto.SubjectId,
                                                                        updateSubjectActivityDto.ActivityTypeId,
                                                                        updateSubjectActivityDto.InstructorId);

            _dbContext.SubjectActivities.Update(subjectActivityDomain);
            await _dbContext.SaveChangesAsync();

            // Get new updated Subject Activity
            subjectActivityDomain = await _dbContext.SubjectActivities.Include(sa => sa.Subject)
                                                              .Include(sa => sa.ActivityType)
                                                              .Include(sa => sa.Classroom)
                                                              .Include(sa => sa.Instructor)
                                                              .FirstOrDefaultAsync(x => x.Id == subjectActivityDomain.Id);

            if (subjectActivityDomain is null)
                throw new Exception("New updated Subject Activity not found !");

            var subjectActivityDto = new SubjectActivityDto()
            {
                Id = subjectActivityDomain.Id,
                Subject = subjectActivityDomain.Subject.Name,
                ActivityType = subjectActivityDomain.ActivityType.Name,
                Classroom = subjectActivityDomain.Classroom.Name,
                Instructor = $"{subjectActivityDomain.Instructor.FirstName} {subjectActivityDomain.Instructor.LastName}"
            };

            return subjectActivityDto;
        }


        public async Task<SubjectActivityDto> DeleteSubjectActivityById(int id)
        {
            var subjectActivityDomain = await _dbContext.SubjectActivities.Include(sa => sa.Subject)
                                                                          .Include(sa => sa.ActivityType)
                                                                          .Include(sa => sa.Classroom)
                                                                          .Include(sa => sa.Instructor)
                                                                          .FirstOrDefaultAsync(x => x.Id == id);

            if (subjectActivityDomain is null)
                throw new Exception($"Subject Activity with id {id} does not exist !");

            var subjectActivityDto = new SubjectActivityDto()
            {
                Id = subjectActivityDomain.Id,
                Subject = subjectActivityDomain.Subject.Name,
                ActivityType= subjectActivityDomain.ActivityType.Name,
                Classroom = subjectActivityDomain.Classroom.Name,
                Instructor = $"{subjectActivityDomain.Instructor.FirstName} {subjectActivityDomain.Instructor.LastName}"
            };

            _dbContext.SubjectActivities.Remove(subjectActivityDomain);
            await _dbContext.SaveChangesAsync();

            return subjectActivityDto;
        }
       
    }
}
