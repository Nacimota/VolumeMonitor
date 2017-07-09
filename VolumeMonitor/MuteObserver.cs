using AudioSwitcher.AudioApi;
using System;
using System.Drawing;

namespace VolumeMonitor
{
    class MuteObserver : IObserver<DeviceMuteChangedArgs>
    {
        IDisposable unsubscriber;
        VMonApplicationContext context;

        public MuteObserver(VMonApplicationContext context)
        {
            this.context = context;
        }

        public void Subscribe(IObservable<DeviceMuteChangedArgs> provider) => unsubscriber = provider.Subscribe(this);        

        public void Unsubscribe() => unsubscriber.Dispose();

        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(DeviceMuteChangedArgs value)
        {
            if (value.IsMuted)
            {
                context.UpdateIconText("X", Color.Tomato, 8);
            }
            else
            {
                context.UpdateIconText(context.Controller.DefaultPlaybackDevice.Volume.ToString());
            }
        }
    }
}
