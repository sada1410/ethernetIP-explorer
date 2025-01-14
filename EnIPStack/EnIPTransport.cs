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
using System.Threading;

namespace System.Net.EnIPStack
{
    public delegate void EncapMessageReceivedHandler(object sender, byte[] packet, Encapsulation_Packet EncapPacket, int offset, int msg_length, IPEndPoint remote_address);
    public delegate void ItemMessageReceivedHandler(object sender, byte[] packet, SequencedAddressItem ItemPacket, int offset, int msg_length, IPEndPoint remote_address);

    // Could be used for client & server implementation
    // for port 0xAF12 as well as 0x8AE (server mode)
    public class EnIPUDPTransport
    {
        public event EncapMessageReceivedHandler EncapMessageReceived;
        public event ItemMessageReceivedHandler ItemMessageReceived;

        private UdpClient m_exclusive_conn;

        public EnIPUDPTransport(String Local_IP, int Port)
        {
            System.Net.EndPoint ep = new IPEndPoint(System.Net.IPAddress.Any, Port);
            if (!string.IsNullOrEmpty(Local_IP)) ep = new IPEndPoint(IPAddress.Parse(Local_IP), Port);

            m_exclusive_conn = new UdpClient(AddressFamily.InterNetwork);
            m_exclusive_conn.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            m_exclusive_conn.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ExclusiveAddressUse, false);
            m_exclusive_conn.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
            m_exclusive_conn.Client.Bind(ep);

            m_exclusive_conn.BeginReceive(OnReceiveData, m_exclusive_conn);
        }

        public void JoinMulticastGroup(String IPMulti)
        {
            try
            {
                m_exclusive_conn.JoinMulticastGroup(IPAddress.Parse(IPMulti));
            }
            catch {}
        }

