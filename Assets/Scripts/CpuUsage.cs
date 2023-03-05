using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using UnityEngine;

public class CpuUsage
{
    private float updateInterval = 1;

    private int processorCount = 0;

    private float cpuUsage = 0f;

    private Thread cpuThread = null;

    private bool keepAlive = false;

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
        keepAlive = false;
        cpuThread?.Abort();
    }

    public void Start()
    {
        keepAlive = true;
        cpuThread?.Start();
    }

    public void Stop()
    {
        keepAlive = false;
        cpuThread?.Abort();
    }

    public float GetCpuUsage()
    {
        if (cpuUsage > 100)
        {
            return 0;
        }

        return cpuUsage;
    }

    public void UpdateProcessorCount()
    {
        processorCount = SystemInfo.processorCount / 2;
    }

    /// Runs in Thread
    private void UpdateCPUUsage()
    {
        var lastCpuTime = new TimeSpan(0);

        while (keepAlive)
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
