namespace TesteFacil.Dominio.ModuloTeste;

public interface IGeradorTeste
{
    byte[] GerarNovoTeste(Teste teste, bool gabarito);
}