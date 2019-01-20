using Diploma.DataBase;
using System;
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
            return Enum.IsDefined(typeof(BusinessUniversityContext.RoleValues), value)
                    && value.ToString() != BusinessUniversityContext.RoleValues.ADMIN.ToString();
        }
    }
}
