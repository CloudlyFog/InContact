﻿DROP TABLE Groupsdata
CREATE TABLE Groupsdata
(
	ID UNIQUEIDENTIFIER,
	Name NVARCHAR(200),
	Description NVARCHAR(MAX),
	UserID UNIQUEIDENTIFIER,
	OwnerID UNIQUEIDENTIFIER
)
INSERT INTO Groupsdata(ID, Name, UserID, OwnerID, Description) VALUES
('3c517966-2781-462a-baed-75067a72488f', 'Astralis', 'A08AB3E5-E3EC-47CD-84EF-C0EB75045A70',  'A08AB3E5-E3EC-47CD-84EF-C0EB75045A70', 'asap'),
('44afd7d0-d054-49b2-908e-9fe21d497072', 'PMFTE', 'A08AB3E5-E3EC-47CD-84EF-C0EB75045A70',  'A08AB3E5-E3EC-47CD-84EF-C0EB75045A70', 'group')

DROP TABLE GroupsNodeOwner
CREATE TABLE GroupsNodeOwner
(
	ID UNIQUEIDENTIFIER,
	OwnerID UNIQUEIDENTIFIER,
	GroupID UNIQUEIDENTIFIER
)
INSERT INTO GroupsNodeOwner(ID, OwnerID, GroupID) VALUES
('4e1f013b-c088-4d23-b667-acf5f85a1bc4', 'A08AB3E5-E3EC-47CD-84EF-C0EB75045A70','3c517966-2781-462a-baed-75067a72488f'),
('15a70cbb-8113-4b96-9dfd-b7027162d860', 'BABF30BF-B436-46C0-B452-39FCC16E27EC','44afd7d0-d054-49b2-908e-9fe21d497072')

DROP TABLE GroupsMembers
CREATE TABLE GroupsMembers
(
	ID UNIQUEIDENTIFIER,
	UserID UNIQUEIDENTIFIER,
	GroupID UNIQUEIDENTIFIER
)
INSERT INTO GroupsMembers(ID, UserID, GroupID) VALUES
('4e1f013b-c088-4d23-b667-acf5f85a1bc4', 'A08AB3E5-E3EC-47CD-84EF-C0EB75045A70','3c517966-2781-462a-baed-75067a72488f'),
('15a70cbb-8113-4b96-9dfd-b7027162d860', 'BABF30BF-B436-46C0-B452-39FCC16E27EC','44afd7d0-d054-49b2-908e-9fe21d497072')
SELECT * FROM GroupsMembers