using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
namespace Matrices
{

    [Serializable]
    public class TimeList
    {
        
        private List<TimeItem> list = new ();

        
        public void Add(TimeItem ob)
        {
            list.Add(ob);
        }

       
        public void Load(string filename)
        {
            FileStream f = new (filename, FileMode.Open);
            try
            {
                BinaryFormatter bf = new();
                if (f.Length != 0)
                    list = (List<TimeItem>)bf.Deserialize(f);
            }
            catch (Exception e)
            {
                Console.WriteLine("Cant load file");
                Console.WriteLine(e);
            }
            finally
            {
                f.Close();
            }
        }

       
        public void Save(string filename)
        {
            try
            {
                BinaryFormatter bf = new ();
                FileStream f = new (filename, FileMode.Open);
                bf.Serialize(f, list);
                f.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Cant load file");
                Console.WriteLine(e);
            }
        }

       
        public override string ToString()
        {
            StringBuilder result = new("Indexes\tRepeats\tC# time\tC++ time\tReciprocal\n");
            int ind = 1;
            foreach (TimeItem timeItem in list)
            {
                result.Append(ind).Append(':').Append(timeItem).Append('\n');
                ind++;
            }
            return result.ToString();
        }
    }

}
