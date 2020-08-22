using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
public class threadClass {
    public void setPerformace() {
        // Define variables to track the peak
        // memory usage of the process.
        long peakPagedMem = 0,
             peakWorkingSet = 0,
             peakVirtualMem = 0;
        int yPos = 0, xPos = 60;
        int StartingPosX = Console.CursorLeft, StartingPosY = Console.CursorTop;
        // Start the process.
        using (Process myProcess = Process.GetCurrentProcess()) {
            // Display the process statistics until
            // the user closes the program.
            do {
                if (!myProcess.HasExited) {
                    StartingPosX = Console.CursorLeft;
                    StartingPosY = Console.CursorTop;
                    // Refresh the current process property values.
                    myProcess.Refresh();

                    Console.SetCursorPosition(xPos, yPos); yPos++; Console.WriteLine();
                    Console.SetCursorPosition(StartingPosX, StartingPosY);

                    // Display current process statistics.

                    Console.SetCursorPosition(xPos, yPos); yPos++; Console.WriteLine($"{myProcess} -");
                    Console.SetCursorPosition(StartingPosX, StartingPosY);
                    Console.SetCursorPosition(xPos, yPos); yPos++; Console.WriteLine("-------------------------------------");
                    Console.SetCursorPosition(StartingPosX, StartingPosY);

                    Console.SetCursorPosition(xPos, yPos); yPos++; Console.WriteLine($"  Physical memory usage     : {myProcess.WorkingSet64}");
                    Console.SetCursorPosition(StartingPosX, StartingPosY);
                    Console.SetCursorPosition(xPos, yPos); yPos++; Console.WriteLine($"  Base priority             : {myProcess.BasePriority}");
                    Console.SetCursorPosition(StartingPosX, StartingPosY);
                    Console.SetCursorPosition(xPos, yPos); yPos++; Console.WriteLine($"  Priority class            : {myProcess.PriorityClass}");
                    Console.SetCursorPosition(StartingPosX, StartingPosY);
                    Console.SetCursorPosition(xPos, yPos); yPos++; Console.WriteLine($"  User processor time       : {myProcess.UserProcessorTime}");
                    Console.SetCursorPosition(StartingPosX, StartingPosY);
                    Console.SetCursorPosition(xPos, yPos); yPos++; Console.WriteLine($"  Privileged processor time : {myProcess.PrivilegedProcessorTime}");
                    Console.SetCursorPosition(StartingPosX, StartingPosY);
                    Console.SetCursorPosition(xPos, yPos); yPos++; Console.WriteLine($"  Total processor time      : {myProcess.TotalProcessorTime}");
                    Console.SetCursorPosition(StartingPosX, StartingPosY);
                    Console.SetCursorPosition(xPos, yPos); yPos++; Console.WriteLine($"  Paged system memory size  : {myProcess.PagedSystemMemorySize64}");
                    Console.SetCursorPosition(StartingPosX, StartingPosY);
                    Console.SetCursorPosition(xPos, yPos); yPos++; Console.WriteLine($"  Paged memory size         : {myProcess.PagedMemorySize64}");
                    Console.SetCursorPosition(StartingPosX, StartingPosY);

                    // Update the values for the overall peak memory statistics.
                    peakPagedMem = myProcess.PeakPagedMemorySize64;
                    peakVirtualMem = myProcess.PeakVirtualMemorySize64;
                    peakWorkingSet = myProcess.PeakWorkingSet64;

                    if (myProcess.Responding) {
                        Console.SetCursorPosition(xPos, yPos); yPos++; Console.WriteLine("Status = Running");
                        Console.SetCursorPosition(StartingPosX, StartingPosY);
                    } else {
                        Console.SetCursorPosition(xPos, yPos); yPos++; Console.WriteLine("Status = Not Responding");
                        Console.SetCursorPosition(StartingPosX + 17, StartingPosY);
                    }
                }
                yPos = 0;
            }
            while (!myProcess.WaitForExit(1000));

            Console.SetCursorPosition(xPos, yPos); yPos++; Console.WriteLine();
            Console.SetCursorPosition(StartingPosX, StartingPosY);
            Console.SetCursorPosition(xPos, yPos); yPos++; Console.WriteLine($"  Process exit code          : {myProcess.ExitCode}");
            Console.SetCursorPosition(StartingPosX, StartingPosY);

            // Display peak memory statistics for the process.
            Console.WriteLine($"  Peak physical memory usage : {peakWorkingSet}");
            Console.WriteLine($"  Peak paged memory usage    : {peakPagedMem}");
            Console.WriteLine($"  Peak virtual memory usage  : {peakVirtualMem}");
        }
    }
}
