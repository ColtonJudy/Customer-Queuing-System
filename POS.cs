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
        public bool acceptsCash;
        public bool acceptsCard;
        public CheckoutType checkoutType;
        public bool isExpress;
        public CheckoutState checkoutState;
        public int maxCustomerCount;
        public List<Customer> customers = new List<Customer>();

        public POS(int pOSNumber, bool acceptsCash, bool acceptsCard, CheckoutType checkoutType, bool isExpress, CheckoutState checkoutState, int maxCustomerCount)
        {
            POSNumber = pOSNumber;
            this.acceptsCash = acceptsCash;
            this.acceptsCard = acceptsCard;
            this.checkoutType = checkoutType;
            this.isExpress = isExpress;
            this.checkoutState = checkoutState;
            this.maxCustomerCount = maxCustomerCount;
        }

        public bool AcceptsPaymentType(PaymentType paymentType)
        {
            if(paymentType == PaymentType.Cash)
            {
                return acceptsCash;
            }
            else
            {
                return acceptsCard;
            }
        }
    }
}
