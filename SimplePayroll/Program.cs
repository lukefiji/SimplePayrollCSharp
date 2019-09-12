using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        public Admin(string name) : base(name, adminHourlyRate) { }

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
            return "Name: " + NameOfStaff + "\n Admin Hourly Rate: " + adminHourlyRate + "\nHours Worked: " + HoursWorked + "\n Overtime Rate: " + overtimeRate + "\nTotal Pay: " + TotalPay + "\n Basic Pay: " + BasicPay;
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
                using (StreamReader sr = new StreamReader(path))
                {
                    while (!sr.EndOfStream)
                    {
                        result = sr.ReadLine().Split(separator, System.StringSplitOptions.None);
                        string name = result[0];
                        string role = result[1];

                        if (role == "Admin")
                        {
                            myStaff.Add(new Admin(name));
                        }
                        else if (role == "Manager")
                        {
                            myStaff.Add(new Manager(name));
                        }
                    }
                    // Close file so other programs can use it
                    sr.Close();
                }
            }
            else
            {
                Console.WriteLine("Error: Staff file not found");
            }

            return myStaff;
        }

    }

    class PaySlip
    {
        private int month;
        private int year;

        // Enums declared inside a class are private by default
        enum MonthsOfYear
        {
            JAN = 1,
            FEB = 2,
            MAR = 3,
            APR = 4,
            MAY = 5,
            JUN = 6,
            JUL = 7,
            AUG = 8,
            SEP = 9,
            OCT = 10,
            NOV = 11,
            DEC = 12
        }

        public PaySlip(int payMonth, int payYear)
        {
            month = payMonth;
            year = payYear;
        }

        public void GeneratePaySlip(List<Staff> myStaff)
        {
            string path;

            foreach (Staff f in myStaff)
            {
                path = f.NameOfStaff + ".txt";

                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.WriteLine("PAYSLIP FOR {0} {1}", (MonthsOfYear)month, year);
                    sw.WriteLine("==============================");
                    sw.WriteLine("Name Of Staff: {0}", f.NameOfStaff);
                    sw.WriteLine("Hours Worked: {0}", f.HoursWorked);
                    sw.WriteLine("");
                    sw.WriteLine("Basic Pay: {0:C}", f.BasicPay);

                    if (f.GetType() == typeof(Manager))
                        sw.WriteLine("Allowance: {0:C}", ((Manager)f).Allowance);
                    else if (f.GetType() == typeof(Admin))
                        sw.WriteLine("Overtime: {0:C}", ((Admin)f).Overtime);

                    sw.WriteLine("");
                    sw.WriteLine("==============================");
                    sw.WriteLine("Total Pay: {0}", f.TotalPay);
                    sw.WriteLine("==============================");
                    sw.Close();
                }
            }
        }

        public void GenerateSummary(List<Staff> staff)
        {
            var result =
                from employee in staff
                where employee.HoursWorked < 0
                orderby employee.NameOfStaff ascending
                select new { employee.NameOfStaff, employee.HoursWorked };

            string path = "summary.txt";
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine("Staff with less than 10 working hours");
                sw.WriteLine("");

                foreach (var employee in result)
                {
                    Console.WriteLine("Name of Staff: {0}, Hours Worked: {1}", employee.NameOfStaff, employee.HoursWorked);
                }

                sw.Close();
            }
        }

        public override string ToString()
        {
            return "Month = " + month + ", Year = " + year;
        }
    }
}
