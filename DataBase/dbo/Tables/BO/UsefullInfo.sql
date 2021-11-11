CREATE TABLE [dbo].[UsefullInfo]
(
	[UsefullInfoId] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [Text] NVARCHAR(MAX) NOT NULL, 
    [DeletedBy] BIGINT NULL, 
    [CreatedBy] BIGINT NOT NULL, 
    [DateCreated] DATETIME NOT NULL, 
    [DateUpdated] DATETIME NULL,
     
    [UpdatedBy] BIGINT NULL, 
    [ImageID] BIGINT NULL, 
    CONSTRAINT [FK_Contract_ToGraphic] FOREIGN KEY (ImageID) REFERENCES [Graphic]([GraphicId]) ON DELETE SET NULL
)
