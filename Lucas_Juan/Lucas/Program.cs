using Lucas.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lucas
{
    internal class Program
    {
        static List<Expression> listExpression = new List<Expression>();

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome. Please type some expression like:\n A = 5 + 5 \n B = A + 3\n\n");

            do
            {
                var text = Console.ReadLine();
                MainFunction(text);

                foreach (var item in listExpression)
                {
                    if (item.value != null && (!item.references.Any() || !item.references.Any(x => listExpression.Any(y => y.word == x && y.value == null))))
                        Console.WriteLine("===> " + item.word + " = " + item.value);
                }
            } while (true);
        }

        private static void MainFunction(string text)
        {
            var textPlited = text.Split("=");

            if (textPlited.Length > 1)
            {
                SetExpressionIfIsLetter(textPlited[0]);
                SetNextExpression(textPlited, 0);
            }
            else
                Console.WriteLine("\n Only expressions like: A = 5 + 5, B = A + 3, is acceptable. \n");
        }

        private static void SetNextExpression(string[] textPlited, int index)
        {
            if (index + 1 < textPlited.Length)
            {
                var textTrim = textPlited[index + 1].Trim();

                if (textTrim.Contains("+"))
                {
                    RemoveReferences(textPlited[0]);

                    var expression = textTrim.Split("+");
                    var first = true;
                    foreach (var item in expression)
                    {
                        SetExpressionIfIsLetter(item);
                        SetValueExpression(textPlited[index], item, !first);
                       
                        first = false;
                    }
                }
                else
                {
                    SetExpressionIfIsLetter(textTrim);
                    SetValueExpression(textPlited[index], textTrim, false);

                    RemoveReferences(textPlited[0]);
                }
            }
        }

        private static void RemoveReferences(string word)
        {
            var remove = listExpression.FirstOrDefault(x => x.word == word);
            if (remove != null)
                remove.references.Clear();
        }

        private static void SetExpressionIfIsLetter(string word)
        {
            if (Char.IsLetter(word[0]))
            {
                if (!listExpression.Any(x => x.word == word))
                    listExpression.Add(new Expression(word, null));
            }
        }
        private static void SetValueExpression(string word, string text, bool sum = true)
        {
            decimal? value = null;

            if (Decimal.TryParse(text, out decimal d))
            {
                value = d;

                if (listExpression.Any(x => x.word == word))
                {
                    var expression = listExpression.FirstOrDefault(x => x.word == word);
                    if (sum)
                        expression.value = (expression.value == null ? value : expression.value + value);
                    else
                        expression.value = value;

                    UpdateReferences(word, sum);
                }
                else
                {
                    listExpression.Add(new Expression(word, value));
                }
            }
            else
            {
                var expression = listExpression.FirstOrDefault(x => x.word == word);

                if (Char.IsLetter(text[0]))
                {
                    if (listExpression.Any(x => x.word == word))
                        if (sum)
                            expression.value = Convert.ToDecimal(expression.value) + listExpression.FirstOrDefault(x => x.word == text).value;
                        else
                            expression.value = listExpression.FirstOrDefault(x => x.word == text).value;

                    UpdateReferences(word, !sum);

                    expression.references.Add(text);
                }
            }
        }

        private static void UpdateReferences(string word, bool sum = false)
        {
            if (listExpression.Any(x => x.references.Any(y => y == word)))
            {
                var refWord = listExpression.FirstOrDefault(x => x.references.Any(y => y == word));

                /*somar expresão*/
                decimal total = 0m;
                foreach (var item in refWord.references)
                {
                    var obj = listExpression.FirstOrDefault(x => x.word == item);
                    total += Convert.ToDecimal(obj.value);
                }
                refWord.value = total;

                UpdateReferences(refWord.word, true);
            }
        }
    }

 
}
