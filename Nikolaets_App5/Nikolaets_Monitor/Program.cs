using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nikolaets_Monitor
{
    class Henhouse
    {
        
       public readonly object _lock = new object();

        public void RefillFood(int portions)
        {
            lock (_lock)
            {
                Console.WriteLine($"Hen refilling food: {portions} portions");
                Thread.Sleep(2000); // Перерва на наповнення їжі
                Program._foodPortions += portions;
                Monitor.PulseAll(_lock);
            }
        }

        public void RequestFood()
        {
            
            lock (_lock)
            {
                while (Program._foodPortions == 0)
                {
                    
                    RefillFood(Program._foodPortionsLimit);
                    

                }
            }
        }
    }

    class Chick
    {
        private readonly Henhouse _henhouse;
        private readonly int _id;

        public Chick(Henhouse henhouse, int id)
        {
            _henhouse = henhouse;
            _id = id;
        }

        public void Eat()
        {
            while (true)
            {
                Console.WriteLine($"Chick {_id} is waiting for food");
                Thread.Sleep(1000); // Чекаємо перед перевіркою доступності їжі

                lock (Program._lock)
                {
                    if (Program._foodPortions == 0)
                    {
                        Monitor.Enter(_henhouse._lock);
                        _henhouse.RequestFood();
                        Monitor.Exit(_henhouse._lock);
                    }

                    Console.WriteLine($"Chick {_id} is eating");
                    Thread.Sleep(2000); // Час прийому їжі

                    Program._foodPortions--;
                }
            }
        }
    }

    class Hen
    {
        private readonly Henhouse _henhouse;

        public Hen(Henhouse henhouse)
        {
            _henhouse = henhouse;
        }

        public  void ProvideFood()
        {
            while (true)
            {
                lock (Program._lock)
                {
                    if (Program._foodPortions == 0)
                    {
                        _henhouse.RefillFood(Program._foodPortionsLimit);
                    }
                }

                Thread.Sleep(1000); // Чекаємо перед перевіркою потреби у нових порціях їжі
            }
        }
    }
    internal class Program
    {
        public static object _lock = new object();
        public static int _foodPortions;
        public static int _foodPortionsLimit;

        static void Main()
        {
            // Ініціалізуємо змінні
            _foodPortions = 0;
            _foodPortionsLimit = 5;

            // Створюємо курника і квочку
            Henhouse henhouse = new Henhouse();
            Hen hen = new Hen(henhouse);

            // Створюємо потоки для пташенят
            for (int i = 0; i < 10; i++)
            {
                Chick chick = new Chick(henhouse, i);
                Thread chickThread = new Thread(chick.Eat);
                chickThread.Start();
            }

            // Потік квочки
            Thread henThread = new Thread(hen.ProvideFood);
            henThread.Start();
        }
    }
}
