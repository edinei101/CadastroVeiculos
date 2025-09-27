using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CadastroVeiculos.Attributes;

namespace CadastroVeiculos.Models
{
    public enum TipoCombustivel
    {
        Gasolina,
        Diesel,
        Etanol,
        Flex,
        Eletrico,
        GNV
    }

    public class VeiculoViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "Placa é obrigatória.")]
        [RegularExpression(@"^[A-Za-z]{3}[0-9][A-Za-z][0-9]{2}$", ErrorMessage = "Placa deve obedecer ao padrão Mercosul (ex.: ABC1D23).")]
        [PlacaUnica]
        [Display(Name = "Placa")]
        public string Placa { get; set; }

        [Required(ErrorMessage = "Renavam é obrigatório.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Renavam deve conter 11 dígitos.")]
        [RenavamValido]
        [Display(Name = "Renavam")]
        public string Renavam { get; set; }

        [Required(ErrorMessage = "Chassi é obrigatório.")]
        [RegularExpression(@"^[A-HJ-NPR-Za-hj-npr-z0-9]{17}$", ErrorMessage = "Chassi deve ter 17 caracteres alfanuméricos e não pode conter I, O ou Q.")]
        [Display(Name = "Chassi")]
        public string Chassi { get; set; }

        [Required(ErrorMessage = "Ano de fabricação é obrigatório.")]
        [Display(Name = "Ano Fabricação")]
        public int AnoFabricacao { get; set; }

        [Required(ErrorMessage = "Ano do modelo é obrigatório.")]
        [Display(Name = "Ano Modelo")]
        public int AnoModelo { get; set; }

        [Required(ErrorMessage = "Tipo de combustível é obrigatório.")]
        [Display(Name = "Tipo de Combustível")]
        public TipoCombustivel? TipoCombustivel { get; set; }

        [Display(Name = "Valor do Seguro")]
        [RequiredIf("AnoModelo", ComparisonType.GreaterThan, 2010, ErrorMessage = "Valor do seguro é obrigatório se Ano do Modelo for maior que 2010.")]
        [Range(500, double.MaxValue, ErrorMessage = "Valor do seguro deve ser no mínimo R$500,00.")]
        public decimal? ValorSeguro { get; set; }

        [Required(ErrorMessage = "Nome do proprietário é obrigatório.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Nome do proprietário deve ter entre 6 e 100 caracteres.")]
        [RegularExpression(@"^[A-Za-zÀ-ÿ\s]+$", ErrorMessage = "Nome do proprietário deve conter apenas letras e espaços.")]
        [Display(Name = "Nome Proprietário")]
        public string NomeProprietario { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            int anoAtual = DateTime.Now.Year;

            if (AnoFabricacao < 1980 || AnoFabricacao > anoAtual)
            {
                yield return new ValidationResult($"Ano de fabricação deve estar entre 1980 e {anoAtual}.", new[] { nameof(AnoFabricacao) });
            }

            if (AnoModelo < AnoFabricacao)
            {
                yield return new ValidationResult("Ano do modelo não pode ser menor que o Ano de fabricação.", new[] { nameof(AnoModelo) });
            }

            if (AnoModelo > anoAtual + 1)
            {
                yield return new ValidationResult($"Ano do modelo não pode ser maior que {anoAtual + 1}.", new[] { nameof(AnoModelo) });
            }
        }
    }
}
