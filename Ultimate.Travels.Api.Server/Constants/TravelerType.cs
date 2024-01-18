namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// The traveler type constants
    /// </summary>
    public class TravelerType
    {
        /// <summary>
        /// The adult traveler type
        /// </summary>
        public const string Adult = "ADULT";

        /// <summary>
        /// The child traveler type
        /// </summary>
        public const string Child = "CHILD";

        /// <summary>
        /// The held infant traveler type
        /// </summary>
        public const string HeldInfant = "HELD_INFANT";

        /// <summary>
        /// The seated infant traveler type
        /// </summary>
        public const string SeatedInfant = "SEATED_INFANT";
    }

    /// <summary>
    /// Extention methods for traveler type
    /// </summary>
    public static class TravelerTypeExtensions
    {
        /// <summary>
        /// Computes the minimum required date of birth for specified traveler type
        /// </summary>
        /// <param name="travelerType">The specified traveler</param>
        /// <returns>The computed minimum required date of birth</returns>
        public static DateTime GetMinimumDateOfBirth(this string travelerType)
        {
            return travelerType switch
            {
                TravelerType.Adult => DateTime.Now.AddYears(-12),
                TravelerType.Child => DateTime.Now.AddYears(-2),
                TravelerType.HeldInfant => DateTime.Now,
                TravelerType.SeatedInfant => DateTime.Now,
                _ => DateTime.Now.AddYears(-12),
            };
        }

        /// <summary>
        /// Computes the maximum required date of birth for specified traveler type
        /// </summary>
        /// <param name="travelerType">The specified traveler type</param>
        /// <returns>The computed maximum required date of birth</returns>
        public static DateTime GetMaximumDateOfBirth(this string travelerType)
        {
            return travelerType switch
            {
                TravelerType.Adult => DateTime.Now.AddYears(-180),
                TravelerType.Child => DateTime.Now.AddYears(-11),
                TravelerType.HeldInfant => DateTime.Now.AddYears(-1),
                TravelerType.SeatedInfant => DateTime.Now.AddYears(-1),
                _ => DateTime.Now.AddYears(-180),
            };
        }

        /// <summary>
        /// Validates specified date of birth with specified traveler type
        /// </summary>
        /// <param name="dateOfBirth">The specified date of birth</param>
        /// <param name="travelerType">The specified traveler type</param>
        /// <returns>The boolean result of the validation</returns>
        public static bool IsValid(this DateTime dateOfBirth, string travelerType)
        {
            return travelerType switch
            {
                TravelerType.Adult => dateOfBirth <= travelerType.GetMinimumDateOfBirth() || dateOfBirth >= travelerType.GetMaximumDateOfBirth(),
                TravelerType.Child => dateOfBirth <= travelerType.GetMinimumDateOfBirth() || dateOfBirth >= travelerType.GetMaximumDateOfBirth(),
                TravelerType.HeldInfant => dateOfBirth <= travelerType.GetMinimumDateOfBirth() || dateOfBirth >= travelerType.GetMaximumDateOfBirth(),
                TravelerType.SeatedInfant => dateOfBirth <= travelerType.GetMinimumDateOfBirth() || dateOfBirth >= travelerType.GetMaximumDateOfBirth(),
                _ => dateOfBirth <= travelerType.GetMinimumDateOfBirth() || dateOfBirth >= travelerType.GetMaximumDateOfBirth(),
            };
        }
    }
}