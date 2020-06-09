Use [UserDB]
-- Create a new table called '[User]' in schema '[dbo]'
-- Drop the table if it already exists
IF OBJECT_ID('[dbo].[User]', 'U') IS NOT NULL
DROP TABLE [dbo].[User]
GO
-- Create the table in the specified schema
CREATE TABLE [dbo].[User](
  [Id] [int] IDENTITY(1,1) NOT NULL,
  [Name] [varchar](255) NULL,
  [Age] int NULL,
  [Email] [varchar](255) NULL
);
GO