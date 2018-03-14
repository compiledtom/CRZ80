using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z80
{
    // I/O port for serial or cosole communication.
    // Port Number is the control register. 
    // Use PortStatusValues to set and read the port status. 
    // Port Base+1 is the data register. 
    //  INP from this port to read incoming serial data. 
    //  OUT to this port to write outgoing serial data. 
    public class SerialPort : GenericPort
    {
        public SerialPort(byte NewPortNumber) : base(NewPortNumber)
        {
        }

        public enum PortStatusValues : byte
        {
            DataWaiting = 0x01,
            SendReady = 0x02,
            Pin_DTR = 0x04,
            Pin_DSR = 0x08,
            Pin_RTS = 0x10,
            Pin_CTS = 0x20,
            Pin_CD = 0x40,
            Pin_RING = 0x80
        }

        
    }
}
