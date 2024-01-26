using System.Collections.Generic;

namespace MaM.Helpers
{
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

    //TODO - test Take(int count)
    public List<T> Take(int count)
    {
      var chosen = new List<T>();
      while (chosen.Count < count)
      {
        chosen.Add(Take());
      }
      return chosen;
    }

    //TODO - test Retake()
    public T Retake()
    {
      _items.Add(_history.Pop());
      _items.Shuffle();
      return Take();
    }

    //TODO - test Retake(int count)
    public List<T> Retake(int count)
    {
      for (var i = 0; i < count; i++)
      {
        if (_history.Count == 0)
        {
          continue;
        }

        _items.Add(_history.Pop());
      }

      return Take(count);
    }

    //TODO - test GetLastTaken()
    public T GetMostRecent()
    {
      return _history.Peek();
    }

    //TODO - test GetLastTaken(int count)
    public List<T> GetMostRecent(int count)
    {
      var items = new List<T>();
      while (items.Count < count)
      {
        if (_history.Count == 0)
        {
          continue;
        }

        _items.Add(_history.Peek());
      }
      return items;
    }

    //TODO - test GetFullHistory()
    public Stack<T> GetFullHistory()
      => _history;
  }
}