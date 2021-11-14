using System.Text.Json;
using System.Reflection;

namespace FH5RP.Data
{
    public class TelemetryData
    {

        #region Enuns/Structs

        public enum DrivetrainType
        {
            FWD = 0,
            RWD = 1,
            AWD = 2,
        }

        public enum CarClass
        {
            D = 0,
            C = 1,
            B = 2,
            A = 3,
            S1 = 4,
            S2 = 5,
            X = 6,
        }

        public struct VehicleInfo
        {
            /// <summary>
            /// The vehicle's unique ID
            /// </summary>
            public int ID { get; set; }

            /// <summary>
            /// The current performance index classification (D-X)
            /// </summary>
            public CarClass Index { get; set; }

            /// <summary>
            /// The current performance index value
            /// </summary>
            public int PIValue { get; set; }

            /// <summary>
            /// The current drivetrain type
            /// </summary>
            public DrivetrainType Drivetrain { get; set; }
        }

        /// <summary>
        /// Engine data
        /// </summary>
        public struct EngineData
        {
            /// <summary>
            /// Max RPM
            /// </summary>
            public float MaxRPM { get; set; }

            /// <summary>
            /// Idle RPM
            /// </summary>
            public float IdleRPM { get; set; }

            /// <summary>
            /// Current RPM
            /// </summary>
            public float CurrentRPM { get; set; }

            /// <summary>
            /// The number of cylinders in this engine
            /// </summary>
            public int NumCylinders { get; set; }
        }

        /// <summary>
        /// 3D transform data
        /// </summary>
        public struct Transform
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Z { get; set; }

            public Transform(float InX, float InY, float InZ)
            {
                X = InX;
                Y = InY;
                Z = InZ;
            }

            public static Transform FromPayload(byte[] data, int XPos, int YPos, int ZPos)
            {
                return new Transform
                {
                    X = BitConverter.ToSingle(data, XPos),
                    Y = BitConverter.ToSingle(data, YPos),
                    Z = BitConverter.ToSingle(data, ZPos)
                };
            }

            public override string ToString()
            {
                return $"X: {X}\nY: {Y}\nZ: {Z}";
            }

            public string ToIntString()
            {
                return $"X: {(int)X}\nY: {(int)Y}\nZ: {(int)Z}";
            }
        }

        /// <summary>
        /// Wheel data
        /// </summary>
        public struct WheelData
        {
            public float FrontLeft { get; set; }
            public float FrontRight { get; set; }
            public float RearLeft { get; set; }
            public float RearRight { get; set; }

            public static WheelData FromPayload(byte[] data, int FL, int FR, int RL, int RR)
            {
                return new WheelData
                {
                    FrontLeft = BitConverter.ToSingle(data, FL),
                    FrontRight = BitConverter.ToSingle(data, FR),
                    RearLeft = BitConverter.ToSingle(data, RL),
                    RearRight = BitConverter.ToSingle(data, RR)
                };
            }
        }

        #endregion

        #region Data

        public VehicleInfo Vehicle { get; set; }

        /// <summary>
        /// True if in a race, false if not (menu or otherwise)
        /// </summary>
        public bool InRace { get; set; }

        /// <summary>
        /// Current timestamp, can eventually overflow to 0
        /// </summary>
        public uint Timestamp { get; set; }

        /// <summary>
        /// Current engine data
        /// </summary>
        public EngineData Engine { get; set; }

        /// <summary>
        /// Current acceleration transform data in local space
        /// </summary>
        public Transform Acceleration { get; set; }

        /// <summary>
        /// Current velocity transform data in local space
        /// </summary>
        public Transform Velocity { get; set; }

        /// <summary>
        /// Current angular velocity transform data in local space
        /// </summary>
        public Transform AngularVelocity { get; set; }

        /// <summary>
        /// Current pitch rotation
        /// </summary>
        public float Pitch { get; set; }

        /// <summary>
        /// Current yaw rotation
        /// </summary>
        public float Yaw { get; set; }

        /// <summary>
        /// Current roll rotation
        /// </summary>
        public float Roll { get; set; }

        /// <summary>
        /// Current normalized suspension travel per wheel
        /// </summary>
        public WheelData NormalizedSuspensionTravel { get; set; }

        /// <summary>
        /// Current tire slip ratio per wheel
        /// </summary>
        public WheelData TireSlipRatio { get; set; }

        /// <summary>
        /// Current wheel rotation speed
        /// </summary>
        public WheelData WheelRotationSpeed { get; set; }

        /// <summary>
        /// If the vehicle's wheels are currently on a rumble strip. 1 = Yes, 0 = No
        /// </summary>
        public WheelData WheelOnRumbleStrip { get; set; }

