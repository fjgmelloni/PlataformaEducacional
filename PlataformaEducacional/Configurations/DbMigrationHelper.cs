using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlataformaEducacional.Api.Data;
using PlataformaEducacional.GestaoAluno.Data;
using PlataformaEducacional.GestaoAluno.Domain;
using PlataformaEducacional.GestaoConteudo.Data;
using PlataformaEducacional.GestaoConteudo.Domain;
using PlataformaEducacional.GestaoConteudo.Domain.ValueObjects;
using PlataformaEducacional.GestaoFinanceira.Business;
using PlataformaEducacional.GestaoFinanceira.Data;

namespace PlataformaEducacional.Configurations
{
    public static class DbMigrationHelperExtension
    {
        public static void UseDbMigrationHelper(this WebApplication app)
        {
            DbMigrationHelper.EnsureSeedData(app).Wait();
        }
    }
    public static class DbMigrationHelper
    {
        public static async Task EnsureSeedData(WebApplication application)
        {
            var service = application.Services.CreateScope().ServiceProvider;
            await EnsureSeedData(service);
        }
        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            var identityContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            var conteudoContext = scope.ServiceProvider.GetRequiredService<GestaoConteudoContext>();
            var alunoContext = scope.ServiceProvider.GetRequiredService<GestaoAlunoContext>();
            var pagamentoContext = scope.ServiceProvider.GetRequiredService<PagamentoContext>();

            if (env.EnvironmentName == "Development" || env.EnvironmentName == "Testing")
            {
                await identityContext.Database.MigrateAsync();
                await conteudoContext.Database.MigrateAsync();
                await alunoContext.Database.MigrateAsync();
                await pagamentoContext.Database.MigrateAsync();

                await SeedUserAndRoles(identityContext);
                await SeedTablesGestaoConteudo(conteudoContext);
                await SeedTablesGestaoAluno(identityContext, conteudoContext, alunoContext, pagamentoContext);
            }
        }

        private static async Task SeedUserAndRoles(ApplicationContext contextIdentity)
        {
            if (contextIdentity.Users.Any()) return;

            var roleAdmin = new IdentityRole
            {
                Name = "ADMIN",
                NormalizedName = "ADMIN"
            };
            var roleAluno = new IdentityRole
            {
                Name = "ALUNO",
                NormalizedName = "ALUNO"
            };

            await contextIdentity.Roles.AddAsync(roleAdmin);
            await contextIdentity.Roles.AddAsync(roleAluno);

            var idAdmin = Guid.NewGuid();
            var usuarioAdmin = new IdentityUser
            {
                Id = idAdmin.ToString(),
                Email = "admin@teste.com",
                EmailConfirmed = true,
                NormalizedEmail = "ADMIN@TESTE.COM",
                UserName = "admin@teste.com",
                AccessFailedCount = 0,
                PasswordHash = "AQAAAAIAAYagAAAAEF/nmfwFGPa8pnY9AvZL8HKI7r7l+aM4nryRB+Y3Ktgo6d5/0d25U2mhixnO4h/K5w==",
                NormalizedUserName = "ADMIN@TESTE.COM"
            };
            await contextIdentity.Users.AddAsync(usuarioAdmin);

            await contextIdentity.UserRoles.AddAsync(new IdentityUserRole<string>
            {
                RoleId = roleAdmin.Id,
                UserId = usuarioAdmin.Id
            });

            var idAluno = Guid.NewGuid();
            var usuarioAluno = new IdentityUser
            {
                Id = idAluno.ToString(),
                Email = "aluno@teste.com",
                EmailConfirmed = true,
                NormalizedEmail = "ALUNO@TESTE.COM",
                UserName = "aluno@teste.com",
                AccessFailedCount = 0,
                PasswordHash = "AQAAAAIAAYagAAAAEF/nmfwFGPa8pnY9AvZL8HKI7r7l+aM4nryRB+Y3Ktgo6d5/0d25U2mhixnO4h/K5w==",
                NormalizedUserName = "ALUNO@TESTE.COM"
            };
            await contextIdentity.Users.AddAsync(usuarioAluno);
            await contextIdentity.UserRoles.AddAsync(new IdentityUserRole<string>
            {
                RoleId = roleAluno.Id,
                UserId = usuarioAluno.Id
            });

            await contextIdentity.SaveChangesAsync();
        }

