using System.IO;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using System;

namespace DSPRE {

    /// <summary>
    /// Class to store ROM data from GEN IV Pokémon games
    /// </summary>

    public class RomInfo {
        public string romID { get; private set; }
        public string workDir { get; private set; }
        public string gameVersion { get; private set; }
        public string gameName { get; private set; }
        public string gameLanguage { get; private set; }
        public long headerTableOffset { get; private set; }
        public string syntheticOverlayPath { get; private set; }
        public string OWSpriteDirPath { get; private set; }

        private string interiorBuildingsPath;
        private string exteriorBuildingModelsPath;

        public string areaDataDirPath { get; private set; }
        public string OWtablePath { get; private set; }
        public string mapTexturesDirPath { get; private set; }
        public string buildingTexturesDirPath { get; private set; }
        public string buildingConfigFilesPath { get; private set; }
        public string matrixDirPath { get; private set; }
        public string mapDirPath { get; private set; }
        public string eventsDirPath { get; private set; }
        public string scriptDirPath { get; private set; }
        public string textArchivesPath { get; private set; }
        public string encounterDirPath { get; private set; }
        public string trainerDataDirPath { get; private set; }
        public string[] narcPaths { get; private set; }
        public string[] extractedNarcDirs { get; private set; }

        public int nullEncounterID { get; private set; }
        public int attackNamesTextNumber { get; private set; }
        public int pokémonNamesTextNumber { get; private set; }
        public int itemNamesTextNumber { get; private set; }

        public readonly int internalNameLength = 16;
        public string internalNamesLocation { get; private set; }


        #region Constructors (1)
        public RomInfo(string id, string workDir) {
            romID = id;
            this.workDir = workDir;

            LoadGameVersion();
            if (gameVersion == null)
                return;

            LoadGameName();
            LoadGameLanguage();
            LoadHeaderTableOffset();

            internalNamesLocation = this.workDir + @"data\fielddata\maptable\mapname.bin";

            mapTexturesDirPath = this.workDir + @"unpacked\maptex";
            buildingTexturesDirPath = this.workDir + @"unpacked\TextureBLD";
            buildingConfigFilesPath = this.workDir + @"unpacked\area_build";
            areaDataDirPath = this.workDir + @"unpacked\area_data";
            textArchivesPath = this.workDir + @"unpacked\msg";
            matrixDirPath = this.workDir + @"unpacked\matrix";
            trainerDataDirPath = this.workDir + @"unpacked\trainerdata";
            mapDirPath = this.workDir + @"unpacked\maps";
            encounterDirPath = this.workDir + @"unpacked\wildPokeData";
            eventsDirPath = this.workDir + @"unpacked\events";
            scriptDirPath = this.workDir + @"unpacked\scripts";
            syntheticOverlayPath = this.workDir + @"unpacked\syntheticOverlayNarc";
            OWSpriteDirPath = this.workDir + @"unpacked\overworlds";

            SetNullEncounterID();           
            SetBuildingModelsDirPath();
            SetOWtablePath();

            SetAttackNamesTextNumber();
            SetPokémonNamesTextNumber();
            SetItemNamesTextNumber();

            SetNarcDirs();
        }
        #endregion

