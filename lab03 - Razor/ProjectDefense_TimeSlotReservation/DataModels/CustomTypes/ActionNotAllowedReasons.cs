using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelsLib.CustomTypes
{
    public class ActionNotAllowedReasons
    {
        public static string AlreadyReserved = "This slot has already been reserved by another student.";
        public static string AlreadyHasReservation = "You have already a reservation.";
        public static string NotOwner = "You are not the owner of this reservation and you can not cancel it.";
        public static string ReservationNotTaken = "You can not cancel Free slot.";
        public static string SlotMissed = "You are trying to reserve slot from the past.";
        public static string SlotBlocked = "You can not perform any operation on blocked slot!";
    }
}
