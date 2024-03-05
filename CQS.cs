using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerQueuingSystem
{
    internal static class CQS
    {
        //adds the customer to store, and returns the recommendation string for the window to display
        public static string AddCustomerToStore(Customer customer, Store store)
        {
            int bestPOSNum;

            //get the best POS index
            if(customer.checkoutChoice == CheckoutType.SCO)
            {
                bestPOSNum = checkSCO_POSs(customer.paymentChoice, store);
            }
            else
            {
                bestPOSNum = checkCashierPOSs(customer.paymentChoice, customer.checkoutChoice, store);
            }

            //if the desired POS is found
            if(bestPOSNum >= 0)
            {
                //add customer to POS and display message

                if(customer.checkoutChoice == CheckoutType.SCO)
                {
                    store.SCO_POSList[bestPOSNum].Customers.Add(customer);

                    return "Please go to self-checkout register #" + store.SCO_POSList[bestPOSNum].POSNumber;
                }
                else
                {
                    store.CashierPOSList[bestPOSNum].Customers.Add(customer);

                    return "Please go to cashier register #" + store.CashierPOSList[bestPOSNum].POSNumber;
                }
            }
            else
            {
                //recommend alternative POS and alert staff

                return "Could not find desired POS. Recommend alternative";
            }
        }

        //returns index of POS to be used. Returns -1 if none were found
        public static int checkSCO_POSs(PaymentType customerPaymentChoice, Store store)
        {
            for (int i = 0; i < store.SCO_POSList.Count(); i++)
            {
                if (store.SCO_POSList[i].CheckoutState == CheckoutState.Open)
                {
                    if (store.SCO_POSList[i].AcceptsPaymentType(customerPaymentChoice))
                    {
                        if (store.SCO_POSList[i].Customers.Count() == 0)
                        {
                            return i;
                        }
                    }
                }
            }
            return -1;
        }

        public static int checkCashierPOSs(PaymentType customerPaymentChoice, CheckoutType customerCheckoutChoice, Store store)
        {
            int currBestLane = -1;
            int currBestLaneNumOfCustomers = 3;
            for (int i = 0; i < store.CashierPOSList.Count(); i++)
            {
                if (store.CashierPOSList[i].CheckoutState == CheckoutState.Open)
                {
                    //if this POS is Express, and the customer wants express, use it
                    if (customerCheckoutChoice == CheckoutType.Express && store.CashierPOSList[i].CheckoutType == CheckoutType.Express)
                    {
                        if (store.CashierPOSList[i].AcceptsPaymentType(customerPaymentChoice))
                        {
                            if (store.CashierPOSList[i].Customers.Count() < 3 && store.CashierPOSList[i].Customers.Count() < currBestLaneNumOfCustomers)
                            {
                                currBestLane = i;
                                currBestLaneNumOfCustomers = store.CashierPOSList[i].Customers.Count();
                            }
                        }
                    }
                    //otherwise, if this POS is Cashier, and the customer wants Cashier, use it
                    else if (customerCheckoutChoice == CheckoutType.Cashier && store.CashierPOSList[i].CheckoutType == CheckoutType.Cashier)
                    {
                        if (store.CashierPOSList[i].AcceptsPaymentType(customerPaymentChoice))
                        {
                            if (store.CashierPOSList[i].Customers.Count() < 3 && store.CashierPOSList[i].Customers.Count() < currBestLaneNumOfCustomers)
                            {
                                currBestLane = i;
                                currBestLaneNumOfCustomers = store.CashierPOSList[i].Customers.Count();
                            }
                        }
                    }
                }
            }
            return currBestLane;
        }
    }
}
