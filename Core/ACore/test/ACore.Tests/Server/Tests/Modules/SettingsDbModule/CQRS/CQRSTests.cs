﻿using System.Reflection;
using ACore.Results;
using ACore.Server.Modules.SettingsDbModule.CQRS.SettingsDbGet;
using ACore.Server.Modules.SettingsDbModule.CQRS.SettingsDbSave;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace ACore.Tests.Server.Tests.Modules.SettingsDbModule.CQRS;

public class CQRSTests : SettingsDbModuleTestsBase
{
  [Fact]
  public async Task BaseTest()
  {
    const string key = "key";
    const string value = "value";

    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      var resultCommand = await Mediator.Send(new SettingsDbSaveCommand(key, value));
      resultCommand.IsSuccess.Should().BeTrue();
      
      var result = await Mediator.Send(new SettingsDbGetQuery(key));
      result.ResultValue.Should().Be(value);
    });
  }

  [Fact]
  public async Task EmptyKeyTest()
  {
    const string key = "";
    const string value = "value";

    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      var result = await Mediator.Send(new SettingsDbSaveCommand(key, value));
      result.IsFailure.Should().Be(true);
      result.IsSuccess.Should().Be(false);
      result.ResultErrorItem.Should().NotBeNull();
      result.ResultErrorItem.Code.Should().NotBeNull().And.Be(ValidationResult.ResultErrorItemValidationInput.Code);
      result.ResultErrorItem.Message.Should().NotBeNull();
      var validationResult = result as ValidationResult;
      validationResult.Should().NotBeNull();
      validationResult?.ValidationErrors.Should().NotBeNull().And.HaveCountGreaterThan(0);
    });
  }
  
  [Fact]
  public async Task MaxLengthKeyTest()
  {
    var key = new string('*', 257);
    const string value = "value";

    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      var result = await Mediator.Send(new SettingsDbSaveCommand(key, value));
      result.IsFailure.Should().Be(true);
      result.IsSuccess.Should().Be(false);
      result.ResultErrorItem.Should().NotBeNull();
      result.ResultErrorItem.Code.Should().NotBeNull().And.Be(ValidationResult.ResultErrorItemValidationInput.Code);
      result.ResultErrorItem.Message.Should().NotBeNull();
      var validationResult = result as ValidationResult;
      validationResult.Should().NotBeNull();
      validationResult?.ValidationErrors.Should().NotBeNull().And.HaveCountGreaterThan(0);
    });
  }
  
  [Fact]
  public async Task MaxLengthValueTest()
  {
    const string key = "test";
    var value = new string('*', 65537);;

    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      var result = await Mediator.Send(new SettingsDbSaveCommand(key, value));
      result.IsFailure.Should().Be(true);
      result.IsSuccess.Should().Be(false);
      result.ResultErrorItem.Should().NotBeNull();
      result.ResultErrorItem.Code.Should().NotBeNull().And.Be(ValidationResult.ResultErrorItemValidationInput.Code);
      result.ResultErrorItem.Message.Should().NotBeNull();
      var validationResult = result as ValidationResult;
      validationResult.Should().NotBeNull();
      validationResult?.ValidationErrors.Should().NotBeNull().And.HaveCountGreaterThan(0);
      validationResult?.ValidationErrors[0].ValidationFailure.FormattedMessagePlaceholderValues.Should().HaveCountGreaterThan(4);
      validationResult?.ValidationErrors[0].ValidationFailure.ErrorCode.Should().NotBeEmpty();
      validationResult?.ValidationErrors[0].ValidationFailure.ErrorMessage.Should().NotBeEmpty();
      validationResult?.ValidationErrors[0].ValidationFailure.PropertyName.Should().NotBeEmpty();
      validationResult?.ValidationErrors[0].ValidationFailure.Severity.Should().Be(Severity.Error);
    });
  }
}