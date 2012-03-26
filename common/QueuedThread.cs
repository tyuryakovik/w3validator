using System;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace KBProject.Common
{
  public class QueuedThread<T>
  {
    public delegate void QueProcessing(T[] msg);
    public delegate void TimeoutDelegate();

    ConcurrentQueue<T> queue;
    Thread thread;
    QueProcessing onReceive;
    EventWaitHandle[] events = new EventWaitHandle[2];
    bool selfTerminate = true;

    TimeoutDelegate onTimeout;
    int timeout = Timeout.Infinite;

    void dummy(T[] msg)
    {
      //do nothing
    }

    private void init(QueProcessing onReceive)
    {
      queue = new ConcurrentQueue<T>();
      if (onReceive == null)
        this.onReceive = dummy;
      else
        this.onReceive = onReceive;
      events[0] = new AutoResetEvent(false);   //new queue item event
      ThreadPool.QueueUserWorkItem(ThreadRun);
    }

    public QueuedThread(QueProcessing onReceive, ManualResetEvent terminateEvent)
    {
      events[1] = terminateEvent;
      selfTerminate = false;
      init(onReceive);
    }

    public QueuedThread(QueProcessing onReceive)
    {
      events[1] = new ManualResetEvent(false); //new terminate event
      init(onReceive);
    }

    public void SetTimeout(TimeoutDelegate onTimeout, int timeout)
    {
      if (onTimeout == null || timeout == 0)
        return;
      this.onTimeout = onTimeout;
      this.timeout = timeout;
    }

    public void ResetTimeout()
    {
      onTimeout = null;
      timeout = Timeout.Infinite;
    }

    public void Send(T msg)
    {
      queue.Enqueue(msg);
      events[0].Set();
    }

    void ThreadRun(Object stateInfo)
    {
      T[] outlist;
      int index;
      while ((index = WaitHandle.WaitAny(events, timeout)) != 1)
      {
        if (index == WaitHandle.WaitTimeout)
          onTimeout();
        else
        {
          if (queue.Count > 0)
          {
            outlist = new T[queue.Count];
            int n = 0;
            while (queue.TryDequeue(out outlist[n++]) && n < outlist.Length) ;
            onReceive(outlist);
          }
        }
          
      }
    }

    public int ThreadId
    {
      get
      {
        return thread.ManagedThreadId;
      }
    }

    public void Terminate()
    {
      if (selfTerminate)
        events[1].Set();
    }
  }
}
