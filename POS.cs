using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerQueuingSystem
{
    public enum PaymentType
    {
        Cash,
        Card
    }

    public enum CheckoutType
    {
        SCO,
        Cashier,
        Express
    }

    public enum CheckoutState
    {
        Open,
        Closed,
        Delayed
    }

    internal class POS
    {
        private List<Customer> customers = new List<Customer>();

        private int totalCustomersServed = 0;

        public int POSNumber { get; set; }
        public bool AcceptsCash { get; set; }
        public bool AcceptsCard { get; set; }
        public CheckoutType CheckoutType { get; set; }
        public bool IsExpress { get; set; }
        public CheckoutState CheckoutState { get; set; }
        public int MaxCustomerCount { get; set; }

        public POS()
        {

        }

        public POS(int pOSNumber, bool acceptsCash, bool acceptsCard, CheckoutType checkoutType, bool isExpress, CheckoutState checkoutState, int maxCustomerCount)
        {
            POSNumber = pOSNumber;
            this.AcceptsCash = acceptsCash;
            this.AcceptsCard = acceptsCard;
            this.CheckoutType = checkoutType;
            this.IsExpress = isExpress;
            this.CheckoutState = checkoutState;
            this.MaxCustomerCount = maxCustomerCount;
        }

        public int CustomerCount()
        {
            return customers.Count;
        }

        public int TotalCustomersServed()
        {
            return totalCustomersServed;
        }

        public void AddCustomer(Customer customer)
        {
            totalCustomersServed++;
            customers.Add(customer);
        }

        public void DeleteCustomer(Customer customer)
        {
            customers.Remove(customer);
        }

        public override string ToString()
        {
            return "POS #" + POSNumber;
        }

        public bool AcceptsPaymentType(PaymentType paymentType)
        {
            if(paymentType == PaymentType.Cash)
            {
                return AcceptsCash;
            }
            else
            {
                return AcceptsCard;
            }
        }
    }
}
