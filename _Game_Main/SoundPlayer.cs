using System.Reflection;
using CSCore;
using CSCore.Codecs;
using CSCore.SoundOut;
using CSCore.Streams;

class SoundPlayer
{
    private ISoundOut _soundOut;
    private IWaveSource? _waveSource;
    private VolumeSource _volumeSource;

    // Default constructor for ambience sound
    public SoundPlayer() : this("./Sounds/ambience.mp3") // Default path
    {
    }

    // Constructor that accepts a relative file path
    public SoundPlayer(string relativeFilePath)
    {
        // Get the directory of the executing assembly
        string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string filePath = Path.Combine(assemblyPath, relativeFilePath);

        if (File.Exists(filePath)) // Check if the file exists
        {
            var sampleSource = CodecFactory.Instance.GetCodec(filePath)
                .ToSampleSource();

            _volumeSource = new VolumeSource(sampleSource)
            {
                Volume = 0.00f
            };

            _waveSource = _volumeSource.ToWaveSource();

            _soundOut = new WasapiOut();
            _soundOut.Initialize(_waveSource);

            _soundOut.Stopped += (s, e) => Loop();
        }
        else
        {
            _waveSource = null; // Set to null if the file does not exist
        }
    }

    public void Play()
    {
        if (_soundOut != null && _soundOut.PlaybackState != PlaybackState.Playing) // Check if _soundOut is initialized
        {
            _soundOut.Play();
        }
    }

    private void Loop()
    {
        Task.Run(() =>
        {
            if (_waveSource != null) // Check if _waveSource is initialized
            {
                _waveSource.Position = 0;
                Play();
            }
        });
    }

    public void Stop()
    {
        if (_soundOut != null && _soundOut.PlaybackState == PlaybackState.Playing) // Check if _soundOut is initialized
        {
            _soundOut.Stop();
        }
    }

    public void SetVolume(float volume)
    {
        if (_volumeSource != null) // Check if _volumeSource is initialized
        {
            _volumeSource.Volume = Math.Clamp(volume, 0.0f, 1.0f);
        }
    }

    public void Dispose()
    {
        _soundOut?.Dispose();
        _waveSource?.Dispose();
    }
}
