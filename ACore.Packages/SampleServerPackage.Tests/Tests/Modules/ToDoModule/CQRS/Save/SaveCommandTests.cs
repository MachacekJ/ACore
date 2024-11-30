using System.Reflection;
using SampleServerPackage.ToDoModulePG.CQRS.Models;
using SampleServerPackage.ToDoModulePG.CQRS.Save;

namespace SampleServerPackage.Tests.Tests.Modules.ToDoModule.CQRS.Save;

public class SaveCommandTests : CQRSTests
{

  [Fact]
  public async Task EmptyTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      // Arrange
      var h = CreateToDoSaveHandlerAsSut();
      var re = new ToDoSaveCommand(new ToDoListData());

      // Act
      await h.Handle(re, CancellationToken.None);

      // Assert
    });
  }

  private ToDoSaveHandler CreateToDoSaveHandlerAsSut()
  {
    return new ToDoSaveHandler(StorageResolver ?? throw new ArgumentNullException(nameof(StorageResolver)));
  }
}