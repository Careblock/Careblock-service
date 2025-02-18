using Careblock.Model.Shared.Enum;

namespace Careblock.Service.BusinessLogic
{
    public class Utils
    {
        public static string GetGenderName(Gender gender) {
            switch (gender) {
                case Gender.Male:
                    return "Male";
                case Gender.Female:
                    return "Female";
                default:
                    return "Other";
            }
        }

    }
}
