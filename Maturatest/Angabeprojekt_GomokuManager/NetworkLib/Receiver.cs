namespace NetworkLib;

public interface Receiver
{
    void ReceiveMessage(MSG m, Transfer t);
    void TransferDisconnected(Transfer t);
}
