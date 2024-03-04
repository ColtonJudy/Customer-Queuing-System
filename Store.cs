using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerQueuingSystem
{
    internal class Store
    {
        public List<POS> CashierPOSList = new List<POS>();
        public List<POS> SCO_POSList = new List<POS>();

        public Store(List<POS> POSList)
        {
            foreach(POS pos in POSList)
            {
                if(pos.checkoutType == CheckoutType.SCO)
                {
                    SCO_POSList.Add(pos);
                }
                else
                {
                    CashierPOSList.Add(pos);
                }
            }
        } 
    }
}
