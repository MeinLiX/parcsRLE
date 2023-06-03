using System;
using System.Linq;
using System.Text;
using System.Threading;
using Parcs;

namespace FirstModule
{
    public class LogicModule: IModule
    {
        public void Run(ModuleInfo info, CancellationToken token = default(CancellationToken))
        {
            bool pack = info.Parent.ReadBoolean();
            string input = info.Parent.ReadString();


            string res = "";

            if (pack)
            {
                res = PackText(input);
            }
            else
            {
                res = UnpackText(input);
            }

            info.Parent.WriteData(res);
        }

        public string PackText(string input)
        {
            try
            {
                StringBuilder res = new StringBuilder();
                char ch;
                int i, k, j;
                for (i = 0; i < input.Length;)
                {
                    ch = input[i];
                    k = 0;
                    if (i == input.Length - 1) 
                    {
                        res.Append(ch);
                        break; 
                    }

                    if (input[i + 1] == ch) //перевіряємо повторювання символу
                    {
                        for (j = i; j < input.Length; j++)
                        {
                            if (input[j] == ch)
                            {
                                if (k == 9) break; // максимальне число пакування 9, демонстраційно, щоб не робити обробку складніше з числами.
                                k++;
                            }
                            else break;
                        }
                        i = j;
                    }
                    else if ("0123456789".Contains(ch))// ігноруємо цифри для пакування, всі циври на вході будуть записуватись як 1N (N - цифра на вході)
                    {
                        k = 1;
                        i++;
                    } else i++;

                    if (k != 0) res.AppendFormat("{0}{1}", k, ch); //пакуємо
                    else res.Append(ch);
                }
                return res.ToString();
            }
            catch
            {
                return null;
            }
        }

        public string UnpackText(string input)
        {
            try
            {
                StringBuilder res = new StringBuilder();

                char ch;
                char symb = 'a';
                int s = 0;
                for (int i = 0; i < input.Length;)
                {
                    ch = input[i];
                    s = 0;
                    if ("123456789".Contains(ch))
                    {
                        if (i == input.Length - 1)
                        {
                            symb = ch;
                            s = 1;
                            i++;
                        }
                        else
                        {
                            symb = input[i + 1]; // отримуємо запакований символ
                            i += 2;
                            s = Convert.ToInt32(ch) - 48; //отримуємо кількість запакованих символів
                        }
                    }
                    else
                    {
                        s = 0;
                        i++;
                    }
                    if (s > 0)
                    {
                        // Запис розпакованих символів з ігноруванням цілого числа
                        for (int j = 0; j < s; j++) res.Append(symb);
                    }
                    else res.Append(ch);
                }
                return res.ToString();
            }
            catch
            {
                return null;
            }

        }
    }
}
