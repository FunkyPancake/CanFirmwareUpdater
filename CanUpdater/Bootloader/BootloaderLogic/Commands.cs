using System.Net.NetworkInformation;

namespace CanUpdater.Bootloader.BootloaderLogic;

internal class Commands {
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
        var cmd = new byte[] {0x0B, 0, 0};
    }

    public void SetProperty() {
    }

    public void FlashEraseAllUnsecure() {
    }

    private bool ProcessCommandNoData() {
        return false;
    }

    public bool Ping(out PingResponse pingData) {
        pingData = null!;
        _tp.Send(PacketWrapper.BuildFramingPacket(PacketType.Ping));
        var status = PacketWrapper.ParsePingResponse(_tp.GetBytes(10), out var response);
        if (status) {
            pingData = new PingResponse(response);
        }

        return status;
    }
}