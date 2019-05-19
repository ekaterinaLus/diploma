using ProjectDiploma.ViewModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectDiploma.Helpers.Attributes
{
    public class CheckUserTagsAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value is HashSet<TagViewModel> castedValue && castedValue.Count == 5;
        }
    }
}
