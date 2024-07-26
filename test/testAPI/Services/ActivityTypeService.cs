using Microsoft.EntityFrameworkCore;
using testAPI.Data;
using testAPI.Interfaces;
using testAPI.Logic;
using testAPI.Models.Domain;
using testAPI.Models.DTO.ActivityTypeDtos;

namespace testAPI.Services
{
    public class ActivityTypeService : IActivityTypeService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly ActivityTypeLogic _activityTypeLogic;
        public ActivityTypeService(ApplicationDBContext dbContext, ActivityTypeLogic activityTypeLogic)
        {
            _dbContext = dbContext;
            _activityTypeLogic = activityTypeLogic;
        }


        public async Task<List<ActivityTypeDto>> GetAllActivityTypes()
        {
            var activityTypesDomain = await _dbContext.ActivityTypes.ToListAsync();

            if (activityTypesDomain is null || activityTypesDomain.Count == 0)
                throw new Exception("Activity types does not exist !");

            
            // Map Domains to DTOs
            return activityTypesDomain.Select(activityTypeDomain => new ActivityTypeDto()
            {
                Id = activityTypeDomain.Id,
                Name = activityTypeDomain.Name,
            }).ToList();
        }


        public async Task<ActivityTypeDto> GetActivityTypeById(int id)
        {
            var activityTypeDomain = await _dbContext.ActivityTypes.FirstOrDefaultAsync(x => x.Id == id);

            if (activityTypeDomain is null)
                throw new Exception($"Activity type with id {id} does not exist !");

            // Map Domain to DTO
            var activityTypeDto = new ActivityTypeDto
            {
                Id = activityTypeDomain.Id,
                Name = activityTypeDomain.Name,
            };

            return activityTypeDto;
        }


        public async Task<ActivityTypeDto> CreateActivityType(CreateActivityTypeDto createActivityTypeDto)
        {
            await _activityTypeLogic.ValidateExistingActivityType(createActivityTypeDto.Name);

            var activityTypeDomain = new ActivityType()
            {
                Name = createActivityTypeDto.Name
            };

            _dbContext.ActivityTypes.Add(activityTypeDomain);
            await _dbContext.SaveChangesAsync();

            // Get new created Activity Type
            activityTypeDomain = await _dbContext.ActivityTypes.FirstOrDefaultAsync(x => x.Id == activityTypeDomain.Id);

            if (activityTypeDomain is null)
                throw new Exception("New created Activity Type not found !");

            // Map Domain to DTO
            var activityTypeDto = new ActivityTypeDto
            {
                Id = activityTypeDomain.Id,
                Name = activityTypeDomain.Name
            };

            return activityTypeDto;
        }


        public async Task<ActivityTypeDto> UpdateActivityTypeById(int id, UpdateActivityTypeDto updateActivityTypeDto)
        {
            var activityTypeDomain = await _dbContext.ActivityTypes.FirstOrDefaultAsync(x => x.Id == id);

            if (activityTypeDomain is null)
                throw new Exception($"Activity Type with id {id} does not exist !");

            await _activityTypeLogic.ValidateExistingActivityType(updateActivityTypeDto.Name);

            activityTypeDomain.Name = updateActivityTypeDto.Name;

            _dbContext.ActivityTypes.Update(activityTypeDomain);
            await _dbContext.SaveChangesAsync();

            // Get new updated Activity Type
            activityTypeDomain = await _dbContext.ActivityTypes.FirstOrDefaultAsync(x => x.Id == id);

            if (activityTypeDomain is null)
                throw new Exception("New updated Activity Type not found !");

            // Map Domain to DTO
            var activityTypeDto = new ActivityTypeDto
            {
                Id = activityTypeDomain.Id,
                Name = activityTypeDomain.Name
            };

            return activityTypeDto;
        }


        public async Task<ActivityTypeDto> DeleteActivityTypeById(int id)
        {
            var activityTypeDomain = await _dbContext.ActivityTypes.FirstOrDefaultAsync(x => x.Id == id);

            if (activityTypeDomain is null)
                throw new Exception($"Activity type with id {id} does not exist !");

            // Map Domain to DTO
            var activityTypeDto = new ActivityTypeDto
            {
                Id = activityTypeDomain.Id,
                Name = activityTypeDomain.Name,
            };

            _dbContext.ActivityTypes.Remove(activityTypeDomain);
            await _dbContext.SaveChangesAsync();

            return activityTypeDto;
        }

    }
}
