CREATE TABLE [dbo].[FollowupProfiles] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [RegisterProfileId] INT            NOT NULL,
    [Name]              NVARCHAR (150) NULL,
    [lookingfor]        NVARCHAR (150) NULL,
    [Mobile]            NVARCHAR (15)  NULL,
    [Email]             NVARCHAR (150) NULL,
    [City]              NVARCHAR (150) NULL,
    [DispositionStatus] NVARCHAR (150) NULL,
    [TodayFollowupDate] DATETIME       NULL,
    [TodayFollowupTime] TIME (7)       NULL,
    [NextFollowupDate]  DATETIME       NULL,
    [NextFollowupTime]  TIME (7)       NULL,
    [Remarkes]          NVARCHAR (550) NULL,
    [IsActive]          BIT            NULL,
    [FollowedBy]        NVARCHAR (150) NULL,
    CONSTRAINT [PK_FollowupProfiles] PRIMARY KEY CLUSTERED ([Id] ASC)
);

