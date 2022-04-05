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

if exists(SELECT top 1 type FROM sys.procedures WHERE NAME = 'GetTVShowsWithCastByID')
begin
Drop procedure GetTVShowsWithCastByID
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
	[Name] varchar(32) not null
)

drop table if exists [TVShow_Cast]
drop table if exists [TVShow_Genres]
drop table if exists [TVShow]

CREATE TABLE [dbo].[TVShow] (
	[ID] int primary key  clustered not null,
	[URL] varchar(256)  null,
	[Name] varchar(64) not null,
	[Type] varchar(16) null,
	[Language] varchar(16) not null,
	[Status] varchar(16) not null,
	[Runtime] int null,
	[AverageRuntime] int null,
	[Premiered] date  null,
	[Ended] date null,
	[OfficialSite] varchar(256)  null,
	[Rating] float null,
	[ScheduleTime] char(5) null, 
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
		[URL] varchar(256)  null,
		[Name] varchar(256) not null,
		[CountryName] varchar(64)  null,
		[CountryCode] varchar(8)  null,
		[CountryTZ] varchar(64)  null,
		[Birthday] date  null,
		[Deathday] date  null,
		[Gender] char(1) null,
		[Image] varchar(1024) null
	)

drop table if exists [Character]

CREATE TABLE [dbo].[Character] (
		[ID] int primary key  clustered not null,
		[Name] varchar(128) not null,
		[URL] varchar(256)  null,
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
	[Name] varchar(32) not null
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
	[URL] varchar(256)  null,
	[Name] varchar(64) not null,
	[Type] varchar(16) null,
	[Language] varchar(16) not null,
	[Status] varchar(16) not null,
	[Runtime] int null,
	[AverageRuntime] int null,
	[Premiered] date null,
	[Ended] date null,
	[OfficialSite] varchar(256)  null,
	[Rating] float null,
	[ScheduleTime] char(5) null, 
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
		[URL] varchar(256)  null,
		[Name] varchar(256) not null,
		[CountryName] varchar(64) null,
		[CountryCode] varchar(8) null,
		[CountryTZ] varchar(64) null,
		[Birthday] date null,
		[Deathday] date  null,
		[Gender] char(1) null,
		[Image] varchar(1024) null
	)

if exists (SELECT 1 FROM sys.types WHERE name = 'TVCharacter')
begin
drop type TVCharacter
end

CREATE TYPE [dbo].[TVCharacter] as TABLE(
		[ID] int primary key  not null,
		[Name] varchar(128) not null,
		[URL] varchar(256)  null,
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
		 	 select 
			tv.*,
			cast.ID as CastID,
			cast.URL as CastURL,
			cast.Name as CastName,
			cast.CountryName as CastCountryName,
			cast.CountryCode as CastCountryCode,
			cast.CountryTZ as CastCountryTZ,
			cast.Birthday,
			cast.Deathday,
			cast.Gender as CastGender,
			cast.Image as CastImage,
			char.ID as CharacterID,
			char.Name as CharacterName,
			char.URL as CharacterURL,
			char.Image as CharacterIMG,
			genre.Name as Genre
			from 
			(
				select * from
				TVShow
				order by id
				offset @Offset rows
				fetch next @Limit rows only
			) tv
			inner join TVShow_Cast tv_cast
			on tv.ID = tv_cast.IDTV
			inner join Cast cast
			on cast.ID = tv_cast.IDCast
			inner join Character char
			on char.ID = tv_cast.IDCharacter
			inner join TVShow_Genres tv_genres
			on tv.ID = tv_genres.IDTV
			inner join Genre genre
			on genre.id = tv_genres.IDGenre
		end
GO

	CREATE PROCEDURE GetTVShowsWithCastByID (@ID int)
	AS
	Begin
		 	 select 
			tv.*,
			cast.ID as CastID,
			cast.URL as CastURL,
			cast.Name as CastName,
			cast.CountryName as CastCountryName,
			cast.CountryCode as CastCountryCode,
			cast.CountryTZ as CastCountryTZ,
			cast.Birthday,
			cast.Deathday,
			cast.Gender as CastGender,
			cast.Image as CastImage,
			char.ID as CharacterID,
			char.Name as CharacterName,
			char.URL as CharacterURL,
			char.Image as CharacterIMG,
			genre.Name as Genre
			from 
			TVShow tv
			inner join TVShow_Cast tv_cast
			on tv.ID = tv_cast.IDTV
			inner join Cast cast
			on cast.ID = tv_cast.IDCast
			inner join Character char
			on char.ID = tv_cast.IDCharacter
			inner join TVShow_Genres tv_genres
			on tv.ID = tv_genres.IDTV
			inner join Genre genre
			on genre.id = tv_genres.IDGenre

			where tv.ID = @ID
		end
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


				DECLARE @GenreID TABLE (ID bigint, Name varchar(32))

				insert into Genre
				OUTPUT INSERTED.ID,INSERTED.NAME into @GenreID(ID, Name)
				select distinct tvGenres.Name
				from @TVShow_Genres tvGenres left join
				genre gen on tvGenres.name = gen.name
				where gen.name is null

				Insert into [TVShow_Genres]
					select
					IDTV,
					genre.ID
					from @TVShow_Genres tvGenres
					inner join @GenreID genre on tvGenres.name = genre.name

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