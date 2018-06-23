using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using XGamer.Data.Core;

namespace XGamer.Data.XML
{
    public class XMLDataProvider : IXGamerDataProvider
    {
        #region private fields

        private static XMLDataProvider instance;
        private static object lockObject = new object();
        private List<Entities.Rom> roms;
        private List<Entities.RomType> romTypes;
        private List<Entities.Emulator> emulators;

        #endregion

        #region constructor

        private XMLDataProvider()
        {
        }

        #endregion

        #region private properties

        public static XMLDataProvider Instance
        {
            get
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new XMLDataProvider();
                    }

                    return instance;
                }
            }
        }

        private List<Entities.Emulator> Emulators
        {
            get
            {
                if (this.emulators == null)
                {

                    XDocument xmlDoc = this.GetXDocument();

                    this.emulators = (from rom in xmlDoc.Descendants("Emulator")
                                                         select new Entities.Emulator
                                                         {
                                                             FileName = rom.Element("FileName").Value,
                                                             Parameters = rom.Element("Parameters").Value,
                                                             IdRomType = Convert.ToInt32(rom.Element("IdRomType").Value),
                                                             RomType = this.GetRomType(Convert.ToInt32(rom.Element("IdRomType").Value))
                                                         }).ToList();

                }

                return this.emulators;
            }
        }

        private List<Entities.RomType> RomTypes
        {
            get 
            {
                if (this.romTypes == null)
                {
                    XDocument xmlDoc = this.GetXDocument();

                    this.romTypes = (from romType in xmlDoc.Descendants("RomType")
                                     select new Entities.RomType
                                     {
                                         Id = Convert.ToInt32(romType.Element("Id").Value),
                                         Description = romType.Element("Description").Value,
                                     }).ToList();
                }

                return this.romTypes;
            }
        }

        private List<Entities.Rom> Games
        {
            get
            {
                if (this.roms == null)
                {
                    XDocument xmlDoc = this.GetXDocument();

                    this.roms = (from rom in xmlDoc.Descendants("Rom")
                                 select new Entities.Rom
                                 {
                                     Id = Convert.ToInt32(rom.Element("Id").Value),
                                     FileName = rom.Element("FileName").Value,
                                     GameName = rom.Element("GameName").Value,
                                     IdRomType = Convert.ToInt32(rom.Element("IdRomType").Value),
                                     Poster1FileName = rom.Element("Poster1FileName").Value,
                                     Poster2FileName = rom.Element("Poster2FileName").Value,
                                     RomType = this.GetRomType(Convert.ToInt32(rom.Element("IdRomType").Value))
                                 }).ToList();
                }

                this.roms.Sort((x, y) => x.GameName.CompareTo(y.GameName));

                return this.roms;
            }
        }

        #endregion

        #region private methods

        private Entities.RomType GetRomType(int idRomType)
        {
            if (this.RomTypes.Exists(x => x.Id == idRomType))
            {
                return this.RomTypes.First(x => x.Id == idRomType);
            }

            throw new NullReferenceException(string.Format("Rom Type Id: {0}. Doesn't exists.", idRomType));
        }

        private XDocument GetXDocument()
        {
            string fileName = @".\xgamer.xml";
            if (File.Exists(fileName))
            {
                return XDocument.Load(fileName);
            }

            throw new NullReferenceException(string.Format("'{0}' doesn't exists.", fileName));
        }

        #endregion

        #region public methods

        public IEnumerable<Entities.Rom> GetAllGames()
        {
            return this.Games;
        }

        public IEnumerable<Entities.Emulator> GetAllEmulators()
        {
            return this.Emulators;
        }

        public Entities.Rom GetGameById(int gameId)
        {
            return this.Games.Find(x => x.Id == gameId);
        }

        #endregion
    }
}
