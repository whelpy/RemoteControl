using LibreHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControl
{
    public class SensorReader : IDisposable
    {
        public class SensorInfo
        {
            public int CpuLoad { get; set; }
            public int GpuLoad { get; set; }
            public int GpuFPS { get; set; }
            public int MemLoad { get; set; }

        }        

        private Computer _computer;

        public SensorReader()
        {
            _computer = new Computer
            {
                IsCpuEnabled = true,
                IsGpuEnabled = true,
                IsMemoryEnabled = true,
                IsMotherboardEnabled = false,
                IsControllerEnabled = false,
                IsNetworkEnabled = false,
                IsStorageEnabled = false
            };
            _computer.Open();
        }

        public SensorInfo GetData()
        {
            _computer.Reset();
            var res = new SensorInfo();            

            var cpu = _computer.Hardware.FirstOrDefault(x => x.HardwareType == HardwareType.Cpu);
            var gpu = _computer.Hardware.FirstOrDefault(x => x.HardwareType == HardwareType.GpuNvidia || x.HardwareType == HardwareType.GpuAmd || x.HardwareType == HardwareType.GpuIntel);
            var mem = _computer.Hardware.FirstOrDefault(x => x.HardwareType == HardwareType.Memory);
            
            if (cpu !=null)
            {
                var load = cpu.Sensors.FirstOrDefault(x => x.Name == "CPU Total");
                if (load != null && load.Value != null)
                    res.CpuLoad = (int)load.Value.Value;
            }

            if (gpu != null)
            {
                var fps = gpu.Sensors.FirstOrDefault(x => x.Name == "Fullscreen FPS");
                if (fps != null && fps.Value != null)
                    res.GpuFPS = (int)fps.Value.Value;

                var load = gpu.Sensors.FirstOrDefault(x => x.Name == "D3D High Priority Compute");
                if (load != null && load.Value != null)
                    res.GpuLoad = (int)load.Value.Value;
            }

            if (mem != null)
            {
                var used = mem.Sensors.FirstOrDefault(x => x.Name == "Memory Used");
                if (used != null && used.Value != null)
                    res.MemLoad = (int)used.Value.Value;
            }


            return res;
        }

        public void Dispose()
        {
            _computer.Close();
        }
    }
}
