using Algorithm;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace KDTreeTest
{
    /// <summary>
    /// 座標資料結構
    /// </summary>
    internal class DataStruct
    {
        public bool Color { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public static int ComparerWithX(DataStruct lhs, DataStruct rhs)
        {
            if (lhs.X > rhs.X) return 1;
            if (lhs.X < rhs.X) return -1;
            return 0;
        }

        public static int ComparerWithY(DataStruct lhs, DataStruct rhs)
        {
            if (lhs.Y > rhs.Y) return 1;
            if (lhs.Y < rhs.Y) return -1;
            return 0;
        }

        public static string ConvertToString(DataStruct data)
        {
            return $"({data.X},{data.Y}):{data.Color}";
        }
    }

    /// <summary>
    /// KD Tree 測試
    /// </summary>
    [TestClass]
    public class KDTreeTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            // 產生資料
            Random random = new Random(0);
            List<DataStruct> list = new List<DataStruct>();
            for (int ii = 0; ii < 100; ii++)
            {
                int x = random.Next(-1000, 1000) / 10 * 10;
                int y = random.Next(-1000, 1000) / 10 * 10;
                list.Add(new DataStruct()
                {
                    X = x,
                    Y = y,
                    Color = x > y,
                });
            }

            // 建立 KD Tree
            var tree = new KDTree<DataStruct>(DataStruct.ComparerWithX, DataStruct.ComparerWithY);
            tree.Insert(list);

            // 印出樹
            Console.WriteLine("樹:");
            string treeString = tree.ToString(DataStruct.ConvertToString);
            Console.WriteLine(treeString);

            // 找點
            foreach (var item in list)
            {
                int x = item.X + random.Next(0, 2);
                int y = item.Y + random.Next(0, 2);

                bool isExist = tree.IsExist(new DataStruct() { X = x, Y = y });
                Console.WriteLine("點({0},{1}) {2}", x, y, isExist ? "Exist" : "Not Exist");
            }

            // 找範圍
            for (int ii = 0; ii < 10; ii++)
            {
                int min = ii * 100;
                int max = min + 100;

                bool isExist = tree.IsExist(new DataStruct() { X = min, Y = min }, new DataStruct { X = max, Y = max });
                Console.WriteLine("範圍[{0}~{1}] {2}", min, max, isExist ? "Exist" : "Not Exist");
            }
        }
    }
}
