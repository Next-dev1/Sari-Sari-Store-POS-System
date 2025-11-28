ALTER PROCEDURE [dbo].[AddProduct] 
		@_ProductName varchar(50), 
		@_Price money, 
		@_Quantity int 
AS BEGIN 

INSERT INTO inventoryTable(ProductName, Price, Quantity, TotalPrice) 
VALUES(@_ProductName, @_Price, @_Quantity, (@_Price * @_Quantity)) 

END