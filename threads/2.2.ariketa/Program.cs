public class Program
{
    static int emaitzah1;
    static int emaitzah2;
    
    static void Batuketa(int a, int b, out int emaitza)
    {
        emaitza = a + b;
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine($"Batuketa: {a} + {b} = {emaitza}");
            Thread.Sleep(300);
        }
    }
    static void biderketa(int a, int b,out int emaitza)
    {
        emaitza = a * b;
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine($"bideketa: {a} * {b} = {emaitza}");
            Thread.Sleep(1000);
        }
    }
    static void BiakBatu(int h1,int h2)
    {

       int emaitza=h1+h2;
        Console.WriteLine($"Batuketa: {h1} + {h2} = {emaitza}");
    }
    public static void Main(string[] args)
    {
        Thread h1=new Thread(() => Batuketa(3,5, out emaitzah1));
        Thread h2=new Thread(() => biderketa(4,6, out emaitzah2));
        Thread h3=new Thread(() => BiakBatu(emaitzah1,emaitzah2));
        
        
        h1.Start();
        h2.Start();

        h1.Join();
        h2.Join();

        
        h3.Start();
        h3.Join();

        Console.WriteLine("Hari guztiak amaitu dira. Programa bukatu da.");
    }
}

