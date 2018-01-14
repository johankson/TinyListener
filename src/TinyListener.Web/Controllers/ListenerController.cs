using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Listener.Controllers
{
    [Route("api/[controller]")]
    public class ListenerController : Controller
    {
        private static Dictionary<string, List<LogData>> log = new Dictionary<string, List<LogData>>();

        // POST api/values
        [HttpPost("{channel}")]
        public void Post(string channel, [FromBody]LogData data)
        {
            //using (StreamWriter writer = System.IO.File.AppendText($"wwwroot/data/logfile-{channel}.txt"))
            //{
            //    writer.WriteLine(data.Data);
            //}

            List<LogData> list = GetOrCreateList(channel);

            var marker = 0;
            if (list.Any())
            {
                marker = list.Max(e => e.Marker) + 1;
            }

            data.Marker = marker;
            list.Add(data);
        }

        private static List<LogData> GetOrCreateList(string channel)
        {
            List<LogData> list;
            lock (log)
            {
                if (!log.ContainsKey(channel))
                {
                    list = new List<LogData>();
                    log[channel] = list;
                }

                list = log[channel];
            }

            return list;
        }

        [HttpDelete("{channel}")]
        public void Delete(string channel)
        {
            //System.IO.File.Delete($"wwwroot/data/logfile-{channel}.txt");
            lock (log)
            {
                if (log.ContainsKey(channel))
                {
                    log.Remove(channel);
                }
            }

        }

        [HttpGet("{channel}")]
        public List<LogData> Get(string channel, int marker)
        {
            if (!log.ContainsKey(channel))
            {
                return new List<LogData>();
            }

            var list = GetOrCreateList(channel);
            list = list.Where(x => x.Marker > marker).ToList();

            return list;
        }
    }

    public class LogData
    {
        public int Marker { get; set; }
        public string Data { get; set; }
    }
}
