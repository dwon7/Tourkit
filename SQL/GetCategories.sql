USE [Tourkit]
GO
/****** Object:  StoredProcedure [dbo].[GetCategories]    Script Date: 1/19/2025 1:47:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[GetCategories]
    @pageIndex  INT,
    @pageSize INT,
    @totalCount INT OUTPUT,
    @jsonValue NVARCHAR(MAX) OUTPUT,
    @keySearch NVARCHAR(250)
AS
BEGIN
    DECLARE @SQLStr NVARCHAR(MAX);
    DECLARE @SQLStrCount NVARCHAR(MAX);
    DECLARE @SQLSearch NVARCHAR(MAX);
    DECLARE @startRowIndex INT;
    SET @startRowIndex = (@pageIndex - 1) * @pageSize;

    SET @SQLSearch = '';

    IF @keySearch IS NOT NULL AND @keySearch != ''
    BEGIN
        SET @SQLSearch = @SQLSearch + ' AND (c.Name LIKE ''%' + @keySearch + '%'')';
    END

    SET NOCOUNT ON;

    SET @SQLStr = N'SET @jsonValue = (SELECT c.Id, c.Name, ';
    SET @SQLStr = @SQLStr + ' (SELECT COUNT(p.Id) FROM Products p WHERE p.CategoryId = c.Id) AS ProductCount, '; -- Số lượng sản phẩm theo loại
    SET @SQLStr = @SQLStr + ' c.Created ';
    SET @SQLStr = @SQLStr + ' FROM Categories c';
    SET @SQLStr = @SQLStr + ' WHERE 1 = 1';
    SET @SQLStr = @SQLStr + @SQLSearch;
    SET @SQLStr = @SQLStr + '  ORDER BY Id DESC';
    SET @SQLStr = @SQLStr + ' OFFSET @startRowIndex ROWS FETCH NEXT @pageSize ROWS ONLY';
    SET @SQLStr = @SQLStr + ' FOR JSON PATH, INCLUDE_NULL_VALUES)';

    EXEC sp_executesql @SQLStr, N'@startRowIndex INT, @pageSize INT, @jsonValue NVARCHAR(MAX) OUTPUT', @startRowIndex, @pageSize, @jsonValue OUTPUT;

    SET @SQLStrCount = N'SELECT @totalCount = COUNT(c.Id) FROM Categories c WHERE 1 = 1';
    SET @SQLStrCount = @SQLStrCount + @SQLSearch;

    EXEC sp_executesql @SQLStrCount, N'@keySearch NVARCHAR(250), @totalCount INT OUTPUT', @keySearch, @totalCount OUTPUT;
END
