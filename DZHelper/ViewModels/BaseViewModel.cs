﻿using CommunityToolkit.Mvvm.ComponentModel;

namespace DZHelper.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        public delegate void ThreadAction();

        public Task _Task;
        public CancellationTokenSource src;
        public PauseTokenSource pauseSource;

        public bool StopTask()
        {
            if (_Task == null)
                return true;
            try
            {
                if (src == null)
                    return true;
                src.Cancel();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task Stop()
        {
            if (src == null)
                return;
            src.Token.ThrowIfCancellationRequested();
            await pauseSource.Token.PauseIfRequestedAsync();
        }

        public void TaskWait(CancellationTokenSource TokenSrouce, PauseTokenSource PauseSource)
        {
            var ct = StartTask(async () =>
            {
                while (true)
                {
                    try
                    {
                        TokenSrouce.Token.ThrowIfCancellationRequested();
                    }
                    catch
                    {
                        return;
                    }
                    await PauseSource.Token.PauseIfRequestedAsync();

                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            }, TokenSrouce, PauseSource);
        }

        public bool StartTask(ThreadAction action, CancellationTokenSource TokenSrouce = null, PauseTokenSource PauseSource = null)
        {
            if (_Task != null)
            {
                StopTask();
                _Task = null;
            }
            try
            {
                src = TokenSrouce;
                pauseSource = PauseSource;

                if (TokenSrouce == null)
                {
                    _Task = Task.Run(() => { action(); });
                }
                else
                {
                    _Task = Task.Run(() => { action(); }, TokenSrouce.Token);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> PauseTask()
        {
            if (_Task == null)
                return true;

            try
            {
                await pauseSource.PauseAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ResumeTask()
        {
            if (_Task == null)
                return true;

            try
            {
                await pauseSource.ResumeAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    public class PauseTokenSource
    {
        private bool _paused = false;
        private bool _pauseRequested = false;

        private TaskCompletionSource<bool> _resumeRequestTcs;
        private TaskCompletionSource<bool> _pauseConfirmationTcs;

        private readonly SemaphoreSlim _stateAsyncLock = new SemaphoreSlim(1);
        private readonly SemaphoreSlim _pauseRequestAsyncLock = new SemaphoreSlim(1);

        public PauseToken Token
        { get { return new PauseToken(this); } }

        public async Task<bool> IsPaused(CancellationToken token = default(CancellationToken))
        {
            await _stateAsyncLock.WaitAsync(token);
            try
            {
                return _paused;
            }
            finally
            {
                _stateAsyncLock.Release();
            }
        }

        public async Task ResumeAsync(CancellationToken token = default(CancellationToken))
        {
            await _stateAsyncLock.WaitAsync(token);
            try
            {
                if (!_paused)
                {
                    return;
                }

                await _pauseRequestAsyncLock.WaitAsync(token);
                try
                {
                    var resumeRequestTcs = _resumeRequestTcs;
                    _paused = false;
                    _pauseRequested = false;
                    _resumeRequestTcs = null;
                    _pauseConfirmationTcs = null;
                    resumeRequestTcs.TrySetResult(true);
                }
                finally
                {
                    _pauseRequestAsyncLock.Release();
                }
            }
            finally
            {
                _stateAsyncLock.Release();
            }
        }

        public async Task PauseAsync(CancellationToken token = default(CancellationToken))
        {
            await _stateAsyncLock.WaitAsync(token);
            try
            {
                if (_paused)
                {
                    return;
                }

                Task pauseConfirmationTask = null;

                await _pauseRequestAsyncLock.WaitAsync(token);
                try
                {
                    _pauseRequested = true;
                    _resumeRequestTcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
                    _pauseConfirmationTcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
                    pauseConfirmationTask = WaitForPauseConfirmationAsync(token);
                }
                finally
                {
                    _pauseRequestAsyncLock.Release();
                }

                await pauseConfirmationTask;

                _paused = true;
            }
            finally
            {
                _stateAsyncLock.Release();
            }
        }

        private async Task WaitForResumeRequestAsync(CancellationToken token)
        {
            using (token.Register(() => _resumeRequestTcs.TrySetCanceled(), useSynchronizationContext: false))
            {
                await _resumeRequestTcs.Task;
            }
        }

        private async Task WaitForPauseConfirmationAsync(CancellationToken token)
        {
            using (token.Register(() => _pauseConfirmationTcs.TrySetCanceled(), useSynchronizationContext: false))
            {
                await _pauseConfirmationTcs.Task;
            }
        }

        internal async Task PauseIfRequestedAsync(CancellationToken token = default(CancellationToken))
        {
            Task resumeRequestTask = null;

            await _pauseRequestAsyncLock.WaitAsync(token);
            try
            {
                if (!_pauseRequested)
                {
                    return;
                }
                resumeRequestTask = WaitForResumeRequestAsync(token);
                _pauseConfirmationTcs.TrySetResult(true);
            }
            finally
            {
                _pauseRequestAsyncLock.Release();
            }

            await resumeRequestTask;
        }
    }

    // PauseToken - consumer side
    public struct PauseToken
    {
        private readonly PauseTokenSource _source;

        public PauseToken(PauseTokenSource source)
        { _source = source; }

        public Task<bool> IsPaused()
        { return _source.IsPaused(); }


        public Task PauseIfRequestedAsync(CancellationToken token = default(CancellationToken))
        {
            return _source.PauseIfRequestedAsync(token);
        }
    }
}
