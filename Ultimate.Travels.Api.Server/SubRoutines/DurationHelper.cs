using System.Text.RegularExpressions;

namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// Convert from one duration format to another
    /// </summary>
    public static class DurationHelper
    {
        /// <summary>
        /// Convert given duration from Iso8601 to simple readable format
        /// </summary>
        /// <param name="duration">The specified duration in Iso8601</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when specified duration is invalid</exception>
        public static string ConvertFromIso8601(this string duration)
        {
            // Use regex to extract hours and minutes from the ISO8601 duration string
            Match match = Regex.Match(duration, @"PT(\d+H)?(\d+M)?");

            if (match.Success)
            {
                // Extract hours and minutes from the regex match
                int hours = match.Groups[1].Success ? int.Parse(match.Groups[1].Value.TrimEnd('H')) : 0;
                int minutes = match.Groups[2].Success ? int.Parse(match.Groups[2].Value.TrimEnd('M')) : 0;

                // Create the formatted duration string
                string formattedDuration = $"{hours}h {minutes}m";

                // Eliminate hour from the string if zero
                formattedDuration = hours == 0 ? formattedDuration.Replace("0h ", "") : formattedDuration;

                // Eliminate minute from the string if zero
                formattedDuration = minutes == 0 ? formattedDuration.Replace(" 0m", "") : formattedDuration;

                // If both hours and minutes are zero, consider the duration as 0 minutes
                if (hours == 0 && minutes == 0)
                {
                    formattedDuration = "0m";
                }

                return formattedDuration;
            }
            else
            {
                // Handle invalid input
                throw new ArgumentException("Invalid ISO8601 duration format");
            }
        }

        /// <summary>
        /// Convert given duration from simple format to Iso8601
        /// </summary>
        /// <param name="duration">The specified duration in simple format</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when specified duration is invalid</exception>
        public static string ConvertToIso8601(this string duration)
        {
            int hours = 0;
            int minutes = 0;

            // Split the input string into parts based on 'h' and 'm'
            string[] parts = duration.Split(new[] { 'h', 'm' }, StringSplitOptions.RemoveEmptyEntries);

            // Parse the values based on the number of parts
            if (parts.Length == 1)
            {
                if (duration.EndsWith('h'))
                {
                    // Only hours provided
                    int.TryParse(parts[0], out hours);
                }
                else if (duration.EndsWith('m'))
                {
                    // Only minutes provided
                    int.TryParse(parts[0], out minutes);
                }
            }
            else if (parts.Length == 2)
            {
                // Both hours and minutes provided
                int.TryParse(parts[0], out hours);
                int.TryParse(parts[1], out minutes);
            }
            else
            {
                // Handle invalid input
                throw new ArgumentException("Invalid duration format");
            }

            // Create the ISO8601 duration string
            string iso8601Duration = "PT";
            
            if (hours != 0)
            {
                iso8601Duration += $"{hours}H";
            }
            if (minutes != 0)
            {
                iso8601Duration += $"{minutes}M";
            }

            return iso8601Duration;
        }

        /// <summary>
        /// Convert specified duration from Iso8601 format to duration in minutes
        /// </summary>
        /// <param name="iso8601Duration">The specified duration</param>
        /// <returns></returns>
        public static int GetDurationInMinutes(this string iso8601Duration)
        {
            // Parse hours and minutes from ISO8601 duration string
            TimeSpan duration = System.Xml.XmlConvert.ToTimeSpan(iso8601Duration);

            // Convert the duration to total minutes
            int durationInMinutes = (int)duration.TotalMinutes;

            // Return the result
            return durationInMinutes;
        }
    }
}