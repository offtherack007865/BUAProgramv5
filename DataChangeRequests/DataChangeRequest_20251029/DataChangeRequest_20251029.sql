-- SQL Server Instance:  smg-sql01
IF (@@SERVERNAME <> 'smg-sql01')
BEGIN
PRINT 'Invalid SQL Server Connection'
RETURN
END

USE [BuildUserAccountsV5];

-----------------------------------------------------------------------------------------------
-- STEP 001 of 001
-- Update phone number for  250 EYE TEC Lenoir City, 251 EYE TEC Knoxville, 252 EYE TEC Harriman, 253 EYE TEC Powell, 254 EYE TEC Morristown
-- to 800-500-4667
SELECT COUNT(*)
FROM 
    [BuildUserAccountsV5].[dbo].[Site]
WHERE
    [id] IN (8813, 8814, 8815, 8816, 8817);
-- 5 records 

BEGIN TRANSACTION;

  UPDATE
    [BuildUserAccountsV5].[dbo].[Site]
  SET
    [Phone] = '800-500-4667'
  WHERE
    [id] IN (8813, 8814, 8815, 8816, 8817);


IF @@ERROR > 0 BEGIN
  SELECT 0;
  ROLLBACK TRANSACTION;
END
ELSE BEGIN
  SELECT 1;
  COMMIT TRANSACTION;
END
-----------------------------------------------------------------------------------------------
