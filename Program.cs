using DataProviderService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift.Server;

namespace EventAnaliyServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("服务运行中");
            EventAnaliyServiceEmp handler = new DataProviderService.EventAnaliyServiceEmp();
            TennisDataAnaliy.TennisDataAnaliy.Processor pro = new TennisDataAnaliy.TennisDataAnaliy.Processor(handler);
            Thrift.Transport.TServerSocket ts = new Thrift.Transport.TServerSocket(1899);
            TServer server = new TSimpleServer(pro,ts);
            server.Serve();
        }
    }
}
