using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using XGamer.Data.Core;

namespace XGamer.Data.XML
{
    public class XmlDataProvider : IXGamerDataProvider
    {
        private static XmlDataProvider _instance;
        private static readonly object LockObject = new object();
        private List<Entities.Rom> _roms;
        private List<Entities.RomType> _romTypes;
        private List<Entities.Emulator> _emulators;

        private XmlDataProvider()
        {
        }

        public static XmlDataProvider Instance
        {
            get
            {
                lock (LockObject)
                {
                    return _instance ?? (_instance = new XmlDataProvider());
                }
            }
        }

        private List<Entities.Emulator> Emulators
        {
            get
            {
                if (_emulators == null)
                {

                    XDocument xmlDoc = GetXDocument();

                    _emulators = (from rom in xmlDoc.Descendants("Emulator")
                        select new Entities.Emulator
                        {
                            FileName = rom.Element("FileName")?.Value,
                            Parameters = rom.Element("Parameters")?.Value,
                            IdRomType = Convert.ToInt32(rom.Element("IdRomType")?.Value),
                            RomType = GetRomType(Convert.ToInt32(rom.Element("IdRomType")?.Value))
                        }).ToList();

                }

                return _emulators;
            }
        }

        private List<Entities.RomType> RomTypes
        {
            get
            {
                if (_romTypes == null)
                {
                    XDocument xmlDoc = GetXDocument();

                    _romTypes = (from romType in xmlDoc.Descendants("RomType")
                        select new Entities.RomType
                        {
                            Id = Convert.ToInt32(romType.Element("Id")?.Value),
                            Description = romType.Element("Description")?.Value,
                        }).ToList();
                }

                return _romTypes;
            }
        }

        private List<Entities.Rom> Games
        {
            get
            {
                if (_roms == null)
                {
                    XDocument xmlDoc = GetXDocument();

                    _roms = (from rom in xmlDoc.Descendants("Rom")
                        select new Entities.Rom
                        {
                            Id = Convert.ToInt32(rom.Element("Id")?.Value),
                            FileName = rom.Element("FileName")?.Value,
                            GameName = rom.Element("GameName")?.Value,
                            IdRomType = Convert.ToInt32(rom.Element("IdRomType")?.Value),
                            Poster1FileName = rom.Element("Poster1FileName")?.Value,
                            Poster2FileName = rom.Element("Poster2FileName")?.Value,
                            RomType = GetRomType(Convert.ToInt32(rom.Element("IdRomType")?.Value))
                        }).ToList();
                }

                _roms.Sort((x, y) => String.Compare(x.GameName, y.GameName, StringComparison.Ordinal));

                return _roms;
            }
        }

        private Entities.RomType GetRomType(int idRomType)
        {
            if (RomTypes.Exists(x => x.Id == idRomType))
            {
                return RomTypes.First(x => x.Id == idRomType);
            }

            throw new NullReferenceException($"Rom Type Id: {idRomType}. Doesn't exists.");
        }

        private XDocument GetXDocument()
        {
            string fileName = @".\xgamer.xml";
            if (File.Exists(fileName))
            {
                return XDocument.Load(fileName);
            }

            throw new NullReferenceException($"'{fileName}' doesn't exists.");
        }

        public IEnumerable<Entities.Rom> GetAllGames()
        {
            return Games;
        }

        public IEnumerable<Entities.Emulator> GetAllEmulators()
        {
            return Emulators;
        }

        public Entities.Rom GetGameById(int gameId)
        {
            return Games.Find(x => x.Id == gameId);
        }
    }
}