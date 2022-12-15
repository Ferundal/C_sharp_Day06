using System;
using System.Collections.Generic;
using System.Threading;

namespace d_06.Model
{
    public class Store
    {
        public delegate bool Comparier(CashRegister cashRegister1, CashRegister cashRegister2);
        
        private Storage _storage;
        private List<CashRegister> _cashRegisters;
        private Thread[] _threads;
        private readonly object _cashRegistersLocker = new object();
        private readonly object _threadsLocker = new object();

        public Store(
            int storageCapacity,
            int cashRegistersAmount,
            int goodServiceTime,
            int customerChangeTime)
        {
            _storage = new Storage(storageCapacity);
            if (cashRegistersAmount < 0)
                cashRegistersAmount = 0;
            _cashRegisters = new List<CashRegister>(cashRegistersAmount);
            for (int index = 0; index < cashRegistersAmount; ++index)
            {
                var cashRegister = new CashRegister(
                    $"Register #{index + 1}",
                    goodServiceTime,
                    customerChangeTime);
                _cashRegisters.Add(cashRegister);
            }
        }

        public void AddToQueue(Customer customer, Comparier comparier)
        {
            int optimalIndex = 0;
            lock (_cashRegistersLocker)
            {
                customer.FillCart(_storage);
                if (customer.GoodsAmount == 0)
                    return;
                if (_cashRegisters == null || _cashRegisters.Count == 0)
                    return;
                for (int index = 1; index < _cashRegisters.Count; ++index)
                {
                    if (!comparier(_cashRegisters[optimalIndex], _cashRegisters[index]))
                    {
                        optimalIndex = index;
                    }
                }
                _cashRegisters[optimalIndex].AddToQueue(customer);
            }

            lock (_threadsLocker)
            {
                if (_threads != null)
                {
                    if (!_threads[optimalIndex].IsAlive)
                    {
                        _threads[optimalIndex] = new Thread(_cashRegisters[optimalIndex].ProcessCustomers);
                        _threads[optimalIndex].Start();
                    }
                }
            }
        }

        public void OpenRegisters()
        {
            _threads = new Thread [_cashRegisters.Count];
            for (var index = 0; index < _cashRegisters.Count; ++index)
            {
                _threads[index] = new Thread(_cashRegisters[index].ProcessCustomers);
            }

            foreach (var thread in _threads)
            {
                thread.Start();
            }
        }

        public void ProceedAllCustomers()
        {
            if (_threads == null)
                OpenRegisters();
            foreach (var thread in _threads)
            {
                if (thread.IsAlive)
                    thread.Join();
            }
        }
        

        public bool IsOpen() => !_storage.IsEmpty();

        public override string ToString()
        {
            string res = "";
            foreach (var cashRegister in _cashRegisters)
            {
                res += $"{cashRegister.Name} with {cashRegister.CustomersCount} customers " +
                       $"{cashRegister.GoodsCount} goods.{Environment.NewLine}";
            }
            return res;
        }

        public string Results()
        {
            string res = "";
            foreach (var cashRegister in _cashRegisters)
            {
                var averageTime = cashRegister.LoadTime / cashRegister.CustomersProceed;
                res += $"{cashRegister.Name} with: " +
                       $"good service time={cashRegister.GoodServiceTime} " +
                       $"customer delay={cashRegister.CustomerChangeTime} " +
                       $"average proceed time={averageTime:g}{Environment.NewLine}";
            }
            return res;
        }
    }
}