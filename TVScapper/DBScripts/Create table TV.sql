BEGIN TRY
    BEGIN TRAN 


if exists(SELECT top 1 type FROM sys.procedures WHERE NAME = 'GetTVShowsWithCast')
begin
Drop procedure GetTVShowsWithCast
end
if exists(SELECT top 1 type FROM sys.procedures WHERE NAME = 'InsertTVShowsWithCast')
begin
Drop procedure InsertTVShowsWithCast
end

if exists (SELECT 1 FROM sys.types WHERE name = 'TVShow_Cast')
begin
drop type TVShow_Cast
end

if exists (SELECT 1 FROM sys.types WHERE name = 'TVShow_Genres')
begin
drop type [TVShow_Genres]
end
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
	[ID] int primary key  clustered not null,
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
		[ID] int primary key  clustered not null,
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
		[ID] int primary key  clustered not null,
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

	CREATE TYPE [dbo].[TVShow_Genres] as TABLE(
	[IDTV] int not null,
	[Name] char(32) not null
	)


	CREATE TYPE [dbo].[TVShow_Cast] as TABLE(
	[IDTV] int not null,
	[IDCast] int not null,
	[IDCharacter] int  not null
	)


if exists (SELECT 1 FROM sys.types WHERE name = 'TVShow')
begin
drop type TVShow
end
	CREATE TYPE [dbo].[TVShow] as TABLE(
	[ID] int primary key  not null,
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


if exists (SELECT 1 FROM sys.types WHERE name = 'Cast')
begin
drop type Cast
end

CREATE TYPE [dbo].[Cast] as TABLE(
		[ID] int primary key  not null,
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

if exists (SELECT 1 FROM sys.types WHERE name = 'TVCharacter')
begin
drop type TVCharacter
end

CREATE TYPE [dbo].[TVCharacter] as TABLE(
		[ID] int primary key  not null,
		[Name] varchar(128) not null,
		[URL] varchar(256)  not null,
		[Image] varchar(1024) null
	)

COMMIT
END TRY
BEGIN CATCH

    IF @@TRANCOUNT > 0
        ROLLBACK

		SELECT ERROR_MESSAGE() AS ErrorMessage;  
		select ERROR_LINE() as errorline;
END CATCH


GO

	CREATE PROCEDURE GetTVShowsWithCast (@Offset int, @Limit int)
	AS
	Begin
	 select 1
	END
GO

	CREATE PROCEDURE InsertTVShowsWithCast (@TVShow TVShow readonly, @Character [TVCharacter] readonly, @Cast [Cast] readonly, @TVShow_Cast [TVShow_Cast] readonly, @TVShow_Genres [TVShow_Genres] readonly)
	AS
	Begin
		Begin Try
			BEGIN TRAN 
				
				Delete from TVShow_Genres where IDTV in (select ID from @TVShow)
				Delete from TVShow_Cast where IDTV in (select ID from @TVShow)
				Delete from TVShow where ID in (select ID from @TVShow)
				
				Delete from Character where ID in (select ID from @Character)
				Delete from [Cast] where ID in (select ID from @Cast)

				Insert into TVShow
					select 
					[ID],
					[URL],
					[Name],
					[Type],
					[Language],
					[Status],
					[Runtime],
					[AverageRuntime],
					[Premiered],
					[Ended],
					[OfficialSite],
					[Rating],
					[ScheduleTime], 
					[Image],
					[Summary],
					[ScheduleMon],
					[ScheduleTue],
					[ScheduleWed],
					[ScheduleThu],
					[ScheduleFri],
					[ScheduleSat],
					[ScheduleSun]
					from @TVShow

				Insert into Character
					select 
					[ID] int,
					[Name],
					[URL],
					[Image]
					from @Character

				Insert into Cast
					select 
					[ID],
					[URL],
					[Name],
					[CountryName],
					[CountryCode],
					[CountryTZ],
					[Birthday],
					[Deathday],
					[Gender],
					[Image]
					from @Cast

				Insert into [TVShow_Cast]
					select
					IDTV,
					IDCast,
					IDCharacter
					from @TVShow_Cast

				insert into Genre
				select Name
				from @TVShow_Genres tvGenres
				where tvGenres.Name <> Name

				Insert into [TVShow_Genres]
					select
					IDTV,
					(select ID from Genre where name = tvGenres.name)
					from @TVShow_Genres tvGenres

			COMMIT TRAN
		end try
		begin catch
			IF @@TRANCOUNT > 0
			ROLLBACK TRAN

			SELECT ERROR_MESSAGE() AS ErrorMessage;  
			select ERROR_LINE() as errorline;
		end catch

	END
GO