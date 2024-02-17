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
        public int POSNumber;
        public PaymentType paymentType;
        public CheckoutType checkoutType;
        public bool isExpress;
        public CheckoutState checkoutState;
        public int maxCustomerCount;
        public List<Customer> customers;
    }
}
