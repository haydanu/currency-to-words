using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UsdCurrencyToWords
{
    public class CurrencyToWords
    {
        private readonly string[] units = { "", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
        private readonly string[] tens = { "", "", "twenty", "thirty", "fourty", "fifty", "sixty", "seventy", "eighty", "ninety" };
        private readonly string[] largeNumbers = { "", "thousand", "million", "billion", "trillion", "quadrillion", "quintillion", "sextillion", "septillion", "octillion", "nonillion", "decillion", "undecillion", "duodecillion", "tredecillion", "Quattuordecillion", "Quindecillion", "Sexdecillion", "Septdecillion", "Octodecillion", "Novemdecillion", "Vigintillion" };
        private readonly string cent = "cent";
        private readonly string cents = "cents";
        private readonly string dollar = "dollar";
        private readonly string dollars = "dollars";



        public Dictionary<string, double> CheckNumber(string currency)
        {
            // check if currency is null or not
            if (string.IsNullOrEmpty(currency))
            {
                throw new FormatException("Input can't be empty");
            }

            // find any aplhabet in input 
            Regex charPattern = new Regex(@"[a-z]", RegexOptions.IgnoreCase);
            MatchCollection matchesChar = charPattern.Matches(currency);

            // if alphabet found
            if (matchesChar.Count >= 1)
            {
                throw new FormatException("Input only accept number or decimal with 2 numbers");
            }

            

            // change data type to double
            Dictionary<string, double> _currencyObj = new Dictionary<string, double>();

            if (currency.Contains("."))
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

            if (currency.Length > 14)
            {
                Console.WriteLine(ulong.Parse(currency) + "quintillion");
                // cannot ! Console.WriteLine(double.Parse(currency) + "quintillion");
                Console.WriteLine(decimal.Parse(currency) + "quintillion");
            }

            return _currencyObj;
        }

        //public string GetTensWords(int _currency)
        //{
        //    string tensWords;
        //    string sentences = _currency.ToString().Length == 1 ? _currency <= 1 ? dollar : dollars : dollars;

        //    if (_currency < 20)
        //    {
        //        tensWords = " and " + units[_currency] + " " + sentences;
        //    }
        //    else
        //    {
        //        int getTensCurrency = _currency / 10;
        //        int getRemainderCurrency = _currency % 10;
        //        string seperator = _currency % 10 == 0 ? "" : "-";
        //        tensWords = " and " + tens[getTensCurrency] + seperator + units[getRemainderCurrency] + " " + sentences;
        //    }

        //    return tensWords.Trim();
        //}

        public string GetDecimalWords(int _decimalNumber)
        {
            string decimalWords;
            int getDecimalTens = _decimalNumber / 10;
            int getRemainderDecimal = _decimalNumber % 10;
            string seperator = _decimalNumber % 10 == 0 ? "" : "-";

            try
            {
                if (_decimalNumber < 20)
                {
                    decimalWords = " and " + units[_decimalNumber];
                }
                else
                {
                    decimalWords = " and " + tens[getDecimalTens] + seperator + units[getRemainderDecimal];
                }
            }
            catch
            {
                decimalWords = " ";
            }

            return decimalWords.Trim();
        }
        public string ChangeCurrencyToWords(string currency)
        {
            // validation input number
            Dictionary<string, double> validNumber = CheckNumber(currency);


            // store valid number to variable
            int _validNumberInt = (int)validNumber["number"];
            int _validNumberDecimal = (int)validNumber["decimalNumber"];

            // valid number below 0 or equal 0
            if (_validNumberInt < 0)
            {
                Console.WriteLine("negative number is not allowed");
            }

            if (_validNumberInt == 0)
            {
                Console.WriteLine("zero");
            }

            // valid words
            string words = "";
            string getDollar = _validNumberInt < 2 ? dollar : dollars;
            string getCent = _validNumberDecimal < 2 ? cent : cents;

            // Change number to words by it's value
            //if (_validNumberInt < 1000)
            //{
            //    int getFirstCharacterOfCurrency = _validNumberInt / 100;
            //    int getRemainderCurrency = _validNumberInt % 100;

            //    if (_validNumberInt % 100 == 0)
            //    {
            //        words = units[getFirstCharacterOfCurrency] + " hundred " + dollars;
            //    }
            //    else
            //    {
            //        words = units[getFirstCharacterOfCurrency] + " hundred " + GetTensWords(getRemainderCurrency) + " " + GetDecimalWords(_decimalNumber);
            //    }
            //}

            if (_validNumberInt < 100)
            {
                if (_validNumberInt % 10 == 0)
                {
                    words = tens[_validNumberInt / 10];
                } else
                {
                    words = tens[_validNumberInt / 10] + "-" + units[_validNumberInt % 10];
                }
                words += " " + getDollar;
            }

            if (_validNumberInt < 20)
            {
                words = units[_validNumberInt];
                words += " " + getDollar;
            }

            // get decimal words
            if (_validNumberDecimal != 0)
            {
                if (words != "")
                {
                    words += " " + GetDecimalWords(_validNumberDecimal) + " " + getCent;
                }
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
