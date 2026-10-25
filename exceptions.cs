/****************/
// MSSV: 202418996
// Họ tên: Nguyễn Khánh Toàn
/****************/

namespace DeviceManagement;

public class DuplicateIdException : Exception
{
    public DuplicateIdException(string duplicateId)
        : base($"Mã {duplicateId} đã tồn tại trong kho.")
    {
        DuplicateId = duplicateId;
    }

    public string DuplicateId { get; }
}

public class NoSuitableItemException : Exception
{
    public NoSuitableItemException()
        : base("Không tìm thấy thiết bị phù hợp để cấp phát.")
    {
    }
}

public class DeviceNotReadyException : Exception
{
    public DeviceNotReadyException(string deviceId, DeviceStatus currentStatus)
        : base($"Thiết bị {deviceId} chưa sẵn sàng. Trạng thái: {currentStatus}.")
    {
        DeviceId = deviceId;
        CurrentStatus = currentStatus;
    }

    public string DeviceId { get; }
    public DeviceStatus CurrentStatus { get; }
}
