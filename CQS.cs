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
                //recommend alternative POS and add customer to it
                return GetRecommendedPOS(customer, store);
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
                    if (customerCheckoutChoice == CheckoutType.Express)
                    {
                        if(store.CashierPOSList[i].CheckoutType == CheckoutType.Express)
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
                        else
                        {
                            if (store.CashierPOSList[i].AcceptsPaymentType(customerPaymentChoice))
                            {
                                if (store.CashierPOSList[i].Customers.Count() < 3 && store.CashierPOSList[i].Customers.Count() + 1 < currBestLaneNumOfCustomers)
                                {
                                    currBestLane = i;
                                    currBestLaneNumOfCustomers = store.CashierPOSList[i].Customers.Count() + 1;
                                }
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

        //this method attempts to get the best possible POS, assuming that no ideal POS is found
        public static string GetRecommendedPOS(Customer customer, Store store)
        {
            //try switching checkout type
            if(customer.checkoutChoice == CheckoutType.SCO)
            {
                //search for cashier POS instead
                int bestPOSNum = checkCashierPOSs(customer.paymentChoice, CheckoutType.Cashier, store);

                if(bestPOSNum >= 0)
                {
                    store.CashierPOSList[bestPOSNum].Customers.Add(customer);
                    return "Sorry for the inconvenience. If you would like to checkout sooner, go to cashier register #" + store.CashierPOSList[bestPOSNum].POSNumber;
                }
            }
            else
            {
                //search for SCO instead
                int bestPOSNum = checkSCO_POSs(customer.paymentChoice, store);

                if (bestPOSNum >= 0)
                {
                    store.SCO_POSList[bestPOSNum].Customers.Add(customer);
                    return "Sorry for the inconvenience. If you would like to checkout sooner, go to SCO register #" + store.SCO_POSList[bestPOSNum].POSNumber;
                }
            }

            //if there is still no recommendation found, try switching payment type
            if(customer.paymentChoice == PaymentType.Cash)
            {
                int bestPOSNum;

                if (customer.checkoutChoice == CheckoutType.SCO)
                {
                    bestPOSNum = checkSCO_POSs(PaymentType.Cash, store);

                    if (bestPOSNum >= 0)
                    {
                        store.SCO_POSList[bestPOSNum].Customers.Add(customer);

                        return "Sorry for the inconvenience. If you would like to checkout sooner, and can pay with cash, go to register #" + store.SCO_POSList[bestPOSNum].POSNumber;
                    }
                }
                else
                {
                    bestPOSNum = checkCashierPOSs(PaymentType.Cash, customer.checkoutChoice, store);

                    if (bestPOSNum >= 0)
                    {
                        store.CashierPOSList[bestPOSNum].Customers.Add(customer);

                        return "Sorry for the inconvenience. If you would like to checkout sooner, and can pay with cash, go to register #" + store.CashierPOSList[bestPOSNum].POSNumber;
                    }
                }


            }
            //check for card
            else
            {
                int bestPOSNum;

                if (customer.checkoutChoice == CheckoutType.SCO)
                {
                    bestPOSNum = checkSCO_POSs(PaymentType.Card, store);

                    if(bestPOSNum >= 0)
                    {
                        store.SCO_POSList[bestPOSNum].Customers.Add(customer);

                        return "Sorry for the inconvenience. If you would like to checkout sooner, and can pay with card, go to register #" + store.SCO_POSList[bestPOSNum].POSNumber;
                    }
                }
                else
                {
                    bestPOSNum = checkCashierPOSs(PaymentType.Card, customer.checkoutChoice, store);

                    if (bestPOSNum >= 0)
                    {
                        store.CashierPOSList[bestPOSNum].Customers.Add(customer);

                        return "Sorry for the inconvenience. If you would like to checkout sooner, and can pay with card, go to register #" + store.CashierPOSList[bestPOSNum].POSNumber;
                    }
                }

            }

            //if there is still no recommendation found, switch both
            if (customer.checkoutChoice == CheckoutType.SCO)
            {
                if (customer.paymentChoice == PaymentType.Cash)
                {
                    int bestPOSNum = checkCashierPOSs(PaymentType.Card, CheckoutType.Cashier, store);

                    if (bestPOSNum >= 0)
                    {
                        store.CashierPOSList[bestPOSNum].Customers.Add(customer);

                        return "Sorry for the inconvenience. If you would like to checkout sooner, and can pay WITH CARD, go to cashier register #" + store.CashierPOSList[bestPOSNum].POSNumber;
                    }
                }
                else
                {
                    int bestPOSNum = checkCashierPOSs(PaymentType.Cash, CheckoutType.Cashier, store);

                    if (bestPOSNum >= 0)
                    {
                        store.CashierPOSList[bestPOSNum].Customers.Add(customer);

                        return "Sorry for the inconvenience. If you would like to checkout sooner, and can pay WITH CASH, go to cashier register #" + store.CashierPOSList[bestPOSNum].POSNumber;
                    }
                }
            }
            else
            {
                if (customer.paymentChoice == PaymentType.Cash)
                {
                    int bestPOSNum = checkSCO_POSs(PaymentType.Card, store);

                    if (bestPOSNum >= 0)
                    {
                        store.SCO_POSList[bestPOSNum].Customers.Add(customer);

                        return "Sorry for the inconvenience. If you would like to checkout sooner, and can pay WITH CARD, go to SCO register #" + store.SCO_POSList[bestPOSNum].POSNumber;
                    }
                }
                else
                {
                    int bestPOSNum = checkSCO_POSs(PaymentType.Cash, store);

                    if (bestPOSNum >= 0)
                    {
                        store.SCO_POSList[bestPOSNum].Customers.Add(customer);

                        return "Sorry for the inconvenience. If you would like to checkout sooner, and can pay WITH CASH, go to SCO register #" + store.SCO_POSList[bestPOSNum].POSNumber;
                    }
                }
            }

            //otherwise, alert store associates that no recommendation was found

            //TODO: UPDATE SIMULATION

            return "Could not find desired POS. Alerting Store Associate";
        }
    }
}
