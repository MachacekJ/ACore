﻿using System.ComponentModel.DataAnnotations;
using ACore.Server.Modules.AuditModule.Attributes;
using ACore.Server.Storages.Contexts.EF.Models.PK;

namespace ACore.UnitTests.Server.Storages.Contexts.EF.EventNotification.FakeClasses.Auditable;

[Auditable(1)]

public class FakeAuditableEntity : PKLongEntity
{
  [MaxLength(10)]
  public string? TestProp { get; set; }
}