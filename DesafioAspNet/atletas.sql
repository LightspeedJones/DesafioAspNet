CREATE TABLE atletas(
	id INT NOT NULL PRIMARY KEY,
	nome VARCHAR(100),
	apelido VARCHAR(50),
	nascimento DATETIME,
	altura NUMERIC(15,2),
	peso NUMERIC(15, 2),
	posicao VARCHAR(100),
	camisa int
)