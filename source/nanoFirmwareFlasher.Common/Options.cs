using CommandLine;
using System.Collections.Generic;

namespace nanoFirmwareFlasher.Common
{
    /*
     * 
     *  You can 'list' devices
     *  You can 'backup' from a device
     *  You can 'erase' a device
     *  You can 'write' to a device
     *  
     *  If you 'list' :
     *  - You choose a 'target' platform. 'target' is always required : 'stm32dfu', 'stm32jtag', 'esp32', 'ti'
     *  - Q/A : Does 'esp32' and 'ti' could 'list' something ?
     *  
     *  If you 'backup' :
     *  - You choose a 'target' platform. 'target' is always required : 'stm32dfu', 'stm32jtag', 'esp32', 'ti'
     *      - For 'stm32dfu' and 'stm32jtag', you choose a 'device'. If missing, take the first one. Or all ?
     *      - For 'esp32', you need to define 'port', 'baud', and 'freq' => 'port' required. The rest ?
     *      - For 'TI' ?
     *  
     *  If you 'erase' :
     *  - You choose a 'target' platform. 'target' is always required : 'stm32dfu', 'stm32jtag', 'esp32', 'ti'
     *      - For 'stm32dfu' and 'stm32jtag', you choose a 'device'. DO NOT erase if not provided
     *      - For 'esp32', you need to define 'port', 'baud', and 'freq' => 'port' required. The rest ?
     *      - For 'TI' ?
     *  
     *  If you 'write' :
     *  - You choose a 'target' platform. 'target' is always required : 'stm32dfu', 'stm32jtag', 'esp32', 'ti'
     *      - For 'stm32dfu' and 'stm32jtag', you choose a 'device'. Required.
     *      - For 'esp32', you need to define 'port', 'baud', and 'freq' => at least 'port' required
     *      - For 'TI' ?
     *  - Set 'image' to write from a local file
     *  - Set 'community' to write after downloaded from the internet
     *      - 'image' and 'community' are exclusive
     *  - With 'community' you can choose a 'nightly' version to download. If missing, the 'stable' version will be downloaded.
     *  - Set 'address' if the file is a bin one, and need to be written to a particular address
     *  
     */
    public enum Platform
    {
        stm32dfu,
        stm32jtag,
        esp32,
        ti
    }

    public abstract class ACommonOptions
    {
        [Option("platform", Required = true, HelpText = "Target platform. Acceptable values are: esp32, stm32dfu, stm32jtag, ti.")]
        public Platform Platform { get; set; }

        #region STM32/JTAG specific
        [Option("device", HelpText = "ID of the DFU/JTAG STM32 device to update. If not specified the first connected device will be used.")]
        public string DeviceId { get; set; }
        #endregion

        #region ESP32 specific
        [Option(
            "port",
            Required = false,
            Default = null,
            HelpText = "Serial port where device is connected to.")]
        public string SerialPort { get; set; }

        [Option(
            "baud",
            Required = false,
            Default = 921600,
            HelpText = "Baud rate to use for the serial port.")]
        public int BaudRate { get; set; }

        [Option(
            "flashfreq",
            Required = false,
            Default = 40,
            HelpText = "Flash frequency to use [MHz].")]
        public int FlashFrequency { get; set; }

        [Option(
            "flashmode",
            Required = false,
            Default = "dio",
            HelpText = "Flash mode to use.")]
        public string FlashMode { get; set; }

        #endregion

        /// <summary>
        /// Allowed values:
        /// q[uiet]
        /// m[inimal]
        /// n[ormal]
        /// d[etailed]
        /// diag[nostic]
        /// </summary>
        [Option(
            'v',
            "verbosity",
            Required = false,
            Default = "n",
            HelpText = "Sets the verbosity level of the command. Allowed values are q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic]. Not supported in every command; see specific command page to determine if this option is available.")]
        public string Verbosity { get; set; }

        public virtual bool PostValidate() { return true; }
    }

    [Verb("list", HelpText = "List all device available for a specific platform")]
    public class ListOptions : ACommonOptions
    {

    }

    [Verb("backup", HelpText = "Backup the firmware for a specific platform")]
    public class BackupOptions : ACommonOptions
    {
        [Option("savepath", Required = true, HelpText = "Full path where to the backup file to create.")]
        public string PathToSave { get; set; }
    }

    [Verb("erase", HelpText = "Erase the firmware for a specific platform")]
    public class EraseOptions : ACommonOptions
    {
        public override bool PostValidate()
        {
            switch (Platform)
            {
                case Platform.stm32dfu:
                case Platform.stm32jtag:
                    if (string.IsNullOrEmpty(DeviceId))
                    {
                        return false;
                    }
                    break;
            }
            return base.PostValidate();
        }
    }

    [Verb("write", HelpText = "Write a firmware for a specific platform. Can write a local file or download it from community repositories")]
    public class WriteOptions : ACommonOptions
    {
        [Option("image", SetName = "LocalFile", Group = "File", Separator = ';')]
        public IEnumerable<string> Images { get; set; }

        [Option("community", SetName = "RemoteFile", Group = "File")]
        public string CommunityImage { get; set; }

        [Option("nightly", HelpText = "Download a nightly build of the firmware. If omitted, the stable version will be used")]
        public bool BetaVersion { get; set; }

        [Option("address")]
        public string Address { get; set; }

        public override bool PostValidate()
        {
            switch (Platform)
            {
                case Platform.stm32dfu:
                case Platform.stm32jtag:
                    if (string.IsNullOrEmpty(DeviceId))
                    {
                        return false;
                    }
                    break;
            }
            return base.PostValidate();
        }
    }
}
