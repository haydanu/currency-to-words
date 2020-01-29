using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using UsdCurrencyToWords;
using System;

namespace UsdCurrencyToWordsTest
{
    [TestClass]
    public class UsdCurrencyToWordsTest
    {
        [TestMethod]
        public void CheckIfInputNumberIsDecimalOrNot()
        {
            string inputNumber = "123";

            Dictionary<string, int> expected = new Dictionary<string, int> { { "number", 123 }, { "decimalNumber", 0 } };

            CurrencyToWords changeCurrencyToWords = new CurrencyToWords();

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
            int inputSingleDigit = 1;
            int inputDoubleDigit = 20;

            CurrencyToWords changeCurrencyToWords = new CurrencyToWords();

            string actualSingleDigitWords = changeCurrencyToWords.GetDecimalWords(inputSingleDigit);
            string actualDoubleDigitWords = changeCurrencyToWords.GetDecimalWords(inputDoubleDigit);

            string expectedSingleDigitWords = "and one cent";
            string expectedDoubleDigitWords = "and twenty- cents";

            Assert.AreEqual(expectedSingleDigitWords, actualSingleDigitWords);
            Assert.AreEqual(expectedDoubleDigitWords, actualDoubleDigitWords);

        }


        [TestMethod]

        public void CheckIfNumberIsTensAndGetTheWords()
        {

        }

    }
}