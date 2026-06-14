-- Create RolePermissions table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='RolePermissions' and xtype='U')
BEGIN
    CREATE TABLE [dbo].[RolePermissions] (
        [Id] [uniqueidentifier] NOT NULL,
        [RoleName] [nvarchar](50) NOT NULL,
        [PermissionName] [nvarchar](100) NOT NULL,
        [PermissionDescription] [nvarchar](500) NULL,
        [IsActive] [bit] NOT NULL DEFAULT 1,
        [HospitalId] [uniqueidentifier] NOT NULL,
        [BranchId] [uniqueidentifier] NOT NULL,
        [CreatedBy] [uniqueidentifier] NOT NULL,
        [CreatedDate] [datetime2] NOT NULL,
        [UpdateBy] [uniqueidentifier] NOT NULL,
        [UpdateDate] [datetime2] NOT NULL,
        [IsDeleted] [bit] NOT NULL DEFAULT 0,
        [DeletedBy] [uniqueidentifier] NULL,
        [DeletedDate] [datetime2] NULL,
        CONSTRAINT [PK_RolePermissions] PRIMARY KEY CLUSTERED ([Id])
    );
    
    CREATE INDEX [IX_RolePermissions_RoleName] ON [dbo].[RolePermissions] ([RoleName]);
END

-- Create UserRoleAssignments table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='UserRoleAssignments' and xtype='U')
BEGIN
    CREATE TABLE [dbo].[UserRoleAssignments] (
        [Id] [uniqueidentifier] NOT NULL,
        [UserId] [uniqueidentifier] NOT NULL,
        [RoleName] [nvarchar](50) NOT NULL,
        [AssignedDate] [datetime2] NOT NULL,
        [ExpiryDate] [datetime2] NULL,
        [IsActive] [bit] NOT NULL DEFAULT 1,
        [AssignedByUserId] [uniqueidentifier] NOT NULL,
        [Remarks] [nvarchar](500) NULL,
        [HospitalId] [uniqueidentifier] NOT NULL,
        [BranchId] [uniqueidentifier] NOT NULL,
        [CreatedBy] [uniqueidentifier] NOT NULL,
        [CreatedDate] [datetime2] NOT NULL,
        [UpdateBy] [uniqueidentifier] NOT NULL,
        [UpdateDate] [datetime2] NOT NULL,
        [IsDeleted] [bit] NOT NULL DEFAULT 0,
        [DeletedBy] [uniqueidentifier] NULL,
        [DeletedDate] [datetime2] NULL,
        CONSTRAINT [PK_UserRoleAssignments] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_UserRoleAssignments_UserMaster_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[UserMaster]([Id]),
        CONSTRAINT [FK_UserRoleAssignments_UserMaster_AssignedByUserId] FOREIGN KEY ([AssignedByUserId]) REFERENCES [dbo].[UserMaster]([Id])
    );
    
    CREATE INDEX [IX_UserRoleAssignments_UserId] ON [dbo].[UserRoleAssignments] ([UserId]);
    CREATE INDEX [IX_UserRoleAssignments_RoleName] ON [dbo].[UserRoleAssignments] ([RoleName]);
    CREATE INDEX [IX_UserRoleAssignments_AssignedByUserId] ON [dbo].[UserRoleAssignments] ([AssignedByUserId]);
END

-- Add migration history entry
INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260327120000_AddRolePermissionTables', N'6.0.26')
