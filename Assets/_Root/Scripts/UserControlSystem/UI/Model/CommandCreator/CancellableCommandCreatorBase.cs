using System;
using System.Threading;
using Abstractions.Commands;
using Utils;
using Zenject;

namespace UserControlSystem
{
    public abstract class CancellableCommandCreatorBase<TCommand, TArgument> 
        : CommandCreatorBase<TCommand> where TCommand : ICommand
    {
        [Inject] private readonly AssetsContext _context;
        [Inject] private readonly IAwaitable<TArgument> _awaitableArgument;

        private CancellationTokenSource _cancellationTokenSource;

        protected abstract TCommand CreateCommand(TArgument argument);

        protected override async void ClassSpecificCommandCreation(Action<TCommand> creationCallback)
        {
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                var argument = await _awaitableArgument.WithCancellation(_cancellationTokenSource.Token);
                creationCallback?.Invoke(_context.Inject(CreateCommand(argument)));
            }
            catch { }
        }

        public override void ProcessCancel()
        {
            base.ProcessCancel();

            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
            }
        }
    }
}