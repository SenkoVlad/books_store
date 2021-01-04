using Microsoft.EntityFrameworkCore.Migrations;

namespace Store.Data.EF.Migrations
{
    public partial class FulltextSearch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE FULLTEXT CATALOG StoreFulltextCatalog AS DEFAULT", suppressTransaction: true);
            migrationBuilder.Sql("CREATE FULLTEXT INDEX ON Books(Author, Title) KEY INDEX PK_Books WITH STOPLIST = SYSTEM", suppressTransaction: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FULLTEXT ON Books", suppressTransaction: true);
            migrationBuilder.Sql("DROP FULLTEXT CATALOG StoreFulltextCatalog", suppressTransaction: true);
        }
    }
}
