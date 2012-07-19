using NUnit.Framework;
using Unprotected.Net;

namespace Unprotected.Net_Tests
{
    [TestFixture]
    public class PrivatePartsExposerTests
    {
        public class TestClass
        {
            public const string TestingString = "abc123";
            public const int TestingInt = 12345;

            private string _privateStringField;
            private int _privateIntField;

            public TestClass()
            {
                PublicStringProperty = TestingString;
                PrivateStringProperty = TestingString;
                PrivateIntProperty = TestingInt;
                _privateStringField = TestingString;
                _privateIntField = TestingInt;
            }

            #region Auto Properties
            public string PublicStringProperty { get; set; }
            private string PrivateStringProperty { get; set; }
            private int PrivateIntProperty { get; set; }
            #endregion

            #region Constant Getters
            private string PrivateStringGetter
            {
                get { return TestingString; }
            }

            private int PrivateIntGetter
            {
                get { return TestingInt; }
            }
            #endregion

            #region Accessors
            public string PrivateStringPropertyAccessor
            {
                get { return PrivateStringProperty; }
            }

            public string PrivateStringFieldAccessor
            {
                get { return _privateStringField; }
            }

            public int PrivateIntPropertyAccessor
            {
                get { return PrivateIntProperty; }
            }

            public int PrivateIntFieldAccessor
            {
                get { return _privateIntField; }
            }
            #endregion

            #region Methods
            private string PrivateStringMethod()
            {
                return TestingString;
            }
            private int PrivateIntMethod()
            {
                return TestingInt;
            }
            #endregion
        }

        protected TestClass TestingObject { get; set; }
        protected dynamic Exposer { get; set; }

        [SetUp]
        public void SetUp()
        {
            TestingObject = new TestClass();
            Exposer = TestingObject.ExposePrivateParts();
        }


        [Test]
        public void ExposerReadsPrivateStringField()
        {
            var stringField = Exposer._privateStringField;

            Assert.That(stringField, Is.EqualTo(TestClass.TestingString));
        }


        [Test]
        public void ExposerReadsPrivateIntField()
        {
            var intField = Exposer._privateIntField;

            Assert.That(intField, Is.EqualTo(TestClass.TestingInt));
        }

        [Test]
        public void ExposerReadsPrivateStringProperty()
        {
            var stringProperty = Exposer.PrivateStringProperty;

            Assert.That(stringProperty, Is.EqualTo(TestClass.TestingString));
        }

        [Test]
        public void ExposerReadsPrivateIntProperty()
        {
            var intProperty = Exposer.PrivateIntProperty;

            Assert.That(intProperty, Is.EqualTo(TestClass.TestingInt));
        }

        [Test]
        public void ExposerReadsPrivateStringMethod()
        {
            var stringMethod = Exposer.PrivateStringMethod();

            Assert.That(stringMethod, Is.EqualTo(TestClass.TestingString));
        }

        [Test]
        public void ExposerReadsPrivateIntMethod()
        {
            var intProperty = Exposer.PrivateIntMethod();

            Assert.That(intProperty, Is.EqualTo(TestClass.TestingInt));
        }
    }
}