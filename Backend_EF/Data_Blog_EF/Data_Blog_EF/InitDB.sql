DROP TABLE Usersdata
CREATE TABLE Usersdata
(
	ID UNIQUEIDENTIFIER,
	Name VARCHAR(40),
	Email VARCHAR(100),
	Password VARCHAR(100),
	Authenticated BIT,
	Access BIT, 
	IncognitoMode BIT
);
INSERT INTO Usersdata(ID, Name, Email, Password, Authenticated, Access, IncognitoMode) VALUES 
('A08AB3E5-E3EC-47CD-84EF-C0EB75045A70', 'Admin','maximkirichenk0.06@gmail.com','1fasdfasd4752', 1, 1, 0),
('0273419B-FD78-4EEC-8CB9-097B549F8789', 'Maxim','hatemtbofferskin@gmail.com','root', 1, 1, 0),
('B560A785-C146-465A-9EEB-A8E588BA023E', 'matvey','matvey@gmail.com','matvey', 1, 0, 0),
('68F02B91-31C9-432C-AE46-22DAB576F4C6', 'FAQ','faqblogms@gmail.com','128asfdas2Hasf234_', 1, 1, 0),
('BABF30BF-B436-46C0-B452-39FCC16E27EC', 'msi','msi@gmail.com','msi', 1, 0, 0)

DROP TABLE Newsdata
CREATE TABLE Newsdata
(
	ID UNIQUEIDENTIFIER,
	PostID UNIQUEIDENTIFIER,
	UserID UNIQUEIDENTIFIER,
	GroupName NVARCHAR(100),
	Body NVARCHAR(MAX),
	Likes INT,
	BindID UNIQUEIDENTIFIER
);
INSERT INTO Newsdata(BindID, ID, PostID, UserID, GroupName, Body, Likes) VALUES
('e33479d3-f5a3-490d-8456-577a73d99a68' ,'3eb61090-b070-48ab-afd0-02d2e70d9220', '49896593-D1F4-4632-A06F-7293D5D1AEB0', 'A08AB3E5-E3EC-47CD-84EF-C0EB75045A70', 'Astralis', 'hello', 100),
('78e136e6-2a34-4c14-bc6e-0db7988534c6', '00889CBA-1669-4A8B-B0E8-3440F0A12830', '68F02B91-31C9-432C-AE46-22DAB576F4C6', 'A08AB3E5-E3EC-47CD-84EF-C0EB75045A70',  'PMFTE', 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industrys standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.', 100)


DROP TABLE UserNodeLike
CREATE TABLE UserNodeLike
(
	ID INT IDENTITY(1,1),
	PostNewsID UNIQUEIDENTIFIER,
	UserID UNIQUEIDENTIFIER,
	PRIMARY KEY(ID)
);
INSERT INTO UserNodeLike(PostNewsID, UserID) VALUES
('49896593-D1F4-4632-A06F-7293D5D1AEB0', 'A08AB3E5-E3EC-47CD-84EF-C0EB75045A70'),
('68F02B91-31C9-432C-AE46-22DAB576F4C6', 'A08AB3E5-E3EC-47CD-84EF-C0EB75045A70')

DROP TABLE Messagedata
CREATE TABLE Messagedata
(
	ID UNIQUEIDENTIFIER,
	UserID UNIQUEIDENTIFIER,
	Email VARCHAR(100),
	Message VARCHAR(500),
	ToEmail VARCHAR(100)
)
INSERT INTO Messagedata(ID, UserID, Email, Message, ToEmail) VALUES
('BDD225C9-F419-4B29-BB1F-8BD387CFC073', 'A08AB3E5-E3EC-47CD-84EF-C0EB75045A70', 'hatemtbofferskin@gmail.com','hello to admin!','maximkirichenk0.06@gmail.com')


DROP TABLE Notes
CREATE TABLE Notes
(
	IdNote UNIQUEIDENTIFIER,
	ID UNIQUEIDENTIFIER,
	Title VARCHAR(500),
	Body VARCHAR(500)
)
INSERT INTO Notes(ID, IdNote, Title, Body) VALUES
('0273419B-FD78-4EEC-8CB9-097B549F8789', 'EA157230-5202-421E-95B5-D52C875DE833','First note', 'It`s the first note!'),
('0273419B-FD78-4EEC-8CB9-097B549F8789', 'F5AD0E5C-17B0-42A0-920A-9A2442A23E51','commit', 'commit to branch master'),
('BABF30BF-B436-46C0-B452-39FCC16E27EC', 'BB4929D2-29FE-4E2C-9EB9-3901909675EA','First note', 'first note from msi'),
('BABF30BF-B436-46C0-B452-39FCC16E27EC', '49896593-D1F4-4632-A06F-7293D5D1AEB0','Second note', 'second note from msi'),
('BABF30BF-B436-46C0-B452-39FCC16E27EC', 'BDD225C9-F419-4B29-BB1F-8BD387CFC073','Second note', 'second note from msi'),
('BABF30BF-B436-46C0-B452-39FCC16E27EC', '7AE58CAD-27D4-44CA-AB54-2909BF9541DD','handler', 'need to add note handler in project'),
('BABF30BF-B436-46C0-B452-39FCC16E27EC', '00889CBA-1669-4A8B-B0E8-3440F0A12830','commit', 'then i have to commit it')
