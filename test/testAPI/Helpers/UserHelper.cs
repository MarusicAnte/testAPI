using testAPI.Constants;
using testAPI.Models.Domain;

namespace testAPI.Helpers
{
    public class UserHelper
    {
        public bool isAdminOrProfessor(Role role)
        {
           if(role.Name.Equals(RolesConstant.Administrator) || role.Name.Equals(RolesConstant.Profesor))
                return true;

           return false;
        }
    }
}
