using System.Text.RegularExpressions;

namespace backend_SG.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNomeValido(this string nome)
        {
            if (string.IsNullOrWhiteSpace(nome)) 
                return false;

            var regex = new Regex(@"^[a-zA-ZÀ-ÿ\s]+$");
            return regex.IsMatch(nome);
        }

        public static bool IsEmailValido(this string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsCpfValido(this string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf)) 
                return false;

            cpf = Regex.Replace(cpf, @"[^\d]", "");

            if (cpf.Length != 11) 
                return false;

            if (new string(cpf[0], 11) == cpf) 
                return false;

            int[] multiplicadores1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma = 0;
            for (int i = 0; i < 9; i++)
            {
                soma += (cpf[i] - '0') * multiplicadores1[i];
            }
            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            if (cpf[9] - '0' != digito1) 
                return false;

            int[] multiplicadores2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            soma = 0;
            for (int i = 0; i < 10; i++)
            {
                soma += (cpf[i] - '0') * multiplicadores2[i];
            }
            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            return cpf[10] - '0' == digito2;
        }

        public static bool IsCnpjValido(this string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj)) 
                return false;
            cnpj = Regex.Replace(cnpj, @"[^\d]", "");
            if (cnpj.Length != 14) 
                return false;
            if (new string(cnpj[0], 14) == cnpj) 
                return false;
            int[] multiplicadores1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma = 0;
            for (int i = 0; i < 12; i++)
            {
                soma += (cnpj[i] - '0') * multiplicadores1[i];
            }
            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;
            if (cnpj[12] - '0' != digito1) 
                return false;
            int[] multiplicadores2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            soma = 0;
            for (int i = 0; i < 13; i++)
            {
                soma += (cnpj[i] - '0') * multiplicadores2[i];
            }
            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;
            return cnpj[13] - '0' == digito2;
        }

        public static bool IsTelefoneValido(this string telefone)
        {
            if (string.IsNullOrWhiteSpace(telefone)) 
                return false;
            var regex = new Regex(@"^\(?\d{2}\)?[\s-]?\d{4,5}-?\d{4}$");
            return regex.IsMatch(telefone);
        }

        public static bool IsSenhaValida(this string senha)
        {
            if (string.IsNullOrWhiteSpace(senha)) 
                return false;
            if (senha.Length < 8) 
                return false;
            if (!senha.Any(char.IsDigit)) 
                return false;

            return true;
        }
    }
}