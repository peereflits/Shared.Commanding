using System.Threading.Tasks;

namespace Peereflits.Shared.Commanding.Tests.Helpers;

internal interface ITestService
{
    ValueTask<bool> CanExecute();  
    Task Execute();
    Task<T> ExecuteWithResult<T>();
}