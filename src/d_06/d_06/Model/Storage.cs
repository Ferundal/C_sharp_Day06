using System.Threading;

namespace d_06.Model
{
    public class Storage
    {
        public int Count { get; private set; }

        public int Capacity { get; }
        private readonly object _locker = new object();

        public Storage(int capacity)
        {
            if (capacity > 0)
                Capacity = capacity;
            else
                Capacity = 0;
            Count = Capacity;
        }

        public int GetGoods(int goodsAmount)
        {
            lock (_locker)
            {
                if (goodsAmount > Count)
                {
                    goodsAmount = Count;
                    Count = 0;
                }
                else
                {
                    Count -= goodsAmount;
                }
                return goodsAmount;
            }
        }

        public bool IsEmpty() => Count == 0;
    }
}