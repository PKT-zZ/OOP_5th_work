/****************/
// MSSV: 202418996
// Họ tên: Nguyễn Khánh Toàn
/****************/

namespace DeviceManagement;

public interface ISelectionStrategy<T>
{
    T Select(IReadOnlyList<T> candidates);
}

public class HighestPriorityStrategy<T> : ISelectionStrategy<T>
{
    private readonly Func<T, double> _scoreSelector;

    public HighestPriorityStrategy(Func<T, double> scoreSelector)
    {
        _scoreSelector = scoreSelector
            ?? throw new ArgumentNullException(nameof(scoreSelector));
    }

    public T Select(IReadOnlyList<T> candidates)
    {
        if (candidates == null)
            throw new ArgumentNullException(nameof(candidates));

        if (candidates.Count == 0)
            throw new NoSuitableItemException();

        T best = candidates[0];
        double bestScore = _scoreSelector(best);

        for (int i = 1; i < candidates.Count; i++)
        {
            double score = _scoreSelector(candidates[i]);

            if (score > bestScore)
            {
                best = candidates[i];
                bestScore = score;
            }
        }

        return best;
    }
}

public class FirstAvailableStrategy<T> : ISelectionStrategy<T>
{
    public T Select(IReadOnlyList<T> candidates)
    {
        if (candidates == null)
            throw new ArgumentNullException(nameof(candidates));

        if (candidates.Count == 0)
            throw new NoSuitableItemException();

        return candidates[0];
    }
}
