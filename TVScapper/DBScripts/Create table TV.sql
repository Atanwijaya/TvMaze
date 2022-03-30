BEGIN TRY
    BEGIN TRAN 

drop table if exists [TVShow_Genres]
drop table if exists [Genre]

Create table [dbo].Genre(
	[ID] int primary key clustered identity(1,1) not null,
	[Name] char(32) not null
)

drop table if exists [TVShow_Cast]
drop table if exists [TVShow_Genres]
drop table if exists [TVShow]

CREATE TABLE [dbo].[TVShow] (
	[ID] int primary key  clustered identity(1,1) not null,
	[URL] varchar(256)  not null,
	[Name] char(64) not null,
	[Type] char(16) not null,
	[Language] char(16) not null,
	[Status] char(16) not null,
	[Runtime] int not null,
	[AverageRuntime] int not null,
	[Premiered] date not null,
	[Ended] date null,
	[OfficialSite] varchar(256)  null,
	[Rating] float null,
	[ScheduleTime] time not null, 
	[Image] varchar(1024) null,
	[Summary] varchar(max) null,
	[ScheduleMon] bit DEFAULT 0,
	[ScheduleTue] bit DEFAULT 0,
	[ScheduleWed] bit DEFAULT 0,
	[ScheduleThu] bit DEFAULT 0,
	[ScheduleFri] bit DEFAULT 0,
	[ScheduleSat] bit DEFAULT 0,
	[ScheduleSun] bit DEFAULT 0
)

Create table [dbo].TVShow_Genres(
		[ID] bigint primary key clustered identity(1,1) not null,
		[IDTV] int foreign key references TVShow(ID) not null,
		[IDGenre] int foreign key references Genre(ID) not null
	)
	
drop table if exists [Cast]
CREATE TABLE [dbo].[Cast] (
		[ID] int primary key  clustered identity(1,1) not null,
		[URL] varchar(256)  not null,
		[Name] varchar(128) not null,
		[CountryName] char(16) not null,
		[CountryCode] char(8) not null,
		[CountryTZ] varchar(32) not null,
		[Birthday] date not null,
		[Deathday] date  null,
		[Gender] char(1) not null,
		[Image] varchar(1024) null
	)

drop table if exists [Character]

CREATE TABLE [dbo].[Character] (
		[ID] int primary key  clustered identity(1,1) not null,
		[Name] varchar(128) not null,
		[URL] varchar(256)  not null,
		[Image] varchar(1024) null

)

Create table [dbo].TVShow_Cast(
	[ID] bigint primary key clustered identity(1,1) not null,
	[IDTV] int foreign key references TVShow(ID) not null,
	[IDCast] int foreign key references [Cast](ID) not null,
	[IDCharacter] int foreign key references [Character](ID) not null
)



if not exists (SELECT 1 FROM sys.types WHERE name = 'TVShow')
begin
	CREATE TYPE [dbo].[TVShow] as TABLE(
	[URL] varchar(256)  not null,
	[Name] char(64) not null,
	[Type] char(16) not null,
	[Language] char(16) not null,
	[Status] char(16) not null,
	[Runtime] int not null,
	[AverageRuntime] int not null,
	[Premiered] date not null,
	[Ended] date null,
	[OfficialSite] varchar(256)  null,
	[Rating] float null,
	[ScheduleTime] time not null, 
	[Image] varchar(1024) null,
	[Summary] varchar(max) null,
	[ScheduleMon] bit DEFAULT 0,
	[ScheduleTue] bit DEFAULT 0,
	[ScheduleWed] bit DEFAULT 0,
	[ScheduleThu] bit DEFAULT 0,
	[ScheduleFri] bit DEFAULT 0,
	[ScheduleSat] bit DEFAULT 0,
	[ScheduleSun] bit DEFAULT 0
	)
end


if not exists (SELECT 1 FROM sys.types WHERE name = 'Cast')
begin
	CREATE TYPE [dbo].[Cast] as TABLE(
		[URL] varchar(256)  not null,
		[Name] varchar(128) not null,
		[CountryName] char(16) not null,
		[CountryCode] char(8) not null,
		[CountryTZ] varchar(32) not null,
		[Birthday] date not null,
		[Deathday] date  null,
		[Gender] char(1) not null,
		[Image] varchar(1024) null
	)
end


if not exists (SELECT 1 FROM sys.types WHERE name = 'TVCharacter')
begin
	CREATE TYPE [dbo].[TVCharacter] as TABLE(
		[Name] varchar(128) not null,
		[URL] varchar(256)  not null,
		[Image] varchar(1024) null
	)
end

COMMIT
END TRY
BEGIN CATCH

    IF @@TRANCOUNT > 0
        ROLLBACK

		SELECT ERROR_MESSAGE() AS ErrorMessage;  
		select ERROR_LINE() as errorline;
END CATCH


if exists(SELECT top 1 type FROM sys.procedures WHERE NAME = 'GetTVShowsWithCast')
Drop procedure GetTVShowsWithCast
GO

	CREATE PROCEDURE GetTVShowsWithCast (@Offset int, @Limit int)
	AS
	Begin
	 select 1
	END
GO


if exists(SELECT top 1 type FROM sys.procedures WHERE NAME = 'InsertTVShowsWithCast')
Drop procedure InsertTVShowsWithCast
GO

	CREATE PROCEDURE InsertTVShowsWithCast (@TVShow TVShow readonly, @Character [TVCharacter] readonly, @Cast [Cast] readonly)
	AS
	Begin
	 select 1
	END
GO