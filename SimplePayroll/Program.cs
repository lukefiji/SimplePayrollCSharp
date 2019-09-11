using System;
using System.Collections.Generic;
using System.IO;

namespace SimplePayroll
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    class Staff
    {
        private float hourlyRate;
        private int hWorked;
        public float TotalPay { get; protected set; }
        public float BasicPay { get; private set; }
        public string NameOfStaff { get; private set; }
        public int HoursWorked
        {
            get
            {
                return hWorked;
            }
            set
            {
                hWorked = value > 0 ? hWorked : 0;
            }
        }

        public Staff(string name, float rate)
        {
            NameOfStaff = name;
            hourlyRate = rate;
        }

        public virtual void CalculatePay()
        {
            Console.WriteLine("Calculating Pay...");

            BasicPay = hWorked * hourlyRate;
            TotalPay = BasicPay;
        }

        public override string ToString()
        {
            return "Name: " + NameOfStaff + "\nHourly Rate: " + hourlyRate + "\nHours Worked: " + HoursWorked + "\nTotal Pay: " + TotalPay + "\n Basic Pay: " + BasicPay;
        }
    }


    class Manager : Staff
    {
        private const float managerHourlyRate = 50;
        public int Allowance { get; private set; }

        public Manager(string name) : base(name, managerHourlyRate) { }

        public override void CalculatePay()
        {
            base.CalculatePay();

            Allowance = 1000;

            if (HoursWorked > 160)
            {
                TotalPay = BasicPay + Allowance;
            }
        }


        public override string ToString()
        {
            return "Name: " + NameOfStaff + "\n Manager Hourly Rate: " + managerHourlyRate + "\nHours Worked: " + HoursWorked + "\nAllowance: " + Allowance + "\nTotal Pay: " + TotalPay + "\n Basic Pay: " + BasicPay;
        }

    }

    class Admin : Staff
    {
        private const float overtimeRate = 15.5f;
        private const float adminHourlyRate = 30;

        public float Overtime { get; private set; }

        public Admin (string name) : base(name, adminHourlyRate) { }

        public override void CalculatePay()
        {
            base.CalculatePay();

            if (HoursWorked > 160)
            {
                Overtime = overtimeRate * (HoursWorked - 160);
                TotalPay = BasicPay + Overtime;
            }
        }

        public override string ToString()
        {
            return "Name: " + NameOfStaff + "\n Admin Hourly Rate: " + adminHourlyRate + "\nHours Worked: " + HoursWorked  + "\n Overtime Rate: " + overtimeRate + "\nTotal Pay: " + TotalPay + "\n Basic Pay: " + BasicPay;
        }

    }

    class FileReader
    {
        public List<Staff> ReadFile()
        {
            List<Staff> myStaff = new List<Staff>();
            string[] result = new string[2];
            string path = "staff.txt";
            string[] separator = { ", " };

            if (File.Exists(path))
            {
                // Using ensures that Dispose() method is always called
                // - Closes/releases any unmanaged resources such as files & streams
                using (StreamReader sr=new StreamReader(path))
                {
                    while (!sr.EndOfStream)
                    {
                        result = sr.ReadLine().Split(separator, System.StringSplitOptions.None);
                        if (result[1] == "Admin")
                        {

                        } else if (result[1] == "Manager")
                        {

                        } else
                        {

                        }
                    }
                    // Close file so other programs can use it
                    sr.Close();
                }
            } else
            {
                Console.WriteLine("Error: Staff file not found");
            }
        }

    }

    class PaySlip
    {

    }
}
