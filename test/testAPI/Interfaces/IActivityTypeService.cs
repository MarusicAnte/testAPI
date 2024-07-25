using testAPI.Models.DTO.ActivityTypeDtos;

namespace testAPI.Interfaces
{
    public interface IActivityTypeService
    {
        public Task<List<ActivityTypeDto>> GetAllActivityTypes();
        public Task<ActivityTypeDto> GetActivityTypeById(int id);
        public Task<ActivityTypeDto> CreateActivityType(CreateActivityTypeDto createActivityTypeDto);
        public Task<ActivityTypeDto> UpdateActivityTypeById(int id, UpdateActivityTypeDto updateActivityTypeDto);
        public Task<ActivityTypeDto> DeleteActivityTypeById(int id);
    }
}
