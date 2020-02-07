using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UsdCurrencyToWords
{
    public class CurrencyToWords
    {
        private readonly string[] units = { "", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
        private readonly string[] tens = { "", "", "twenty", "thirty", "fourty", "fifty", "sixty", "seventy", "eighty", "ninety" };
        private readonly string[] largeNumbers = { "thousand", "million", "billion", "trillion", "quadrillion", "quintillion", "sextillion", "septillion", "octillion", "nonillion", "decillion", "undecillion", "duodecillion", "tredecillion", "quattuordecillion", "quindecillion", "sexdecillion", "septdecillion", "octodecillion", "novemdecillion", "vigintillion" };
        private readonly string cent = "cent";
        private readonly string cents = "cents";
        private readonly string dollar = "dollar";
        private readonly string dollars = "dollars";

        public string ChangeCurrencyToWords(string currency)
        {
            // validation input number
            CheckInputNumber(currency);
            Dictionary<string, double> validNumber = ConvertInputNumber(currency);

            // store valid number to variable
            double _validNumberInt = validNumber["number"];
            double _validNumberDecimal = validNumber["decimalNumber"];
            string getCent = _validNumberDecimal < 2 ? cent : cents;

            // validation for number below 0 or equal 0
            if (_validNumberInt < 0)
            {
                Console.WriteLine("negative number is not allowed");
            }

            if (_validNumberInt == 0)
            {
                Console.WriteLine("zero");
            }

            string words = GetWords(_validNumberInt);

            // get decimal words
            if (_validNumberDecimal != 0)
            {
                if (words != "")
                {
                    words += " " + GetDecimalWords(_validNumberDecimal) + " " + getCent;
                }
            }

            Console.WriteLine(words.ToUpper());
            return words;
        }

        public void CheckInputNumber(string currency)
        {
            // check if currency is null or not
            if (string.IsNullOrEmpty(currency))
            {
                throw new FormatException("Input can't be empty");
            }

            // find any aplhabet in input 
            Regex charPattern = new Regex(@"[^0-9,.]", RegexOptions.IgnoreCase);
            MatchCollection matchesChar = charPattern.Matches(currency);

            // if alphabet found
            if (matchesChar.Count >= 1)
            {
                throw new FormatException("Input only accept number and following by 2 digits decimal number (e.g 123.45)");
            }
        }

        public Dictionary<string, double> ConvertInputNumber(string currency)
        {
            // change data type to double (because range is large) and save it to obj
            Dictionary<string, double> _currencyObj = new Dictionary<string, double>();
            try
            {
                if (currency.Contains(".") | currency.Contains(","))
                {
                    Regex decimalPattern = new Regex(@"\b\d+\b");
                    MatchCollection matches = decimalPattern.Matches(currency);

                    if (matches.Count != 1)
                    {

                        _currencyObj["number"] = double.Parse(matches[0].Value);
                        _currencyObj["decimalNumber"] = double.Parse(matches[1].Value);
                    }
                }
                else
                {
                    _currencyObj["number"] = double.Parse(currency);
                    _currencyObj["decimalNumber"] = 0;
                }
            }
            catch (OverflowException)
            {
                throw new OverflowException("Sorry, exceeds maximum input");
            }

            return _currencyObj;
        }

        public string GetWords(double _validNumberInt)
        {
            // valid words
            string words = "";
            string getDollar = _validNumberInt < 2 ? dollar : dollars;

            // To find scales of large number multiply by 3
            int power = (largeNumbers.Length + 1) * 3;

            // iteration every 3 digits of input if input is larger than large number scales
            while (power > 3)
            {
                double pow = Math.Pow(10, power);

                if (_validNumberInt >= pow)
                {
                    if (_validNumberInt % pow > 0)
                    {
                        words += GetWords(Math.Floor(_validNumberInt / pow)) + " " + largeNumbers[(power / 3) - 1] + " and ";
                    }
                    else if (_validNumberInt % pow == 0)
                    {
                        words += GetWords(Math.Floor(_validNumberInt / pow)) + " " + largeNumbers[(power / 3) - 1];
                    }
                    _validNumberInt %= pow;
                }
                power -= 3;
            }

            // words for thousand and up
            if (_validNumberInt >= 1000)
            {
                if (_validNumberInt % 1000 == 0)
                {
                    words += GetWords(Math.Floor(_validNumberInt / 1000)) + " thousand ";

                }
                else
                {
                    words += GetWords(Math.Floor(_validNumberInt / 1000)) + " thousand and ";
                }
                _validNumberInt %= 1000;
            }

            // words for thousand and below
            if (_validNumberInt < 1000)
            {
                try
                {
                    if (Math.Floor(_validNumberInt / 100) > 0)
                    {
                        words += GetWords(Math.Floor(_validNumberInt / 100));
                        words = Regex.Replace(words, @"\bdollars?\b", "hundred");

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
                    words += " " + getDollar;
                }
                catch (IndexOutOfRangeException)
                {
                    throw new IndexOutOfRangeException("Can't find any number. Your input is out of range");
                }
            }
            return words.Trim();
        }

        public string GetDecimalWords(double _decimalNumber)
        {
            string decimalWords;
            double getDecimalTens = _decimalNumber / 10;
            double getRemainderDecimal = _decimalNumber % 10;
            string seperator = _decimalNumber % 10 == 0 ? "" : "-";

            try
            {
                if (_decimalNumber < 20)
                {
                    decimalWords = " and " + units[(int)_decimalNumber];
                }
                else
                {
                    decimalWords = " and " + tens[(int)getDecimalTens] + seperator + units[(int)getRemainderDecimal];
                }
            }
            catch (IndexOutOfRangeException)
            {
                throw new IndexOutOfRangeException("decimal digit is out of range, only accept maximum 2 number (e.g 123.45)");
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
