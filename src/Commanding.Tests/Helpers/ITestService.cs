using System.Threading.Tasks;

namespace Peereflits.Shared.Commanding.Tests.Helpers;

internal interface ITestService
{
    Task<bool> CanExecute();  
    Task Execute();
    Task<T> ExecuteWithResult<T>();
}