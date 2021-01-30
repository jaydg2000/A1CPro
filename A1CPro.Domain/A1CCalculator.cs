using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace A1CPro.Domain
{
    public class A1CCalculator
    {
        public double Estimate(IList<DiaryEntry> readings)
        {
            var averageBGL = (int)readings.Average(r => r.Sugar);
            var estimatedA1C = (averageBGL + 77.3) / 35.6;

            return estimatedA1C;
        }
    }
}