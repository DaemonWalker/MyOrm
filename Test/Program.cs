using MyOrm.Attributes;
using MyOrm.DBContext;
using MyOrm.Factories;
using MyOrm.QueryProvider;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var rand = new Random();
            //var list = CreateData();

            ShowTime(() =>
            {
                using (var db = new TestDBContext() { DetectEntityChange = false })
                {
                    var l = db.OrderBy(p => p.AGE).First(p => p.TEST_ID % 2 == 0);
                    Console.WriteLine(l.TEST_ID);
                }
            });
        }
        static void ShowTime(Action action)
        {
            var sw = new Stopwatch();
            Console.WriteLine("Start");
            sw.Start();
            action.Invoke();
            sw.Stop();
            Console.WriteLine($"{sw.ElapsedMilliseconds / 1000}\"{sw.ElapsedMilliseconds % 1000}");
        }
        static List<TEST_TABLE> CreateData()
        {
            var rand = new Random();
            var list = new List<TEST_TABLE>();
            for (int i = 0; i < 26; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    for (int k = 0; k < 26; k++)
                    {
                        for (int l = 0; l < 26; l++)
                        {
                            var entity = new TEST_TABLE();
                            entity.ADDRESS = "新街大院14号楼" + (rand.Next(20) * 100 + rand.Next(20));
                            entity.AGE = rand.Next(100);
                            entity.CREATE_DATE = DateTime.Now.ToString("yyyy-MM-dd");
                            entity.CREATE_OPER = "Admin";
                            entity.NAME = $"{(char)('a' + i)}{(char)('a' + j)}{(char)('a' + k)}{(char)('a' + l)}";
                            entity.SEX = rand.Next(2) % 2 == 0 ? "male" : "female";
                            entity.TEL = "1560401" + rand.Next(10000);
                            entity.VALID_STATE = "1";
                            list.Add(entity);
                        }
                    }
                }
            }
            return list;
        }

    }

    public class TestDBContext : AbsDBContext<TEST_TABLE>
    {

    }
    public class TEST_TABLE
    {
        [PrimaryKey]
        public int TEST_ID { get; set; }

        public string NAME { get; set; }
        public string CREATE_DATE { get; set; }
        public string CREATE_OPER { get; set; }
        public string VALID_STATE { get; set; }
        public string TEL { get; set; }
        public string SEX { get; set; }
        public int AGE { get; set; }
        public string ADDRESS { get; set; }
    }
}
