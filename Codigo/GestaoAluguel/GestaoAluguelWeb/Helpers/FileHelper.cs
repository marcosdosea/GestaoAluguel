using Microsoft.AspNetCore.Http;
using System.Text;

namespace GestaoAluguelWeb.Helpers
{
    public class FileHelper
    {

        /// <summary>
        /// Enum para representar os tipos de arquivo suportados.
        /// </summary>
        public enum FileType
        {
            Unknown,
            Pdf,
            Jpeg,
            Png,
            Gif,
            Bmp,
            Docx
        }

        // Dicionário com as assinaturas de bytes para cada tipo de arquivo.
        private static readonly Dictionary<FileType, List<byte[]>> _signatures = new()
    {
        { FileType.Pdf, new List<byte[]> { new byte[] { 0x25, 0x50, 0x44, 0x46 } } }, // %PDF
        { FileType.Jpeg, new List<byte[]>
            {
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xDB }
            }
        },
        { FileType.Png, new List<byte[]> { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } } },
        { FileType.Gif, new List<byte[]> { new byte[] { 0x47, 0x49, 0x46, 0x38 } } },
        { FileType.Bmp, new List<byte[]> { new byte[] { 0x42, 0x4D } } },
        // Arquivos .docx, .xlsx, .pptx são baseados em ZIP, que começa com "PK"
        { FileType.Docx, new List<byte[]> { new byte[] { 0x50, 0x4B, 0x03, 0x04 } } }
    };

        // Mapeamento de FileType para seu MIME type correspondente, essencial para o Data URL.
        private static readonly Dictionary<FileType, string> _mimeTypes = new()
    {
        { FileType.Pdf, "application/pdf" },
        { FileType.Jpeg, "image/jpeg" },
        { FileType.Png, "image/png" },
        { FileType.Gif, "image/gif" },
        { FileType.Bmp, "image/bmp" },
        { FileType.Docx, "application/vnd.openxmlformats-officedocument.wordprocessingml.document" }
    };

        /// <summary>
        /// Identifica o tipo de arquivo com base em sua assinatura de bytes.
        /// </summary>
        public static FileType GetFileType(IFormFile file)
        {
            if (file == null || file.Length == 0) return FileType.Unknown;
            using var stream = file.OpenReadStream();
            return GetFileType(stream);
        }

        public static FileType GetFileType(Stream stream)
        {
            if (stream == null || stream.Length == 0 || !stream.CanRead) return FileType.Unknown;
            stream.Position = 0;
            var maxSignatureLength = _signatures.Values.SelectMany(s => s).Max(m => m.Length);
            var headerBytes = new byte[maxSignatureLength];
            stream.Read(headerBytes, 0, maxSignatureLength);
            foreach (var (type, signatures) in _signatures)
            {
                foreach (var signature in signatures)
                {
                    if (headerBytes.Take(signature.Length).SequenceEqual(signature))
                    {
                        stream.Position = 0;
                        return type;
                    }
                }
            }
            stream.Position = 0;
            return FileType.Unknown;
        }

        /// <summary>
        /// NOVO: Verifica se o tipo real de um arquivo está dentro de uma lista de tipos permitidos.
        /// </summary>
        /// <param name="file">O IFormFile a ser validado.</param>
        /// <param name="allowedTypes">Uma lista de FileTypes que são permitidos.</param>
        /// <returns>Verdadeiro se o tipo do arquivo for permitido, falso caso contrário.</returns>
        public static bool IsValid(IFormFile file, params FileType[] allowedTypes)
        {
            if (allowedTypes == null || allowedTypes.Length == 0)
            {
                return false; // Se nada é permitido, nada é válido.
            }

            var actualType = GetFileType(file);
            return allowedTypes.Contains(actualType);
        }

        public static bool IsValid(Byte[] file, params FileType[] allowedTypes)
        {
            if (allowedTypes == null || allowedTypes.Length == 0)
            {
                return false; // Se nada é permitido, nada é válido.
            }

            var actualType = GetFileType(file);
            return allowedTypes.Contains(actualType);
        }

        /// <summary>
        /// NOVO: Gera um Data URL (string Base64) para um arquivo, pronto para ser usado no atributo 'src' de tags HTML.
        /// </summary>
        /// <param name="file">O arquivo a ser convertido.</param>
        /// <returns>Uma string Data URL ou null se o tipo de arquivo for desconhecido.</returns>
        public static async Task<string?> GetDataUrlAsync(IFormFile file)
        {
            var fileType = GetFileType(file);

            if (fileType == FileType.Unknown || !_mimeTypes.TryGetValue(fileType, out var mimeType))
            {
                return null; // Retorna nulo se não soubermos o tipo ou o MIME type
            }

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray();
                var base64String = Convert.ToBase64String(fileBytes);
                return $"data:{mimeType};base64,{base64String}";
            }
        }

        /// <summary>
        /// NOVO: Identifica o tipo de arquivo a partir de um array de bytes.
        /// </summary>
        /// <param name="fileBytes">O conteúdo do arquivo como byte[].</param>
        /// <returns>O FileType correspondente.</returns>
        public static FileType GetFileType(byte[] fileBytes)
        {
            if (fileBytes == null || fileBytes.Length == 0)
            {
                return FileType.Unknown;
            }
            // Reutiliza a lógica principal convertendo o byte[] para um MemoryStream
            using (var stream = new MemoryStream(fileBytes))
            {
                return GetFileType(stream);
            }
        }

        /// <summary>
        /// NOVO: Gera um Data URL (string Base64) a partir de um array de bytes.
        /// </summary>
        /// <param name="fileBytes">O conteúdo do arquivo como byte[].</param>
        /// <returns>Uma string Data URL ou null se o tipo de arquivo for desconhecido.</returns>
        public static string? GetDataUrl(byte[] fileBytes)
        {
            if (fileBytes == null || fileBytes.Length == 0)
            {
                return null;
            }

            var fileType = GetFileType(fileBytes);

            if (fileType == FileType.Unknown || !_mimeTypes.TryGetValue(fileType, out var mimeType))
            {
                return null;
            }

            var base64String = Convert.ToBase64String(fileBytes);
            return $"data:{mimeType};base64,{base64String}";
        }

        public static async Task<byte[]?> ConverterParaBytes(IFormFile file)
        {
            if (file == null) return null;

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

    }

}

