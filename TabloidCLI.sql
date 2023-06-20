USE [master]

IF db_id('TabloidCLI') IS NULl
BEGIN
    CREATE DATABASE [TabloidCLI]
END;
GO

use [TabloidCLI]
go

DROP TABLE IF EXISTS Journal;
DROP TABLE IF EXISTS BlogTag;
DROP TABLE IF EXISTS PostTag;
DROP TABLE IF EXISTS AuthorTag;
DROP TABLE IF EXISTS Tag;
DROP TABLE IF EXISTS Note;
DROP TABLE IF EXISTS Post;
DROP TABLE IF EXISTS Blog;
DROP TABLE IF EXISTS Author;


CREATE TABLE Author (
    Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
    FirstName NVARCHAR(55) NOT NULL,
    LastName NVARCHAR(55) NOT NULL,
    Bio TEXT
);

CREATE TABLE Blog (
    Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
    Title NVARCHAR(55) NOT NULL,
    URL NVARCHAR(2000) NOT NULL
);

CREATE TABLE Post (
    Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
    Title NVARCHAR(55) NOT NULL,
    URL NVARCHAR(2000) NOT NULL,
    PublishDateTime DATETIME NOT NULL,
    AuthorId INTEGER NOT NULL,
    BlogId INTEGER NOT NULL,

    CONSTRAINT FK_Post_Author FOREIGN KEY(AuthorId) REFERENCES Author(Id),
    CONSTRAINT FK_Post_Blog FOREIGN KEY(BlogId) REFERENCES Blog(Id)
);

CREATE TABLE Note (
    Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
    Title NVARCHAR(55) NOT NULL,
    Content TEXT NOT NULL,
    CreateDateTime DATETIME NOT NULL,
    PostId INTEGER NOT NULL,
    
    CONSTRAINT FK_Note_Posti FOREIGN KEY(PostId) REFERENCES Post(Id)
    ON DELETE CASCADE
);

CREATE TABLE Tag (
    Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
    Name NVARCHAR(55) NOT NULL
);

CREATE TABLE AuthorTag (
    Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
    AuthorId INTEGER NOT NULL,
    TagId INTEGER NOT NULL,

    CONSTRAINT FK_AuthorTag_Author FOREIGN KEY(AuthorId) REFERENCES Author(Id),
    CONSTRAINT FK_AuthorTag_Tag FOREIGN KEY(TagId) REFERENCES Tag(Id)
);

CREATE TABLE PostTag (
    Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
    PostId INTEGER NOT NULL,
    TagId INTEGER NOT NULL,

    CONSTRAINT FK_PostTag_Post FOREIGN KEY(PostId) REFERENCES Post(Id) ON DELETE CASCADE,
    CONSTRAINT FK_PostTag_Tag FOREIGN KEY(TagId) REFERENCES Tag(Id)
);

CREATE TABLE BlogTag (
    Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
    BlogId INTEGER NOT NULL,
    TagId INTEGER NOT NULL,

    CONSTRAINT FK_BlogTag_Blog FOREIGN KEY(BlogId) REFERENCES Blog(Id),
    CONSTRAINT FK_BlogTag_Tag FOREIGN KEY(TagId) REFERENCES Tag(Id)
);

CREATE TABLE Journal (
    Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
    Title NVARCHAR(55) NOT NULL,
    Content TEXT NOT NULL,
    CreateDateTime DATETIME NOT NULL
);


INSERT INTO Author ( FirstName, LastName, Bio) VALUES ( 'Scott', 'Hanselman', '.net advocate. Works at Micrsoft' );
INSERT INTO Author ( FirstName, LastName, Bio) VALUES ( 'Eric',  'Elliott', 'Opinionated javascript developer' );
INSERT INTO Author ( FirstName, LastName, Bio) VALUES ( 'Felienne', 'Hermans', ' associate professor at the Leiden Institute of Advanced Computer Science at Leiden University, where head the PERL group that researches programming education');
INSERT INTO Author ( FirstName, LastName, Bio) VALUES ( 'Jake', 'Archibald', 'Javascript dev, developer relations at Google' );
INSERT INTO Author ( FirstName, LastName, Bio) VALUES ( 'Julie', 'Lerman', 'Entity Framework expert' );

