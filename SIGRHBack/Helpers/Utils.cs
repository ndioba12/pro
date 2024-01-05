namespace SIGRHBack.Helpers
{
    public static class Utils
    {
        /// <summary>
        /// Formatter un nombre avec un format de cinq caractères remplie à gauche par 0.
        /// </summary>
        /// <param name="val"></param>
        /// <returns>une chaine de caractères avec 5 caractères remplie de 0</returns>
        public static string FormatReference(this long val)
        {
            var resultStr = val.ToString();
            var result = resultStr.PadLeft(5, '0');
            return result;
        }
        /// <summary>
        /// Convertir un fichier quelconque en tableau de bytes.
        /// </summary>
        /// <param name="file">fichier à convertir </param>
        /// <returns>Tableau de bytes</returns>
        public static async Task<byte[]> ConvertToBytes(IFormFile file)
        {
            var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            return stream.ToArray();
        }
    }
}
