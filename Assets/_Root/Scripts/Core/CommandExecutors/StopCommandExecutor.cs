using Abstractions.Commands.CommandsInterfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace Core
{
    public sealed class StopCommandExecutor : CommandExecutorBase<IStopCommand>
    {
        public CancellationTokenSource CancellationTokenSource { get; set; }

        public override Task ExecuteSpecificCommand(IStopCommand command)
        {
            CancellationTokenSource?.Cancel();
            return Task.CompletedTask;
        }

        public async Task ExecuteOtherCommandWithCancellation(IAwaitable<AsyncExtensions.Void> awaitable, Action CancelCommand)
        {
            CancellationTokenSource = new();
            try
            {
                await Task.Run(() => awaitable.WithCancellation(CancellationTokenSource.Token));
            }
            catch
            {
                CancelCommand();
            }
            CancellationTokenSource = null;
        }
    }
}