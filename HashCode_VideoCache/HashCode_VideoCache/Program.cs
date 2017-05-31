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
        public List<Video> videos;
    }
    class Program
    {
        private static int _videos = 0;
        private static int _endpoints = 0;
        private static int _requests = 0;
        private static int _cache = 0;
        private static int _sizeCache = 0;

        private static void Main(string[] args)
        {
            
            Console.WriteLine("Started");
            TextReader reader = File.OpenText("me_at_the_zoo.in");
            ReadInputData(reader, "me_at_the_zoo.out");

            Console.WriteLine("started 2");
            reader = File.OpenText("kittens.in");
            ReadInputData(reader, "kittens.out");
            Console.WriteLine("started 2");

            reader = File.OpenText("trending_today.in");
            ReadInputData(reader, "trending_today.out");
            Console.WriteLine("started 2");

            reader = File.OpenText("videos_worth_spreading.in");
            ReadInputData(reader, "videos_worth_spreading.out");

            Console.WriteLine("started 2");
            
            reader = File.OpenText("example.in");
            ReadInputData(reader, "example.out");
        }

        public static bool ReadInputData(TextReader reader, string output)
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
                videos.Add(new Video() { Id=i, Size = int.Parse(bits[i]) });
            }

            //Endpoint
            List<Endpoint> endpoints = new List<Endpoint>(_endpoints);
            List<Cache> cache = new List<Cache>();


            for (int i = 0; i < _cache; i++)
            {
                cache.Add(new Cache() { Id = i, Size = _sizeCache, videos = new List<Video>() });
            }
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
                    //cache.Add();
                    endpoint.CachesLatency.Add(new Tuple<Cache, int>(cache[endpointId], latency));
                    
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
                int numberOfRequests = int.Parse(bits[2]);


                endpoints[endpointId].VideosRequests.Add(new Tuple<Video, int>(videos[videoId], numberOfRequests));
                endpoints[endpointId].latencySum += numberOfRequests * endpoints[endpointId].Datacenter;
            }

            foreach (var endpoint in endpoints)
            {
                endpoint.VideosRequests = endpoint.VideosRequests.OrderByDescending(o => o.Item2/Math.Sqrt(o.Item1.Size)).ToList();
                endpoint.CachesLatency = endpoint.CachesLatency.OrderBy(o => o.Item2).ToList();
            }


            // Counting
            endpoints = endpoints.OrderByDescending(o => o.VideosRequests.First().Item2).ToList();


            while(endpoints.Count > 0)
            {
                var endp = endpoints[0];

                int found = 0;

                foreach (var cacheSer in endp.CachesLatency)
                {
                    if (cacheSer.Item1.videos.Contains(endp.VideosRequests[0].Item1))
                    {
                        endp.latencySum -= (endp.VideosRequests[0].Item2 * (endp.Datacenter - cacheSer.Item2));
                        found = 1;
                        break;
                    }
                }

                if (found == 0)
                {
                    foreach (var cacheServer in endp.CachesLatency)
                    {


                        int videoSize = endp.VideosRequests[0].Item1.Size;
                        if (cacheServer.Item1.Size > videoSize)
                        {
                            cacheServer.Item1.Size -= videoSize;
                            cacheServer.Item1.videos.Add(endp.VideosRequests[0].Item1);
                            endp.latencySum -= (endp.VideosRequests[0].Item2 * (endp.Datacenter - cacheServer.Item2));
                            break;
                        }

                    }
                }

                endp.VideosRequests.RemoveAt(0);
                

                if (endp.VideosRequests.Count == 0)
                {
                    endpoints.Remove(endp);
                }

                endpoints = endpoints.OrderByDescending(o => o.latencySum).ToList();


            }
            int cacheListCount = cache.Count(o => o.videos.Count > 0);

            var file = new System.IO.StreamWriter(output);
            file.WriteLine(cacheListCount);

            foreach (var cacheO in cache.Where(o=>o.videos.Count>0))
            {
                string videoss = " ";
                foreach (var VARIABLE in cacheO.videos)
                {
                    videoss += VARIABLE.Id + " ";
                }
                file.WriteLine(cacheO.Id + videoss);
            }

            file.Close();

           
            return true;
        }
    }
}
