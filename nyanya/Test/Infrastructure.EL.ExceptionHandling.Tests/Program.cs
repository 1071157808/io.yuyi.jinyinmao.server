using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExceptionHandler;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using ExceptionHandlingExample;

namespace TestExceptionHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            ExceptionManager exManager = ExceptionHandler.ExceptionHandler.GetExMgr();
            ExceptionPolicy.SetExceptionManager(exManager, false);
            try
            {
                try
                {
                    SalaryCalculator calc = new SalaryCalculator();
                    decimal w = calc.GetWeeklySalary("123", 0);
                }
                catch (Exception ex)
                {
                    Exception exceptionToThrow;
                    ExceptionPolicy.HandleException(ex, "LogAndReplace", out exceptionToThrow);
                    throw exceptionToThrow;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Message:{0}, StackTrace:{1}", ex.Message, ex.StackTrace);
                if (ex.InnerException == null)
                {
                    Console.WriteLine("No inner exception");
                }
                else
                {
                    Console.WriteLine("Inner exception message:{0}, stackTrace:{1}", ex.InnerException.Message, ex.InnerException.StackTrace);
                }
            }
            Console.ReadKey();
        }
    }
}
