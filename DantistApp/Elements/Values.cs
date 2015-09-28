using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DantistApp.Elements
{
    public static class Values
    {
        public static Dictionary<string, int> RootInfSealYShift = new Dictionary<string, int>
        {
            {"18", 16 },
            {"17", 19 },
            {"16", 22 },
            {"15", 25 },
            {"14", 16 },
            {"13", 28 },
            {"12", 26 },
            {"11", 27 },
            {"21", 27 },
            {"22", 26 },
            {"23", 28 },
            {"24", 16 },
            {"25", 25 },
            {"26", 22 },
            {"27", 19 },
            {"28", 16 },

            {"48", 1 },
            {"47", 2 },
            {"46", 2 },
            {"45", 2 },
            {"44", 5 },
            {"43", 5 },
            {"42", 4 },
            {"41", 3 },
            {"31", 3 },
            {"32", 4 },
            {"33", 5 },
            {"34", 5 },
            {"35", 2 },
            {"36", 2 },
            {"37", 2 },
            {"38", 1 }
        };

        public static Dictionary<string, int> RootSealYShift = new Dictionary<string, int>
        {
            {"18", -1 },
            {"17", -1 },
            {"16", 0 },
            {"15", 0 },
            {"14", -2 },
            {"13", 1 },
            {"12", 2 },
            {"11", 3 },
            {"21", 3 },
            {"22", 2 },
            {"23", 1 },
            {"24", -2 },
            {"25", 0 },
            {"26", 0 },
            {"27", -1 },
            {"28", -1 },

            {"48", 1 },
            {"47", 2 },
            {"46", 2 },
            {"45", 2 },
            {"44", 5 },
            {"43", 5 },
            {"42", 4 },
            {"41", 3 },
            {"31", 3 },
            {"32", 4 },
            {"33", 5 },
            {"34", 5 },
            {"35", 2 },
            {"36", 2 },
            {"37", 2 },
            {"38", 1 }
        };

        public static Dictionary<string, double[]> CrownPosition = new Dictionary<string, double[]>
        {
            {"18", new double[] {85.5, 95.69} },
            {"17", new double[] {111, 99.44} },
            {"16", new double[] {144, 102.7} },
            {"15", new double[] {176, 105.87} },
            {"14", new double[] {199, 106.23} },
            {"13", new double[] {224, 105.5} },
            {"12", new double[] {253.5, 107.27} },
            {"11", new double[] {274, 109.31} },
            {"21", new double[] {305, 109.31} },
            {"22", new double[] {333, 108} },
            {"23", new double[] {359, 105.5} },
            {"24", new double[] {386.5, 106.23} },
            {"25", new double[] {410.5, 107.26} },
            {"26", new double[] {435, 104.7} },
            {"27", new double[] {468, 99.44} },
            {"28", new double[] {501.5, 94.69} },

            {"48", new double[] {81, 210} },
            {"47", new double[] {107.5, 215.11} },
            {"46", new double[] {142.18, 220.16} },
            {"45", new double[] {176, 223.11} },
            {"44", new double[] {205.2, 224.88} },
            {"43", new double[] {231.55, 218.72} },
            {"42", new double[] {259.65, 229.52} },
            {"41", new double[] {283, 222.24} },
            {"31", new double[] {305, 221.43} },
            {"32", new double[] {329.65, 229.52} },
            {"33", new double[] {351.55, 218.72} },
            {"34", new double[] {378, 224.41} },
            {"35", new double[] {405, 223.11} },
            {"36", new double[] {434, 220.84} },
            {"37", new double[] {468.53, 215.04} },
            {"38", new double[] {501, 210} }
        };
    }
}
