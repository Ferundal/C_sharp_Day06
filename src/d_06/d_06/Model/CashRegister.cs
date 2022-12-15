using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace d_06.Model
{
    public class CashRegister : IEquatable<CashRegister>
    {
        public TimeSpan LoadTime { get; private set; } = default;
        public string Name { get; }
        public int CustomersProceed { get; private set; } = 0;
        public int CustomersCount => _customers.Count;

        public int GoodsCount
        {
            get
            {
                return _customers.Sum(customer => customer.GoodsAmount);
            }
        }
        private BlockingCollection<Customer> _customers;
        public TimeSpan GoodServiceTime { get; private set; }
        public TimeSpan CustomerChangeTime { get; private set; }
        private static readonly Random Random = new Random();

        public CashRegister(string name, int goodServiceTime, int customerChangeTime)
        {
            Name = name;
            _customers = new BlockingCollection<Customer>(new ConcurrentQueue<Customer>());
            goodServiceTime = Random.Next(1, goodServiceTime + 1);
            GoodServiceTime = new TimeSpan(0, 0, 0, goodServiceTime);
            customerChangeTime = Random.Next(1, customerChangeTime + 1);
            CustomerChangeTime = new TimeSpan(0, 0, 0, customerChangeTime);
        }

        public void AddToQueue(Customer customer)
        {
            _customers.Add(customer);
        }

        public Customer First()
        {
            if (_customers.Count > 0)
                return _customers.First();
            return null;
        }

        public void ProcessCustomers()
        {
            while (Process()) { }
        }
        
        public bool Process()
        {
            if (_customers.Count > 0)
            {
                var customer = _customers.First();
                _customers.Take();
                ++CustomersProceed;
                var workTime = GoodServiceTime * customer.GoodsAmount + CustomerChangeTime;
                LoadTime += workTime;
                Thread.Sleep(workTime);
                Console.WriteLine(
                    $"{DateTime.Now.ToString("HH:mm:ss")}: {Name} finish to process {customer.Name}" +
                    $" with {customer.GoodsAmount} goods" +
                    $"({CustomersCount} customers and {GoodsCount} goods left in queue). " +
                    $"Time spend: {LoadTime}"
                );
                return true;
            }
            return false;
        }

        public bool Equals(CashRegister other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CashRegister)obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}