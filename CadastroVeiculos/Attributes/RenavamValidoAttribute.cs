using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CadastroVeiculos.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RenavamValidoAttribute : ValidationAttribute
    {
        public RenavamValidoAttribute()
        {
            ErrorMessage = "Renavam inválido.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var renavam = (value as string)?.Trim();
            if (string.IsNullOrWhiteSpace(renavam))
                return ValidationResult.Success;

            renavam = Regex.Replace(renavam, @"\D", "");

            if (renavam.Length != 11)
                return new ValidationResult("Renavam deve conter 11 dígitos.");

            if (!RenavamCheck(renavam))
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }

        private bool RenavamCheck(string renavam)
        {
            int[] pesos = {3, 2, 9, 8, 7, 6, 5, 4, 3, 2};
            int soma = 0;
            for (int i = 0; i < 10; i++)
            {
                int d = renavam[i] - '0';
                soma += d * pesos[i];
            }

            int resto = soma % 11;
            int digito = 11 - resto;
            if (digito == 10 || digito == 11) digito = 0;

            int ultimo = renavam[10] - '0';
            return digito == ultimo;
        }
    }
}