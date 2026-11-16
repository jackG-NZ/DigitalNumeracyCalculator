namespace TestMethods
{
    [TestClass]
    public sealed class TestNumberTheory
    {
        [TestMethod]
        public void TestNumPrime()
        {
            bool expectedResult = true;

            bool actualResult = DigitalNumeracyCalculator.Program.NumPrime("numprime 7");

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestNumCheckDigit()
        {
            string expectedResult = "EAN-13";

            string actualResult = DigitalNumeracyCalculator.Program.NumCheckDigit("numcheckdigit 400638133362 7");

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestEvaluateCheckDigit()
        {
            int expectedResult = 2;

            int actualResult = DigitalNumeracyCalculator.Program.EvaluateCheckDigit("numcheckdigit 890155202338 2", "EAN-13", 12);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestNumRand()
        {
            /*I didn't really know how I should test this method so I decided to just check if the method generates the correct number of unique tokens
             * for a given input, probably not very robust so I should so something else if I have time and/or if I remember*/

            int expectedResult = 8;

            int actualResult = DigitalNumeracyCalculator.Program.NumRand("numrand 3 1 7 16");

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
