namespace TestMethods
{
    [TestClass]
    public sealed class TestCryptography
    {
        [TestMethod]
        public void TestCaesarEn()
        {
            string expectedResult = "hqfubsw";

            string? actualResult = DigitalNumeracyCalculator.Program.CaesarInput("""caesaren "encrypt" """, true);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestCaesarDe()
        {
            string expectedResult = "encrypt";

            string? actualResult = DigitalNumeracyCalculator.Program.CaesarInput("""caesaren "hqfubsw" """, false);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestAffineEn()
        {
            string expectedResult = "cvspyfz";

            string? actualResult = DigitalNumeracyCalculator.Program.AffineInput("""affineen 5 8 "encrypt" """, true);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestAffineDe()
        {
            string expectedResult = "encrypt";

            string? actualResult = DigitalNumeracyCalculator.Program.AffineInput("""affinede 5 8 "cvspyfz" """, false);

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}

/*I don't really know how I would go about testing the brute-force affine method considering it returns 312 unique decryptions*/
