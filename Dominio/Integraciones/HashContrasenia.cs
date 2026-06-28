using System;
using System.Security.Cryptography;

namespace Integraciones
{
    /// <summary>
    /// Hashing de contraseñas con PBKDF2 (SHA-256) usando la API nativa de
    /// System.Security.Cryptography (no requiere paquetes externos).
    /// El valor almacenado es autocontenido con el formato
    /// "iteraciones.saltBase64.hashBase64": el salt (aleatorio por usuario) y
    /// el costo viajan junto al hash, de modo que se puede verificar -y migrar
    /// el costo a futuro- sin columnas adicionales en la base de datos.
    /// Las contraseñas NO se pueden recuperar, solo verificar.
    /// </summary>
    public static class HashContrasenia
    {
        private const int Iteraciones = 100000;
        private const int TamanioSaltBytes = 16;
        private const int TamanioHashBytes = 32;
        private static readonly HashAlgorithmName Algoritmo = HashAlgorithmName.SHA256;

        /// <summary>
        /// Genera un hash autocontenido (salt + costo + hash) para guardar en
        /// AccesoUsuarios.Pass. Cada llamada usa un salt aleatorio nuevo, por lo
        /// que dos usuarios con la misma contraseña producen hashes distintos.
        /// </summary>
        public static string Hashear(string contrasenia)
        {
            if (contrasenia == null)
                throw new ArgumentNullException(nameof(contrasenia));

            byte[] salt = new byte[TamanioSaltBytes];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            byte[] hash = Derivar(contrasenia, salt, Iteraciones, TamanioHashBytes);

            return string.Join(".",
                Iteraciones.ToString(),
                Convert.ToBase64String(salt),
                Convert.ToBase64String(hash));
        }

        /// <summary>
        /// Verifica una contraseña en texto plano contra el hash almacenado.
        /// Devuelve false ante cualquier formato invalido o si no coincide.
        /// </summary>
        public static bool Verificar(string contrasenia, string hashAlmacenado)
        {
            if (string.IsNullOrEmpty(contrasenia) || string.IsNullOrEmpty(hashAlmacenado))
                return false;

            string[] partes = hashAlmacenado.Split('.');
            if (partes.Length != 3)
                return false;

            if (!int.TryParse(partes[0], out int iteraciones) || iteraciones <= 0)
                return false;

            byte[] salt;
            byte[] hashEsperado;
            try
            {
                salt = Convert.FromBase64String(partes[1]);
                hashEsperado = Convert.FromBase64String(partes[2]);
            }
            catch (FormatException)
            {
                return false;
            }

            byte[] hashCalculado = Derivar(contrasenia, salt, iteraciones, hashEsperado.Length);

            return ComparacionSegura(hashCalculado, hashEsperado);
        }

        private static byte[] Derivar(string contrasenia, byte[] salt, int iteraciones, int tamanio)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(contrasenia, salt, iteraciones, Algoritmo))
            {
                return pbkdf2.GetBytes(tamanio);
            }
        }

        // Comparacion en tiempo constante: no corta al primer byte distinto
        // para no filtrar informacion por timing.
        private static bool ComparacionSegura(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;

            int diferencia = 0;
            for (int i = 0; i < a.Length; i++)
                diferencia |= a[i] ^ b[i];

            return diferencia == 0;
        }
    }
}
