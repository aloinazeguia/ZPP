public class Program
{
    static void Batuketa(int a, int b)
    {
        int emaitza = a + b;
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine($"Batuketa: {a} + {b} = {emaitza}");
            Thread.Sleep(300);
        }
    }
    static void biderketa(int a, int b)
    {
        int emaitza = a * b;
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine($"bideketa: {a} * {b} = {emaitza}");
            Thread.Sleep(1000);
        }
    }
    public static void Main(string[] args)
    {
        Thread h1=new Thread(() => Batuketa(3,5));
        Thread h2=new Thread(() => biderketa(4,6));
        Thread h3=new Thread(() => Batuketa(7,8));
        h1.Start();
        h2.Start();
        h3.Start();
        h1.Join();
        h2.Join();
        h3.Join();
        Console.WriteLine("Hari guztiak amaitu dira. Programa bukatu da.");
    }
}
