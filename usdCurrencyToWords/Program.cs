using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UsdCurrencyToWords
{
    public class CurrencyToWords
    {
        private readonly string[] units = { "", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
        private readonly string[] tens = { "", "", "twenty", "thirty", "fourty", "fifty", "sixty", "seventy", "eighty", "ninety" };
        private readonly string cent = "cent";
        private readonly string cents = "cents";
        private readonly string dollar = "dollar";
        private readonly string dollars = "dollars";

        public Dictionary<string, int> CheckNumber(string currency)
        {
            if (string.IsNullOrEmpty(currency))
            {
                throw new FormatException("Input can't be empty");
            }

            Regex charPattern = new Regex(@"[a-z]", RegexOptions.IgnoreCase);
            MatchCollection matchesChar = charPattern.Matches(currency);

            if (matchesChar.Count >= 1)
            {
                throw new FormatException("Input only accept number or decimal with 2 numbers");
            }

            Dictionary<string, int> _currency = new Dictionary<string, int>();

            if (currency.Contains("."))
            {
                Regex decimalPattern = new Regex(@"\b\d+\b");
                MatchCollection matches = decimalPattern.Matches(currency);

                if (matches.Count != 1)
                {
                    _currency["number"] = Convert.ToInt32(matches[0].Value);
                    _currency["decimalNumber"] = Convert.ToInt32(matches[1].Value);
                }
            }
            else
            {
                _currency["number"] = Convert.ToInt32(currency);
                _currency["decimalNumber"] = 0;

            }
            return _currency;
        }
        public string GetTensWords(int _currency)
        {
            string tensWords;
            int getTensCurrency;
            int getRemainderCurrency = _currency % 10;
            string sentences = _currency.ToString().Length == 1 ? _currency <= 1 ? dollar : dollars : dollars;

            if (_currency < 20)
            {
                tensWords = " and " + units[_currency] + " " + sentences;
            }
            else
            {
                getTensCurrency = (int)Math.Floor(_currency / 10.0);
                tensWords = " and " + tens[getTensCurrency] + "-" + units[getRemainderCurrency] + " " + sentences;
            }
            return tensWords.Trim();
        }
        public string GetDecimalWords(int _decimalNumber)
        {
            string decimalWords;
            if (_decimalNumber != 0)
            {
                string _decimalString = _decimalNumber.ToString();

                if (_decimalString.Length == 1)
                {
                    decimalWords = " and " + units[_decimalNumber] + " " + cent;
                }
                else
                {
                    int getDecimalTens = (int)Math.Floor(_decimalNumber / 10.0);
                    int getRemainderDecimal = _decimalNumber % 10;
                    decimalWords = " and " + tens[getDecimalTens] + "-" + units[getRemainderDecimal] + " " + cents;
                }
            }
            else
            {
                decimalWords = " ";
            }
            return decimalWords.Trim();
        }
        public string ChangeCurrencyToWords(string currency)
        {

            Dictionary<string, int> _currencyString = CheckNumber(currency);

            int _currency = _currencyString["number"];
            int _decimalNumber = _currencyString["decimalNumber"];

            if (_currency < 0)
            {
                Console.WriteLine("negative number is not allowed");
                return "";
            }

            if (_currency == 0)
            {
                Console.WriteLine("zero");
                return "";
            }

            string words = "";

            if (_currency < 20)
            {
                if (_currency == 1)
                {
                    words = units[_currency] + " " + dollar + " " + GetDecimalWords(_decimalNumber);
                }
                else
                {
                    words = units[_currency] + " " + dollars + " " + GetDecimalWords(_decimalNumber);
                }
            }

            if (_currency >= 20 & _currency < 100)
            {
                int getTensCurrency = (int)Math.Floor(_currency / 10.0);
                int getRemainderCurrency = _currency % 10;

                if (_currency == 10)
                {
                    words = tens[0] + " " + dollars + " " + GetDecimalWords(_decimalNumber);
                }
                else if (_currency == 20)
                {
                    words = tens[2] + " " + dollars + " " + GetDecimalWords(_decimalNumber);
                }
                else
                {
                    words = tens[getTensCurrency] + " " + units[getRemainderCurrency] + " " + dollars + " " + GetDecimalWords(_decimalNumber);
                }
            }

            if (_currency >= 100 & _currency < 1000)
            {
                int getFirstCharacterOfCurrency = (int)Math.Floor(_currency / 100.0);
                int getRemainderCurrency = _currency % 100;

                words = units[getFirstCharacterOfCurrency] + " hundred " + GetTensWords(getRemainderCurrency) + " " + GetDecimalWords(_decimalNumber);
            }
            Console.WriteLine(words.ToUpper());
            return words.ToUpper();
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
                        break;
                    };
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return;
        }
    }
}
