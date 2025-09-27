using System;
using System.ComponentModel.DataAnnotations;
using CadastroVeiculos.Data;

namespace CadastroVeiculos.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PlacaUnicaAttribute : ValidationAttribute
    {
        public PlacaUnicaAttribute()
        {
            ErrorMessage = "Já existe um veículo cadastrado com esta placa.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var placa = value as string;
            if (string.IsNullOrWhiteSpace(placa))
                return ValidationResult.Success;

            if (PlacaRepository.Existe(placa))
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
}