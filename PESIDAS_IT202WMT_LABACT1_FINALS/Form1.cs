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
		private ParkingTransaction currentTransaction;

		public Form1()
		{
			InitializeComponent();
			cmbVehicleType.SelectedIndex = 0; 
			cmbDiscount.SelectedIndex = 0;
		}

		private void btnRegisterVehicle_Click(object sender, EventArgs e)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(txtPlateNumber.Text))
					throw new ArgumentException("Plate Number cannot be empty.");

				if (string.IsNullOrWhiteSpace(txtAssignedSlot.Text))
					throw new ArgumentException("Please select a parking slot first.");

				if (!int.TryParse(txtHourParked.Text, out int hours))
					throw new FormatException("Hours Parked must be a numeric value.");

				currentTransaction = new ParkingTransaction(
					txtPlateNumber.Text,
					cmbVehicleType.Text,
					hours,
					txtAssignedSlot.Text
				);

				
				lblDisplayPlate.Text = currentTransaction.PlateNumber;
				lblDisplayInfo.Text = currentTransaction.VehicleType;
				lblDisplayDuration.Text = $"{currentTransaction.HoursParked} hrs";
				lblDisplaySlot.Text = currentTransaction.Slot;

				
				double standard = currentTransaction.CalculateStandardFee();
				double overtime = currentTransaction.CalculateOvertime();
				double service = currentTransaction.GetServiceCharge();
				double total = standard + overtime + service;

				lblStandardFee.Text = standard.ToString("N2");
				lblDisplayOvertime.Text = overtime.ToString("N2");
				lblServiceCharge.Text = service.ToString("N2");
				lblTotal.Text = total.ToString("N2");

				
				ResetDuplicatePlate(currentTransaction.PlateNumber);
				UpdateSlotToOccupied(currentTransaction.Slot, currentTransaction.PlateNumber);
			}
			catch (ArgumentException ex)
			{
				MessageBox.Show(ex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
			catch (FormatException ex)
			{
				MessageBox.Show(ex.Message, "Input Format Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (Exception ex)
			{
				MessageBox.Show("An error occurred: " + ex.Message);
			}
		}

		private void btnUpdateStatus_Click(object sender, EventArgs e)
		{
			try
			{
				if (string.IsNullOrEmpty(txtAssignedSlot.Text) || string.IsNullOrEmpty(txtPlateNumber.Text))
					throw new Exception("Fill in Plate Number and Slot to update status.");

				ResetDuplicatePlate(txtPlateNumber.Text);
				UpdateSlotToOccupied(txtAssignedSlot.Text, txtPlateNumber.Text);
			}
			catch (Exception ex) { MessageBox.Show(ex.Message); }
		}

		private void btnProcessPayment_Click(object sender, EventArgs e)
		{
			try
			{
				if (lblTotal.Text == "-------" || string.IsNullOrEmpty(lblTotal.Text))
					throw new InvalidOperationException("No vehicle registered to pay for.");

				if (!double.TryParse(txtPayAmount.Text, out double cash))
					throw new FormatException("Please enter a valid numeric amount for payment.");

				double standard = currentTransaction.CalculateStandardFee();
				double overtime = currentTransaction.CalculateOvertime();
				double service = currentTransaction.GetServiceCharge();
				double total = standard + overtime + service;

				if (cmbDiscount.Text == "Senior Citizen")
				{
					total *= 0.80;
				}
				else if (cmbDiscount.Text == "Employee")
				{
					total *= 0.85;
				}
				if (cash < total)
					throw new Exception($"Insufficient Cash. Needed: P{total:N2}");

				lblChange.Text = (cash - total).ToString("N2");
				lblTotal.Text = total.ToString("N2"); // Show discounted total

				MessageBox.Show("Payment Processed Successfully!", "Success");
			}
			catch (Exception ex) { MessageBox.Show(ex.Message, "Payment Error"); }
		}

		private void UpdateSlotToOccupied(string slotId, string plateNum)
		{
			Control[] foundButtons = this.Controls.Find("btn" + slotId, true);
			if (foundButtons.Length > 0 && foundButtons[0] is Button btn)
			{
				btn.Text = plateNum;
				btn.BackColor = Color.Red;
				btn.ForeColor = Color.White;
			}
		}

		private void ResetDuplicatePlate(string plateNum)
		{
			ResetSearchRecursive(this, plateNum);
		}

		private void ResetSearchRecursive(Control container, string plateNum)
		{
			foreach (Control c in container.Controls)
			{
				if (c is Button btn && btn.Text == plateNum)
				{
					btn.Text = btn.Name.Replace("btn", "");
					btn.BackColor = Color.Lime;
					btn.ForeColor = Color.Black;
				}
				if (c.HasChildren) ResetSearchRecursive(c, plateNum);
			}
		}

		private void btnGenerateReceipt_Click(object sender, EventArgs e)
		{
			if (lblDisplayPlate.Text == "-------")
			{
				MessageBox.Show("No transaction data available.");
				return;
			}

			rtbReceipt.Clear();
			rtbReceipt.AppendText("      SMART PARKING SYSTEM\n");
			rtbReceipt.AppendText("===============================\n");
			rtbReceipt.AppendText($"Plate No:    {lblDisplayPlate.Text}\n");
			rtbReceipt.AppendText($"Vehicle:     {lblDisplayInfo.Text}\n");
			rtbReceipt.AppendText($"Slot:        {lblDisplaySlot.Text}\n");
			rtbReceipt.AppendText($"Duration:    {lblDisplayDuration.Text}\n");
			rtbReceipt.AppendText("-------------------------------\n");
			rtbReceipt.AppendText($"Total Due:   P{lblTotal.Text}\n");
			rtbReceipt.AppendText($"Cash:        P{txtPayAmount.Text}\n");
			rtbReceipt.AppendText($"Change:      P{lblChange.Text}\n");
			rtbReceipt.AppendText("===============================\n");
			rtbReceipt.AppendText("      THANK YOU & DRIVE SAFE!");
		}

		private void btnClearForm_Click(object sender, EventArgs e)
		{
			txtPlateNumber.Clear();
			txtHourParked.Clear();
			txtAssignedSlot.Clear();
			txtPayAmount.Clear();
			rtbReceipt.Clear();
			lblDisplayPlate.Text = "-------";
			lblDisplayInfo.Text = "-------";
			lblDisplayDuration.Text = "-------";
			lblDisplaySlot.Text = "-------";
			lblStandardFee.Text = "0.00";
			lblDisplayOvertime.Text = "0.00";
			lblServiceCharge.Text = "0.00";
			lblTotal.Text = "-------";
			lblChange.Text = "0";
		}

		// Slot Buttons
		private void btnA1_Click(object sender, EventArgs e)
		{
			txtAssignedSlot.Text = "A1";
		}
		private void btnA2_Click(object sender, EventArgs e)
		{
			txtAssignedSlot.Text = "A2";
		}
		private void btnA3_Click(object sender, EventArgs e)
		{
			txtAssignedSlot.Text = "A3";
		}
		private void btnA4_Click(object sender, EventArgs e)
		{
			txtAssignedSlot.Text = "A4";
		}
		private void btnA5_Click(object sender, EventArgs e)
		{
			txtAssignedSlot.Text = "A5";
		}
		private void btnB1_Click(object sender, EventArgs e)
		{
			txtAssignedSlot.Text = "B1";
		}
		private void btnB2_Click(object sender, EventArgs e)
		{
			txtAssignedSlot.Text = "B2";
		}
		private void btnB3_Click(object sender, EventArgs e)
		{
			txtAssignedSlot.Text = "B3";
		}
		private void btnB4_Click(object sender, EventArgs e)
		{
			txtAssignedSlot.Text = "B4";
		}
		private void btnB5_Click(object sender, EventArgs e)
		{
			txtAssignedSlot.Text = "B5";
		}
		private void btnC1_Click(object sender, EventArgs e)
		{
			txtAssignedSlot.Text = "C1";
		}
		private void btnC2_Click(object sender, EventArgs e)
		{
			txtAssignedSlot.Text = "C2";
		}
		private void btnC3_Click(object sender, EventArgs e)
		{
			txtAssignedSlot.Text = "C3";
		}
		private void btnC4_Click(object sender, EventArgs e)
		{
			txtAssignedSlot.Text = "C4";
		}
		private void btnC5_Click(object sender, EventArgs e)
		{
			txtAssignedSlot.Text = "C5";
		}
		private void btnD1_Click(object sender, EventArgs e)
		{
			txtAssignedSlot.Text = "D1";
		}
		private void btnD2_Click(object sender, EventArgs e)
		{
			txtAssignedSlot.Text = "D2";
		}
		private void btnD3_Click(object sender, EventArgs e)
		{
			txtAssignedSlot.Text = "D3";
		}
		private void btnD4_Click(object sender, EventArgs e)
		{
			txtAssignedSlot.Text = "D4";
		}
		private void btnD5_Click(object sender, EventArgs e)
		{
			txtAssignedSlot.Text = "D5";
		}
		private void btnE1_Click(object sender, EventArgs e)
		{
			txtAssignedSlot.Text = "E1";
		}
		private void btnE2_Click(object sender, EventArgs e)
		{
			txtAssignedSlot.Text = "E2";
		}
		private void btnE3_Click(object sender, EventArgs e)
		{
			txtAssignedSlot.Text = "E3";
		}
		private void btnE4_Click(object sender, EventArgs e)
		{
			txtAssignedSlot.Text = "E4";
		}
		private void btnE5_Click(object sender, EventArgs e)
		{
			txtAssignedSlot.Text = "E5";
		}
		private void btnF1_Click(object sender, EventArgs e)
		{
			txtAssignedSlot.Text = "F1";
		}
		private void btnF2_Click(object sender, EventArgs e)
		{
			txtAssignedSlot.Text = "F2";
		}
		private void btnF3_Click(object sender, EventArgs e)
		{
			txtAssignedSlot.Text = "F3";
		}
		private void btnF4_Click(object sender, EventArgs e)
		{
			txtAssignedSlot.Text = "F4";
		}
		private void btnF5_Click(object sender, EventArgs e)
		{
			txtAssignedSlot.Text = "F5";
		}
		private void btnG1_Click(object sender, EventArgs e)
		{
			txtAssignedSlot.Text = "G1";
		}
		private void btnG2_Click(object sender, EventArgs e)
		{
			txtAssignedSlot.Text = "G2";
		}
		private void btnG3_Click(object sender, EventArgs e)
		{
			txtAssignedSlot.Text = "G3";
		}
		private void btnG4_Click(object sender, EventArgs e)
		{
			txtAssignedSlot.Text = "G4";
		}
		private void btnG5_Click(object sender, EventArgs e)
		{
			txtAssignedSlot.Text = "G5";
		}
	}
}