        private void OnReceiveData(IAsyncResult asyncResult)
        {
            System.Net.Sockets.UdpClient conn = (System.Net.Sockets.UdpClient)asyncResult.AsyncState;
            try
            {
                System.Net.IPEndPoint ep = new IPEndPoint(System.Net.IPAddress.Any, 0);
                byte[] local_buffer;
                int rx = 0;

                try
                {
                    local_buffer = conn.EndReceive(asyncResult, ref ep);
                    rx = local_buffer.Length;
                }
                catch (Exception) // ICMP port unreachable
                {
                    //restart data receive
                    conn.BeginReceive(OnReceiveData, conn);
                    return;
                }
                
                if (rx <14)    // Sure it's too small
                {
                    //restart data receive
                    conn.BeginReceive(OnReceiveData, conn);
                    return;
                }
                
                try
                {
                    int Offset = 0;
                    Encapsulation_Packet Encapacket = new Encapsulation_Packet(local_buffer, ref Offset, rx);
                    //verify message
                    if (Encapacket.IsOK)
                    {
                        if (EncapMessageReceived != null)
                            EncapMessageReceived(this, local_buffer, Encapacket, Offset, rx, ep);
                    }
                    else
                    {
                        SequencedAddressItem Itempacket = new SequencedAddressItem(local_buffer, ref Offset, rx);
                        if (Itempacket.IsOK&&(ItemMessageReceived != null))
                            ItemMessageReceived(this, local_buffer, Itempacket, Offset, rx, ep);
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Exception in udp recieve: " + ex.Message);
                }
                finally
                {
                    //restart data receive
                    conn.BeginReceive(OnReceiveData, conn);
                }
            }
            catch (Exception ex)
            {
                //restart data receive
                if (conn.Client != null)
                {
                    Trace.TraceError("Exception in Ip OnRecieveData: " + ex.Message);
                    conn.BeginReceive(OnReceiveData, conn);
                }
            }
        }

        public void Send(Encapsulation_Packet Packet, IPEndPoint ep)
        {
            byte[] b=Packet.toByteArray();
            m_exclusive_conn.Send(b, b.Length, ep);
        }

        public void Send(SequencedAddressItem Packet, IPEndPoint ep)
        {
            byte[] b=Packet.toByteArray();
            m_exclusive_conn.Send(b, b.Length, ep);
        }
        
        // A lot of problems on Mono (Raspberry) to get the correct broadcast @
        // so this method is overridable (this allows the implementation of operating system specific code)
        // Marc solution http://stackoverflow.com/questions/8119414/how-to-query-the-subnet-masks-using-mono-on-linux for instance
        //
        protected virtual IPEndPoint _GetBroadcastAddress()
        {
            // general broadcast
            System.Net.IPEndPoint ep = new IPEndPoint(System.Net.IPAddress.Parse("255.255.255.255"), 0xAF12);
            // restricted local broadcast (directed ... routable)
            foreach (System.Net.NetworkInformation.NetworkInterface adapter in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
                foreach (System.Net.NetworkInformation.UnicastIPAddressInformation ip in adapter.GetIPProperties().UnicastAddresses)
                    if (LocalEndPoint.Address.Equals(ip.Address))
                    {
                        try
                        {
                            string[] strCurrentIP = ip.Address.ToString().Split('.');
                            string[] strIPNetMask = ip.IPv4Mask.ToString().Split('.');
                            StringBuilder BroadcastStr = new StringBuilder();
                            for (int i = 0; i < 4; i++)
                            {
                                BroadcastStr.Append(((byte)(int.Parse(strCurrentIP[i]) | ~int.Parse(strIPNetMask[i]))).ToString());
                                if (i != 3) BroadcastStr.Append('.');
                            }
                            ep = new IPEndPoint(System.Net.IPAddress.Parse(BroadcastStr.ToString()), 0xAF12);
                        }
                        catch { }  //On mono IPv4Mask feature not implemented
                    }

            return ep;
        }

        IPEndPoint BroadcastAddress = null;
        public IPEndPoint GetBroadcastAddress()
        {
            if (BroadcastAddress == null) BroadcastAddress = _GetBroadcastAddress();
            return BroadcastAddress;
        }

        // Give 0.0.0.0:xxxx if the socket is open with System.Net.IPAddress.Any
        // Some more complex solutions could avoid this, that's why this property is virtual
        public virtual IPEndPoint LocalEndPoint
        {
            get
            {
                return (IPEndPoint)m_exclusive_conn.Client.LocalEndPoint;
            }
        }
    }

    public class EnIPTCPClientTransport
    {
        private TcpClient Tcpclient;
        private int Timeout = 100;

        public EnIPTCPClientTransport(int Timeout)
        {
            this.Timeout = Timeout;
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
        public bool Connect(IPEndPoint ep)
        {
            if (IsConnected()) return true;
            try
            {
                IPEndPoint LocalEp = new IPEndPoint(IPAddress.Parse(EnIPExplorer.Properties.Settings.Default.DefaultIPInterface), 0);
                Tcpclient = new TcpClient(LocalEp);
                Tcpclient.ReceiveTimeout = this.Timeout;

                SocketAsyncEventArgs AsynchEvent = new SocketAsyncEventArgs();
                AsynchEvent.RemoteEndPoint = ep;
                AsynchEvent.Completed += new EventHandler<SocketAsyncEventArgs>(On_ConnectedACK);

                // Go
                ConnectedEvAndLock.Reset();
                Tcpclient.Client.ConnectAsync(AsynchEvent);
                bool ret = ConnectedEvAndLock.WaitOne(Timeout * 2);  // Wait transaction 2 * Timeout

                // In fact if the connection ACK-SYN is late, it will be OK after
                if (!ret)
                    Trace.WriteLine("Connection fail to " + ep.ToString());

                return ret;
            }
            catch
            {
                Tcpclient = null;
                Trace.WriteLine("Connection fail to " + ep.ToString());
                return false;
            }
        }

        public void Disconnect()
        {
            if (Tcpclient != null)
                Tcpclient.Close();
            Tcpclient = null;
        }

        public int SendReceive(Encapsulation_Packet SendPkt, out Encapsulation_Packet ReceivePkt, out int Offset, ref byte[] packet)
        {
            ReceivePkt = null;
            Offset=0;

            int Lenght = 0;
            try
            {
                // We are not working on a continous flow but with query/response datagram
                // So if something is here it's a previous lost (timeout) response packet
                // Flush all content.
                 while (Tcpclient.Available!=0) 
                     Tcpclient.Client.Receive(packet);

                Tcpclient.Client.Send(SendPkt.toByteArray());
                Lenght = Tcpclient.Client.Receive(packet);
                if (Lenght > 24)
                    ReceivePkt = new Encapsulation_Packet(packet, ref Offset, Lenght);
                if (Lenght ==0)
                    Trace.WriteLine("Reception timeout with " + Tcpclient.Client.RemoteEndPoint.ToString());
            }
            catch
            {
                Trace.WriteLine("Error in TcpClient Send Receive");
                Tcpclient = null;
            }

            return Lenght;
        }

        public void Send(Encapsulation_Packet SendPkt)
        {
            try
            {
                Tcpclient.Client.Send(SendPkt.toByteArray());
            }
            catch
            {
                Trace.WriteLine("Error in TcpClient Send");
                Tcpclient = null;
            }
        }
    }

    public class EnIPTCPServerTransport
    {
        public event EncapMessageReceivedHandler MessageReceived;
        private TcpListener tcpListener;

        private List<TcpClient> ClientsList = new List<TcpClient>();

        public EnIPTCPServerTransport()
        {
            this.tcpListener = new TcpListener(IPAddress.Any, 0xAF12);
            Thread listenThread = new Thread(ListenForClients);
            listenThread.IsBackground = true;
            listenThread.Start();
        }

        private void ListenForClients()
        {
            try
            {
                this.tcpListener.Start();

                for (; ; )
                {
                    // Blocking
                    TcpClient client = this.tcpListener.AcceptTcpClient();
                    Trace.WriteLine("Arrival of " + ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());

                    ClientsList.Add(client);

                    //Thread 
                    Thread clientThread = new Thread(HandleClientComm);
                    clientThread.IsBackground = true;
                    clientThread.Start(client);
                }
            }
            catch 
            {
                Trace.TraceError("Fatal Error in Tcp Listener Thread");
            }
        }

        public bool Send(byte[] packet, int size, IPEndPoint ep)
        {
            TcpClient tcpClient=ClientsList.Find((o) => ((IPEndPoint)(o.Client.RemoteEndPoint)).Equals(ep));

            if (tcpClient == null) return false;

            tcpClient.Client.Send(packet, 0, size, SocketFlags.None);

            return true;
        }

        private void HandleClientComm(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            byte[] Rcp = new byte[1500];

            try
            {
                NetworkStream clientStream = tcpClient.GetStream();

                int Lenght = clientStream.Read(Rcp, 0, 1500);

                if (Lenght >= 24)
                    try
                    {
                        int Offset = 0;
                        Encapsulation_Packet Encapacket = new Encapsulation_Packet(Rcp, ref Offset, Lenght);
                        if (MessageReceived != null)
                            MessageReceived(this, Rcp, Encapacket, Offset, Lenght, (IPEndPoint)tcpClient.Client.RemoteEndPoint);
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError("Exception in tcp recieve: " + ex.Message);
                    }
            }
            catch
            {
                // Client disconnected
                ClientsList.Remove(tcpClient);
            }
        }
    }
}
