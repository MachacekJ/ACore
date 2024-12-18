﻿using ACore.Models.Result;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestAudit.Delete;

public class TestAuditDeleteCommand<T>(T id): TestModuleRequest<Result>
{
  public T Id => id;
}