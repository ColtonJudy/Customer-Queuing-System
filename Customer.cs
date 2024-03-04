using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerQueuingSystem
{


    internal class Customer
    {
        public PaymentType paymentChoice;
        public CheckoutType checkoutChoice;
        public bool wantsExpress;

        public Customer(PaymentType paymentChoice, CheckoutType checkoutChoice, bool wantsExpress)
        {
            this.paymentChoice = paymentChoice;
            this.checkoutChoice = checkoutChoice;
            this.wantsExpress = wantsExpress;
        }
    }
}
