using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Enums
{
    public class Entitity
    {
        public enum ReservationStatus
        {
            Aktiv,
            Færdig,
            Annulleret
        }

        public enum UserRole
        {
            Admin,
            Bogholder,
            Mekaniker,
            Kunde
        }
    }
}
