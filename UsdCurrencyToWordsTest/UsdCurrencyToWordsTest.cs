using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using UsdCurrencyToWords;

namespace UsdCurrencyToWordsTest
{
    [TestClass]
    public class UsdCurrencyToWordsTest
    {
        [TestMethod] // 1
        public void CheckIfUserInputIsEmptyString()
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

        [TestMethod] // 2
        public void CheckIfUserInputIsNotAValidNumber()
        {
            CurrencyToWords changeCurrencyToWords = new CurrencyToWords();

            try
            {
                changeCurrencyToWords.ChangeCurrencyToWords("123.4S/*-*/-+@#!#!#@#!##_*&**");
            }
            catch (System.FormatException e)
            {
                StringAssert.Contains(e.Message, "Input only accept number and following by 2 digits decimal number (e.g 123.45)");
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [TestMethod] // 3
        public void CheckIfInputNumberIsInteger()
        {
            CurrencyToWords changeCurrencyToWords = new CurrencyToWords();

            string inputNumber = "123";

            Dictionary<string, double> expected = new Dictionary<string, double> { { "number", 123 }, { "decimalNumber", 0 } };

            Dictionary<string, double> actual = changeCurrencyToWords.ConvertInputNumber(inputNumber);

            foreach (KeyValuePair<string, double> kvp in actual)
            {
                Assert.AreEqual(expected.ContainsKey(kvp.Key), actual.ContainsKey(kvp.Key));
                Assert.AreEqual(expected.ContainsValue(kvp.Value), actual.ContainsValue(kvp.Value));
            }
        }

        [TestMethod] // 4
        public void CheckIfInputNumberIsDecimal()
        {
            CurrencyToWords changeCurrencyToWords = new CurrencyToWords();

            string inputNumber = "123.45";

            Dictionary<string, double> expected = new Dictionary<string, double> { { "number", 123 }, { "decimalNumber", 45 } };

            Dictionary<string, double> actual = changeCurrencyToWords.ConvertInputNumber(inputNumber);

            foreach (KeyValuePair<string, double> kvp in actual)
            {
                Assert.AreEqual(expected.ContainsKey(kvp.Key), actual.ContainsKey(kvp.Key));
                Assert.AreEqual(expected.ContainsValue(kvp.Value), actual.ContainsValue(kvp.Value));
            }
        }

        [TestMethod] // 5
        public void CheckIfNumberIsDecimalAndGetTheWords()
        {
            CurrencyToWords changeCurrencyToWords = new CurrencyToWords();

            double inputSingleDigit = 1;
            double inputDoubleDigit = 20;

            string actualSingleDigitWords = changeCurrencyToWords.GetDecimalWords(inputSingleDigit);
            string actualDoubleDigitWords = changeCurrencyToWords.GetDecimalWords(inputDoubleDigit);

            string expectedSingleDigitWords = "and one";
            string expectedDoubleDigitWords = "and twenty";

            Assert.AreEqual(expectedSingleDigitWords, actualSingleDigitWords);
            Assert.AreEqual(expectedSingleDigitWords.Contains("cent"), actualSingleDigitWords.Contains("cent"));
            Assert.AreEqual(expectedDoubleDigitWords, actualDoubleDigitWords);
            Assert.AreEqual(expectedDoubleDigitWords.Contains("cents"), actualDoubleDigitWords.Contains("cents"));
        }

        [TestMethod] // 6
        public void GetWordsWhenInputIsValid()
        {
            CurrencyToWords usd = new CurrencyToWords();
            string number = "123.45";

            string actual = usd.ChangeCurrencyToWords(number);
            string expected = "one hundred and twenty-three dollars and fourty-five cents";

            Assert.AreEqual(expected, actual.ToLower().Trim());
        }

        [TestMethod] // 7
        public void GetCentsWordsIfDecimalValid()
        {
            CurrencyToWords usd = new CurrencyToWords();
            string number = "8.1";

            string actual = usd.ChangeCurrencyToWords(number);
            string expected = "eight dollars and one cent";

            Assert.AreEqual(expected, actual.ToLower().Trim());
        }
    }
}