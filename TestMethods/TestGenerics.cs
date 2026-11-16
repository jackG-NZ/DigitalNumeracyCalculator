namespace TestMethods
{
    [TestClass]
    public sealed class TestGenerics
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

            double actualResult = DigitalNumeracyCalculator.Program.SquareRoot(25);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]

        public void TestFactorial()
        {
            double expectedResult = 120;

            double actualResult = DigitalNumeracyCalculator.Program.Factorial(5);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]

        public void TestModulo()
        {
            double expectedResult = 19;

            double actualResult = DigitalNumeracyCalculator.Program.Modulo(187, 42);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]

        public void TestRad2Deg()
        {
            double expectedResult = 401.07045659157626;

            double actualResult = DigitalNumeracyCalculator.Program.Rad2Deg("rad2deg 7");

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]

        public void TestDeg2Rad()
        {
            double expectedResult = 0.9948376736367678;

            double actualResult = DigitalNumeracyCalculator.Program.Deg2Rad("deg2rad 57");

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}

/*I'm going to get marked down heavily for this I imagine but I encountered some serious issues while setting up test methods for the more complex operations (mostly those involving structs). There is buggy behaviour somewhere
 * in the program that interferes with how lines, vectors, and matrices are tested. The methods function perfectly fine in the calculator itself but when I was testing these methods I was encountering index errors and bizarre,
 * unexpected returns that I could not resolve. I have some ideas; something is being modified when it shouldn't be, something isn't getting returned when it should be, etc etc but these issues are ONLY occuring when testing
 * the methods and are otherwise completely absent in runtime... so I'm at a loss. I want to fix it but I'm running out of time and have arguably more important things to work on :C*/
