using System;
using System.Diagnostics;
using System.IO;
using XGamer.Data.Entities;

namespace XGamer.Core
{
    public class EmulatorManager : IDisposable
    {
        private static EmulatorManager _instance;
        private Process _process;

        private EmulatorManager()
        {
            _instance = null;
            _process = null;
        }

        public static EmulatorManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EmulatorManager();
                }

                return _instance;
            }
        }

        public bool RunGame(Emulator emulator, Rom game)
        {
            Environment.CurrentDirectory = Path.Combine(XGamerEnvironment.EmulatorsPath, emulator.RomType.Description);
            string emulatorPath = emulator.FileName;
            string romPath = Path.Combine(XGamerEnvironment.RomsPath, game.RomType.Description);
            string parameters;
            if (game.IdRomType == 3)
            {
                parameters = string.Format(emulator.Parameters, game.FileName);
            }
            else 
            {
                parameters = string.Format(emulator.Parameters, Path.Combine(romPath, game.FileName));
            }

            _process = new Process
            {
                StartInfo = new ProcessStartInfo(emulatorPath, parameters)
            };

            try
            {
                _process.Start();
                return true;
            }
            catch
            {
                _process.Dispose();
                _process = null;
                return false;   
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool type = false)
        {
            if (_process != null)
            {
                _process.Dispose();
                _process = null;
            }
        }
    }
}