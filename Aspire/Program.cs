using Projects;

var builder = DistributedApplication.CreateBuilder(args);



builder.AddProject<ACoreAuditModule>("audit-module");


builder.Build().Run();