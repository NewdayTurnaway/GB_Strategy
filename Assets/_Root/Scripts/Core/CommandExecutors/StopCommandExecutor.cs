using Abstractions.Commands;
using Abstractions.Commands.CommandsInterfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace Core
{
    public sealed class StopCommandExecutor : CommandExecutorBase<IStopCommand>
    {
        private CancellationTokenSource _cancellationTokenSource;

        public override Task ExecuteSpecificCommand(IStopCommand command)
        {
            _cancellationTokenSource?.Cancel();
            return Task.CompletedTask;
        }

        public async Task ExecuteOtherCommandWithCancellation(IAwaitable<AsyncExtensions.Void> awaitable, Action CancelCommand)
        {
            _cancellationTokenSource = new();
            try
            {
                await Task.Run(() => awaitable.WithCancellation(_cancellationTokenSource.Token));
            }
            catch
            {
                CancelCommand();
            }
            _cancellationTokenSource = null;
        }
    }
}