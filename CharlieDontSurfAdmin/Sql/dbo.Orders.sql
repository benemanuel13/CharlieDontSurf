USE [CharlieDontSurf]
GO

/****** Object: Table [dbo].[Orders] Script Date: 03/06/2018 17:11:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Orders] (
    [Id]           INT         NOT NULL,
    [UserId]       NCHAR (128) NOT NULL,
    [Recipient]    NCHAR (50)  NOT NULL,
    [AddressLine1] NCHAR (50)  NOT NULL,
    [AddressLine2] NCHAR (50)  NOT NULL,
    [City]         NCHAR (30)  NOT NULL,
    [County]       NCHAR (30)  NOT NULL,
    [Postcode]     NCHAR (15)  NOT NULL,
    [Country]      NCHAR (30)  NOT NULL,
    [Fulfilled]    BIT         NOT NULL
);


