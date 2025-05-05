using System.Collections.Concurrent;
using System.Xml.Serialization;
using MultithreadingModels;

namespace MultithreadingServices;

public static class ParallelMergerService
{
    public static void MergeAlternating<T>(string filePath1, string filePath2, string outputPath)
    {
        var queue1 = new Queue<T>();
        var queue2 = new Queue<T>();

        var lockObj = new object();
        bool done1 = false;
        bool done2 = false;

        void ReadAndEnqueue(string path, Queue<T> queue, ref bool doneFlag)
        {
            using var stream = new FileStream(path, FileMode.Open);
            var serializer = new XmlSerializer(typeof(List<T>));
            var list = (List<T>)serializer.Deserialize(stream);

            foreach (var item in list)
            {
                lock (lockObj)
                {
                    queue.Enqueue(item);
                    Monitor.PulseAll(lockObj);
                }

                Thread.Sleep(10);
            }

            lock (lockObj)
            {
                doneFlag = true;
                Monitor.PulseAll(lockObj);
            }
        }

        var thread1 = new Thread(() => ReadAndEnqueue(filePath1, queue1, ref done1));
        var thread2 = new Thread(() => ReadAndEnqueue(filePath2, queue2, ref done2));

        var writerThread = new Thread(() =>
        {
            var result = new List<T>();

            while (true)
            {
                T? item1 = default;
                T? item2 = default;
                bool gotAny = false;

                lock (lockObj)
                {
                    while (queue1.Count == 0 && !done1 || queue2.Count == 0 && !done2)
                    {
                        Monitor.Wait(lockObj);
                    }

                    if (queue1.Count > 0)
                    {
                        item1 = queue1.Dequeue();
                        gotAny = true;
                        result.Add(item1);
                    }

                    if (queue2.Count > 0)
                    {
                        item2 = queue2.Dequeue();
                        gotAny = true;
                        result.Add(item2);
                    }

                    if (!gotAny && done1 && done2)
                    {
                        break;
                    }
                }
            }

            XmlSerializerService.SaveToXml(result, outputPath);
        });

        thread1.Start();
        thread2.Start();
        writerThread.Start();

        thread1.Join();
        thread2.Join();
        writerThread.Join();
    }
}
