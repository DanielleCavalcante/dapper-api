CREATE TABLE Alunos (
	Id INTEGER PRIMARY KEY IDENTITY(1,1),
	Nome VARCHAR(50) NOT NULL,
	Email VARCHAR(50) NOT NULL,
	DataNascimento DATETIME NOT NULL,
	Ativo BIT NOT NULL,
	DataCriacao DATETIME NOT NULL,
	Curso VARCHAR(50),
	Turma VARCHAR(50),
	Turno VARCHAR(50)
)