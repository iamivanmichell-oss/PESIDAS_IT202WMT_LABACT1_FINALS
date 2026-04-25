using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PESIDAS_IT202WMT_LABACT1_FINALS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public class ParkingTransaction
        {
            public string PlateNumber { get; set; }
            public string VehicleType { get; set; }
            public int HoursParked { get; set; }
            public string SlotNumber { get; set; }
            private const double ServiceCharge = 20.00;
            private const double OvertimeRate = 15.00;

            public ParkingTransaction(string plate, string type, double hours, string slot)
            {
                PlateNumber = plate;
                VehicleType = type;
                HoursParked = (int)hours; 
                SlotNumber = slot;
            }


            public double ComputeTotalFee(string DiscountType)
            {
                double hourlyRate = 0;


                switch (VehicleType)
                {
                    case "Car": hourlyRate = 50; break;
                    case "Motorcycle": hourlyRate = 30; break;
                    case "Van": hourlyRate = 70; break;
                }

                double baseFee = HoursParked * hourlyRate;
                double overtimeFee = 0;


                if (HoursParked > 8)
                {
                    overtimeFee = (HoursParked - 8) * OvertimeRate;

                }

                double total = baseFee + overtimeFee + ServiceCharge;


                if (DiscountType == "")
                {
                    total *= 0.80;
                }

                return total;
            }
        }
        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        { 

            string platenumber = txtPlateNumber.Text;
            string vehicletype = cmbVehicleType.SelectedItem.ToString();
            double hoursparked = double.TryParse(txtHoursPark.Text, out double hours) ? hours : 0;
            string slotnumber = txtAssignedSlot.Text;

            ParkingTransaction pt = new ParkingTransaction(platenumber, vehicletype, hoursparked, slotnumber);

            lblPlateNumber.Text = pt.PlateNumber;
            lblVehicleInfo.Text = pt.VehicleType;
            lblDuration.Text = pt.HoursParked.ToString() + " hours";
            lblSlot.Text = pt.SlotNumber;
            lblOTFee.Text = (hoursparked > 8) ? ((hoursparked - 8) * 15).ToString("C") : "0.00";


            lblServiceCharge.Text = "20.00";
            lblTotal.Text = pt.ComputeTotalFee(chkDiscount.Checked).ToString("C");




        }
    }
}

