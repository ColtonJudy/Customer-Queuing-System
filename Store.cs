using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerQueuingSystem
{
    internal class Store
    {
        public int countSCO;
        public int countCashier;
        public int countExpress;
        public int countCash;
        public int countCard;

        List<POS> CashierPOSList;
        List<POS> SCO_POSList;

        public void noteCustomer()
        {
            //get the customer payment type
            PaymentType customerPaymentType = getPaymentType();
            CheckoutType customerCheckoutType = getCheckoutType();
            bool customerIsExpress = getIsExpress();
        }

        //returns index of POS to be used. Returns -1 if none were found
        public int checkSCO_POSs(PaymentType customerPaymentType)
        {
            for(int i = 0; i < SCO_POSList.Count(); i++)
            {
                if (SCO_POSList[i].checkoutState == CheckoutState.Open)
                {
                    if (SCO_POSList[i].paymentType == customerPaymentType)
                    {
                        if (SCO_POSList[i].customers.Count() == 0)
                        {
                            return SCO_POSList[i].POSNumber;
                        }
                    }
                }
            }
            return -1;
        }

        public int checkCashierPOSs(PaymentType customerPaymentType, CheckoutType customerCheckoutType)
        {
            int currBestLane = -1;
            int currBestLaneNumOfCustomers = 3;
            for (int i = 0; i < CashierPOSList.Count(); i++)
            {
                if (CashierPOSList[i].checkoutState == CheckoutState.Open)
                {
                    if (customerCheckoutType == CheckoutType.Express && CashierPOSList[i].checkoutType == CheckoutType.Express)
                    {
                        if (CashierPOSList[i].customers.Count() < 3 && CashierPOSList[i].customers.Count() < currBestLaneNumOfCustomers)
                        {
                            currBestLane = CashierPOSList[i].POSNumber;
                            currBestLaneNumOfCustomers = CashierPOSList[i].customers.Count();
                        }
                    }
                    else if (CashierPOSList[i].paymentType == customerPaymentType)
                    {
                        if (CashierPOSList[i].customers.Count() < 3 && CashierPOSList[i].customers.Count() < currBestLaneNumOfCustomers)
                        {
                            currBestLane = CashierPOSList[i].POSNumber;
                            currBestLaneNumOfCustomers = CashierPOSList[i].customers.Count();
                        }
                    }
                }
            }
            return currBestLane;
        }
    }
}
