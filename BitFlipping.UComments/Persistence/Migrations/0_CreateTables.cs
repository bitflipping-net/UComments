using Umbraco.Core;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;
using Umbraco.Core.Logging;
using BitFlipping.UComments.Core.Persistence.Models;

namespace BitFlipping.UComments.Core.Persistence.Migrations
{
    [Migration("0.0.1", 1, Constants.PluginName)]
    public class CreateTables : MigrationBase
    {
        private readonly UmbracoDatabase _database = ApplicationContext.Current.DatabaseContext.Database;
        private readonly DatabaseSchemaHelper _schemaHelper;

        public CreateTables(ISqlSyntaxProvider sqlSyntax, ILogger logger)
            : base(sqlSyntax, logger)
        {
            _schemaHelper = new DatabaseSchemaHelper(_database, logger, sqlSyntax);
        }

        public override void Up()
        {
            _schemaHelper.CreateTable<CommentEntity>();
        }

        public override void Down()
        {
            _schemaHelper.DropTable<CommentEntity>();
        }
    }
}