using System;

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

    }

    class FileReader
    {

    }

    class PaySlip
    {

    }
}
