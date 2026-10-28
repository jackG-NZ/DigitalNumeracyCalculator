namespace TestMethods
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void TestConversionToPostfix()
        {
            string expectedResult = "2 -9 * 4 5 + / 5 +";

            string actualResult = DigitalNumeracyCalculator.Program.ConvertToPostfix("( 2 * -9 ) / ( 4 + 5 ) + 5");

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestPostfixEvaluation()
        {
            double expectedResult = 3;

            double actualResult = DigitalNumeracyCalculator.Program.EvaluatePostfix("2 -9 * 4 5 + / 5 +");

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]

        public void TestSqrt()
        {
            double expectedResult = 5;

            double actualResult = DigitalNumeracyCalculator.Program.Interpreter("!sqrt 25");

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
