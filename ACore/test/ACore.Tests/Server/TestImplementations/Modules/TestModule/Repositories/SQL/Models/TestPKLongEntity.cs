﻿// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.ComponentModel.DataAnnotations;
using ACore.Server.Modules.AuditModule.Attributes;
using ACore.Server.Storages.Contexts.EF.Models.PK;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestPKLong.Models;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.SQL.Models;

[Auditable(1)]
internal class TestPKLongEntity : PKLongEntity
{
  [MaxLength(200)]
  public string Name { get; set; } = string.Empty;

  public static TestPKLongEntity Create(TestPKLongData data)
    => ToEntity<TestPKLongEntity>(data);
}

