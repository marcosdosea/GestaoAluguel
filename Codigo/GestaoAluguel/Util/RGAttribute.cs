using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    /// <summary>
    /// Validação customizada para RG (Registro Geral)
    /// </summary>
    public class RGAttribute : ValidationAttribute
    {
        /// <summary>
        /// Validação server
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object? value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return true;

            var rg = Methods.RemoveSpecialsCaracts((string)value);

            if (rg.Length != 9 && rg.Length != 8) // Verifica o comprimento do RG
                return false;

            if (rg.Length == 8) // Se for RG com 8 dígitos, considerar apenas os 8 primeiros
            {
                string numeroBase = rg;
                string digitoVerificador = "";
                return CalcularDigitoVerificador(numeroBase, digitoVerificador);
            }
            else // Se for RG com 9 dígitos, separa os 8 primeiros do dígito verificador
            {
                string numeroBase = rg.Substring(0, 8);
                string digitoVerificador = rg.Substring(8, 1);
                return CalcularDigitoVerificador(numeroBase, digitoVerificador);
            }

        }

        /// <summary>
        /// Lógica para verificação do dígito verificador do RG
        /// </summary>
        /// <param name="numeroBase"></param>
        /// <param name="digitoVerificador"></param>
        /// <returns></returns>
        private static bool CalcularDigitoVerificador(string numeroBase, string digitoVerificador)
        {
            int[] pesos = { 2, 3, 4, 5, 6, 7, 8, 9 };
            int soma = 0;

            for (int i = 0; i < numeroBase.Length; i++)
            {
                if (!char.IsDigit(numeroBase[i]))
                    return false;

                int digito = int.Parse(numeroBase[i].ToString());
                soma += digito * pesos[pesos.Length - 1 - i];
            }

            int resto = soma % 11;
            int digitoCalculado;

            if (resto == 10 || resto == 11)
            {
                digitoCalculado = 0;
            }
            else
            {
                digitoCalculado = resto;
            }

            if (string.IsNullOrEmpty(digitoVerificador))
            {
                return true; //Se não houver dígito verificador, considera válido apenas se for RG com 8 dígitos
            }

            if (int.Parse(digitoVerificador) == digitoCalculado)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        public string GetErrorMessage() => $"RG Inválido";

    }
}
