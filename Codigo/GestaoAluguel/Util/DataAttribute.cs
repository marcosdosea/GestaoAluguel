using System.ComponentModel.DataAnnotations;

namespace Util
{
    
    public class DataAttribute : ValidationAttribute
    {
        private readonly int _anosMaximos;
        public DataAttribute(int anosMaximos)
        {
            _anosMaximos = anosMaximos;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not DateTime dataNascimento)
            {
                return ValidationResult.Success; 
            }
            var dataLimite = DateTime.Today.AddYears(-_anosMaximos);

            if (dataNascimento < dataLimite)
            {
                // Retorna a mensagem de erro.
                return new ValidationResult(ErrorMessage ?? $"A data não pode ser mais antiga que {_anosMaximos} anos.");
            }

            return ValidationResult.Success;
        }
    }
}