        #region Methods (22)
        private void SetNullEncounterID() {
            switch (gameVersion) {
                case "D":
                case "P":
                case "Plat":
                    nullEncounterID = 65535;
                    break;
                case "HG":
                case "SS":
                    nullEncounterID = 255;
                    break;
            }
        }
        public void SetNarcDirs () {
            extractedNarcDirs = new string[] {
                syntheticOverlayPath,
                textArchivesPath,

                matrixDirPath,

                mapDirPath,
                exteriorBuildingModelsPath,
                buildingConfigFilesPath,
                buildingTexturesDirPath,
                mapTexturesDirPath,
                areaDataDirPath,

                eventsDirPath,
                trainerDataDirPath,
                OWSpriteDirPath,

                scriptDirPath,
                encounterDirPath,

                interiorBuildingsPath
            };
            
            switch (gameVersion) {
                case "D":
                case "P":
                    narcPaths = new string[] {
                        @"data\data\weather_sys.narc",
                        @"data\msgdata\msg.narc",

                        @"data\fielddata\mapmatrix\map_matrix.narc",

                        @"data\fielddata\land_data\land_data_release.narc",
                        @"data\fielddata\build_model\build_model.narc",
                        @"data\fielddata\areadata\area_build_model\area_build.narc",
                        @"data\fielddata\areadata\area_build_model\areabm_texset.narc",
                        @"data\fielddata\areadata\area_map_tex\map_tex_set.narc",
                        @"data\fielddata\areadata\area_data.narc",

                        @"data\fielddata\eventdata\zone_event_release.narc",
                        @"data\poketool\trainer\trdata.narc",
                        @"data\data\mmodel\mmodel.narc",

                        @"data\fielddata\script\scr_seq_release.narc",
                        @"data\fielddata\encountdata\" + char.ToLower(gameVersion[0]) + '_' + "enc_data.narc"

                    };
                    break;
                case "Plat":
                    narcPaths = new string[] {
                        @"data\data\weather_sys.narc",
                        @"data\msgdata\" + gameVersion.Substring(0,2).ToLower() + '_' + "msg.narc",

                        @"data\fielddata\mapmatrix\map_matrix.narc",

                        @"data\fielddata\land_data\land_data.narc",
                        @"data\fielddata\build_model\build_model.narc",
                        @"data\fielddata\areadata\area_build_model\area_build.narc",
                        @"data\fielddata\areadata\area_build_model\areabm_texset.narc",
                        @"data\fielddata\areadata\area_map_tex\map_tex_set.narc",
                        @"data\fielddata\areadata\area_data.narc",

                        @"data\fielddata\eventdata\zone_event.narc",
                        @"data\poketool\trainer\trdata.narc",
                        @"data\data\mmodel\mmodel.narc",

                        @"data\fielddata\script\scr_seq.narc",
                        @"data\fielddata\encountdata\" + gameVersion.Substring(0,2).ToLower() + '_' + "enc_data.narc"
                    };
                    break;
                case "HG":
                case "SS":
                    narcPaths = new string[] {
                        @"data\a\0\2\8",
                        @"data\a\0\2\7",

                        @"data\a\0\4\1",

                        @"data\a\0\6\5",
                        @"data\a\0\4\0",
                        @"data\a\0\4\3",
                        @"data\a\0\7\0",
                        @"data\a\0\4\4",
                        @"data\a\0\4\2",

                        @"data\a\0\3\2",
                        @"data\a\0\5\5",
                        @"data\a\0\8\1",

                        @"data\a\0\1\2",
                        @"data\a\0\3\7",

                        @"data\a\1\4\8"
                    };
                    break;
                    /*
                default:
                    extractedNarcDirs = new string[] {
                        buildingTexturesDirPath,
                        buildingConfigFilesPath,
                        areaDataDirPath,
                        mapTexturesDirPath,
                        eventsDirPath,
                        mapDirPath,
                        matrixDirPath,
                        textArchivesPath,
                        scriptDirPath,
                        GetOWSpriteDirPath(),
                        GetTrainerDataDirPath(),
                        encounterDirPath(),
                    };
                    narcPaths = new string[] {
                        @"data\a\0\7\0",
                        @"data\a\0\4\3",
                        @"data\a\0\4\2",
                        @"data\a\0\4\4",
                        @"data\a\0\4\0",
                        @"data\a\0\3\2",
                        @"data\a\0\6\5",
                        @"data\a\0\4\1",
                        @"data\a\0\2\7",
                        @"data\a\0\1\2",
                        @"data\a\0\8\1",
                        @"data\a\0\5\5",
                        @"data\a\0\3\7",
                        @"data\a\1\4\8"
                    };
                    break;
                    */
            }
        }
        public void SetBuildingModelsDirPath() {
            switch (gameVersion) {
                case "D":
                case "P":
                case "Plat":
                    exteriorBuildingModelsPath = workDir + @"unpacked\DPPtBuildings";
                    break;
                default:
                    interiorBuildingsPath = workDir + @"unpacked\HGSSBuildingsIN";
                    exteriorBuildingModelsPath = workDir + @"unpacked\HGSSBuildingsOUT";
                    break;
            }
        }
        public string GetBuildingModelsDirPath(bool interior) {
            if (interior)
                return interiorBuildingsPath;
            else
                return exteriorBuildingModelsPath;
        }
        public int GetBuildingCount(bool interior) {
            if (interior)
                return Directory.GetFiles(interiorBuildingsPath).Length;
            else
                return Directory.GetFiles(exteriorBuildingModelsPath).Length;
        }

