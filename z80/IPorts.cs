namespace z80
{
    public interface IPorts
    {
        byte ReadPort(ushort address);
        void WritePort(ushort address, byte value);
        bool NMIAsserted { get; }
        bool MIAsserted { get; }
        byte Data { get; }
    }
}