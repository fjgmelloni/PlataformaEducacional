public sealed class MatricularAlunoCommand : PlataformaEducacao.Core.Messages.Command
{
    public Guid AlunoId { get; }
    public Guid CursoId { get; }

    public MatricularAlunoCommand(Guid alunoId, Guid cursoId)
    {
        AlunoId = alunoId;
        CursoId = cursoId;
        AggregateId = alunoId;
    }
}

public sealed class MatricularAlunoHandler : MediatR.IRequestHandler<MatricularAlunoCommand, bool>
{
    public async Task<bool> Handle(MatricularAlunoCommand request, CancellationToken ct)
    {

        return await Task.FromResult(true);
    }
}