        /// <summary>
        /// If the vehicle's wheels are currently in a puddle. Values are 0-1 where 1 is the deepest
        /// </summary>
        public WheelData WheelPuddleDepth { get; set; }

        /// <summary>
        /// Non-dimensional surface rumble values passed to the controller's force feedback
        /// </summary>
        public WheelData SurfaceRumble { get; set; }

        /// <summary>
        /// Current tire slip angle, normalized. 0 = 100% grip, and |angle| > 1.0 means loss of grip
        /// </summary>
        public WheelData TireSlipAngle { get; set; }

        /// <summary>
        /// Current normalized tire combined slip. 0 = 100% grip, and |slip| > 1.0 means loss of grip
        /// </summary>
        public WheelData TireCombinedSlip { get; set; }

        /// <summary>
        /// Actual suspension in travel per wheel, in meters
        /// </summary>
        public WheelData SuspensionTravel { get; set; }

        /// <summary>
        /// Current world position in meters
        /// </summary>
        public Transform Position { get; set; }

        /// <summary>
        /// Current amount of speed in meters per second
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        /// Current amount of power in watts
        /// </summary>
        public float Power { get; set; }

        /// <summary>
        /// Current amount of torque in newton-meters
        /// </summary>
        public float Torque { get; set; }

        /// <summary>
        /// Current tire temperature per wheel in celcius
        /// </summary>
        public WheelData TireTemp { get; set; }

        /// <summary>
        /// Current amount of boost
        /// </summary>
        public float Boost { get; set; }

        /// <summary>
        /// Current amount of fuel (not used in Forza Horizon)
        /// </summary>
        public float Fuel { get; set; }

        /// <summary>
        /// Total distance traveled in meters
        /// </summary>
        public float DistanceTraveled { get; set; }

        /// <summary>
        /// Current best lap time
        /// </summary>
        public float BestLap { get; set; }

        /// <summary>
        /// Last lap time
        /// </summary>
        public float LastLap { get; set; }

        /// <summary>
        /// Current lap time 
        /// </summary>
        public float CurrentLap { get; set; }

        /// <summary>
        /// Total race time
        /// </summary>
        public float TotalRaceTime { get; set; }

        /// <summary>
        /// Current lap number
        /// </summary>
        public ushort LapNumber { get; set; }

        /// <summary>
        /// Current race position
        /// </summary>
        public byte RacePosition { get; set; }

        /// <summary>
        /// Current amount of acceleration being applied
        /// </summary>
        public byte Accel { get; set; }

        /// <summary>
        /// Current amount of brake being applied
        /// </summary>
        public byte Brake { get; set; }

        /// <summary>
        /// Current amount of clutch being applied
        /// </summary>
        public byte Clutch { get; set; }

        /// <summary>
        /// Current amount of handbrake being applied
        /// </summary>
        public byte Handbrake { get; set; }

        /// <summary>
        /// Current gear
        /// </summary>
        public byte Gear { get; set; }

        /// <summary>
        /// Current amount of steering being applied
        /// </summary>
        public sbyte Steering { get; set; }

        /// <summary>
        /// Current normalized driving line
        /// </summary>
        public sbyte NormalizedDrivingLine { get; set; }

        /// <summary>
        /// Current normalized AI brake difference
        /// </summary>
        public sbyte NormalizedAIBrakeDifference { get; set; }

        #endregion

        #region Helper functions

        /// <summary>
        /// The current speed in miles per hour
        /// </summary>
        /// <returns>The forward velocity in miles per hour</returns>
        public float GetMPH() { return Math.Abs(Velocity.Z * 2.23694f); }

        /// <summary>
        /// The current speed in kilometers per hour
        /// </summary>
        /// <returns>The forward velocity in kilometers per hour</returns>
        public float GetKPH() { return Math.Abs(Velocity.Z * 3.6f); }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, options: new JsonSerializerOptions()
            {
                WriteIndented = true
            });
        }

        public static TelemetryData FromString(string data)
        {
            return JsonSerializer.Deserialize<TelemetryData>(data) ?? new TelemetryData();
        }

        /*
        public static object? GetPropValue(TelemetryData src, string propName)
        {
            if (propName.Contains("."))
            {
                string[] props = propName.Split(".");
                switch (props[0].ToLower())
                {
                    case "vehicle":
                        return typeof(VehicleInfo).GetProperty(propName).GetValue(src.Vehicle, null);
                        break;
                    case "engine":
                        return typeof(EngineData).GetProperty(propName).GetValue(src.Engine, null);
                        break;
                    case "acceleration":
                        return typeof(VehicleInfo).GetProperty(propName).GetValue(src.Vehicle, null);
                        break;
                }
            }
            else return src.GetType().GetProperty(propName).GetValue(src, null);
        }
        */

        #endregion
    }
}
