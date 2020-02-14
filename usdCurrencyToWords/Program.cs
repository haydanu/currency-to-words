using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Numerics;

namespace UsdCurrencyToWords
{
    public class CurrencyToWords
    {
        private readonly string[] units = { "", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
        private readonly string[] tens = { "", "", "twenty", "thirty", "fourty", "fifty", "sixty", "seventy", "eighty", "ninety" };
        private readonly string[] largeNumbers = { "thousand", "million", "billion", "trillion", "quadrillion", "quintillion", "sextillion", "septillion", "octillion", "nonillion", "decillion", "undecillion", "duodecillion", "tredecillion", "quattuordecillion", "quindecillion", "sexdecillion", "septendecillion", "octodecillion", "novemdecillion", "vigintillion" };
        private readonly string cent = "cent";
        private readonly string cents = "cents";
        private readonly string dollar = "dollar";
        private readonly string dollars = "dollars";
        private readonly BigInteger maximumValue = (BigInteger)Math.Pow(10, 63);

        public string ChangeCurrencyToWords(string currency)
        {
            CheckInputNumber(currency);

            Dictionary<string, BigInteger> validNumber = ConvertInputNumber(currency);

            BigInteger _validNumberInt = validNumber["number"];
            BigInteger _validNumberDecimal = validNumber["decimalNumber"];
            string getDollar = _validNumberInt < 2 ? dollar : dollars;
            string getCent = _validNumberDecimal < 2 ? cent : cents;

            string words = GetWords(_validNumberInt) + " " + getDollar;

            if (_validNumberInt == 0)
            {
                words = "";
            }

            if (_validNumberDecimal != 0)
            {
                if (words != "")
                {
                    words += " and " + GetDecimalWords(_validNumberDecimal) + " " + getCent;
                }
                else
                {
                    words = GetDecimalWords(_validNumberDecimal) + " " + getCent;
                }
            }

            Console.WriteLine(words.ToUpper());
            return words;
        }

        public void CheckInputNumber(string currency)
        {
            if (string.IsNullOrEmpty(currency))
            {
                throw new FormatException("Input can't be empty");
            }

            if (currency.StartsWith("-"))
            {
                throw new FormatException("Negative number is not allowed");
            }

            Regex charPattern = new Regex(@"[^0-9,.]", RegexOptions.IgnoreCase);
            MatchCollection matchesChar = charPattern.Matches(currency);

            if (matchesChar.Count >= 1)
            {
                throw new FormatException("Input accepts only a number format followed by 2 digits decimal numbers (e.g. 123.45)");
            }

            
        }

        public Dictionary<string, BigInteger> ConvertInputNumber(string currency)
        {
            Dictionary<string, BigInteger> _currencyObj = new Dictionary<string, BigInteger>();
            Regex decimalPattern = new Regex(@"\b\d+\b");

            try
            {
                if (currency.StartsWith(".") | currency.StartsWith(","))
                {
                    string modifiedCurrency = currency.Replace(".", ",");
                    if (modifiedCurrency.Length > 3)
                    {
                        throw new IndexOutOfRangeException("Decimal digits is out of range, only accept maximum 2 number (e.g. 123.45)");
                    }
                    double roundedCurrency = Math.Round(double.Parse(modifiedCurrency), 2);

                    MatchCollection matches = decimalPattern.Matches(roundedCurrency.ToString("0.00"));

                    if (matches.Count != 1)
                    {
                        _currencyObj["number"] = BigInteger.Parse(matches[0].Value);
                        _currencyObj["decimalNumber"] = BigInteger.Parse(matches[1].Value);
                    }
                }
                else if (currency.Contains(".") | currency.Contains(","))
                {
                    MatchCollection matches = decimalPattern.Matches(currency);
                    
                    if (matches.Count != 1)
                    {
                        _currencyObj["number"] = BigInteger.Parse(matches[0].Value);
                        _currencyObj["decimalNumber"] = BigInteger.Parse(matches[1].Value);
                    } else
                    {
                        _currencyObj["number"] = BigInteger.Parse(matches[0].Value);
                        _currencyObj["decimalNumber"] = 0;
                    }
                }
                else
                {
                    _currencyObj["number"] = BigInteger.Parse(currency);
                    _currencyObj["decimalNumber"] = 0;
                }
            }
            catch (OverflowException)
            {
                throw new OverflowException("Sorry, exceeds maximum input");
            }

            if(_currencyObj["number"] > maximumValue)
            {
                throw new OverflowException("Sorry, exceeds maximum input");
            }

            return _currencyObj;
        }

        public string GetWords(BigInteger _validNumberInt)
        {
            string words = "";

            int power = (largeNumbers.Length) * 3;

            while (power > 3)
            {
                BigInteger pow = BigInteger.Pow(10, power);
                if (_validNumberInt >= pow)
                {
                    if (_validNumberInt % pow > 0)
                    {
                        words += GetWords(BigInteger.Divide(_validNumberInt, pow)) + " " + largeNumbers[(power / 3) - 1] + " and ";
                    }
                    else if (_validNumberInt % pow == 0)
                    {
                        words += GetWords(BigInteger.Divide(_validNumberInt, pow)) + " " + largeNumbers[(power / 3) - 1];
                    }
                    _validNumberInt %= pow;
                }
                power -= 3;
            }

            if (_validNumberInt >= 1000)
            {
                if (_validNumberInt % 1000 == 0)
                {
                    words += GetWords(BigInteger.Divide(_validNumberInt, 1000)) + " thousand";
                }
                else
                {
                    words += GetWords(BigInteger.Divide(_validNumberInt, 1000)) + " thousand and ";
                }

                _validNumberInt %= 1000;
            }

            if (_validNumberInt < 1000)
            {
                try
                {
                    if (BigInteger.Divide(_validNumberInt, 100) > 0)
                    {
                        words += GetWords(BigInteger.Divide(_validNumberInt, 100)) + " hundred";

                        _validNumberInt %= 100;
                    }

                    if (_validNumberInt > 0)
                    {
                        if (words != "")
                            words += " and ";

                        if (_validNumberInt < 20)
                            words += units[(int)_validNumberInt];
                        else
                        {
                            words += tens[(int)_validNumberInt / 10];
                            if ((_validNumberInt % 10) > 0)
                                words += "-" + units[(int)_validNumberInt % 10];
                        }
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    throw new IndexOutOfRangeException("Can't find any number. Your input is out of range");
                }
            }

            return words.Trim();
        }

        public string GetDecimalWords(BigInteger _decimalNumber)
        {
            string decimalWords = "";
            
            try
            {
                if (_decimalNumber < 20)
                {
                    decimalWords += units[(int)_decimalNumber];
                }
                else
                {
                    decimalWords += tens[(int)_decimalNumber / 10];

                    if ((_decimalNumber % 10) > 0)
                        decimalWords += "-" + units[(int)_decimalNumber % 10];
                }
            }
            catch (OverflowException)
            {
                throw new OverflowException("Decimal digits is out of range, only accept maximum 2 number (e.g. 123.45)");
            }
            catch (IndexOutOfRangeException)
            {
                throw new IndexOutOfRangeException("Decimal digits is out of range, only accept maximum 2 number (e.g. 123.45)");
            }
            return decimalWords.Trim();
        }

        public static void Main()
        {
            CurrencyToWords Usd = new CurrencyToWords();

            bool endApp = false;

            Console.WriteLine("Console USD Currency To Words in C#\r");
            Console.WriteLine("------------------------\n");

            while (!endApp)
            {
                try
                {
                    Console.Write("Please input the currency you want to change into words = ");
                    string userInput = Console.ReadLine();
                    Console.WriteLine("\t");

                    Usd.ChangeCurrencyToWords(userInput);

                    Console.WriteLine("\t");

                    Console.WriteLine("\ta - Try again");
                    Console.WriteLine("\tq - Quit");
                    Console.Write("Your option? ");

                    userInput = Console.ReadLine();

                    if (userInput == "q")
                    {
                        endApp = true;
                        Console.WriteLine("Thanks, See you again !");
                        break;
                    };
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return;
        }
    }
}
