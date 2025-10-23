using MediatR;
using PlataformaEducacao.Api.Extensions;
using PlataformaEducacao.Core.Communication.Mediator;
using PlataformaEducacao.Core.DomainObjects;
using PlataformaEducacao.Core.Messages.CommonMessages.IntegrationEvents;
using PlataformaEducacao.Core.Messages.CommonMessages.Notifications;
using PlataformaEducacao.GestaoAluno.Application.Commands.AdicionarAluno;
using PlataformaEducacao.GestaoAluno.Application.Commands.CancelarMatricula;
using PlataformaEducacao.GestaoAluno.Application.Commands.FinalizarCurso;
using PlataformaEducacao.GestaoAluno.Application.Commands.FinalizarMatricula;
using PlataformaEducacao.GestaoAluno.Application.Commands.GerarCertificado;
using PlataformaEducacao.GestaoAluno.Application.Commands.MatricularAlunoCurso;
using PlataformaEducacao.GestaoAluno.Application.Commands.PagamentoMatricula;
using PlataformaEducacao.GestaoAluno.Application.Commands.RealizarAula;
using PlataformaEducacao.GestaoAluno.Application.Events;
using PlataformaEducacao.GestaoAluno.Application.Queries;
using PlataformaEducacao.GestaoAluno.Data.Repository;
using PlataformaEducacao.GestaoAluno.Domain.Repositories;
using PlataformaEducacao.GestaoConteudo.Application.Commands;
using PlataformaEducacao.GestaoConteudo.Application.Queries;
using PlataformaEducacao.GestaoConteudo.Data.Repository;
using PlataformaEducacao.GestaoConteudo.Domain;
using PlataformaEducacao.GestaoFinanceira.AntiCorruption;
using PlataformaEducacao.GestaoFinanceira.Business;
using PlataformaEducacao.GestaoFinanceira.Business.Events;
using PlataformaEducacao.GestaoFinanceira.Data;
using PlataformaEducacao.GestaoFinanceira.Data.Repository;
using Financeira = PlataformaEducacao.GestaoFinanceira.AntiCorruption;

namespace PlataformaEducacional.Configurations
{
    public static class DependencyInjectingConfig
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            // Mediator
            services.AddMediatR(
               typeof(AdicionarAulaCommand).Assembly,
               typeof(CursoCommandHandler).Assembly
           );

            services.AddScoped<IMediatorHandler, MediatorHandler>();

            // Notifications
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            // Gestão de Conteúdos
            services.AddScoped<ICursoRepository, CursoRepository>();
            services.AddScoped<ICursoQueries, CursoQueries>();

            services.AddScoped<IRequestHandler<AdicionarAulaCommand, bool>, CursoCommandHandler>();
            services.AddScoped<IRequestHandler<AdicionarCursoCommand, bool>, CursoCommandHandler>();
            services.AddScoped<IRequestHandler<AtualizarCursoCommand, bool>, CursoCommandHandler>();

            // Gestão de Alunos
            services.AddScoped<IAlunoRepository, AlunoRepository>();
            services.AddScoped<IAlunoQueries, AlunoQueries>();

            services.AddScoped<IRequestHandler<AdicionarAlunoCommand, bool>, AdicionarAlunoCommadHandler>();
            services.AddScoped<IRequestHandler<FinalizarMatriculaCommand, bool>, FinalizarMatriculaHandler>();
            services.AddScoped<IRequestHandler<MatricularAlunoCursoCommand, bool>, MatricularAlunoCursoHandler>();
            services.AddScoped<IRequestHandler<PagamentoMatriculaCommand, bool>, PagamentoMatriculaHandler>();
            services.AddScoped<IRequestHandler<CancelarMatriculaCommand, bool>, CancelarMatriculaCommandHandler>();
            services.AddScoped<IRequestHandler<RealizarAulaCommand, bool>, RealizarAulaCommandHandler>();
            services.AddScoped<IRequestHandler<FinalizarCursoCommand, bool>, FinalizarCursoCommandHandler>();
            services.AddScoped<IRequestHandler<GerarCertificadoCommand, bool>, GerarCertificadoCommandHandler>();

            services.AddScoped<INotificationHandler<MatriculaPagamentoRealizadoEvent>, MatriculaEventHandler>();
            services.AddScoped<INotificationHandler<MatriculaPagamentoRecusadoEvent>, MatriculaEventHandler>();
            services.AddScoped<INotificationHandler<CursoFinalizadoEvent>, MatriculaEventHandler>();

            // Pagamento
            services.AddScoped<IPagamentoRepository, PagamentoRepository>();
            services.AddScoped<IPagamentoService, PagamentoService>();
            services.AddScoped<IPagamentoCartaoCreditoFacade, PagamentoCartaoCreditoFacade>();
            services.AddScoped<IPayPalGateway, PayPalGateway>();
            services.AddScoped<Financeira.IConfigurationManager, Financeira.ConfigurationManager>();
            services.AddScoped<INotificationHandler<IniciaPagamentoEvent>, PagamentoEventHandler>();
            services.AddScoped<PagamentoContext>();

            services.AddScoped<IAppIdentityUser, AppIdentityUser>();
            return services;
        }
    }
}
