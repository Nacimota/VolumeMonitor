using AudioSwitcher.AudioApi;
using System;
using System.Drawing;

namespace VolumeMonitor
{
    class VolumeObserver : IObserver<DeviceVolumeChangedArgs>
    {
        IDisposable unsubscriber;
        VMonApplicationContext context;

        public VolumeObserver(VMonApplicationContext context)
        {
            this.context = context;
        }

        public void Subscribe(IObservable<DeviceVolumeChangedArgs> provider)
        {
            unsubscriber = provider.Subscribe(this);
            var volume = context.Controller.DefaultPlaybackDevice.Volume;

            if (volume > 0)
            {
                context.UpdateIconText(volume.ToString());
            }
            else
            {
                context.UpdateIconText("X", Color.Tomato, 8);
            }
        }

        public void Unsubscribe() => unsubscriber.Dispose();

        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(DeviceVolumeChangedArgs value)
        {
            if (value.Volume > 0)
            {
                context.UpdateIconText(value.Volume.ToString());
            }
            else
            {
                context.UpdateIconText("X", Color.Tomato, 8);
            }
        }
    }
}
