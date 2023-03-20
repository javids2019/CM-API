CREATE TABLE [dbo].[Users] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (150) NULL,
    [EmailId]     NVARCHAR (150) NULL,
    [Designation] NVARCHAR (150) NULL,
    [Role]        NVARCHAR (150) NULL,
    [IsActive]    BIT            NULL,
    [CreatedBy]   NVARCHAR (150) NULL,
    [CreatedDate] DATETIME       NULL,
    [UpdatedBy]   NVARCHAR (150) NULL,
    [UpdatedDate] DATETIME       NULL,
    [Password]    NVARCHAR (150) NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);

