namespace TestingSystem.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTestResult : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuestionResults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TestResultId = c.Int(nullable: false),
                        IsTrueAnswer = c.Boolean(nullable: false),
                        QuestionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .ForeignKey("dbo.TestResults", t => t.TestResultId, cascadeDelete: true)
                .Index(t => t.TestResultId)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.TestResults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false),
                        TestId = c.Int(nullable: false),
                        SummaryResult = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuestionResults", "TestResultId", "dbo.TestResults");
            DropForeignKey("dbo.QuestionResults", "QuestionId", "dbo.Questions");
            DropIndex("dbo.QuestionResults", new[] { "QuestionId" });
            DropIndex("dbo.QuestionResults", new[] { "TestResultId" });
            DropTable("dbo.TestResults");
            DropTable("dbo.QuestionResults");
        }
    }
}
