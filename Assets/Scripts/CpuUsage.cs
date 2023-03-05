using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using UnityEngine;

public class CpuUsage
{
    private float updateInterval = 1;

    private int processorCount;

    private float cpuUsage;

    private Thread cpuThread;

    private float lastCpuUsage;

    // Start is called before the first frame update
    public CpuUsage()
    {
        cpuThread = new Thread(UpdateCPUUsage)
        {
            IsBackground = true,
            Priority = System.Threading.ThreadPriority.BelowNormal
        };
    }

    ~CpuUsage()
    {
        //UnityEngine.Debug.Log("~CpuUsage()");
        cpuThread?.Abort();
    }

    public void Start()
    {
        cpuThread?.Start();
    }

    public void Stop()
    {
        cpuThread?.Abort();
    }

    public float GetCpuUsage()
    {
        // for more efficiency skip if nothing has changed
        if (Mathf.Approximately(lastCpuUsage, cpuUsage))
            return lastCpuUsage;

        // the first two values will always be "wrong"
        // until _lastCpuTime is initialized correctly
        // so simply ignore values that are out of the possible range
        if (cpuUsage < 0 || cpuUsage > 100)
            return lastCpuUsage;

        lastCpuUsage = cpuUsage;

        return cpuUsage;
    }

    public void UpdateProcessorCount()
    {
        processorCount = SystemInfo.processorCount / 2;
    }

    /// <summary>
    /// Runs in Thread
    /// </summary>
    private void UpdateCPUUsage()
    {
        var lastCpuTime = new TimeSpan(0);

        // This is ok since this is executed in a background thread
        while (true)
        {
            var cpuTime = new TimeSpan(0);

            // Get a list of all running processes in this PC
            var AllProcesses = Process.GetProcesses();

            // Sum up the total processor time of all running processes
            cpuTime = AllProcesses.Aggregate(cpuTime, (current, process) => current + process.TotalProcessorTime);

            // get the difference between the total sum of processor times
            // and the last time we called this
            var newCPUTime = cpuTime - lastCpuTime;

            // update the value of _lastCpuTime
            lastCpuTime = cpuTime;

            // The value we look for is the difference, so the processor time all processes together used
            // since the last time we called this divided by the time we waited
            // Then since the performance was optionally spread equally over all physical CPUs
            // we also divide by the physical CPU count
            cpuUsage = 100f * (float)newCPUTime.TotalSeconds / updateInterval / processorCount;

            // Wait for UpdateInterval
            Thread.Sleep(Mathf.RoundToInt(updateInterval * 1000));
        }
    }
}
