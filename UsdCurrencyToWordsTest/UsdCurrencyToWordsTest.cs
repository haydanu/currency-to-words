using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using UsdCurrencyToWords;
using System;
using System.Numerics;

namespace UsdCurrencyToWordsTest
{
    [TestClass]
    public class UsdCurrencyToWordsTest
    {
        [TestMethod] // 1
        public void Check_If_User_Input_Is_Empty_String()
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
        public void Check_If_User_Input_Is_Not_A_Valid_Number_Or_Uncommon_Input()
        {
            CurrencyToWords changeCurrencyToWords = new CurrencyToWords();

            try
            {
                changeCurrencyToWords.ChangeCurrencyToWords("sh52425@#!123.4S/*-*/-+@#!#!#@#!##_*&**");
            }
            catch (System.FormatException e)
            {
                StringAssert.Contains(e.Message, "Input only accept number and following by 2 digits decimal number (e.g 123.45)");
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [TestMethod] // 3
        public void Check_If_Input_Number_Is_Integer()
        {
            CurrencyToWords changeCurrencyToWords = new CurrencyToWords();

            string inputNumber = "123";

            Dictionary<string, BigInteger> expected = new Dictionary<string, BigInteger> { { "number", 123 }, { "decimalNumber", 0 } };

            Dictionary<string, BigInteger> actual = changeCurrencyToWords.ConvertInputNumber(inputNumber);

            foreach (KeyValuePair<string, BigInteger> kvp in actual)
            {
                Assert.AreEqual(expected.ContainsKey(kvp.Key), actual.ContainsKey(kvp.Key));
                Assert.AreEqual(expected.ContainsValue(kvp.Value), actual.ContainsValue(kvp.Value));
            }
        }

        [TestMethod] // 4
        public void Check_If_Input_Number_Is_Decimal()
        {
            CurrencyToWords changeCurrencyToWords = new CurrencyToWords();

            string inputNumber = "123.45";

            Dictionary<string, BigInteger> expected = new Dictionary<string, BigInteger> { { "number", 123 }, { "decimalNumber", 45 } };

            Dictionary<string, BigInteger> actual = changeCurrencyToWords.ConvertInputNumber(inputNumber);

            foreach (KeyValuePair<string, BigInteger> kvp in actual)
            {
                Assert.AreEqual(expected.ContainsKey(kvp.Key), actual.ContainsKey(kvp.Key));
                Assert.AreEqual(expected.ContainsValue(kvp.Value), actual.ContainsValue(kvp.Value));
            }
        }

        [TestMethod] // 5
        public void Check_If_Decimal_Digits_More_Than_Two()
        {
            CurrencyToWords changeCurrencyToWords = new CurrencyToWords();

            string inputNumber = "123.4512314556422342";

            try
            {
                changeCurrencyToWords.ChangeCurrencyToWords(inputNumber);
            }
            catch (System.OverflowException e)
            {
                StringAssert.Contains(e.Message, "decimal digit is out of range, only accept maximum 2 number (e.g 123.45)");
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [TestMethod] // 6
        public void Check_If_Number_Is_Decimal_And_Get_The_Words()
        {
            CurrencyToWords changeCurrencyToWords = new CurrencyToWords();

            BigInteger inputSingleDigit = 1;
            BigInteger inputDoubleDigit = 20;

            string actualSingleDigitWords = changeCurrencyToWords.GetDecimalWords(inputSingleDigit);
            string actualDoubleDigitWords = changeCurrencyToWords.GetDecimalWords(inputDoubleDigit);

            string expectedSingleDigitWords = "one";
            string expectedDoubleDigitWords = "twenty";

            Assert.AreEqual(expectedSingleDigitWords, actualSingleDigitWords);
            Assert.AreEqual(expectedDoubleDigitWords, actualDoubleDigitWords);
        }

        [TestMethod] // 7
        public void Get_Words_When_Input_Is_Valid()
        {
            CurrencyToWords usd = new CurrencyToWords();
            List<string> number = new List<string>() { "123.45", "8.1", "8", "24", "100", "1455", "1234567898765432123456789" };
            List<string> actual = new List<string>();


            number.ForEach(delegate (String inputNumber)
            {
                actual.Add(usd.ChangeCurrencyToWords(inputNumber));
            });

            List<string> expected = new List<string>() {
                "ONE HUNDRED AND TWENTY-THREE DOLLARS AND FOURTY-FIVE CENTS",
                "EIGHT DOLLARS AND ONE CENT",
                "EIGHT DOLLARS",
                "TWENTY-FOUR DOLLARS",
                "ONE HUNDRED DOLLARS",
                "ONE THOUSAND AND FOUR HUNDRED AND FIFTY-FIVE DOLLARS",
                "ONE SEPTILLION AND TWO HUNDRED AND THIRTY-FOUR SEXTILLION AND FIVE HUNDRED AND SIXTY-SEVEN QUINTILLION AND EIGHT HUNDRED AND NINETY-EIGHT QUADRILLION AND SEVEN HUNDRED AND SIXTY-FIVE TRILLION AND FOUR HUNDRED AND THIRTY-TWO BILLION AND ONE HUNDRED AND TWENTY-THREE MILLION AND FOUR HUNDRED AND FIFTY-SIX THOUSAND AND SEVEN HUNDRED AND EIGHTY-NINE DOLLARS" };

            for (int i = 0; i < actual.Count; i++)
            {
                Assert.AreEqual(expected[i].ToLower().Trim(), actual[i]);
            }
        }

        [TestMethod] // 8
        public void Get_Cent_Word_If_Decimal_Is_Valid()
        {
            CurrencyToWords usd = new CurrencyToWords();
            string number = "8.1";

            string actual = usd.ChangeCurrencyToWords(number);
            string expected = "eight dollars and one cent";

            Assert.AreEqual(expected.Contains("cent"), actual.ToLower().Trim().Contains("cent"));
        }
    }
}