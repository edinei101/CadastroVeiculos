using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CadastroVeiculos.Attributes
{
    public enum ComparisonType
    {
        Equal,
        NotEqual,
        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class RequiredIfAttribute : ValidationAttribute
    {
        public string DependentProperty { get; }
        public ComparisonType Comparison { get; }
        public object ComparisonValue { get; }

        public RequiredIfAttribute(string dependentProperty, ComparisonType comparison, object comparisonValue)
        {
            DependentProperty = dependentProperty;
            Comparison = comparison;
            ComparisonValue = comparisonValue;
            ErrorMessage = "{0} é obrigatório.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo depProp = validationContext.ObjectType.GetProperty(DependentProperty);
            if (depProp == null)
                return new ValidationResult($"Propriedade dependente '{DependentProperty}' não encontrada.");

            var depValue = depProp.GetValue(validationContext.ObjectInstance);

            if (depValue == null)
                return ValidationResult.Success;

            bool cond = Compare(depValue, ComparisonValue);

            if (cond && (value == null || (value is string s && string.IsNullOrWhiteSpace(s))))
            {
                var displayName = validationContext.DisplayName;
                return new ValidationResult(string.Format(ErrorMessageString, displayName));
            }

            return ValidationResult.Success;
        }

        private bool Compare(object left, object right)
        {
            if (decimal.TryParse(left.ToString(), out var lNum) && decimal.TryParse(right.ToString(), out var rNum))
            {
                return Comparison switch
                {
                    ComparisonType.Equal => lNum == rNum,
                    ComparisonType.NotEqual => lNum != rNum,
                    ComparisonType.GreaterThan => lNum > rNum,
                    ComparisonType.LessThan => lNum < rNum,
                    ComparisonType.GreaterThanOrEqual => lNum >= rNum,
                    ComparisonType.LessThanOrEqual => lNum <= rNum,
                    _ => false
                };
            }
            else
            {
                int cmp = string.Compare(left.ToString(), right.ToString(), StringComparison.OrdinalIgnoreCase);
                return Comparison switch
                {
                    ComparisonType.Equal => cmp == 0,
                    ComparisonType.NotEqual => cmp != 0,
                    ComparisonType.GreaterThan => cmp > 0,
                    ComparisonType.LessThan => cmp < 0,
                    ComparisonType.GreaterThanOrEqual => cmp >= 0,
                    ComparisonType.LessThanOrEqual => cmp <= 0,
                    _ => false
                };
            }
        }
    }
}
