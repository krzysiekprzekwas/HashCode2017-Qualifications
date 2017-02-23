using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HashCode_VideoCache
{
    public class Video
    {
        public int Id;
        public int Size;
    }
    public class Endpoint
    {
        public int latencySum;
        public int Id;
        public int Datacenter;
        public List<Tuple<Cache,int>> CachesLatency; //Cache, latency
        public List<Tuple<Video,int>> VideosRequests; //Video, no of requests
    }
    public class Cache
    {
        public int Id;
        public int Size;
    }
    class Program
    {
        static private int _videos = 0;
        static private int _endpoints = 0;
        static private int _requests = 0;
        static private int _cache = 0;
        static private int _sizeCache = 0;
        
        
        static void Main(string[] args)
        {
            TextReader reader = File.OpenText("me_at_the_zoo.in");
            ReadInputData(reader);
            int a = 10;
        }

        static public bool ReadInputData(TextReader reader)
        {
            var text = reader.ReadLine();
            if (text == null)
                return false;

            var bits = text.Split(' ');

            _videos = int.Parse(bits[0]);
            _endpoints = int.Parse(bits[1]);
            _requests = int.Parse(bits[2]);
            _cache = int.Parse(bits[3]);
            _sizeCache = int.Parse(bits[4]);

            //Size
            text = reader.ReadLine();
            bits = text.Split(' ');
            List<Video> videos = new List<Video>(_videos);

            for (int i = 0; i < _videos; i++)
            {
                videos.Add(new Video() {Size= int.Parse(bits[i]) });
            }

            //Endpoint
            List<Endpoint> endpoints = new List<Endpoint>(_endpoints);

          

            for (int i = 0; i < _endpoints; i++)
            {
                text = reader.ReadLine();
                bits = text.Split(' ');
                int datacenter = int.Parse(bits[0]); //datacenter
                int cacheConnection = int.Parse(bits[1]);

                Endpoint endpoint = new Endpoint();
                endpoint.Datacenter = datacenter;
                endpoint.VideosRequests = new List<Tuple<Video, int>>();

                endpoint.CachesLatency = new List<Tuple<Cache, int>>();

                for (int j = 0; j < cacheConnection; j++)
                {
                    text = reader.ReadLine();
                    //if (text)
                    bits = text.Split(' ');

                    int endpointId = int.Parse(bits[0]);
                    int latency = int.Parse(bits[1]);

                    endpoint.latencySum = 0;

                    endpoint.CachesLatency.Add(new Tuple<Cache, int>(new Cache() {Id = endpointId, Size=_sizeCache}, latency));
                }
               
                endpoints.Add(endpoint);
                
            }

            //Requests
            for (int i = 0; i < _requests; i++)
            {
                text = reader.ReadLine();
                bits = text.Split(' ');
                int videoId = int.Parse(bits[0]);
                int endpointId = int.Parse(bits[1]);
                int number = int.Parse(bits[2]);
                

                endpoints[endpointId].VideosRequests.Add(new Tuple<Video, int>(videos[videoId], number));
                endpoints[endpointId].latencySum += number * endpoints[endpointId].Datacenter;
            }

            foreach (var endpoint in endpoints)
            {
                endpoint.VideosRequests = endpoint.VideosRequests.OrderByDescending(o => o.Item2).ToList();
                endpoint.CachesLatency = endpoint.CachesLatency.OrderByDescending(o => o.Item2).ToList();
            }


            // Counting
            endpoints = endpoints.OrderByDescending(o => o.latencySum).ToList();


            var endp = endpoints[0];
            int videoSize = endp.VideosRequests[0].Item1.Size;
            if (endp.CachesLatency[0].Item1.Size > videoSize)
            {
                endp.CachesLatency[0].Item1.Size -= videoSize;

            }


            int a = 10;
            return true;
        }
    }
}
