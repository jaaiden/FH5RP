using System;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using FH5RP.Data;
using Timer = System.Timers.Timer;

namespace FH5RP.Net
{
    public class TelemetryServer
    {
        public static TelemetryServer Instance { get; private set; }

        private UdpClient Socket;
        private IPEndPoint EndPoint;

        public TelemetryData LastUpdate { get; private set; }

        public delegate void DataUpdateEvent(TelemetryData Data);

        public DataUpdateEvent OnDataUpdated;

        public TelemetryServer()
        {
            if (Instance is null)
            {
                Instance = this;
            }
            else return;

            try
            {
                EndPoint = new IPEndPoint(IPAddress.Loopback, 9909);
                Socket = new UdpClient(EndPoint);
                Socket.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            RPC.Initialize();

            Timer t = new Timer(1250);
            t.Elapsed += (s, a) => { RPC.UpdatePresence(LastUpdate); };
            t.Start();
        }

        public void Start()
        {
            Console.WriteLine($"Starting listen server on {EndPoint.Address}:{EndPoint.Port}.");
            Socket.BeginReceive(new AsyncCallback(OnSocketReceive), null);
        }

        private void OnSocketReceive(IAsyncResult ar)
        {
            byte[] response = Socket.EndReceive(ar, ref EndPoint);

            TelemetryData data = CreateFromPayload(response);
            if (data is null) return;

            if (data.Vehicle.ID != 0)
                LastUpdate = data;

            OnDataUpdated?.Invoke(LastUpdate);

            Socket.BeginReceive(new AsyncCallback(OnSocketReceive), null);
        }

        private TelemetryData CreateFromPayload(byte[] data)
        {
            return new TelemetryData
            {

                Vehicle = new TelemetryData.VehicleInfo
                {
                    ID = BitConverter.ToInt32(data, 212),
                    Index = (TelemetryData.CarClass)BitConverter.ToInt32(data, 216),
                    PIValue = BitConverter.ToInt32(data, 220),
                    Drivetrain = (TelemetryData.DrivetrainType)BitConverter.ToInt32(data, 224)
                },

                InRace = BitConverter.ToBoolean(data, 0),

                Timestamp = BitConverter.ToUInt32(data, 4),

                Engine = new TelemetryData.EngineData
                {
                    MaxRPM = BitConverter.ToSingle(data, 8),
                    IdleRPM = BitConverter.ToSingle(data, 12),
                    CurrentRPM = BitConverter.ToSingle(data, 16),
                    NumCylinders = BitConverter.ToInt32(data, 228)
                },

                Acceleration = TelemetryData.Transform.FromPayload(data, 20, 24, 28),

                Velocity = TelemetryData.Transform.FromPayload(data, 32, 36, 40),

                AngularVelocity = TelemetryData.Transform.FromPayload(data, 44, 48, 52),

                Pitch = BitConverter.ToSingle(data, 60),

                Yaw = BitConverter.ToSingle(data, 56),

                Roll = BitConverter.ToSingle(data, 64),

                NormalizedSuspensionTravel = TelemetryData.WheelData.FromPayload(data, 68, 72, 76, 80),

                TireSlipRatio = TelemetryData.WheelData.FromPayload(data, 84, 88, 92, 96),

                WheelRotationSpeed = TelemetryData.WheelData.FromPayload(data, 100, 104, 108, 112),

                WheelOnRumbleStrip = TelemetryData.WheelData.FromPayload(data, 116, 120, 124, 128),

                WheelPuddleDepth = TelemetryData.WheelData.FromPayload(data, 132, 136, 140, 144),

                SurfaceRumble = TelemetryData.WheelData.FromPayload(data, 148, 152, 156, 160),

                TireSlipAngle = TelemetryData.WheelData.FromPayload(data, 164, 168, 172, 176),

                TireCombinedSlip = TelemetryData.WheelData.FromPayload(data, 180, 184, 188, 192),

                SuspensionTravel = TelemetryData.WheelData.FromPayload(data, 196, 200, 204, 208),

                Position = TelemetryData.Transform.FromPayload(data, 232, 236, 240),

                Speed = BitConverter.ToSingle(data, 244),

                Power = BitConverter.ToSingle(data, 248),

                Torque = BitConverter.ToSingle(data, 252),

                TireTemp = TelemetryData.WheelData.FromPayload(data, 256, 260, 264, 268),

                Boost = BitConverter.ToSingle(data, 272),

                Fuel = BitConverter.ToSingle(data, 276),

                DistanceTraveled = BitConverter.ToSingle(data, 280),

                BestLap = BitConverter.ToSingle(data, 284),

                LastLap = BitConverter.ToSingle(data, 288),

                CurrentLap = BitConverter.ToSingle(data, 292),

                TotalRaceTime = BitConverter.ToSingle(data, 296),

                LapNumber = BitConverter.ToUInt16(data, 300),

                RacePosition = data[302],

                Accel = data[303],

                Brake = data[304],

                Clutch = data[305],

                Handbrake = data[306],

                Gear = data[307],

                Steering = (sbyte)data[308],

                NormalizedDrivingLine = (sbyte)data[309],

                NormalizedAIBrakeDifference = (sbyte)data[310]

            };
        }
    }
}
