using System.ComponentModel.DataAnnotations;

namespace TesteFacil.Dominio.ModuloMateria;

public enum SerieMateria
{
    //Fundamental
    [Display(Name = "Primeira Série")] PrimeiraSerie = 1,
    [Display(Name = "Segunda Série")] SegundaSerie = 2,
    [Display(Name = "Terceira Série")] TerceiraSerie = 3,
    [Display(Name = "Quarta Série")] QuartaSerie = 4,
    [Display(Name = "Quinta Série")] QuintaSerie = 5,
    [Display(Name = "Sexta Série")] SextaSerie = 6,
    [Display(Name = "Sétima Série")] SetimaSerie = 7,
    [Display(Name = "Oitava Série")] OitavaSerie = 8,
    [Display(Name = "Nona Série")] NonaSerie = 9,

    //Ensino Médio
    [Display(Name = "Primeiro Ano")] PrimeiroAno = 10,
    [Display(Name = "Segundo Ano")] SegundoAno = 11,
    [Display(Name = "Terceiro Ano")] TerceiroAno = 12,

    //Ensino Superior
    [Display(Name = "Primeira Fase")] PrimeiraFase = 13,
    [Display(Name = "Segunda Fase")] SegundaFase = 14,
    [Display(Name = "Terceira Fase")] TerceiraFase = 15,
    [Display(Name = "Quarta Fase")] QuartaFase = 16,
    [Display(Name = "Quinta Fase")] QuintaFase = 17,
    [Display(Name = "Sexta Fase")] SextaFase = 18,
    [Display(Name = "Sétima Fase")] SetimaFase = 19,
    [Display(Name = "Oitava Fase")] OitavaFase = 20,
    [Display(Name = "Nona Fase")] NonaFase = 21,
    [Display(Name = "Décima Fase")] DecimaFase = 22
}