using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoreMovies.Migrations
{
    /// <inheritdoc />
    public partial class UDFs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                                CREATE FUNCTION InvoiceDetailSum
                                (
                                 @InvoiceId INT
                                )
                                RETURNS int 
                                AS BEGIN

                                DECLARE @sum INT;

                                SELECT @sum = SUM(price)
                                FROM InvoiceDetails WHERE InvoiceId = @invoiceId

                                RETURN @sum;

                                END
                                ");

            migrationBuilder.Sql(@"
                                CREATE FUNCTION InvoiceDetailAverage
                                (
                                 @InvoiceId INT
                                )
                                RETURNS decimal(18,2)
                                AS 
                                BEGIN

                                DECLARE @average decimal(18,2);

                                SELECT @average = AVG(price)
                                FROM InvoiceDetails WHERE InvoiceId = @invoiceId

                                RETURN @average;

                                END
                                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION [dbo].[InvoiceDetailSum]");
            migrationBuilder.Sql("DROP FUNCTION [dbo].[InvoiceDetailAverage]");
        }
    }
}
