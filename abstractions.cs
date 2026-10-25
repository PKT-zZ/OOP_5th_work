/****************/
// MSSV: 202418996
// Họ tên: Nguyễn Khánh Toàn
/****************/

namespace DeviceManagement;

public enum DeviceStatus
{
    Available,
    Borrowed,
    UnderMaintenance
}

public interface IIdentifiable
{
    string Id { get; }
}

//lop truu tuong chua phan chung cua cac thiet bi
public abstract class TeachingDevice : IIdentifiable
{
    protected TeachingDevice(
        string id,
        string name,
        string storageLocation)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Mã thiết bị không được rỗng.", nameof(id));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tên thiết bị không được rỗng.", nameof(name));

        if (string.IsNullOrWhiteSpace(storageLocation))
            throw new ArgumentException("Vị trí lưu trữ không được rỗng.", nameof(storageLocation));

        Id = id.Trim();
        Name = name.Trim();
        StorageLocation = storageLocation.Trim();
        Status = DeviceStatus.Available;
    }

    public string Id { get; }
    public string Name { get; }
    public string StorageLocation { get; }
    public DeviceStatus Status { get; protected set; }

    public abstract bool IsReadyForUse();
    public abstract double GetDefaultPriorityScore();

    public void Borrow()
    {
        if (!IsReadyForUse())
            throw new DeviceNotReadyException(Id, Status);

        Status = DeviceStatus.Borrowed;
    }

    public void Return()
    {
        if (Status != DeviceStatus.Borrowed)
            throw new InvalidOperationException("Thiết bị chưa được mượn.");

        Status = DeviceStatus.Available;
    }

    public override string ToString() =>
        $"[{Id}] {GetType().Name} - {Name} | Kho: {StorageLocation} | {Status}";
}
