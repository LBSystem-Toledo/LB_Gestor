using System;

namespace LB_Gestor_API.Utils
{
    public static class Utils
    {
        public static string SoNumero(this object valor)
        {
            if (valor == null)
                return string.Empty;
            string ret = string.Empty;
            foreach (char c in valor.ToString().ToCharArray())
                if (char.IsNumber(c))
                    ret += c;
            return ret;
        }
        public static bool IsDateTime(this object valor)
        {
            try
            {
                Convert.ToDateTime(valor);
                return true;
            }
            catch { return false; }
        }
        public static bool IsDiaUtil(this DateTime data)
        {
            return data.DayOfWeek != DayOfWeek.Sunday &&
                data.DayOfWeek != DayOfWeek.Saturday;
        }
    }
}
