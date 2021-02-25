CREATE TABLE [dbo].[Filmid]
(
	[Id] INT NOT NULL PRIMARY KEY identity (1,1), 
    [Nimi] VARCHAR(50) NULL, 
    [Aeg] DATETIME NULL, 
    [Pilet] MONEY NULL, 
    [Kirjeldus] TEXT NULL,
	
)
