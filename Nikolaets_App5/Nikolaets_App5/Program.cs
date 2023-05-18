using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nikolaets_App5
{
    /// <summary>
    /// Клас курятник
    /// </summary>
    class Henhouse
    {
        private readonly SemaphoreSlim _henSemaphore;

        public Henhouse()
        {
            _henSemaphore = new SemaphoreSlim(0, 1);
        }

        public void RefillFood(int portions)
        {
            Console.WriteLine($"Hen refilling food: {portions} portions");
            Thread.Sleep(2000); // Перерва на наповнення їжі
            Program._foodPortions += portions;
            _henSemaphore.Release();
        }

        public void RequestFood()
        {
            _henSemaphore.Wait();
        }
    }
    /// <summary>
    /// Клас курча
    /// </summary>
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
            int i = 0;
            while (i<=100)
            {
                Console.WriteLine($"Chick {_id} is waiting for food");
                Thread.Sleep(1000); // Чекаємо перед перевіркою доступності їжі

                if (Program._foodPortions == 0)
                {
                    _henhouse.RequestFood();
                }

                Console.WriteLine($"Chick {_id} is eating");
                Thread.Sleep(2000); // Час прийому їжі

                Program._foodPortions--;
                i++;
            }
        }
    }
    /// <summary>
    /// Клас куриця
    /// </summary>
    class Hen
    {
        private readonly Henhouse _henhouse;

        public Hen(Henhouse henhouse)
        {
            _henhouse = henhouse;
        }

        public void ProvideFood()
        {
            int i = 0;
            while (i<=100)
            {
                if (Program._foodPortions == 0)
                {
                    _henhouse.RefillFood(Program._foodPortionsLimit);
                }

                Thread.Sleep(1000); // Чекаємо перед перевіркою потреби у нових порціях їжі
                i++;
            }
        }
    }
    internal class Program
    {
        static SemaphoreSlim _bowlSemaphore;
        public static int _foodPortions;
        public static int _foodPortionsLimit;
        static void Main(string[] args)
        {
            // Ініціалізуємо змінні
            _foodPortions = 0;
            _foodPortionsLimit = 5;
            _bowlSemaphore = new SemaphoreSlim(0, _foodPortionsLimit);

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
