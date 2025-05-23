﻿// using ACore.Base.CQRS.Results;
// using ACore.Server.Repositories.Services.StorageResolvers;
// using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Repositories.SQL.Models;
//
// namespace ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestPKLong.Delete;
//
// internal class TestPKLongAuditDeleteHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestPKLongAuditDeleteCommand, Result>(storageResolver)
// {
//   public override async Task<Result> Handle(TestPKLongAuditDeleteCommand request, CancellationToken cancellationToken)
//   {
//     var allTask = new List<Task>();
//     
//     foreach (var storage in WriteTestContexts())
//     {
//       var t2 = storage.DeleteTestEntity<TestPKLongEntity, long>(request.Id);
//       allTask.Add(t2);
//     }
//     
//     await Task.WhenAll(allTask);
//     return Result.Success();
//   }
// }