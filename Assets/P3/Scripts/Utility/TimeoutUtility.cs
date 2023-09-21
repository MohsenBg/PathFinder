
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public static class TimeoutUtility
{
  static int timeOut = 10;

  public static async Task<bool> ExecuteWithTimeout(Action action)
  {
    using (var cancellationTokenSource = new CancellationTokenSource())
    {
      var task = Task.Run(action, cancellationTokenSource.Token);

      // Wait for the task to complete within the timeout period
      if (await Task.WhenAny(task, Task.Delay(timeOut)) == task)
      {
        // The task completed successfully within the timeout
        cancellationTokenSource.Cancel();
        return true;
      }
      else
      {
        // The task did not complete within the timeout
        Debug.LogWarning($"{nameof(action)} is most likely an infinite loop");
        return false;
      }
    }
  }
}

