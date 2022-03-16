using Reactive.Bindings;

namespace StreamKit
{
    public class Character
    {
        public bool IsAlive
        {
            get => IsAliveObservable.Value;
            set => IsAliveObservable.Value = value;
        }
        public IReactiveProperty<bool> IsAliveObservable { get; private set; }

        public Character()
        {
            this.IsAliveObservable = new ReactiveProperty<bool>(
                true, 
                ReactivePropertyMode.RaiseLatestValueOnSubscribe);
        }
    }
}
