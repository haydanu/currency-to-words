using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using UsdCurrencyToWords;

namespace UsdCurrencyToWordsTest
{
    [TestClass]
    public class UsdCurrencyToWordsTest
    {
        [TestMethod]
        public void CheckIfInputNumberIsIntegers()
        {
            CurrencyToWords changeCurrencyToWords = new CurrencyToWords();

            string inputNumber = "123";

            Dictionary<string, int> expected = new Dictionary<string, int> { { "number", 123 }, { "decimalNumber", 0 } };

            Dictionary<string, int> actual = changeCurrencyToWords.CheckNumber(inputNumber);

            foreach (KeyValuePair<string, int> kvp in actual)
            {
                Assert.AreEqual(expected.ContainsKey(kvp.Key), actual.ContainsKey(kvp.Key));
                Assert.AreEqual(expected.ContainsValue(kvp.Value), actual.ContainsValue(kvp.Value));
            }
        }

        [TestMethod]
        public void CheckIfInputNumberIsDecimals()
        {
            CurrencyToWords changeCurrencyToWords = new CurrencyToWords();

            string inputNumber = "123.45";

            Dictionary<string, int> expected = new Dictionary<string, int> { { "number", 123 }, { "decimalNumber", 45 } };

            Dictionary<string, int> actual = changeCurrencyToWords.CheckNumber(inputNumber);

            foreach (KeyValuePair<string, int> kvp in actual)
            {
                Assert.AreEqual(expected.ContainsKey(kvp.Key), actual.ContainsKey(kvp.Key));
                Assert.AreEqual(expected.ContainsValue(kvp.Value), actual.ContainsValue(kvp.Value));
            }
        }

        [TestMethod]
        public void CheckIfNumberIsDecimalAndGetTheWords()
        {
            CurrencyToWords changeCurrencyToWords = new CurrencyToWords();

            int inputSingleDigit = 1;
            int inputDoubleDigit = 20;

            string actualSingleDigitWords = changeCurrencyToWords.GetDecimalWords(inputSingleDigit);
            string actualDoubleDigitWords = changeCurrencyToWords.GetDecimalWords(inputDoubleDigit);

            string expectedSingleDigitWords = "and one cent";
            string expectedDoubleDigitWords = "and twenty- cents";

            Assert.AreEqual(expectedSingleDigitWords, actualSingleDigitWords);
            Assert.AreEqual(expectedSingleDigitWords.Contains("cent"), actualSingleDigitWords.Contains("cent"));
            Assert.AreEqual(expectedDoubleDigitWords, actualDoubleDigitWords);
            Assert.AreEqual(expectedDoubleDigitWords.Contains("cents"), actualDoubleDigitWords.Contains("cents"));
        }


        [TestMethod]

        public void CheckIfUserInputIsInvalid()
        {
            CurrencyToWords changeCurrencyToWords = new CurrencyToWords();

            try
            {
                changeCurrencyToWords.ChangeCurrencyToWords("");
            }
            catch (System.FormatException e)
            {
                StringAssert.Contains(e.Message, "Input can't be empty");
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [TestMethod]

        public void CheckIfInputIsDoubleDigitsAndGetWords()
        {
            CurrencyToWords usd = new CurrencyToWords();
            int number = 24;

            string actual = usd.GetTensWords(number);
            string expected = "and twenty-four dollars";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]

        public void GetWordsWhenInputIsValid()
        {
            CurrencyToWords usd = new CurrencyToWords();
            string number = "123.45";

            string actual = usd.ChangeCurrencyToWords(number);
            string expected = "one hundred and twenty-three dollars and fourty-five cents";

            Assert.AreEqual(expected, actual.ToLower().Trim());
        }


    }
}