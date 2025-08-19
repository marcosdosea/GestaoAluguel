using System.ComponentModel.DataAnnotations;


namespace Util
{
    /// <summary>
    /// Validação customizada para CPF
    /// </summary>
    public class CepAttribute : ValidationAttribute
    {
        /// <summary>
        /// Construtor
        /// </summary>
        public CepAttribute() { }

        /// <summary>
        /// Validação server
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object? value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return false;
            var valueNoEspecial = Methods.RemoveSpecialsCaracts((string)value);
            if (valueNoEspecial.ToString().Length != 8)
                return false;
            if (valueNoEspecial.ToString().StartsWith('0'))
                return false;
            return true;
        }

        public static string GetErrorMessage() =>
            $"CEP Inválido";
    }
}
