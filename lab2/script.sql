mysql -u root -p

CREATE DATABASE atelie_mode

CREATE TABLE Category(
	ID int AUTO_INCREMENT NOT NULL,
	Name char(50) NOT NULL,
	`desc` char(50) NOT NULL,
	PRIMARY KEY (ID)
);
INSERT INTO Category (Name, `desc`) VALUES
    ('Europe', 'europe'),
    ('Asia', 'asia');

SELECT * FROM Category;

CREATE TABLE atnic_ambasador(
	ID int AUTO_INCREMENT NOT NULL,
	country char(50) NOT NULL,
	region char(50) NOT NULL,
	square int NOT NULL,
	conID int NOT NULL,
	year int NOT NULL,
	PRIMARY KEY (ID)
);

INSERT INTO atnic_ambasador (country, region, square, conID, year) VALUES
    ('123', '13', 200, 1, 100),
    ('121233', '1323', 202, 2, 101);

	ALTER TABLE Category MODIFY Name CHAR(50) CHARACTER SET cp1251;