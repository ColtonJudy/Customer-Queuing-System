using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CustomerQueuingSystem
{
    internal class Simulation
    {
        //used for running the simulation console
        [DllImport("Kernel32")]
        static extern void AllocConsole();

        [DllImport("Kernel32")]
        static extern void FreeConsole();

        Store store;

        //opens the simulation console
        public Simulation(Store store)
        {
            this.store = store;
            AllocConsole();
        }

        //starts the simulation
        public void Start()
        {
            PrintSim();
        }

        //updates the simulation object with any changes to the store
        public void Update(Store store)
        {
            this.store = store;

            PrintSim();
        }

        private void PrintSim()
        {
            Console.Clear();
            Console.WriteLine("Cashiers\n");
            foreach (POS cashier in store.CashierPOSList)
            {
                Console.Write(cashier + ": ");
                for (int i = 0; i < cashier.Customers.Count; i++)
                {
                    Console.Write("X");
                }
                Console.Write("\n\n");
            }

            Console.WriteLine("\nSelf Checkout\n");
            foreach (POS cashier in store.SCO_POSList)
            {
                Console.Write(cashier + ": ");
                for (int i = 0; i < cashier.Customers.Count; i++)
                {
                    Console.Write("X");
                }
                Console.Write("\n\n");
            }
        }
    }
}
