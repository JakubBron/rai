using System;
using System.ComponentModel.DataAnnotations;
using ConferenceRoomReservations.Helpers;

public class Reservation: IValidatableObject
{
    public string UserName { get; set; }   // Who made the reservation
    public int RoomId { get; set; }   // Room being reserved
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsValid
    {
        get
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(this);
            return Validator.TryValidateObject(this, context, validationResults, validateAllProperties: true);
        }
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EndTime < StartTime)
        {
            yield return new ValidationResult(AppValidationErrors.EndDateLowerThanStartDate,
                new[] { nameof(StartTime), nameof(EndTime) });
        }

        if (EndTime - StartTime < AppReservationLimits.minReservationDuration)
        {
            yield return new ValidationResult(
                AppValidationErrors.reservationTimeToLow,
                new[] { nameof(StartTime), nameof(EndTime) });
        }

        if (EndTime - StartTime > AppReservationLimits.maxReservationDuration)
        {
            yield return new ValidationResult(
                AppValidationErrors.reservationTimeToBig,
                new[] { nameof(StartTime), nameof(EndTime) });
        }
    }


}
