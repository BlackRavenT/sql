ALTER PROCEDURE view_empl 
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		Employees.empl_name,
		Employees.science_degree,
		Employees.hours
	from Employees
	
END
