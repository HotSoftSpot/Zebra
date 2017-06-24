namespace Zebra
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Zebra zebra = new Zebra(args);
            zebra.Run();
            return 0;
        }
    }
}
