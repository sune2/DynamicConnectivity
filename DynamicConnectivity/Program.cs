using System;

namespace DynamicConnectivity
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var dc = new DynamicConnectivity(5);
            dc.Link(1, 2);
            dc.Link(0, 1);
            dc.Link(3, 2);
            dc.Link(0, 4);
            dc.Link(4, 3);
            Console.WriteLine(dc.IsConnected(0, 3));
            dc.Cut(2, 1);
            Console.WriteLine(dc.IsConnected(0, 3));
            dc.Cut(4, 3);
            Console.WriteLine(dc.IsConnected(0, 3));
            foreach (var a in dc.GetConnectedNodes(0))
            {
                Console.WriteLine(a);
            }
        }
    }
}
