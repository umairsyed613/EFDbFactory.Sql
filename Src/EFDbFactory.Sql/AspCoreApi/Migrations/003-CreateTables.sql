CREATE TABLE [Author](
    [Id] [integer] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](256) NOT NULL,
	[Email] [nvarchar](256) NULL,
    CONSTRAINT [PK_Author] PRIMARY KEY ([Id] ASC)
);

GO

CREATE TABLE [Book](
    [Id] [integer] IDENTITY(1,1) NOT NULL,
    [Title] [nvarchar](256) NOT NULL,
	[AuthorId] [int] NOT NULL,
    CONSTRAINT [PK_Book] PRIMARY KEY ([Id] ASC),
	CONSTRAINT [FK_Author_Book] FOREIGN KEY([AuthorId]) REFERENCES [Author] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [BookRating](
	[Id] [integer] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[BookId] [integer] NOT NULL,
	[UserId] [integer] NOT NULL,
	CONSTRAINT [FK_BookRating_Book] FOREIGN KEY([BookId]) REFERENCES [Book] ([Id]) ON DELETE CASCADE
);