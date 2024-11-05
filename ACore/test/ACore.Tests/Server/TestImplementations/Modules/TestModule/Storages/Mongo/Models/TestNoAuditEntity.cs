﻿using System.ComponentModel.DataAnnotations;
using ACore.Attributes;
using ACore.Server.Storages.Contexts.EF.Models.PK;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestNoAudit.Models;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.Storages.Mongo.Models;

[SumHash]
internal class TestNoAuditEntity : PKMongoEntity
{
  [MaxLength(200)]
  public string Name { get; set; } = string.Empty;

  public DateTime Created { get; set; }

  public static TestNoAuditEntity Create<TPK>(TestNoAuditData<TPK> data)
    => ToEntity<TestNoAuditEntity>(data);
}