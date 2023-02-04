using Serilog;

namespace CanUpdater.Bootloader;

public class KinetisBootloader
{
    private readonly ILogger _logger;
    private readonly TransportLayer _tp;

    public KinetisBootloader(ILogger logger, ICanDevice tp)
    {
        _logger = logger;
        _tp = tp;
    }

    //Not supported when security enabled
    public void Execute(uint jumpAddr, uint arg,uint stackPtrAddr)
    {
        
        var cmd = new [] {jumpAddr, arg, stackPtrAddr};
    }

    public void FLashEraseAll()
    {
    }

    public void FlashEraseRegion()
    {
    }

    public void WriteMemory()
    {
    }

    public void ReadMemory()
    {
    }

    //Supported when security enabled
    public void FLashSecurityDisable()
    {
    }

    public void GetProperty()
    {
    }

    public void Reset()
    {
        var cmd = new byte[] {0x0B, 0, 0};
        
            
    }

    public void SetProperty()
    {
    }

    public void FlashEraseAllUnsecure()
    {
    }

    // private bool SendCommandFull()
    // {
    //     Send(command);
    //     if (WaitForAck() != Ok)
    //     {
    //         _logger.Error("Ack status failed.");
    //         return false;
    //     }
    //     foreach (var dataPacket in outDataPacket)
    //     {
    //         Send(dataPacket);
    //         if (WaitForAck() == kStatus_AbortDataPhase)
    //             break;
    //     }
    //
    //     WaitForResponse();
    //     SendAck();
    //     return true;
    // }
    //
    // private CmdStatus SendCommand(Command command)
    // {
    //     Send(command);
    //     return WaitForAck();
    // }
    //
    // private byte WaitForResponse()
    // {
    //     
    // }
    //
    // private void SendAck()
    // {
    //     _tp.SendFrame();
    // }
    //
    // private void WaitForAck()
    // {
    //     _tp.GetFrame();
    // }
    //
    // private void Send(object command)
    // {
    //     throw new NotImplementedException();
    // }
}