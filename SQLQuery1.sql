ALTER PROCEDURE BillingTransactionn
    @_ProductCode NVARCHAR(50),      -- The Code of the product being purchased
    @_QuantityToDeduct INT,          -- The quantity ordered by the customer
    @_Date datetime,                 -- The date of the transaction
    @_Payment MONEY                  -- The payment amount from the customer
AS
BEGIN
    -- Set the number of rows affected to be off, optimizing performance
    SET NOCOUNT ON;

    -- Ensure the transaction is atomic (all-or-nothing)
    BEGIN TRANSACTION;

    -- Declare variables to hold values retrieved from the Inventory
    DECLARE @ProductName NVARCHAR(100);
    DECLARE @PricePerItem MONEY;
    DECLARE @CurrentStock INT;
    DECLARE @TotalCost MONEY;

    -- 1. Retrieve current data from the inventoryTable
    SELECT 
        @ProductName = ProductName,
        @PricePerItem = Price,
        @CurrentStock = Quantity
    FROM 
        inventoryTable
    WHERE 
        ProductCode = @_ProductCode;

    -- Basic Stock Check (Guard Clause)
    IF @CurrentStock IS NULL
    BEGIN
        -- Product not found
        RAISERROR('Product Code not found in Inventory.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    IF @CurrentStock < @_QuantityToDeduct
    BEGIN
        -- Insufficient stock
        RAISERROR('Insufficient stock to complete the order.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    -- 2. Calculate the TotalPrice for the transaction record
    SET @TotalCost = @PricePerItem * @_QuantityToDeduct;

    IF @_Payment < @TotalCost
    BEGIN
        -- Insufficient payment
        RAISERROR('Insufficient payment amount.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    -- 3. Insert the Transaction Record into transactionTable
    INSERT INTO transactionTable (
        ProductCode,
        ProductName,
        Quantity,
        PricePerItem,
        TotalPrice, -- Name from your schema
        Date,
        PaymentAmount,
        ChangeAmount
    )
    VALUES (
        @_ProductCode,
        @ProductName,
        @_QuantityToDeduct,
        @PricePerItem,
        @TotalCost,
        @_Date,
        @_Payment,
        @_Payment - @TotalCost
        )
    -- 4. Update the Inventory Quantity (Deduct the stock)
    UPDATE 
        inventoryTable
    SET 
        Quantity = Quantity - @_QuantityToDeduct
    WHERE 
        ProductCode = @_ProductCode;

    -- Commit the transaction if all steps completed successfully
    COMMIT TRANSACTION;
END