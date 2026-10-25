/****************/
// MSSV: 202418996
// Họ tên: Nguyễn Khánh Toàn
/****************/

namespace DeviceManagement;

public class AllocationService<T> where T : TeachingDevice
{
    private readonly IRepository<T> _repository;
    private ISelectionStrategy<T> _strategy;

    public AllocationService(
        IRepository<T> repository,
        ISelectionStrategy<T> strategy)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
    }

    public void ChangeStrategy(ISelectionStrategy<T> newStrategy)
    {
        _strategy = newStrategy ?? throw new ArgumentNullException(nameof(newStrategy));
    }

    //loc truoc, chon va muon thiet bi sau
    public T Allocate(Predicate<T>? additionalCondition = null)
    {
        IReadOnlyList<T> candidates = _repository.FindAll(
            d => d.IsReadyForUse()
                 && (additionalCondition == null || additionalCondition(d)));

        T selected = _strategy.Select(candidates);
        selected.Borrow();
        return selected;
    }
}
