namespace TestMethods
{
    [TestClass]
    public sealed class TestBinary
    {
        [TestMethod]
        public void TestUnsignedBinary()
        {
            double expectedResult = 30;

            double actualResult = DigitalNumeracyCalculator.Program.UnsignedBinary("ubinary 1111 + 1111");

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
