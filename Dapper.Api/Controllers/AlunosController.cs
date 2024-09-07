using Dapper.Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Dapper.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlunosController : ControllerBase
    {
        private readonly string _connectionString;

        public AlunosController(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        [HttpGet(Name = "BuscarAlunos")]
        public async Task<IActionResult> Get()
        {
            using var connection = new SqlConnection(_connectionString);
            var alunos = await connection.QueryAsync<Aluno>("SELECT * FROM Alunos");

            return Ok(alunos);
        }

        [HttpGet("{id}", Name = "BuscarAlunoPorId")]
        public async Task<IActionResult> Get(int id) 
        {
            using var connection = new SqlConnection(_connectionString);
            var aluno = await connection
                .QueryFirstOrDefaultAsync<Aluno>("SELECT * FROM Alunos WHERE Id = @id", new {id});

            if (aluno is null) 
                return NotFound();

            return Ok(aluno);
        }

        [HttpPost(Name = "CadastraAluno")]
        public async Task<IActionResult> Post([FromBody] Aluno aluno)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO Alunos (Nome, Email, DataNascimento, Ativo, DataCriacao, Curso,Turma, Turno)" +
                       "VALUES (@Nome, @Email, @DataNascimento, 1, GETDATE(), @Curso, @Turma, @Turno);" +
                       "SELECT CAST(SCOPE_IDENTITY() AS INT)";
            var id = await connection.ExecuteScalarAsync<int>(sql, aluno);
            aluno.Id = id;
            return CreatedAtRoute("BuscarAlunoPorId", new { id }, aluno);
        }

        [HttpPut("{id}", Name = "AtualizarAluno")]
        public async Task<IActionResult> Put(int id, [FromBody] Aluno aluno)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "@UPDATE Alunos SET Nome = @Nome, Email = @Email, DataNascimento = @DataNascimento, Curso = @Curso, Turma = @Turma, Turno = @Turno WHERE Id = @id;";
            var linhasAfetadas = await connection.ExecuteAsync(sql, new {id, aluno});

            if (linhasAfetadas == 0)
                return NotFound();

            return NoContent();
        }
    }
}
