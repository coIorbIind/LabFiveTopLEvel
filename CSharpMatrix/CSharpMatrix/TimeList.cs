using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpMatrix
{
    
    class TimeList
    {
        private List<TimeItem> timeItems = new List<TimeItem>();
        public void Add(TimeItem item)
        {
            timeItems.Add(item);
        }

        public bool Save(string fileName)
        {
            Stream SaveFileStream = null;
            try
            {
                SaveFileStream = File.Create(fileName);
                BinaryFormatter serializer = new BinaryFormatter();
                serializer.Serialize(SaveFileStream, timeItems);
                SaveFileStream.Close();
                return true;
            }
            catch
            {
                Console.WriteLine("Возникли проблемы при записи объекта");
                SaveFileStream?.Close();
                return false;
            }
            finally
            {
                SaveFileStream?.Close();
            }
            //using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Create)))
            //{
            //    writer.Write(1.250F);
            //    writer.Write(@"c:\Temp");
            //    writer.Write(10);
            //    writer.Write(true);
            //}
        }//запись объекта в файл

        public bool Load(string filename)
        {
            if (File.Exists(filename))
            {
                Stream openFileStream = null;
                try
                {
                    openFileStream = File.OpenRead(filename);
                    BinaryFormatter deserializer = new BinaryFormatter();
                    timeItems = (List<TimeItem>)deserializer.Deserialize(openFileStream);
                    return true;
                }
                catch
                {
                    openFileStream?.Close();
                    return false;
                }
                finally
                {
                    openFileStream?.Close();
                }
            }
            return false;
        } //чтение объекта из файла

        public override string ToString()
        {
            string text = String.Empty;
            foreach (TimeItem item in timeItems)
            {
                text += item.ToString();
            }
            return text;
        }
    }
}