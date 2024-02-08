using System.Collections.Generic;
using System.Linq;

namespace MaM.Helpers;

public class DistributionBag<T>
{
  private readonly List<T> _items;
  private readonly Dictionary<T, int> _distributionTemplate;

  private readonly bool _shuffleBagOnFill;
  private readonly bool _shuffleBagOnTake;

  private readonly Stack<T> _history;

  public DistributionBag(Dictionary<T, int> distributionTemplate, bool shuffleBagOnFill = true, bool shuffleBagOnTake = false)
  {
    _items = new List<T>();

    _distributionTemplate = distributionTemplate;

    _shuffleBagOnFill = shuffleBagOnFill;
    _shuffleBagOnTake = shuffleBagOnTake;

    _history = new Stack<T>();

    Fill();
  }

  private void Fill()
  {
    foreach (var pair in _distributionTemplate)
    {
      for (var i = 0; i < pair.Value; i++)
      {
        _items.Add(pair.Key);
      }
    }

    if (_shuffleBagOnFill)
    {
      _items.Shuffle();
    }
  }

  public T Take()
  {
    if (_items.Count == 0)
    {
      Fill();
    }

    if (_shuffleBagOnTake)
    {
      _items.Shuffle();
    }

    var chosen = _items[0];
    _items.RemoveAt(0);
    _history.Push(chosen);
    return chosen;
  }

  public List<T> Take(int count)
  {
    var chosen = new List<T>();
    while (chosen.Count < count)
    {
      chosen.Add(Take());
    }
    return chosen;
  }

  public T GetMostRecent()
    => _history.Count > 0 ? _history.Peek() : default;

  public List<T> GetMostRecent(int count)
    => _history.Take(count).ToList();

  public Stack<T> GetHistory()
    => _history;

  public T Retake()
  {
    if (_history.TryPop(out var result))
    {
      _items.Add(result);
    }

    _items.Shuffle();
    return Take();
  }

  public List<T> Retake(int count)
  {
    for (var i = 0; i < count; i++)
    {
      if (_history.Count == 0)
      {
        break;
      }

      _items.Add(_history.Pop());
    }

    return Take(count);
  }
}