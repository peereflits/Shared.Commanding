using System.Threading.Tasks;

namespace Peereflits.Shared.Commanding;

public interface IExecutable<in TRequest> : IAction
{
    Task<bool> CanExecute(TRequest request);
}

public interface IExecutable : IAction
{
    Task<bool> CanExecute();
}