INSERT INTO Blog (Title, URL) VALUES ( 'The Data Farm', 'https://thedatafarm.com/blog/' );
INSERT INTO Blog (Title, URL) VALUES ( '.NET Blog', 'https://devblogs.microsoft.com/dotnet/' );
INSERT INTO Blog (Title, URL) VALUES ( 'felienne.com', 'https://www.felienne.com/' );
INSERT INTO Blog (Title, URL) VALUES ( 'NETFLIX Tech Blog', 'https://netflixtechblog.com/' );
INSERT INTO Blog (Title, URL) VALUES ( 'jakearchibald.com', 'https://jakearchibald.com/' );
INSERT INTO Blog (Title, URL) VALUES ( 'Develop Together', 'https://dev.to/' );
INSERT INTO Blog (Title, URL) VALUES ( 'Scott Hanselman Blog', 'https://www.hanselman.com/blog/' );

INSERT INTO Post ( Title, URL, PublishDateTime, AuthorId, BlogId ) VALUES ('Forms of notional Cinnamon', 'https://www.felienne.com/archives/6392', '2019-07-12', 3, 3);
INSERT INTO Post ( Title, URL, PublishDateTime, AuthorId, BlogId ) VALUES ('Forms of notional Ketchup', 'https://www.felienne.com/archives/546984', '2019-07-12', 1, 4);
INSERT INTO Post ( Title, URL, PublishDateTime, AuthorId, BlogId ) VALUES ('Forms of notional Paprika', 'https://www.felienne.com/archives/6874', '2019-07-12', 2, 2);
INSERT INTO Post ( Title, URL, PublishDateTime, AuthorId, BlogId ) VALUES ('Forms of notional Mustard', 'https://www.felienne.com/archives/3527', '2019-07-12', 4, 1);
INSERT INTO Post ( Title, URL, PublishDateTime, AuthorId, BlogId ) VALUES ('Forms of notional Sand', 'https://www.felienne.com/archives/684856', '2019-07-12', 5, 5);
INSERT INTO Post ( Title, URL, PublishDateTime, AuthorId, BlogId ) VALUES ('Forms of notional Pepper', 'https://www.felienne.com/archives/789654', '2019-07-12', 1, 6);
INSERT INTO Post ( Title, URL, PublishDateTime, AuthorId, BlogId ) VALUES ('Forms of notional Salt', 'https://www.felienne.com/archives/32168', '2019-07-12', 3, 7);

INSERT INTO Note ( Title, Content, CreateDateTime, PostId ) VALUES ('Random', 'Educational', CURRENT_TIMESTAMP, 1);
INSERT INTO Note ( Title, Content, CreateDateTime, PostId ) VALUES ('Specific', 'Stupid', CURRENT_TIMESTAMP, 3);
INSERT INTO Note ( Title, Content, CreateDateTime, PostId ) VALUES ('Semi-Specific', 'Noneducational', CURRENT_TIMESTAMP, 2);
INSERT INTO Note ( Title, Content, CreateDateTime, PostId ) VALUES ('Not Random', 'Semi-Educational', CURRENT_TIMESTAMP, 5);

INSERT INTO Tag ( Name ) VALUES ( 'nerdy' );
INSERT INTO Tag ( Name ) VALUES ( 'cool' );
INSERT INTO Tag ( Name ) VALUES ( 'sucks' );
INSERT INTO Tag ( Name ) VALUES ( 'great' );


--INSERT INTO AuthorTag ( AuthorId, TagId) VALUES ( );

INSERT INTO PostTag ( PostId, TagId ) VALUES (1, 1 );
INSERT INTO PostTag ( PostId, TagId ) VALUES (5, 2 );
INSERT INTO PostTag ( PostId, TagId ) VALUES (6, 3 );
INSERT INTO PostTag ( PostId, TagId ) VALUES (7, 4 );

--INSERT INTO BlogTag ( BlogId, TagId ) VALUES ( );

INSERT INTO Journal ( Title, Content, CreateDateTime ) VALUES ( 'My Big Day', 'I had a big day today. Would you believe I saw a dog????', '2020-04-30' ) ;

