using System.Collections.Generic;

namespace d_06.Model
{
    public static class CustomerExtensions
    {
        public static bool HasLessGoods(CashRegister cashRegister1, CashRegister cashRegister2)
        {
            if (cashRegister1.GoodsCount < cashRegister2.GoodsCount)
                return true;
            return false;
        }
        
        public static bool HasLessCustomers(CashRegister cashRegister1, CashRegister cashRegister2)
        {
            if (cashRegister1.CustomersCount < cashRegister2.CustomersCount)
                return true;
            return false;
        }
    }
}