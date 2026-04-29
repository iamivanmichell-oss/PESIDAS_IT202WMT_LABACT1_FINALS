using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PESIDAS_IT202WMT_LABACT1_FINALS
{
	public class ParkingTransaction
	{
		// Properties
		public string PlateNumber { get; set; }
		public string VehicleType { get; set; }
		public int HoursParked { get; set; }
		public string Slot { get; set; }

		private const double ServiceCharge = 20.0;
		private const double OvertimeRate = 15.0; 

		// Constructor
		public ParkingTransaction(string plate, string type, int hours, string slot)
		{
			PlateNumber = plate;
			VehicleType = type;
			HoursParked = hours;
			Slot = slot;
		}

		
		public double CalculateStandardFee()
		{
			double rate = 0;
			switch (VehicleType)
			{
				case "Car": rate = 50; break;
				case "Motorcycle": rate = 30; break;
				case "Van": rate = 70; break;
			}
			return rate * HoursParked;
		}

		
		public double CalculateOvertime()
		{
			if (HoursParked > 8)
				return (HoursParked - 8) * OvertimeRate;
			return 0;
		}

		public double GetServiceCharge() => ServiceCharge;
	}
}
