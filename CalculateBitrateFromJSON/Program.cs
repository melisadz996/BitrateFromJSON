using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace CalculateBitrateFromJSON
{
    public class NICData
    {
        public string Device { get; set; }
        public string Model { get; set; }
        public NIC NIC { get; set; }
    }

    public class NIC
    {
        public string Description { get; set; }
        public string MAC { get; set; }
        public string Timestamp { get; set; }
        public ulong Rx { get; set; }
        public ulong Tx { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            const double pollingRate = 2; // Hz

            string jsonData = @"
            {
                'Device': 'Arista',
                'Model': 'X-Video',
                'NIC': {
                    'Description': 'Linksys ABR',
                    'MAC': '14:91:82:3C:D6:7D',
                    'Timestamp': '2020-03-23T18:25:43.511Z',
                    'Rx': '3698574500',
                    'Tx': '122558800'
                }
            }";

            // Parse JSON using JObject
            var dataParse1 = JObject.Parse(jsonData);
            var nic = dataParse1["NIC"];
            double bitrateRx1 = Convert.ToUInt64(nic["Rx"]) * 8 / pollingRate / 1000000;
            double bitrateTx1 = Convert.ToUInt64(nic["Tx"]) * 8 / pollingRate / 1000000;
            Console.WriteLine($"Parse1 Bitrate for {nic["Description"]} - Rx: {bitrateRx1} Mbps, Tx: {bitrateTx1} Mbps");

            // Parse JSON using dynamic
            dynamic dataParse2 = JsonConvert.DeserializeObject(jsonData);
            double bitrateRx2 = Convert.ToUInt64(dataParse2.NIC.Rx) * 8 / pollingRate / 1000000;
            double bitrateTx2 = Convert.ToUInt64(dataParse2.NIC.Tx) * 8 / pollingRate / 1000000;
            Console.WriteLine($"Parse2 Bitrate for {dataParse2.NIC.Description} - Rx: {bitrateRx2} Mbps, Tx: {bitrateTx2} Mbps");

            // Parse JSON using NICData class
            NICData dataParse3 = JsonConvert.DeserializeObject<NICData>(jsonData);
            double bitrateRx3 = Convert.ToUInt64(dataParse3.NIC.Rx) * 8 / pollingRate / 1000000;
            double bitrateTx3 = Convert.ToUInt64(dataParse3.NIC.Tx) * 8 / pollingRate / 1000000 ;
            Console.WriteLine($"Parse3 Bitrate for {dataParse3.NIC.Description} - Rx: {bitrateRx3} Mbps, Tx: {bitrateTx3} Mbps");

            Console.ReadLine();
        }
    }
}
