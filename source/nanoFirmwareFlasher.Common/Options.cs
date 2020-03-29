using CommandLine;
using System;

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
     *  - You choose a 'target' platform. 'target' is always required : 'stm32', 'stm32jtag', 'esp32', 'ti'
     *  - Q/A : Does 'esp32' and 'ti' could 'list' something ?
     *  
     *  If you 'backup' :
     *  - You choose a 'target' platform. 'target' is always required : 'stm32', 'stm32jtag', 'esp32', 'ti'
     *      - For 'stm32' and 'stm32jtag', you choose a 'device'. If missing, take the first one. Or all ?
     *      - For 'esp32', you need to define 'port', 'baud', and 'freq' => 'port' required. The rest ?
     *      - For 'TI' ?
     *  
     *  If you 'erase' :
     *  - You choose a 'target' platform. 'target' is always required : 'stm32', 'stm32jtag', 'esp32', 'ti'
     *      - For 'stm32' and 'stm32jtag', you choose a 'device'. DO NOT erase if not provided
     *      - For 'esp32', you need to define 'port', 'baud', and 'freq' => 'port' required. The rest ?
     *      - For 'TI' ?
     *  
     *  If you 'write' :
     *  - You choose a 'target' platform. 'target' is always required : 'stm32', 'stm32jtag', 'esp32', 'ti'
     *      - For 'stm32' and 'stm32jtag', you choose a 'device'. Required.
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
        stm32,
        stm32jtag,
        esp32,
        ti
    }

    public abstract class CommonOptions
    {
    }

    [Verb("list")]
    public class ListOptions : CommonOptions
    {

    }

    [Verb("backup")]
    public class BackupOptions : CommonOptions
    {
    }

    [Verb("erase")]
    public class EraseOptions : CommonOptions
    {
    }

    [Verb("write")]
    public class WriteOptions : CommonOptions
    {
    }
}
