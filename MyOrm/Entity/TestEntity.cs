using System;
using System.Collections.Generic;
using System.Text;

namespace MyOrm.Entity
{
    [Serializable]
    public class TestEntity
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime BirthDay { get; set; }
    }
}
