/****************/
// MSSV: 202418996
// Họ tên: Nguyễn Khánh Toàn
/****************/

namespace DeviceManagement;

public interface IRepository<T> where T : IIdentifiable
{
    void Add(T item);
    bool Remove(string id);
    T? FindById(string id);
    IReadOnlyList<T> GetAll();
    IReadOnlyList<T> FindAll(Predicate<T> condition);
}

//kho generic, ko can biet loai thiet bi cu the
public class InMemoryRepository<T> : IRepository<T> where T : IIdentifiable
{
    private readonly List<T> _items = new();

    public void Add(T item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        if (string.IsNullOrWhiteSpace(item.Id))
            throw new ArgumentException("Mã đối tượng không được rỗng.", nameof(item));

        if (_items.Any(i => i.Id == item.Id))
            throw new DuplicateIdException(item.Id);

        _items.Add(item);
    }

    public bool Remove(string id)
    {
        T? item = FindById(id);

        if (item == null)
            return false;

        return _items.Remove(item);
    }

    public T? FindById(string id) =>
        _items.FirstOrDefault(i => i.Id == id);

    //tra ban sao de ben ngoai ko sua truc tiep danh sach
    public IReadOnlyList<T> GetAll() =>
        _items.ToList().AsReadOnly();

    public IReadOnlyList<T> FindAll(Predicate<T> condition)
    {
        if (condition == null)
            throw new ArgumentNullException(nameof(condition));

        return _items.FindAll(condition).AsReadOnly();
    }
}
