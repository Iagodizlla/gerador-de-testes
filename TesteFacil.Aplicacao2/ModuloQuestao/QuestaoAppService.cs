using FluentResults;
using Microsoft.Extensions.Logging;
using TesteFacil.Aplicacao.Compartilhado;
using TesteFacil.Dominio.Compartilhado;
using TesteFacil.Dominio.ModuloMateria;
using TesteFacil.Dominio.ModuloQuestao;
using TesteFacil.Dominio.ModuloTeste;

namespace TesteFacil.Aplicacao.ModuloQuestao;

public class QuestaoAppService
{
    private readonly IRepositorioQuestao repositorioQuestao;
    private readonly IRepositorioTeste repositorioTeste;
    private readonly IUnitOfWork unitOfWork;
    private readonly ILogger<QuestaoAppService> logger;

    public QuestaoAppService(
        IRepositorioQuestao repositorioQuestao,
        IRepositorioTeste repositorioTeste,
        IUnitOfWork unitOfWork,
        ILogger<QuestaoAppService> logger
    )
    {
        this.repositorioQuestao = repositorioQuestao;
        this.repositorioTeste = repositorioTeste;
        this.unitOfWork = unitOfWork;
        this.logger = logger;
    }

    public Result Cadastrar(Questao questao)
    {
        var registros = repositorioQuestao.SelecionarRegistros();

        if (registros.Any(i => i.Enunciado.Equals(questao.Enunciado)))
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("Já existe uma questão registrada com este enunciado."));

        try
        {
            repositorioQuestao.Cadastrar(questao);

            unitOfWork.Commit();

            return Result.Ok();
        }
        catch (Exception ex)
        {
            unitOfWork.Rollback();

            logger.LogError(
                ex,
                "Ocorreu um erro durante o registro de {@Registro}.",
                questao
            );

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public Result Editar(Guid id, Questao questaoEditada)
    {
        var registros = repositorioQuestao.SelecionarRegistros();

        if (registros.Any(i => !i.Id.Equals(id) && i.Enunciado.Equals(questaoEditada.Enunciado)))
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("Já existe uma questão registrada com este enunciado."));

        try
        {
            repositorioQuestao.Editar(id, questaoEditada);

            unitOfWork.Commit();

            return Result.Ok();
        }
        catch (Exception ex)
        {
            unitOfWork.Rollback();

            logger.LogError(
                ex,
                "Ocorreu um erro durante a edição do registro {@Registro}.",
                questaoEditada
            );

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public Result Excluir(Guid id)
    {
        try
        {
            var testes = repositorioTeste.SelecionarRegistros();

            if (testes.Any(t => t.Questoes.Any(q => q.Id == id)))
            {
                var erro = ResultadosErro
                    .ExclusaoBloqueadaErro("A questão não pôde ser excluída pois está em um ou mais testes ativos.");

                return Result.Fail(erro);
            }

            repositorioQuestao.Excluir(id);

            unitOfWork.Commit();

            return Result.Ok();

        }
        catch (Exception ex)
        {
            unitOfWork.Rollback();

            logger.LogError(
                ex,
                "Ocorreu um erro durante a exclusão do registro {Id}.",
                id
            );

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public Result<Questao> SelecionarPorId(Guid id)
    {
        try
        {
            var registroSelecionado = repositorioQuestao.SelecionarRegistroPorId(id);

            if (registroSelecionado is null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(id));

            return Result.Ok(registroSelecionado);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ocorreu um erro durante a seleção do registro {Id}.",
                id
            );

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public Result<List<Questao>> SelecionarTodos()
    {
        try
        {
            var registros = repositorioQuestao.SelecionarRegistros();

            return Result.Ok(registros);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ocorreu um erro durante a seleção de registros."
            );

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public Result AdicionarAlternativaEmQuestao(Guid questaoId, string respostaAlternativa, bool alternativaCorreta)
    {
        var registroSelecionado = repositorioQuestao.SelecionarRegistroPorId(questaoId);

        if (registroSelecionado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(questaoId));

        if (registroSelecionado.Alternativas.Any(a => a.Resposta.Equals(respostaAlternativa)))
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("Já existe uma alternativa registrada com esta resposta."));

        if (alternativaCorreta && registroSelecionado.Alternativas.Any(a => a.Correta))
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("Já existe uma alternativa registrada como correta."));

        try
        {
            registroSelecionado.AdicionarAlternativa(respostaAlternativa, alternativaCorreta);

            unitOfWork.Commit();

            return Result.Ok();
        }
        catch (Exception ex)
        {
            unitOfWork.Rollback();

            logger.LogError(
                ex,
                "Ocorreu um erro durante a edição do registro {@Registro}.",
                registroSelecionado
            );

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public Result RemoverAlternativaDeQuestao(char letra, Guid questaoId)
    {
        var registro = repositorioQuestao.SelecionarRegistroPorId(questaoId);

        if (registro is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(questaoId));

        try
        {
            registro.RemoverAlternativa(letra);

            unitOfWork.Commit();

            return Result.Ok();
        }
        catch (Exception ex)
        {
            unitOfWork.Rollback();

            logger.LogError(
                ex,
                "Ocorreu um erro durante a edição do registro {@Registro}.",
                registro
            );

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}