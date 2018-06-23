using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using XGamer.Data.Entities;

namespace XGamer.Core
{
    public class EmulatorManager : IDisposable
    {
        private static EmulatorManager instance;
        private Process process;

        private EmulatorManager()
        {
            instance = null;
            this.process = null;
        }

        public static EmulatorManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EmulatorManager();
                }

                return instance;
            }
        }

        public bool RunGame(Emulator emulator, Rom game)
        {
            Environment.CurrentDirectory = Path.Combine(XGamerEnvironment.EmulatorsPath, emulator.RomType.Description);
            string emulatorPath = emulator.FileName;
            string romPath = Path.Combine(XGamerEnvironment.RomsPath, game.RomType.Description);
            string parameters = string.Empty;
            if (game.IdRomType == 3)
            {
                parameters = string.Format(emulator.Parameters, game.FileName);
            }
            else 
            {
                parameters = string.Format(emulator.Parameters, Path.Combine(romPath, game.FileName));
            }

            this.process = new Process();
            this.process.StartInfo = new ProcessStartInfo(emulatorPath, parameters);
            
            try
            {
                this.process.Start();
                return true;
            }
            catch
            {
                this.process.Dispose();
                this.process = null;
                return false;   
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool type = false)
        {
            if (this.process != null)
            {
                this.process.Dispose();
                this.process = null;
            }
        }
    }
}
