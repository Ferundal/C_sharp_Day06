using System;

namespace d_06.Model
{
    public class Customer : IEquatable<Customer>
    {
        public int SerialNumber { get; }
        public string Name { get; }
        
        public int GoodsAmount { get; private set; }
        private static Random _random = null;

        public Customer(string name, int serialNumber)
        {
            SerialNumber = serialNumber;
            Name = name;
            GoodsAmount = 0;
            _random ??= new Random();
        }

        public void ShoppingList(int maximumCartCapacity)
        {
            GoodsAmount = _random.Next(1, maximumCartCapacity + 1);
        }
        
        public void FillCart(Storage storage)
        {
            GoodsAmount = storage.GetGoods(GoodsAmount);
        }

        public static bool operator ==(Customer customer1, Customer customer2)
        {
            if (ReferenceEquals(customer1, customer2))
                return true;
            if (ReferenceEquals(customer1, null))
                return false;
            if (ReferenceEquals(customer2, null))
                return false; 
            if (customer1.SerialNumber == customer2.SerialNumber 
                && customer1.Name == customer2.Name)
                return true;
            return false;
        }

        public static bool operator !=(Customer customer1, Customer customer2)
        {
            if (ReferenceEquals(customer1, customer2))
                return false;
            if (ReferenceEquals(customer1, null))
                return true;
            if (ReferenceEquals(customer2, null))
                return true; 
            if (customer1.SerialNumber != customer2.SerialNumber
                || customer1.Name != customer2.Name) 
                return true; 
            return false;
        }

        public bool Equals(Customer other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return SerialNumber == other.SerialNumber && Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Customer)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SerialNumber, Name);
        }
        
        public override string ToString()
        {
            return $"{Name}, customer #{SerialNumber}";
        }

    }
}