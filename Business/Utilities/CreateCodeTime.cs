using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Business.Utilities
{
    public static class CreateCodeTime
    {
        public static string CreateTime()
        {
            DateTime dateTime = DateTime.Now;
            string day = dateTime.Day.ToString();
            string month = dateTime.Month.ToString();
            string year = dateTime.Year.ToString();
            year = year.Substring(year.Length - 3);

            return day + month + year + dateTime.Minute + dateTime.Second;
        }

        public static string GenerateOrderCode()
        {
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();

            // Gün, ay, yıl ve saniye bilgilerini alıyoruz
            string day = DateTime.Now.Day.ToString("00");
            string month = DateTime.Now.Month.ToString("00");
            string year = DateTime.Now.Year.ToString().Substring(2); // Sadece son iki rakamı alıyoruz
            string second = DateTime.Now.Second.ToString("00");

            // Order code'un geri kalanı için rastgele harf ve rakamlar seçiyoruz
            char[] code = new char[10]; // Toplam uzunluk 10 karakter olacak

            for (int i = 0; i < 2; i++)
            {
                code[i] = day[i];
            }

            for (int i = 0; i < 2; i++)
            {
                code[i + 2] = month[i];
            }

            for (int i = 0; i < 2; i++)
            {
                code[i + 4] = year[i];
            }

            for (int i = 0; i < 2; i++)
            {
                code[i + 6] = second[i];
            }

            for (int i = 0; i < 2; i++)
            {
                code[i + 8] = characters[random.Next(characters.Length)];
            }

            return new string(code);
        }
    }
}
