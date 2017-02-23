using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public int Id;
        public int Datacenter;
        public Dictionary<Cache,int> CachesLatency; //Cache, latency
        public Dictionary<Video,int> VideosRequests; //Video, no of requests
    }
    public class Cache
    {
        public int Id;
        public int Size;
    }
    class Program
    {
        private int _videos = 0;
        private int _endpoints = 0;
        private int _requests = 0;
        private int _cache = 0;
        private int _sizeCache = 0;
        
        
        static void Main(string[] args)
        {
            Console.WriteLine("Dupa");
            Console.WriteLine("Dupa");

        }
        private bool ReadInputData(TextReader reader)
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

                for (int j = 0; j < cacheConnection; i++)
                {
                    text = reader.ReadLine();
                    bits = text.Split(' ');

                    int endpointId = int.Parse(bits[0]);
                    int latency = int.Parse(bits[1]);

                    endpoint.CachesLatency.Add(new Cache() {Id = endpointId, Size=_sizeCache}, latency);
                }
               
            }

            //Requests
            for (int i = 0; i < _requests; i++)
            {
                text = reader.ReadLine();
                bits = text.Split(' ');
                int videoId = int.Parse(bits[0]);
                int endpointId = int.Parse(bits[1]);
                int number = int.Parse(bits[2]);

                endpoints[endpointId].VideosRequests.Add(videos[videoId], number);
            }
            return true;
        }
    }
}
