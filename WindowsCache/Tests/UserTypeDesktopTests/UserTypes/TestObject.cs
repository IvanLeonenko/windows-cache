using System;
using System.Collections.Generic;

namespace UserTypeDesktopTests.UserTypes
{
    public class TestObject
    {
        public List<String> Strings { get; set; }
        public DateTime PropertyDateTime { get; set; }

        public void Init(int size = 100)
        {
            Strings = new List<string>();

            for (var i = 0; i < size; i++)
            {
                Strings.Add(Guid.NewGuid().ToString());
            }
            PropertyDateTime = DateTime.Now;
        }

    }
}
