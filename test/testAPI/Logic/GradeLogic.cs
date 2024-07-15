using testAPI.Data;
using testAPI.Helpers;

namespace testAPI.Logic
{
    public class GradeLogic
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly UserHelper _userHelper;
        public GradeLogic(ApplicationDBContext dbContext, UserHelper userHelper)
        {
            _dbContext = dbContext;
            _userHelper = userHelper;
        }


    }
}
