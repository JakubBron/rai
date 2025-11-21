using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelsLib.CustomTypes
{
    public class ValidationResultsMessages
    {
        public static string FirstDayAfterLastDay = "First day must be earlier than last day.";
        public static string StartTimeAfterEndTime = "Start time must be earlier than end time.";
        public static string DurationNegative = "Duration must be positive";
        public static string DurationGreaterThanTime = "Duration must be less than the total time between start and end time.";
        public static string SlotTaken = "Slot taken";
        public static string SlotNotFound = "Slot not found";
        public static string UserNotFound = "User not found";
        public static string UserIsNotStudent = "User is not a student";
        public static string UserHasReservations = "User already has a reservation";
    }
}
