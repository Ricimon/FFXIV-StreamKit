using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Conditions;
using StreamKit;
using StreamKit.Extensions;
using StreamKit.Log;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace StreamKitDalamud
{
    public class ClientStateListener : IDalamudHook
    {
        private readonly ClientState clientState;
        private readonly Condition condition;
        private readonly Character character;
        private readonly ILogger logger;

        private readonly CompositeDisposable disposables = new();

        public ClientStateListener(
            ClientState clientState,
            Condition condition,
            Character character,
            ILogger logger)
        {
            this.clientState = clientState ?? throw new ArgumentNullException(nameof(clientState));
            this.condition = condition ?? throw new ArgumentNullException(nameof(condition));
            this.character = character ?? throw new ArgumentNullException(nameof(character));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Dispose()
        {
            this.disposables.Dispose();
        }

        public void HookToDalamud()
        {
            Observable.FromEvent<Condition.ConditionChangeDelegate?, (ConditionFlag Flag, bool Value)>(
                h => (ConditionFlag conditionFlag, bool value) => h((conditionFlag, value)),
                h => this.condition.ConditionChange += h,
                h => this.condition.ConditionChange -= h)
                .Subscribe(args =>
                {
                    var flag = args.Flag;
                    var value = args.Value;

                    this.logger.Trace($"Condition flag {flag} changed to {value}");
                    if (flag == ConditionFlag.Unconscious)
                    {
                        UpdatePlayerConsciousness();
                    }
                })
                .DisposeWith(this.disposables);

            UpdatePlayerConsciousness();
        }

        private void UpdatePlayerConsciousness()
        {
            this.character.IsAlive = !this.condition[ConditionFlag.Unconscious];
            this.logger.Trace("Local player consciousness value: " + this.character.IsAlive.ToString());
        }
    }
}
