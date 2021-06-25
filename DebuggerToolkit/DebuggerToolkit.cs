using System;
using System.IO;
using Neo.VM;
using System.Linq;
using System.Numerics;
using System.Text;
using Neo.SmartContract;


namespace DebuggerToolkit
{
    class DebuggerToolkit
    {
        public static void Run(Script script)
        {
            using ExecutionEngine engine = new();
            engine.LoadScript(script);
            var result = engine.Execute();
            Console.WriteLine("VMSTATE:" + result);
            Console.WriteLine("--------------");
            Console.WriteLine("ResultStackCount:" + engine.ResultStack.Count);
            while (engine.ResultStack.Count > 0)
            {
                try
                {
                    var item = engine.ResultStack.Pop();
                    var itemS = JsonSerializer.Serialize(item);
                    Console.WriteLine(itemS);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            Console.WriteLine("InvocationStackCount:" + engine.InvocationStack.Count);
            Console.WriteLine("--------------");
            while (engine.InvocationStack.Count > 0)
            {
                try
                {
                    var item = engine.InvocationStack.Pop();
                    while(item.EvaluationStack.Count>0)
                    {
                        try
                        {
                            var itemEva = item.EvaluationStack.Pop();
                            var itemEvaS = JsonSerializer.Serialize(itemEva);
                            Console.WriteLine(itemEvaS);

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        static void Main(string[] args)
        {

            Console.WriteLine("DebugToolkit Started");
            FileStream fs = new FileStream("", FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            MemoryStream ms = new();
            BinaryWriter bw = new BinaryWriter(ms);
            for(int i = 0; i < fs.Length; i++)
            {
                bw.Write(br.ReadByte());
            }
            Run(ms.ToArray());
            Console.WriteLine("DebugToolkit ended");
            br.Close();
            fs.Close();
            bw.Dispose();
        }
    }
}
