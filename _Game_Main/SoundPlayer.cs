using CSCore;
using CSCore.Codecs;
using CSCore.SoundOut;
using CSCore.Streams;

class SoundPlayer
{
    private ISoundOut _soundOut;
    private IWaveSource _waveSource;
    private VolumeSource _volumeSource;

    public SoundPlayer(string filePath)
    {
        var sampleSource = CodecFactory.Instance.GetCodec(filePath)
            .ToSampleSource();

        _volumeSource = new VolumeSource(sampleSource)
        {
            Volume = 0.05f
        };

        _waveSource = _volumeSource.ToWaveSource();

        _soundOut = new WasapiOut();
        _soundOut.Initialize(_waveSource);

        _soundOut.Stopped += (s, e) => Loop();
    }

    public void Play()
    {
        if (_soundOut.PlaybackState != PlaybackState.Playing)
        {
            _soundOut.Play();
        }
    }

    private void Loop()
    {
        Task.Run(() =>
        {
            _waveSource.Position = 0;
            Play();
        });
    }

    public void Stop()
    {
        if (_soundOut.PlaybackState == PlaybackState.Playing)
        {
            _soundOut.Stop();
        }
    }

    public void SetVolume(float volume)
    {
        _volumeSource.Volume = Math.Clamp(volume, 0.0f, 1.0f);
    }

    public void Dispose()
    {
        _soundOut.Dispose();
        _waveSource.Dispose();
    }
}