        public void SetOWtablePath () {
            switch (gameVersion) {
                case "D":
                case "P":
                case "Plat":
                    OWtablePath = workDir + "overlay" + "\\" + "overlay_0005.bin";
                    break;
                default:
                    OWtablePath = workDir + "overlay" + "\\" + "overlay_0001.bin";
                    break;
            }
        }
        public void LoadHeaderTableOffset() {
            Dictionary<string, int> offsets = new Dictionary<string, int>() {
                ["ADAE"] = 0xEEDBC,
                ["APAE"] = 0xEEDBC,

                ["ADAS"] = 0xEEE08,
                ["APAS"] = 0xEEE08,

                ["ADAI"] = 0xEED70,
                ["APAI"] = 0xEED70,

                ["ADAF"] = 0xEEDFC,
                ["APAF"] = 0xEEDFC,

                ["ADAD"] = 0xEEDCC,
                ["APAD"] = 0xEEDCC,

                ["ADAJ"] = 0xF0C28,
                ["APAJ"] = 0xF0C28,

                ["CPUE"] = 0xE601C,
                ["CPUS"] = 0xE60B0,
                ["CPUI"] = 0xE6038,
                ["CPUF"] = 0xE60A4,
                ["CPUD"] = 0xE6074,
                ["CPUJ"] = 0xE56F0,

                ["IPKE"] = 0xF6BE0,
                ["IPGE"] = 0xF6BE0,

                ["IPKS"] = 0xF6BC8,
                ["IPGS"] = 0xF6BD0,

                ["IPKI"] = 0xF6B58,
                ["IPGI"] = 0xF6B58,

                ["IPKF"] = 0xF6BC4,
                ["IPGF"] = 0xF6BC4,

                ["IPKD"] = 0xF6B94,
                ["IPGD"] = 0xF6B94,

                ["IPKJ"] = 0xF6390,
                ["IPGJ"] = 0xF6390
            };
            headerTableOffset = offsets[this.romID];
        }
        public void LoadGameVersion() {
            Dictionary<string, string> versions = new Dictionary<string, string>() {
                ["ADAE"] = "D",
                ["ADAS"] = "D",
                ["ADAI"] = "D",
                ["ADAF"] = "D",
                ["ADAD"] = "D",
                ["ADAJ"] = "D",

                ["APAE"] = "P",
                ["APAS"] = "P",
                ["APAI"] = "P",
                ["APAF"] = "P",
                ["APAD"] = "P",
                ["APAJ"] = "P",

                ["CPUE"] = "Plat",
                ["CPUS"] = "Plat",
                ["CPUI"] = "Plat",
                ["CPUF"] = "Plat",
                ["CPUD"] = "Plat",
                ["CPUJ"] = "Plat",

                ["IPKE"] = "HG",
                ["IPKS"] = "HG",
                ["IPKI"] = "HG",
                ["IPKF"] = "HG",
                ["IPKD"] = "HG",
                ["IPKJ"] = "HG",

                ["IPGE"] = "SS",
                ["IPGS"] = "SS",
                ["IPGI"] = "SS",
                ["IPGF"] = "SS",
                ["IPGD"] = "SS",
                ["IPGJ"] = "SS"
            };
            try {
                gameVersion = versions[romID];
            } catch (KeyNotFoundException) {
                System.Windows.Forms.MessageBox.Show("The ROM you attempted to load is not supported.\nYou can only load Gen IV Pokémon ROMS, for now.", "Unsupported ROM",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadGameName() {
            switch (gameVersion) {
                case "D":
                    gameName = "Diamond";
                    break;
                case "P":
                    gameName = "Pearl";
                    break;
                case "Plat":
                    gameName = "Platinum";
                    break;
                case "HG":
                    gameName = "HeartGold";
                    break;
                case "SS":
                    gameName = "SoulSilver";
                    break;
            }
        }
        public void LoadGameLanguage() {
            switch (romID) {
                case "ADAE":
                case "APAE":
                case "CPUE":
                case "IPKE":
                case "IPGE":
                    gameLanguage = "ENG";
                    break;

                case "ADAS":
                case "APAS":
                case "CPUS":
                case "IPKS":
                case "IPGS":
                case "LATA":
                    gameLanguage = "ESP";
                    break;

                case "ADAI":
                case "APAI":
                case "CPUI":
                case "IPKI":
                case "IPGI":
                    gameLanguage = "ITA";
                    break;

                case "ADAF":
                case "APAF":
                case "CPUF":
                case "IPKF":
                case "IPGF":
                    gameLanguage = "FRA";
                    break;

                case "ADAD":
                case "APAD":
                case "CPUD":
                case "IPKD":
                case "IPGD":
                    gameLanguage = "GER";
                    break;

                default:
                    gameLanguage = "JAP";
                    break;
            }
        }
        public int GetHeaderCount() {
            return (int)new FileInfo(internalNamesLocation).Length / internalNameLength;
        }
        public void SetAttackNamesTextNumber() {
            switch (gameVersion) {
                case "D":
                case "P":
                    attackNamesTextNumber = 588;
                    break;
                case "Plat":
                    attackNamesTextNumber = 647;
                    break;
                default:
                    if (gameLanguage == "JAP")
                        attackNamesTextNumber = 739;
                    else
                        attackNamesTextNumber = 750;
                    break;
            }
        }
        public void SetItemNamesTextNumber() {
            switch (gameVersion) {
                case "D":
                case "P":
                    itemNamesTextNumber = 344;
                    break;
                case "Plat":
                    itemNamesTextNumber = 392;
                    break;
                default:
                    if (gameLanguage == "JAP")
                        itemNamesTextNumber = 219;
                    else
                        itemNamesTextNumber = 222;
                    break;
            }
        }
        public int GetLocationNamesTextNumber() {
            int fileNumber;
            switch (gameVersion) {
                case "D":
                case "P":
                    fileNumber = 382;
                    break;
                case "Plat":
                    fileNumber = 433;
                    break;
                default:
                    if (gameLanguage == "JAP") 
                        fileNumber = 272;
                    else 
                        fileNumber = 279;
                    break;
            }
            return fileNumber;
        }
        public int GetItemScriptFileNumber() {
            int fileNumber;
            switch (gameVersion) {
                case "D":
                case "P":
                    fileNumber = 370;
                    break;
                case "Plat":
                    fileNumber = 404;
                    break;
                default:
                    fileNumber = 141;
                    break;
            }
            return fileNumber;
        }
        public void SetPokémonNamesTextNumber() {
            switch (gameVersion) {
                case "D":
                case "P":
                    pokémonNamesTextNumber = 362;
                    break;
                case "Plat":
                    pokémonNamesTextNumber = 412;
                    break;
                default:
                    if (gameLanguage == "JAP")
                        pokémonNamesTextNumber = 232;
                    else
                        pokémonNamesTextNumber = 237;
                    break;
            }
        }
        public int GetTrainerNamesMessageNumber() {
            int fileNumber;
            switch (gameVersion) {
                case "D":
                case "P":
                    fileNumber = 559;
                    break;
                case "Plat":
                    fileNumber = 618;
                    break;
                default:
                    if (gameLanguage == "JAP")
                        fileNumber = 719;
                    else
                        fileNumber = 729;
                    break;
            }
            return fileNumber;
        }
        public int GetTrainerClassMessageNumber() {
            int fileNumber;
            switch (gameVersion) {
                case "D":
                case "P":
                    fileNumber = 560;
                    break;
                case "Plat":
                    fileNumber = 619;
                    break;
                default:
                    if (gameLanguage == "JAP")
                        fileNumber = 720;
                    else
                        fileNumber = 730;
                    break;
            }
            return fileNumber;
        }
        public int GetAreaDataCount() {
            return Directory.GetFiles(areaDataDirPath).Length; ;
        }
        public int GetMapTexturesCount() {
            return Directory.GetFiles(mapTexturesDirPath).Length;
        }
        public int GetBuildingTexturesCount() {
            return Directory.GetFiles(buildingTexturesDirPath).Length;
        }
        public int GetMatrixCount() {
            return Directory.GetFiles(matrixDirPath).Length;
        }
        public int GetTextArchivesCount() {
            return Directory.GetFiles(textArchivesPath).Length;
        }
        public int GetMapCount() {
            return Directory.GetFiles(mapDirPath).Length;
        }
        public int GetEventCount() {
            return Directory.GetFiles(eventsDirPath).Length;
        }
        public int GetScriptCount() {
            return Directory.GetFiles(scriptDirPath).Length;
        }
        #endregion
    }
}