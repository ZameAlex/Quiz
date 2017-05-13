using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Models;
using TestProject.Repositories;

namespace TestProject
{


    class Program
    {
        static void Write(object x)
        {
            var y = x.GetType().GetProperties();
            foreach (System.Reflection.PropertyInfo item in y)
            {
                Console.WriteLine(item.GetValue(x));
            }
        }
        static void Main(string[] args)
        {
            int i = 1;
            Console.WriteLine("i={0}",i++);
            
            Console.ReadKey();
        }
    }
}