        private static async Task SeedTablesGestaoConteudo(GestaoConteudoContext conteudoContext)
        {
            if (!conteudoContext.Cursos.Any())
            {
                var curso = new Curso(".NET", new ConteudoProgramatico("Conteudo do Curso", 30), 500, true);
                for (int i = 1; i <= 5; i++)
                {
                    var aula = new Aula($"Aula {i}", $"Conteudo da Aula {i}", i, $"Segue link dos materiais da aula {i}");
                    curso.AdicionarAula(aula);
                }
                await conteudoContext.Cursos.AddAsync(curso);

                curso = new Curso(".NET Core", new ConteudoProgramatico("Conteudo do Curso de .NET Core", 30), 500, true);
                for (int i = 1; i <= 1; i++)
                {
                    var aula = new Aula($"Aula {i}", $"Conteudo da Aula {i}", i, $"Segue link dos materiais da aula {i}");
                    curso.AdicionarAula(aula);
                }
                await conteudoContext.Cursos.AddAsync(curso);

                curso = new Curso("Dominios Ricos", new ConteudoProgramatico("Conteudo do Curso de Dominios Ricos", 30), 500, true);
                for (int i = 1; i <= 1; i++)
                {
                    var aula = new Aula($"Aula {i}", $"Conteudo da Aula {i}", i, $"Segue link dos materiais da aula {i}");
                    curso.AdicionarAula(aula);
                }
                await conteudoContext.Cursos.AddAsync(curso);

                await conteudoContext.SaveChangesAsync();
            }
        }

        private static async Task SeedTablesGestaoAluno(ApplicationContext contextIdentity, GestaoConteudoContext conteudoContext, GestaoAlunoContext alunoContext, PagamentoContext pagamentoContext)
        {
            if (!alunoContext.Matriculas.Any())
            {
                var userAluno = await contextIdentity.Users.FirstOrDefaultAsync(x => x.Email == "aluno@teste.com");
                var curso = await conteudoContext.Cursos.Include(c => c.Aulas).FirstOrDefaultAsync(c => c.Nome == ".NET");
                var cursoCore = await conteudoContext.Cursos.Include(c => c.Aulas).FirstOrDefaultAsync(c => c.Nome == ".NET Core");
                var cursoDominiosRicos = await conteudoContext.Cursos.Include(c => c.Aulas).FirstOrDefaultAsync(c => c.Nome == "Dominios Ricos");

                var aluno = new Aluno(Guid.Parse(userAluno!.Id), "Aluno ");
                var matricula = new Matricula(curso!.Id, curso.Nome, curso.Aulas.Count, 500);
                var matriculaCore = new Matricula(cursoCore!.Id, cursoCore.Nome, cursoCore.Aulas.Count, 500);
                var matriculaDominiosRicos = new Matricula(cursoDominiosRicos!.Id, cursoDominiosRicos.Nome, cursoDominiosRicos.Aulas.Count, 500);
                aluno.RealizarMatricula(matricula);
                aluno.RealizarMatricula(matriculaCore);
                aluno.RealizarMatricula(matriculaDominiosRicos);

                var pagamento = new Pagamento(matricula.Id, curso.Valor, new DadosCartao("RINALDO", "111222333444", "05/27", "123"));
                var pagamentoCore = new Pagamento(matriculaCore.Id, cursoCore.Valor, new DadosCartao("RINALDO", "111222333444", "05/27", "123"));
                var pagamentoDominiosRicos = new Pagamento(matriculaDominiosRicos.Id, cursoDominiosRicos.Valor, new DadosCartao("RINALDO", "111222333444", "05/27", "123"));
                var transacao = new Transacao(pagamento.Id, pagamento.Valor);
                var transacaoCore = new Transacao(pagamentoCore.Id, pagamentoCore.Valor);
                var transacaoDominiosRicos = new Transacao(pagamentoDominiosRicos.Id, pagamentoDominiosRicos.Valor);
                transacao.AlterarStatus(StatusTransacao.Pago);
                transacaoCore.AlterarStatus(StatusTransacao.Pago);
                transacaoDominiosRicos.AlterarStatus(StatusTransacao.Pago);

                matricula.Ativar();
                matriculaCore.Ativar();
                matriculaDominiosRicos.Ativar();

                foreach (var aula in curso.Aulas)
                {
                    var progresso = new ProgressoAula(aula.Id);
                    matricula.RegistrarAula(progresso);
                }
                matricula.FinalizarCurso();

                foreach (var aula in cursoDominiosRicos.Aulas)
                {
                    var progresso = new ProgressoAula(aula.Id);
                    matriculaDominiosRicos.RegistrarAula(progresso);
                }

                await alunoContext.Alunos.AddAsync(aluno);

                var certificado = new Certificado(matricula.Id);
                await alunoContext.Certificados.AddAsync(certificado);

                await pagamentoContext.Pagamentos.AddAsync(pagamento);
                await pagamentoContext.Transacoes.AddAsync(transacao);
                await pagamentoContext.Pagamentos.AddAsync(pagamentoCore);
                await pagamentoContext.Transacoes.AddAsync(transacaoCore);
                await pagamentoContext.Pagamentos.AddAsync(pagamentoDominiosRicos);
                await pagamentoContext.Transacoes.AddAsync(transacaoDominiosRicos);
                await pagamentoContext.SaveChangesAsync();
                await alunoContext.SaveChangesAsync();
            }



        }
    }
}
