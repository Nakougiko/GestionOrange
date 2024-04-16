using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionOrange.Services
{
    public static class FormatDonnee
    {
        public static string ConvertToDatabaseFormat(string nom)
        {
            return nom.Trim().ToLower().Replace(" ", "_");
        }

        public static string ConvertToDisplayFormat(string nom)
        {
            nom = nom.Replace("_", " ");
            nom = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(nom.ToLower());
            return nom;
        }

        public static string ConvertNumeroToDatabaseFormat(string numero)
        {
            return numero.Replace(" ", "");
        }

        public static string ConvertNumeroToDisplayFormat(string numero)
        {
            for (int i = 2; i < numero.Length; i += 3)
            {
                numero = numero.Insert(i, " ");
            }
            return numero;
        }
    }
}
