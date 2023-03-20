CREATE TABLE [dbo].[RegisterProfiles] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [Name]              NVARCHAR (150) NULL,
    [lookingfor]        NVARCHAR (150) NULL,
    [Mobile]            NVARCHAR (15)  NULL,
    [Email]             NVARCHAR (150) NULL,
    [City]              NVARCHAR (150) NULL,
    [IsActive]          BIT            NULL,
    [CreatedBy]         NVARCHAR (150) NULL,
    [CreatedDate]       DATETIME       NULL,
    [UpdatedBy]         NVARCHAR (150) NULL,
    [UpdatedDate]       DATETIME       NULL,
    [AllocatedTo]       NVARCHAR (150) NULL,
    [AllocatedDateTime] DATETIME       NULL,
    CONSTRAINT [PK_RegisterProfiles] PRIMARY KEY CLUSTERED ([Id] ASC)
);

