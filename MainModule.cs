using System;
using System.Reflection;
using System.Threading;
using Parcs;
using System.IO;
using System.Linq;

namespace FirstModule
{
    class MainModule : IModule
    {
        public static void Main(string[] args)
        {

            var job = new Job();
            if (!job.AddFile(Assembly.GetExecutingAssembly().Location/*"MyFirstModule.exe"*/))
            {
                Console.WriteLine("File doesn't exist");
                return;
            }

            (new MainModule()).Run(new ModuleInfo(job, null));
            Console.ReadKey();
        }

        public void Run(ModuleInfo info, CancellationToken token = default(CancellationToken))
        {
            string inpt = File.ReadAllText("input.txt");

            bool pack = inpt.StartsWith("1");
            string text = inpt.Substring(1);

            const int pointsNum = 2;
            var points = new IPoint[pointsNum];
            var channels = new IChannel[pointsNum];
            for (int i = 0; i < pointsNum; ++i)
            {
                points[i] = info.CreatePoint();
                channels[i] = points[i].CreateChannel();
                points[i].ExecuteClass("FirstModule.LogicModule");
            }

            //minimal brain and splitter for pointsNum
            int clusterL = text.Length / pointsNum;
            /*string[] inputText=new string[pointsNum];
            for (int i = 0; i < pointsNum-1; i++)
                inputText[i] = inpt.Substring(i* clusterL, (i+1) * clusterL);
            inputText[pointsNum - 1] = inpt.Substring(pointsNum * clusterL);
            */
            for (int i = 0; i < pointsNum; ++i)
            {
                channels[i].WriteData(pack);
                channels[i].WriteData(inpt.Substring(i * clusterL, (i + 1) * clusterL));
            }

            DateTime time = DateTime.Now;
            Console.WriteLine("Waiting for result...");

            string[] res = new string[pointsNum] { "", "" };
            for (int i = pointsNum - 1; i >= 0; --i)
            {
                res[i] = channels[i].ReadString();
            }

            File.WriteAllText("output.txt", string.Join("", res));

            Console.WriteLine("Result found in output.txt file, time = {0}", Math.Round((DateTime.Now - time).TotalSeconds, 3));

        }
    }
}
