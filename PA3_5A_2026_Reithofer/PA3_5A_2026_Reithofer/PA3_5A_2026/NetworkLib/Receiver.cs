    public interface Receiver
    {
        void ReceiveMessage(MSG m, Transfer t);
        void TransferDisconnected(Transfer t);
        void AddDebugInfo(Transfer t, String m, bool sent);
    }

