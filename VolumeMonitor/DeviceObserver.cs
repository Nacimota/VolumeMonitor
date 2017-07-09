using AudioSwitcher.AudioApi;
using System;
using System.Drawing;

namespace VolumeMonitor
{
    class DeviceObserver : IObserver<DeviceChangedArgs>
    {
        IDisposable unsubscriber;
        VolumeObserver volObserver;
        MuteObserver muteObserver;
        VMonApplicationContext context;

        public DeviceObserver(VolumeObserver volObserver, MuteObserver muteObserver, VMonApplicationContext context)
        {
            this.volObserver = volObserver;
            this.muteObserver = muteObserver;
            this.context = context;
        }

        public void Subscribe(IObservable<DeviceChangedArgs> provider) => unsubscriber = provider.Subscribe(this);

        public void Unsubscribe() => unsubscriber.Dispose();

        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(DeviceChangedArgs value)
        {
            volObserver.Subscribe(value.Device.VolumeChanged);
            muteObserver.Subscribe(value.Device.MuteChanged);

            if (value.Device.IsMuted)
            {
                context.UpdateIconText("X", Color.Tomato, 8);
            }
        }
    }
}
