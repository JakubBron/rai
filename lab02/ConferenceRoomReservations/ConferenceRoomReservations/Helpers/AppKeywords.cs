namespace ConferenceRoomReservations.Helpers
{
    public static class AppKeywords
    {
        public const string AuthError_LoginRequired = "authError_loginRequired";
    }

    public static class AppRoles
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }

    public static class AppValidationErrors
    {
        public const string reservationTimeToLow = "timeToLow";
        public const string reservationTimeToBig = "timeToBig";
        public const string reservationRoomConflict = "roomConflict";
        public const string EndDateLowerThanStartDate = "EndDateLowerThanStartDate";
    }

    public static class AppReservationLimits
    {
        public static readonly TimeSpan maxReservationDuration = TimeSpan.FromHours(3);
        public static readonly TimeSpan minReservationDuration = TimeSpan.FromMinutes(15);
    }
}
