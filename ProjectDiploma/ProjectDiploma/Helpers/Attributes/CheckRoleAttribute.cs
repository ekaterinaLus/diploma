using Diploma.DataBase;
using System.ComponentModel.DataAnnotations;

namespace ProjectDiploma.Helpers.Attributes
{
    /// <summary>
    /// Проверяет имя роли
    /// </summary>
    public class CheckRoleAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return BusinessUniversityContext.RoleName.CheckRole(value.ToString()) 
                    && value.ToString() != BusinessUniversityContext.RoleName.ADMIN_ROLE_NAME;
        }
    }
}
