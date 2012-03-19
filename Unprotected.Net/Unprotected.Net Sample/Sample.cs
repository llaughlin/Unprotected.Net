using System;

namespace Unprotected.Net_Sample
{
    using Net;

    public class Sample
    {
        public static void Main(string[] args)
        {
            var foo = new Foo();
            Console.WriteLine("MyPublicString before alteration: {0}", foo.MyPublicString);
            Console.WriteLine("GetMyPrivateString before alteration: {0}", foo.GetMyPrivateString);
            Console.WriteLine("GetMyPrivateStringField before alteration: {0}", foo.GetMyPrivateStringField);
            Console.WriteLine("GetMyPrivateInt before alteration: {0}", foo.GetMyPrivateInt);
            Console.WriteLine();

            dynamic exposer = new PrivatePartsExposer<Foo>(foo);

            exposer.MyPublicString = "Altered public string";
            exposer.MyPrivateString = "Altered private string";
            exposer.MyPrivateInt = 5555;
            exposer.MyPrivateStringField = "Altered string field";

            Console.WriteLine("After public string alteration: {0}", foo.MyPublicString);
            Console.WriteLine("After private string alteration: {0}", foo.GetMyPrivateString);
            Console.WriteLine("GetMyPrivateStringField before alteration: {0}", foo.GetMyPrivateStringField);
            Console.WriteLine("After private int alteration: {0}", foo.GetMyPrivateInt);
            Console.WriteLine();

            Console.WriteLine("Value of private string getter: {0}", exposer.MyPrivateStringGetter);
            Console.WriteLine("Value of private int getter: {0}", exposer.MyPrivateIntGetter);
            Console.WriteLine();

            Console.WriteLine("Press any key to continue...");
            Console.Read();
        }
    }

    public class Foo
    {
        public string MyPublicString { get; set; }
        private string MyPrivateString { get; set; }
        private string MyPrivateStringField = "Default field string value";

        private int MyPrivateInt { get; set; }

        private string MyPrivateStringGetter
        {
            get { return "Got Private String"; }
        }

        private int MyPrivateIntGetter
        {
            get { return 9999; }
        }

        public string GetMyPrivateString { get { return MyPrivateString; } }
        public string GetMyPrivateStringField { get { return MyPrivateStringField; } }
        public int GetMyPrivateInt { get { return MyPrivateInt; } }

        public Foo()
        {
            MyPublicString = "Default MyString";
            MyPrivateString = "Default MyPrivateString";
            MyPrivateInt = 1000;
        }
    }
}
