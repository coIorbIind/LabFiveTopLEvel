using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpMatrix
{
    [Serializable]
    class TimeItem
    {
        public int Rank { get; set; }
        public int Count { get; set; }
        public double Cpp_time { get; set; }
        public double Cs_time { get; set; }
        public double Ratio_cs_to_cpp
        {
            get
            {
                if (Cpp_time == 0)
                    return -1;
                else
                    return Cs_time / Cpp_time;
            }
        }

        public TimeItem(int rank, int count, double cpp_time, double cs_time)
        {
            Rank = rank;
            Count = count;
            Cpp_time = cpp_time;
            Cs_time = cs_time;
        }

        public override string ToString()
        {
            string result = $"Решение матрицы ранга {Rank} {Count} раз заняло {Cpp_time} у программы C++ и {Cs_time} у программы C#.\n" +
                $"Отношение времени C# к времени C++ {Ratio_cs_to_cpp}";
            

            return result;
        } 
    }
}
