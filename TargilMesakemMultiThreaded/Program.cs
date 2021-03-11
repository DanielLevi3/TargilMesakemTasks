
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TargilMesakemMultiThreaded
{
    class Program
    {
        static Random r = new Random();
 
        public delegate double sumFunctionGet2Doubles(double a, double b); //question 3  
        
        static void PrintInvokedResult(sumFunctionGet2Doubles s , double x ,double y)
        {//question 3
            double res = s(x, y);
            Console.WriteLine($"{res}");
        }
        static double Add (double a , double b )
        {// question 3 
            return a + b;
        }

        // שאלת אתגר 5 
        static void SendSmsAfterGettingMinus(object sender, BankAccountMinusEventArgs e)
        {
            Console.WriteLine($"SMS: recieving by {sender}...");
            Console.WriteLine($"SMS: -- Body: you tried to withdraw {e.Amount} dollars and now your balance is: {e.Balance - e.Amount}  --");
        }
        private static event EventHandler<BankAccountMinusEventArgs> invocationMethodsList;
        private static void Withdrawing(int balance, int amount)
        {
            Console.WriteLine($"Withdraing {amount} from {balance}");
            Thread.Sleep(3000);

            if (invocationMethodsList != null)
            {
                if (balance < amount)
                {
                    invocationMethodsList("ATM withdraw",
                        new BankAccountMinusEventArgs { Amount = amount, Balance = balance });
                }
            }

        }
        //                   ^^    שאלת אתגר 5   ^^

        //                   question 9
        static void LongOperation()
        {
            for (int i = 0; i < 1000000000; i++)
            {

            }
            Console.WriteLine("Done");
        }
        //            question 14
        static void DownloadinFile()
        {
            Console.WriteLine("Downloading file ....");
            Thread.Sleep(10000);
            Console.WriteLine("Download Completed!");
        }
        static int Multiply(int x , int y)
        {
            return x * y;
        }

        //      question 19
        static object key = new object();
        static void DoctorTreatment()
        {
            lock(key)
            {
                Thread.Sleep(500);
                Console.WriteLine("Waiting for my turn");
                Thread.Sleep(300);
                Monitor.Pulse(key);
                Console.WriteLine("Getting treatment");
                Thread.Sleep(2000);
                NurseCheck();
            }
        }
        static void NurseCheck()
        {
            lock(key)
            {
                Console.WriteLine("Nurse is checking");
                Thread.Sleep(5000);
                Console.WriteLine("Next patient please !");
                Thread.Sleep(500);
            }
        }
        // question 22
        //a
        //static ManualResetEvent host = new ManualResetEvent(false);
        //b
        static AutoResetEvent host = new AutoResetEvent(false);
        static void EnterClub()
        {
            Console.WriteLine("Waiting to enter the club");
            host.WaitOne();
            Console.WriteLine("Let's party!!!!");
          
        }
        //question 33
        static CancellationTokenSource source = new CancellationTokenSource();
        static int counter = 0;
       
        static void MyTaimer()
        {
            while (!source.IsCancellationRequested)
            {
                    Thread.Sleep(1000);
                    counter++;
            }
            if(source.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }
        }

        // question 34 
        class WorkersDemo
        {
            private const int num_of_workers = 5;
            private List<Task> workers = new List<Task>();
            private Queue<int> work_queue = new Queue<int>();

            private object key = new object();
           
            public void DoWork()
            {
                Task.WaitAll(workers.ToArray());
            }

            public WorkersDemo(Queue<int> work)
            {
                for (int i = 0; i < num_of_workers; i++)
                {
                    workers.Add(Task.Run(() =>
                    {
                        while (work.Count > 0)
                        {
                            int work_number = 0;
                            try
                            {
                               lock(key)
                                {
                                    work_number = work.Dequeue();
                                }
                                Console.WriteLine("Worker calculate " + work_number * 2);
                            }
                            catch(AggregateException ag)
                            {
                                Console.WriteLine($"Aggregate exception as accourd");
                            }
                        }
                    }));
                }
            }
        }
        // question 38 
        public class MyFileManager
        {

            public const string FILE_PATH = @"C:\Users\levid\final.txt";
            public MyFileManager()
            {

            }

            public async void ReadFromFile()
            {
                try
                {
                    Monitor.Enter(key);
                    while (!File.Exists(FILE_PATH))
                    {
                        Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} will wait");
                        Monitor.Wait(key);
                    }
                    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} done waiting");
                    await Task.Run(() =>
                    {
                        string my_text = File.ReadAllText(FILE_PATH);
                        Console.WriteLine(my_text);
                    });   
                }
                finally
                {
                    Monitor.Exit(key);
                }
            }
        }

        static void Main(string[] args)
        {
           
            /*
            int max = 100;
            List<int> numbers = new List<int>();
            for (int i =0; i < max; i++)
            {
                numbers.Insert(i, r.Next(0, 50));
            }
            var numLessThen10 = from n in numbers
                                where n < 10
                                select n;
// סעיף 1 שאלה 1
            numLessThen10.ToList().ForEach(_ => Console.WriteLine(_));

            var numDivBy3 = from n in numbers
                            where n % 3 == 0
                            select n;
// סעיף 2 שאלה 1
            Console.WriteLine("=================================");
            numDivBy3.ToList().ForEach(_ => Console.WriteLine(_));

            var numBigThen20 = from n in numbers
                               where n > 20 && n % 2 == 0
                               select n;
// סעיף 3 שאלה 1

            Console.WriteLine("=================================");
            numBigThen20.ToList().ForEach(_ => Console.WriteLine(_));

            var numInOrder = from n in numbers
                             orderby n
                             select n;
// סעיף 4 שאלה 1
            Console.WriteLine("=================================");
            numInOrder.ToList().ForEach(_ => Console.WriteLine(_));


            List<string> names = new List<string>();
            names.Add("Daniel");
            names.Add("Dor");
            names.Add("David");
            names.Add("Gil");
            names.Add("Ronen");
            names.Add("Hadar");
            names.Add("Aviel");
            names.Add("Sara");
            names.Add("Jacob");
            names.Add("Barbara");

            var moreThen4 = from na in names
                            where na.Length >= 4
                            select na;
            Console.WriteLine("===============Question 2 part 1===============");
            moreThen4.ToList().ForEach(_ => Console.WriteLine(_));
         
            var containsA = from na in names
                            where na.Contains("a") || na.Contains("A")
                            select na;
            Console.WriteLine("================Question 2 part 2 ================");
            containsA.ToList().ForEach(_ => Console.WriteLine(_));

            var orderNames = from na in names
                             orderby na
                             select na;
            Console.WriteLine("=====================Question 2 part 3 ========================");
            orderNames.ToList().ForEach(_ => Console.WriteLine(_));

            Console.WriteLine("================Question 3========================");

            PrintInvokedResult(Add, 1, 2);
            Console.WriteLine("==============question 5 challange=============");
            invocationMethodsList += SendSmsAfterGettingMinus;
            Withdrawing(5500, 1000);

            Console.WriteLine("==================question 9==================");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 5; i++)
            {
                LongOperation();
            }
            var time = sw.ElapsedMilliseconds;
            Console.WriteLine($"time ={time} ms");
            sw.Stop();
            sw.Reset();
            List<Thread> threads = new List<Thread>();
            for (int i = 0; i <5; i++)
            {
               Thread t= new Thread(() =>
                {
                    LongOperation();
                });
                threads.Add(t);
            }
            sw.Start();
            threads.ForEach(t => t.Start());
            threads.ForEach(t => t.Join());
            sw.Stop();
            time = sw.ElapsedMilliseconds;
            Console.WriteLine($"time = {time} ms");
            
            Console.WriteLine("===================question 12========================");
            Thread t1 = new Thread(() => Console.WriteLine("Hello World!"));
            List<int> numbers12 = new List<int>() { 1, 2, 3, 4, 5, 6 };
            Thread t2 = new Thread((num) =>
           {
               if(numbers12.Contains((int)num))
               {
                   Console.WriteLine($"List Contains num :{num} ");
               }
               else
               {
                   Console.WriteLine($"List does not contains num: {num}");
               }
           });
            t2.Start(7);
           
            Console.WriteLine("=========================question 14===========================");
            ThreadPool.QueueUserWorkItem((o) =>
            {
                Console.WriteLine($"Multiply is :{Multiply(2, 5)}");
            }, null);


            Thread t3 = new Thread(() =>
             {
                 DownloadinFile();
             });
            t3.Start();
            t3.Join();
            
            Console.WriteLine("============================Question 19============================");
            List<Thread> list_t = new List<Thread>();
            for (int i = 0; i < 5; i++)
            {
                Thread t = new Thread(() =>
                {
                    DoctorTreatment();
                });
                list_t.Add(t);
            }
            foreach (Thread item in list_t)
            {
                item.Start();
            }
             
            Console.WriteLine("=======================Question 22==============================");
            List<Thread> party_threads = new List<Thread>();
            for (int i = 0; i < 50; i++)
            {
                Thread t = new Thread(() =>
                {
                    EnterClub();
                });
                party_threads.Add(t);
            }
            foreach (Thread item in party_threads)
            {
                item.Start();
            }
            Thread.Sleep(3000);
            host.Set();
            
             Console.WriteLine("================Question 25 b=================");
             new Task(() =>
             {
                 Thread.Sleep(10000);
                 Console.WriteLine("Hello World!");
             }, TaskCreationOptions.LongRunning).RunSynchronously();

             Console.WriteLine("====================Question 26===========================");
             int result = 0;
             Task<int> t_res = Task.Run<int>(() =>
             {
                 int x = 8;
                 int y = 9;

                 return x + y;
             });
             result = t_res.Result;
             Console.WriteLine($"{result}");


             Console.WriteLine("=======================Question 27b==============================");
             Task task1 = Task.Run(() =>
              {
                  Console.WriteLine("Hello World!");
              }).ContinueWith((Task papa) =>
              {
                  Console.WriteLine("Goodbye...");
              });
             task1.Wait();
        
            Console.WriteLine("========================Question 28=========================");
            Task t_shirshur = Task.Run(()=>
            {
                DateTime date = DateTime.Now;
                Console.WriteLine($"date is {date}");
            }).ContinueWith((Task son) =>
             {
                 DateTime date = DateTime.Now;
                 Console.WriteLine($"son date : {date}");
             }, TaskContinuationOptions.NotOnFaulted);
            t_shirshur.Wait();
          
            Console.WriteLine("=======================Question 29 b===================");
            List<Task> tasks = new List<Task>();
            Task ts1 = Task.Run(() =>
            {
                Thread.Sleep(5000);
            });
            Task ts2 = Task.Run(() =>
            {
                Thread.Sleep(5000);
            });
            Task ts3 = Task.Run(() =>
            {
                Thread.Sleep(5000);
            });
            tasks.Add(ts1);
            tasks.Add(ts2);
            tasks.Add(ts3);
            Task.WaitAll(tasks.ToArray());
            if(ts1.IsCompleted && ts2.IsCompleted && ts3.IsCompleted)
                Console.WriteLine("All tasks are done!");
            
            Console.WriteLine("========================Question 30 b========================");
            List<Task> tasks_any = new List<Task>();
            Task t4 = Task.Run(() =>
             {
                 Thread.Sleep(r.Next(5000, 10000));
             });tasks_any.Add(t4);
            Task t5 = Task.Run(() =>
            {
                Thread.Sleep(r.Next(5000, 10000));
            }); tasks_any.Add(t5);
            Task t6 = Task.Run(() =>
            {
                Thread.Sleep(r.Next(5000, 10000));
            }); tasks_any.Add(t6);
            Task.WaitAny(tasks_any.ToArray());
            if(t4.IsCompleted||t5.IsCompleted||t6.IsCompleted)
                Console.WriteLine("One task is done!");
           
            Console.WriteLine("===================Question 31 c ================");
            Task<int> t_res1 = Task.Run<int>(() =>
             {
                 int a = 5;
                 int b = 0;
                 int c = a / b;
                 return c;
             });
            try
            {
                t_res1.Wait();
            }
            catch(AggregateException e)
            {
                Console.WriteLine($"Exception handled {e} ");
                Console.WriteLine("Cannot Divide by zero");
            }
           
            Console.WriteLine($"{t_res1}");
            
            Console.WriteLine("=========================Question 33==========================");
            Task timer = Task.Run(()=>
            {
                MyTaimer();
            });
            Console.WriteLine("Timer is starting");
            Console.WriteLine("Press enter to stop timer");
            Console.ReadLine();
            source.Cancel();
            Console.WriteLine($"Timer is stopped,count :{counter} seconds");
            
            Console.WriteLine("==================Question 34=======================");
            Queue<int> numbers = new Queue<int>(8);
            
            numbers.Enqueue(1);
            numbers.Enqueue(2);
            numbers.Enqueue(3);
            numbers.Enqueue(4);
            numbers.Enqueue(5);
            numbers.Enqueue(6);
            numbers.Enqueue(7);
            numbers.Enqueue(8);
            WorkersDemo w = new WorkersDemo(numbers);
            w.DoWork();// there's an exception here 
            

            Console.WriteLine("=====================Question 38===============================");

            MyFileManager m = new MyFileManager();
            m.ReadFromFile();
            */
        }
    }
}
