using CanUpdater.Bootloader.BootloaderLogic;
using Serilog;

namespace CanUpdater.Bootloader;

/// <summary>
/// 
/// </summary>
public class KinetisBootloader {
    private readonly ILogger _logger;
    private readonly Commands _commands;
    private bool _isConnected = false;
    private bool _isFileLoaded = false;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Major"></param>
    /// <param name="Minor"></param>
    /// <param name="Bugfix"></param>
    public record SoftwareVersion(int Major, int Minor, int Bugfix);

    /// <summary>
    /// 
    /// </summary>
    public SoftwareVersion BootloaderVersion { get; private set; } = new(0, 0, 0);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="tp"></param>
    public KinetisBootloader(ILogger logger, ITransportProtocol tp) {
        _logger = logger;
        _commands = new Commands(tp);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool Connect() {
        if (_commands.Ping(out var response) != true) {
            return false;
        }
        _logger.Information("Connection successful.");
        BootloaderVersion =
            new SoftwareVersion(Major: response.Major, Minor: response.Minor, Bugfix: response.Bugfix);
        _isConnected = true;
        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    public void Disconnect() {
    }

    /// <summary>
    /// 
    /// </summary>
    public void GetSoftwareVersion() {
        if (!_isConnected) {
            _logger.Error("GetSoftwareVersion() - Target not connected.");
            return;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool Program() {
        if (!_isConnected) {
            _logger.Error("GetSoftwareVersion() - Target not connected.");
            return false;
        }

        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool Verify() {
        if (!_isConnected) {
            _logger.Error("GetSoftwareVersion() - Target not connected.");
            return false;
        }

        return true;
    }
}