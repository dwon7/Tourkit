USE [Tourkit]
GO
/****** Object:  StoredProcedure [dbo].[GetProducts]    Script Date: 1/19/2025 1:47:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[GetProducts]
    @pageIndex  INT,
    @pageSize INT,
    @totalCount INT OUTPUT,
    @jsonValue NVARCHAR(MAX) OUTPUT,
    @category INT,
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
        SET @SQLSearch = @SQLSearch + ' AND (p.Name LIKE ''%' + @keySearch + '%'')';
    END

    IF @category IS NOT NULL AND @category != 0
	BEGIN
		SET @SQLSearch = @SQLSearch + ' AND p.CategoryId = ' + CAST(@category AS NVARCHAR(10));
	END

    PRINT @SQLSearch;

    SET NOCOUNT ON;

    SET @SQLStr = N'SET @jsonValue = (SELECT p.Id, p.Name, p.Price, c.Name AS CategoryName, p.Created';
    SET @SQLStr = @SQLStr + ' FROM Products p';
    SET @SQLStr = @SQLStr + ' LEFT JOIN Categories c ON p.CategoryId = c.Id WHERE 1 = 1';
    SET @SQLStr = @SQLStr + @SQLSearch;
	 SET @SQLStr = @SQLStr + '  ORDER BY p.Id DESC';
    SET @SQLStr = @SQLStr + ' OFFSET @startRowIndex ROWS FETCH NEXT @pageSize ROWS ONLY';
    SET @SQLStr = @SQLStr + ' FOR JSON PATH,INCLUDE_NULL_VALUES)';
	PRINT @SQLStr;
    EXEC sp_executesql @SQLStr, N'@startRowIndex INT, @pageSize INT, @jsonValue NVARCHAR(MAX) OUTPUT', @startRowIndex, @pageSize, @jsonValue OUTPUT;

    SET @SQLStrCount = N'SELECT @totalCount = COUNT(p.Id) FROM Products p LEFT JOIN Categories c ON p.CategoryId = c.Id WHERE 1 = 1';
    SET @SQLStrCount = @SQLStrCount + @SQLSearch;
		PRINT @SQLStrCount;
    EXEC sp_executesql @SQLStrCount, N'@category INT, @keySearch NVARCHAR(250), @totalCount INT OUTPUT', @category, @keySearch, @totalCount OUTPUT;
END
