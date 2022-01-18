using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrices
{
    [Serializable]
    public class TimeItem
    {
        public int MatrixSize { get; set; }
        public int Repeats { get; set; }
        public double CSTime { get; set; }
        public double CPPTime { get; set; }
        public double k { get { return CSTime / CPPTime; } }

        public TimeItem(int n, int repeats, double csTime, double cppTime)
        {
            this.MatrixSize = n;
            this.Repeats = repeats;
            this.CSTime = csTime;
            this.CPPTime = cppTime;
        }


        public override string ToString()
        {
            return new StringBuilder()
                .Append(MatrixSize).Append('\t')
                .Append(Repeats).Append('\t')
                .Append(CSTime).Append('\t')
                .Append(CPPTime).Append('\t')
                .Append(k).Append('\t')
                .ToString();
        }
    }
}
