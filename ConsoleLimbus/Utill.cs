using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLimbus
{
    static class Utill
    {
        public static void SetConsoleColor(eItemGrade itemGrade)
        {
            switch (itemGrade)
            {
                case eItemGrade.C:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case eItemGrade.B:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case eItemGrade.A:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case eItemGrade.S:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case eItemGrade.SS:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
            }
        }
    }
}
