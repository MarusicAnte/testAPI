using Microsoft.EntityFrameworkCore;
using testAPI.Data;

namespace testAPI.Logic
{
    public class ActivityTypeLogic
    {
        private readonly ApplicationDBContext _dbContext;
        public ActivityTypeLogic(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task ValidateExistingActivityType(string activityTypeName)
        {
            var activityType = await _dbContext.ActivityTypes.AnyAsync(x => x.Name == activityTypeName);

            if(activityType)
                throw new Exception($"Activity Type with name {activityTypeName} already exist !");
        }
    }
}
