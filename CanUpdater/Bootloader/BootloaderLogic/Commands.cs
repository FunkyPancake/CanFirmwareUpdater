using System.Data;
using System.Net.NetworkInformation;

namespace CanUpdater.Bootloader.BootloaderLogic;

internal class Commands {
    private const int PingTimeoutMs = 1000;
    private const int CommandTimeoutMs = 500;

    private const int AckTimeoutMs = 1000;
    private readonly ITransportProtocol _tp;

    public Commands(ITransportProtocol tp) {
        _tp = tp;
    }

    public void Execute(uint jumpAddr, uint arg, uint stackPtrAddr) {
        var cmd = new[] {jumpAddr, arg, stackPtrAddr};
    }

    public void FLashEraseAll() {
    }

    public void FlashEraseRegion() {
    }

    public void WriteMemory() {
    }

    public void ReadMemory() {
    }

    //Supported when security enabled
    public void FLashSecurityDisable() {
    }

    public void GetProperty() {
    }

    public void Reset() {
        _tp.Send(PacketWrapper.BuildCommandPacket(Command.Reset, 0, Array.Empty<uint>()));
    }

    public void SetProperty() {
    }

    public void FlashEraseAllUnsecure() {
    }

    private bool ProcessCommandNoData() {
        return false;
    }

    public bool Ping(out SoftwareVersion pingData) {
        pingData = null!;
        _tp.Send(PacketWrapper.BuildFramingPacket(PacketType.Ping));
        var status = PacketWrapper.ParsePingResponse(_tp.GetBytes(10, PingTimeoutMs), out var response);
        if (status) {
            pingData = new SoftwareVersion(Major: response[2], Minor: response[1], Bugfix: response[0]);
        }

        return status;
    }

    private bool CommandNoData(CommandType commandType, byte[] bytes) {
        _tp.Send(PacketWrapper.BuildFramingPacket(PacketType.Command));
        GetAck();
        SendAck();
        return true;
    }


    private void SendAck() {
        _tp.Send(PacketWrapper.BuildFramingPacket(PacketType.Ack));
    }

    private bool GetAck() {
        return PacketWrapper.ParseAck(_tp.GetBytes(2, AckTimeoutMs));
    }
}