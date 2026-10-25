/****************/
// MSSV: 202418996
// Họ tên: Nguyễn Khánh Toàn
/****************/

namespace DeviceManagement;

public class ProjectorRemote : TeachingDevice
{
    public ProjectorRemote(
        string id,
        string name,
        string storageLocation,
        int batteryLevel)
        : base(id, name, storageLocation)
    {
        if (batteryLevel < 0 || batteryLevel > 100)
            throw new ArgumentOutOfRangeException(nameof(batteryLevel));

        BatteryLevel = batteryLevel;
    }

    public int BatteryLevel { get; }

    public override bool IsReadyForUse() =>
        Status == DeviceStatus.Available && BatteryLevel >= 20;

    public override double GetDefaultPriorityScore() => BatteryLevel;
}

public class AirConditionerRemote : TeachingDevice
{
    private readonly HashSet<string> _compatibleRooms;

    public AirConditionerRemote(
        string id,
        string name,
        string storageLocation,
        IEnumerable<string> compatibleRooms)
        : base(id, name, storageLocation)
    {
        if (compatibleRooms == null)
            throw new ArgumentNullException(nameof(compatibleRooms));

        _compatibleRooms = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (string room in compatibleRooms)
        {
            if (string.IsNullOrWhiteSpace(room))
                throw new ArgumentException("Mã phòng không được rỗng.", nameof(compatibleRooms));

            _compatibleRooms.Add(room.Trim());
        }
    }

    public IReadOnlySet<string> CompatibleRooms => _compatibleRooms;

    public bool IsCompatibleWith(string roomId) =>
        !string.IsNullOrWhiteSpace(roomId) && _compatibleRooms.Contains(roomId.Trim());

    public override bool IsReadyForUse() =>
        Status == DeviceStatus.Available;

    //diem mac dinh, viec loc theo phong do service xu ly
    public override double GetDefaultPriorityScore() =>
        _compatibleRooms.Count > 0 ? 100 : 0;
}

public class WirelessMicrophone : TeachingDevice
{
    public WirelessMicrophone(
        string id,
        string name,
        string storageLocation,
        int batteryLevel,
        int signalQuality)
        : base(id, name, storageLocation)
    {
        if (batteryLevel < 0 || batteryLevel > 100)
            throw new ArgumentOutOfRangeException(nameof(batteryLevel));

        if (signalQuality < 0 || signalQuality > 100)
            throw new ArgumentOutOfRangeException(nameof(signalQuality));

        BatteryLevel = batteryLevel;
        SignalQuality = signalQuality;
    }

    public int BatteryLevel { get; }
    public int SignalQuality { get; }

    public override bool IsReadyForUse() =>
        Status == DeviceStatus.Available && BatteryLevel >= 30 && SignalQuality >= 40;

    public override double GetDefaultPriorityScore() =>
        (BatteryLevel + SignalQuality) / 2.0;
}
