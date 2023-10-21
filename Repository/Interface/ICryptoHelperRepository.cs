namespace ProvaPortal.Repository.Interface
{
    public interface ICryptoHelperRepository
    {
        byte[] GerarChaveDeCriptografia();
        byte[] GerarVetorDeInicializacao();
        void CriptografarArquivo(Stream entrada, Stream saida, byte[] chave, byte[] vetorDeInicializacao);
        void DescriptografarArquivo(Stream entrada, Stream saida, byte[] chave, byte[] vetorDeInicializacao);
    }
}
