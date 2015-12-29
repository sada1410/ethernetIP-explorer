﻿/**************************************************************************
*                           MIT License
* 
* Copyright (C) 2016 Frederic Chaxel <fchaxel@free.fr>
*
* Permission is hereby granted, free of charge, to any person obtaining
* a copy of this software and associated documentation files (the
* "Software"), to deal in the Software without restriction, including
* without limitation the rights to use, copy, modify, merge, publish,
* distribute, sublicense, and/or sell copies of the Software, and to
* permit persons to whom the Software is furnished to do so, subject to
* the following conditions:
*
* The above copyright notice and this permission notice shall be included
* in all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
* EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
* MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
* IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
* CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
* TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
* SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;

namespace System.Net.EnIPStack
{
    public delegate void DeviceArrivalHandler(EnIPRemoteDevice device);

    public enum EnIPNetworkStatus { OnLine, OnLineReadRejected, OnLineWriteRejected, OffLine  };

    public class EnIPClient
    {
        EnIPUDPTransport udp;
        int TcpTimeout;

        public event DeviceArrivalHandler DeviceArrival;

        public EnIPClient(String End_point, int TcpTimeout=100)
        {
            this.TcpTimeout = TcpTimeout;
            udp = new EnIPUDPTransport(End_point, false);
            udp.Start();
            udp.MessageReceived += new MessageReceivedHandler(on_MessageReceived);
        }

        void on_MessageReceived(object sender, byte[] packet, EncapsulationPacket EncapPacket, int offset, int msg_length, System.Net.IPEndPoint remote_address)
        {
            // ListIdentity response
            if ((EncapPacket.Command == (ushort)EncapsulationCommands.ListIdentity) && (EncapPacket.Length != 0) && EncapPacket.IsOK)
            {
                if (DeviceArrival != null)
                {
                    int NbDevices = BitConverter.ToUInt16(packet, offset);

                    offset += 2;
                    for (int i = 0; i < NbDevices; i++)
                    {
                        EnIPRemoteDevice device = new EnIPRemoteDevice(remote_address, TcpTimeout, packet, ref offset);
                        DeviceArrival(device);
                    }
                }
            }
        }

        // Unicast ListIdentity
        public void DiscoverServers(IPEndPoint ep)
        {
            EncapsulationPacket p = new EncapsulationPacket(EncapsulationCommands.ListIdentity);
            p.Command = (ushort)EncapsulationCommands.ListIdentity;
            udp.Send(p, ep);
            Trace.WriteLine("Send ListIdentity to "+ep.Address.ToString());
        }
        // Broadcast ListIdentity
        public void DiscoverServers()
        {
            DiscoverServers(udp.GetBroadcastAddress());
        }
    }

    public class EnIPRemoteDevice
    {
        // Data comming from the reply to ListIdentity query
        // get set are used by the property grid in EnIPExplorer
        public ushort DataLength;
        public ushort EncapsulationVersion { get; set; }
        private SocketAddress SocketAddress;
        public ushort VendorId { get; set; }
        public ushort DeviceType { get; set; }
        public ushort ProductCode { get; set; }
        public string Revision { get { return _Revision[0].ToString() + "." + _Revision[1].ToString(); } set { } }
        public byte[] _Revision = new byte[2];
        public short Status { get; set; }
        public uint SerialNumber { get; set; }
        public string ProductName { get; set; }
        public IdentityObjectState State { get; set; }

        private IPEndPoint ep;
        public string IPString() { return ep.Address.ToString(); }

        public bool autoConnect = true;
        public bool autoRegisterSession = true;

        private UInt32 SessionHandle; // When Register Session is set

        private TcpClient Tcpclient;
        private int Timeout=100;

        // A global packet for response frames
        byte[] packet = new byte[1500];

        public List<EnIPClass> SupportedClassLists = new List<EnIPClass>();

        public event DeviceArrivalHandler DeviceArrival;

        private void FromListIdentityResponse(byte[] DataArray, ref int Offset)
        {
            Offset += 2; // 0x000C 

            DataLength = BitConverter.ToUInt16(DataArray, Offset);
            Offset += 2;

            EncapsulationVersion = BitConverter.ToUInt16(DataArray, Offset);
            Offset += 2;

            // Maybe it should be used in place of the ep
            // if a host embbed more than one device, sure it sends different tcp/udp port ?
            // FIXME if you know.
            SocketAddress = new SocketAddress(DataArray, ref Offset);

            VendorId = BitConverter.ToUInt16(DataArray, Offset);
            Offset += 2;

            DeviceType = BitConverter.ToUInt16(DataArray, Offset);
            Offset += 2;

            ProductCode = BitConverter.ToUInt16(DataArray, Offset);
            Offset += 2;

            _Revision[0] = DataArray[Offset];
            Offset++;

            _Revision[1] = DataArray[Offset];
            Offset++;

            Status = BitConverter.ToInt16(DataArray, Offset);
            Offset += 2;

            SerialNumber = BitConverter.ToUInt32(DataArray, Offset);
            Offset += 4;

            int strSize = DataArray[Offset];
            Offset += 1;

            ProductName = System.Text.ASCIIEncoding.ASCII.GetString(DataArray, Offset, strSize);
            Offset += strSize;

            State = (IdentityObjectState)DataArray[Offset];

            Offset += 1;
        }
        // The udp endpoint is given here, it's also the tcp one
        // this constuctor is used with the ListIdentity response buffer
        public EnIPRemoteDevice(IPEndPoint ep, int TcpTimeout, byte[] DataArray, ref int Offset)
        {
            this.ep = ep;
            Timeout = TcpTimeout;
            FromListIdentityResponse(DataArray, ref Offset);
        }

        public EnIPRemoteDevice(IPEndPoint ep, int TcpTimeout=100)
        {
            this.ep = ep;
            Timeout = TcpTimeout;
            ProductName = "";
        }

        public void CopyData(EnIPRemoteDevice newset)
        {
            DataLength = newset.DataLength;
            EncapsulationVersion = newset.EncapsulationVersion;
            SocketAddress = newset.SocketAddress;
            VendorId = newset.VendorId;
            DeviceType=newset.DeviceType;
            ProductCode = newset.ProductCode;
            _Revision = newset._Revision;
            Status = newset.Status;
            SerialNumber = newset.SerialNumber;
            ProductName = newset.ProductName;
            State = newset.State;
        }

        public bool Equals(EnIPRemoteDevice other)
        {
            return ep.Equals(other.ep);
        }

        public bool IsConnected()
        {
            if (Tcpclient == null) return false;
            return Tcpclient.Connected;
        }

        ManualResetEvent ConnectedEvAndLock = new ManualResetEvent(false);
        // Asynchronous connection is the best way to manage the timeout
        void On_ConnectedACK(object sender, SocketAsyncEventArgs e)
        {
            ConnectedEvAndLock.Set();
        }

        public bool Connect()
        {
            lock (ConnectedEvAndLock) // to avoid to connection request
            {
                if (IsConnected()) return true;

                SessionHandle = 0;
                try
                {
                    Tcpclient = new TcpClient();
                    Tcpclient.ReceiveTimeout = this.Timeout;

                    SocketAsyncEventArgs AsynchEvent = new SocketAsyncEventArgs();
                    AsynchEvent.RemoteEndPoint = ep;
                    AsynchEvent.Completed += new EventHandler<SocketAsyncEventArgs>(On_ConnectedACK);
                    ConnectedEvAndLock.Reset();
                    Tcpclient.Client.ConnectAsync(AsynchEvent);
                    bool ret = ConnectedEvAndLock.WaitOne(Timeout * 2);  // Wait transaction 2 * Timeout

                    if (!ret)
                        Trace.WriteLine("Connection fail to " + ep.ToString());

                    return ret;
                    //return true;
                }
                catch
                {
                    Tcpclient = null;
                    Trace.WriteLine("Connection fail to " + ep.ToString());
                    return false;
                }
            }
        }

        public void Disconnect()
        {
            try
            {
                if (Tcpclient != null)
                    Tcpclient.Close();
            }
            catch { }

            Tcpclient = null;
        }

        // Unicast TCP ListIdentity for remote device
        public bool DiscoverServer()
        {
            if (autoConnect) Connect();

            try
            {
                if (IsConnected())
                {
                    EncapsulationPacket p = new EncapsulationPacket(EncapsulationCommands.ListIdentity);
                    p.Command = (ushort)EncapsulationCommands.ListIdentity;
                    Tcpclient.Client.Send(p.toByteArray());
                    Trace.WriteLine("Send ListIdentity to " + ep.Address.ToString());

                    int Length = Tcpclient.Client.Receive(packet);
                    if (Length < 26) return false;

                    int Offset = 0;
                    EncapsulationPacket Encapacket = new EncapsulationPacket(packet, ref Offset);
                    if ((Encapacket.Command == (ushort)EncapsulationCommands.ListIdentity) && (Encapacket.Length != 0) && Encapacket.IsOK)
                    {
                        Offset += 2;
                        FromListIdentityResponse(packet, ref Offset);
                        if (DeviceArrival != null)
                            DeviceArrival(this);
                        return true;
                    }
                }
            }
            catch 
            {
                Trace.WriteLine("Unicast TCP ListIdentity fail");
            }

            return false;
        }

        // Needed for a lot of operations
        private void RegisterSession()
        {
            if (autoConnect) Connect();

            if ((IsConnected() == true) && (SessionHandle == 0))
            {
                byte[] b = new byte[] { 1, 0, 0, 0 };
                EncapsulationPacket p = new EncapsulationPacket(EncapsulationCommands.RegisterSession, 0, b);

                byte[] buffer = p.toByteArray();
                Tcpclient.Client.Send(buffer);

                int ret = Tcpclient.Client.Receive(buffer); // re-use of buffer, large enought
                if (ret == 28)
                {
                    int Offset = 0;
                    EncapsulationPacket rep = new EncapsulationPacket(buffer, ref Offset);
                    if (rep.IsOK)
                        SessionHandle = rep.Sessionhandle;
                }
            }
        }

        public EnIPNetworkStatus SetClassInstanceAttribut_Data(byte[] DataPath, ControlNetService Service, byte[] data, ref int Offset, ref int Lenght, out byte[] packet)
        {
            packet = this.packet;

            if (autoRegisterSession) RegisterSession();
            if (SessionHandle == 0) return EnIPNetworkStatus.OffLine;

            try
            {
                UCMM_RR_Packet m = new UCMM_RR_Packet();
                m.Path = DataPath;
                m.Service = (byte)Service;
                m.Data = data;

                EncapsulationPacket p = new EncapsulationPacket(EncapsulationCommands.SendRRData, SessionHandle, m.toByteArray());
                Tcpclient.Client.Send(p.toByteArray());

                Lenght = Tcpclient.Client.Receive(packet);
                if (Lenght > 24)
                {
                    Offset = 0;
                    p = new EncapsulationPacket(packet, ref Offset);
                    if ((p.IsOK) && (p.Command == (ushort)EncapsulationCommands.SendRRData))
                    {
                        m = new UCMM_RR_Packet(packet, ref Offset);
                        if (m.IsOK)
                        {
                            return EnIPNetworkStatus.OnLine;
                        }
                    }
                }
                Trace.WriteLine("Service not supported : " + Service.ToString() + " on " + Path.GetPath(DataPath));
                if (Service == ControlNetService.SetAttributeSingle)
                    return EnIPNetworkStatus.OnLineWriteRejected;
                else
                    return EnIPNetworkStatus.OnLineReadRejected;
            }
            catch
            {
                Trace.TraceWarning("Error while sending request");
                return EnIPNetworkStatus.OffLine;;
            }
        }

        public EnIPNetworkStatus GetClassInstanceAttribut_Data(byte[] ClassDataPath, ControlNetService Service, ref int Offset, ref int Lenght, out byte[] packet)
        {
            return SetClassInstanceAttribut_Data(ClassDataPath, Service, null, ref Offset, ref Lenght, out packet);
        }

        public List<EnIPClass> GetObjectList()
        {
            SupportedClassLists.Clear();

            if (autoRegisterSession) RegisterSession();
            if (SessionHandle == 0) return null;

            // Class 2, Instance 1, Attribut 1
            byte[] MessageRouterObjectList = Path.GetPath("2.1.1");

            int Lenght = 0;
            int Offset = 0;

            if (GetClassInstanceAttribut_Data(MessageRouterObjectList, ControlNetService.GetAttributeSingle, ref Offset, ref Lenght, out packet) == EnIPNetworkStatus.OnLine)
            {
                ushort NbClasses = BitConverter.ToUInt16(packet, Offset);
                Offset += 2;
                for (int i = 0; i < NbClasses; i++)
                {
                    SupportedClassLists.Add(new EnIPClass(this, BitConverter.ToUInt16(packet, Offset)));
                    Offset += 2;
                }
            }
            return SupportedClassLists;
        }

        public void UnRegisterSession()
        {
            if ((IsConnected() == true) && (SessionHandle != 0))
            {
                EncapsulationPacket p = new EncapsulationPacket(EncapsulationCommands.RegisterSession, SessionHandle);
                byte[] buffer = p.toByteArray();
                Tcpclient.Client.Send(buffer);
                SessionHandle = 0;
            }
        }
    }

    // Device data dictionnary top hierachy 
    abstract public class EnIPCIPObject
    {
        public ushort Id { get; set; }
        public EnIPNetworkStatus Status { get; set; }
        public object DecodedMembers { get; set; }
        public byte[] RawData { get; set; }

        public abstract EnIPNetworkStatus ReadDataFromNetwork();
        public abstract EnIPNetworkStatus WriteDataToNetwork();
    }
    
    public class EnIPClass : EnIPCIPObject
    {
        public EnIPRemoteDevice RemoteDevice;

        public EnIPClass(EnIPRemoteDevice RemoteDevice, ushort Id)
        {
            this.Id = Id;
            this.RemoteDevice = RemoteDevice;
            Status=EnIPNetworkStatus.OffLine;
        }

        public override EnIPNetworkStatus WriteDataToNetwork()
        {
            return EnIPNetworkStatus.OnLineWriteRejected;
        }

        public override EnIPNetworkStatus ReadDataFromNetwork()
        {
            return GetClassData();
        }

        public EnIPNetworkStatus GetClassData()
        {
            //if (RawData != null) return true;
            byte[] ClassDataPath = Path.GetPath(Id, 0, null);

            int Offset = 0;
            int Lenght = 0;
            byte[] packet;
            Status=RemoteDevice.GetClassInstanceAttribut_Data(ClassDataPath, ControlNetService.GetAttributesAll, ref Offset, ref Lenght, out packet);
            
            if (Status== EnIPNetworkStatus.OnLine)
            {
                RawData = new byte[Lenght - Offset];
                Array.Copy(packet, Offset, RawData, 0, Lenght - Offset);
            }
            return Status;
        }
    }

    public class EnIPClassInstance : EnIPCIPObject
    {
        public EnIPClass Class;
        public EnIPRemoteDevice RemoteDevice;

        public EnIPClassInstance(EnIPClass Class, byte Id)
        {
            this.Id = Id;
            this.Class = Class;
            this.RemoteDevice = Class.RemoteDevice;
            Status = EnIPNetworkStatus.OffLine;
        }

        public EnIPNetworkStatus GetClassInstanceAttributList()
        {
            byte[] ClassDataPath = Path.GetPath(Class.Id, Id, null);

            int Offset = 0;
            int Lenght = 0;
            byte[] packet;

            Status = RemoteDevice.GetClassInstanceAttribut_Data(ClassDataPath, ControlNetService.GetAttributeList, ref Offset, ref Lenght, out packet);

            return Status;
        }

        public override EnIPNetworkStatus WriteDataToNetwork()
        {
            return EnIPNetworkStatus.OnLineWriteRejected;
        }

        public override EnIPNetworkStatus ReadDataFromNetwork()
        {
            return GetClassInstanceData();
        }

        public EnIPNetworkStatus GetClassInstanceData()
        {

            byte[] ClassDataPath = Path.GetPath(Class.Id, Id, null);

            int Offset = 0;
            int Lenght = 0;
            byte[] packet;

            Status = RemoteDevice.GetClassInstanceAttribut_Data(ClassDataPath, ControlNetService.GetAttributesAll, ref Offset, ref Lenght, out packet);
            if (Status == EnIPNetworkStatus.OnLine)
            {
                RawData = new byte[Lenght - Offset];
                Array.Copy(packet, Offset, RawData, 0, Lenght - Offset);
            }
            return Status;
        }
    }

    public class EnIPInstanceAttribut : EnIPCIPObject
    {
        public EnIPClassInstance Instance;
        EnIPRemoteDevice RemoteDevice;

        public EnIPInstanceAttribut(EnIPClassInstance Instance, byte Id)
        {
            this.Id = Id;
            this.Instance = Instance;
            this.RemoteDevice = Instance.RemoteDevice;
            Status = EnIPNetworkStatus.OffLine;
        }

        public override EnIPNetworkStatus WriteDataToNetwork()
        {
            return SetInstanceAttributData();
        }

        public EnIPNetworkStatus SetInstanceAttributData()
        {
            byte[] ClassDataPath = Path.GetPath(Instance.Class.Id, Instance.Id, Id);

            int Offset = 0;
            int Lenght = 0;
            byte[] packet;

            Status = RemoteDevice.SetClassInstanceAttribut_Data(ClassDataPath, ControlNetService.SetAttributeSingle, RawData, ref Offset, ref Lenght, out packet);

            return Status; 
        }

        public override EnIPNetworkStatus ReadDataFromNetwork()
        {
            return GetInstanceAttributData();
        }

        public EnIPNetworkStatus GetInstanceAttributData()
        {
            byte[] ClassDataPath = Path.GetPath(Instance.Class.Id, Instance.Id, Id);

            int Offset = 0;
            int Lenght = 0;
            byte[] packet;

            Status = RemoteDevice.GetClassInstanceAttribut_Data(ClassDataPath, ControlNetService.GetAttributeSingle, ref Offset, ref Lenght, out packet);
            if (Status == EnIPNetworkStatus.OnLine)           
            {
                RawData = new byte[Lenght - Offset];
                Array.Copy(packet, Offset, RawData, 0, Lenght - Offset);
            }
            return Status;
        }
    }
}
