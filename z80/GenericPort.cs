using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z80
{
    // If interrupts (NMI or MI) are enabled, receiving data on this port
    // will raise the interrupt. Reading from this port will lower the interrupt
    // Suggested port addresses are: 
    //  0/1 - console port
    //  16/17 - COM1
    //  18/19 - COM2 
    public class GenericPort : IPorts
    {

        const int NumberOfPorts = 0x10000;
        public static SortedList<int,GenericPort> Ports = new SortedList<int,GenericPort>();

        public readonly int PortNumber;
        byte InData;
        byte OutData;

        public bool NMIEnabled = false;
        static bool _NMIAsserted = false;
        public bool MIEnabled = false;
        static bool _MIAsserted = false;


        public GenericPort(int NewPortNumber)
        {
            if (NewPortNumber < 0 || NewPortNumber >= NumberOfPorts)
                throw new Exception("Invalid port number. Valid values are 0-" + (NumberOfPorts - 1).ToString());
            this.PortNumber = NewPortNumber;
            Ports.Add(PortNumber, this);
        }

        public static void DataReceived(int PortNumber, byte Data)
        {
            if (Ports.ContainsKey(PortNumber))
                Ports[PortNumber].Data = Data;
        }

        public delegate void PortIOEvent(object Sender, int Port, byte Data);
        public event PortIOEvent DataSent;
        /// <summary>
        /// Data needs to be sent to the outside world. Use this event 
        /// to trigger your own I/O code (eg: send a byte to the serial port or
        /// write to the screen.)
        /// </summary>
        /// <param name="Port"></param>
        /// <param name="Data"></param>
        protected void OnDataSent(int Port, byte Data)
        {
            if (DataSent == null)
                return;

            DataSent(this, Port, Data);
        }

        /// <summary>
        /// Called by the CPU to send data to this port. The host will be notified via
        /// the DataSet event, where the output should be handled. 
        /// </summary>
        /// <param name="Data"></param>
        public void Output(byte Data)
        {
            OutData = Data;
            OnDataSent(this.PortNumber, Data);
        }

        public byte ReadPort(ushort address)
        {
            throw new NotImplementedException();
        }

        public void WritePort(ushort address, byte value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Called by the CPU to read data from this port
        /// </summary>
        /// <returns></returns>
        public byte Data
        {
            get
            {
                // de-assert the interrupt line if this port asserted it
                byte ret = InData;
                if (NMIEnabled && NMIAsserted)
                    NMIAsserted = false;
                if (MIEnabled && _MIAsserted)
                    _MIAsserted = false;
                return ret;
            }
            protected set
            {
                InData = value;
                if (NMIEnabled)
                    NMIAsserted = true;
                if (MIEnabled)
                    _MIAsserted = true;
            }
        }

        public static bool NMIAsserted { get => _NMIAsserted; set => _NMIAsserted = value; }
    }
}
