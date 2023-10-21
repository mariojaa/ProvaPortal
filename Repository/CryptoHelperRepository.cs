using ProvaPortal.Repository.Interface;
using System.Security.Cryptography;

namespace ProvaPortal.Repository
{
    public class CryptoHelperRepository : ICryptoHelperRepository
    {
        public byte[] GerarChaveDeCriptografia()
        {
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.GenerateKey();
                return aesAlg.Key;
            }
        }

        public byte[] GerarVetorDeInicializacao()
        {
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.GenerateIV();
                return aesAlg.IV;
            }
        }

        public void CriptografarArquivo(Stream entrada, Stream saida, byte[] chave, byte[] vetorDeInicializacao)
        {
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = chave;
                aesAlg.IV = vetorDeInicializacao;

                using (var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                using (var cryptoStream = new CryptoStream(saida, encryptor, CryptoStreamMode.Write))
                {
                    entrada.CopyTo(cryptoStream);
                }
            }
        }

        public void DescriptografarArquivo(Stream entrada, Stream saida, byte[] chave, byte[] vetorDeInicializacao)
        {
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = chave;
                aesAlg.IV = vetorDeInicializacao;

                using (var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                using (var cryptoStream = new CryptoStream(entrada, decryptor, CryptoStreamMode.Read))
                {
                    cryptoStream.CopyTo(saida);
                }
            }
        }
    }
}
