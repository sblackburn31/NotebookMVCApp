CREATE VIEW "NotebookListView" 
AS 
SELECT [dbo].[Note].[NoteId]
,LEFT([dbo].[Note].[Subject], 40) AS ListSubject
,LEFT([dbo].[Note].[NoteText], 20) AS ListNotetext
,FORMAT(dbo.[Note].[CreateDate], 'MM/dd/yyyy hh/mm/ss') AS ListCreated
,FORMAT(dbo.[Note].[UpdateDate], 'MM/dd/yyyy hh/mm/ss') AS ListUpdated
,[dbo].[NoteBook].[Email]
FROM [dbo].[Note]
JOIN [dbo].[NoteBook] ON [dbo].[Note].[NoteBookId] = [dbo].[NoteBook].[NotebookId]
