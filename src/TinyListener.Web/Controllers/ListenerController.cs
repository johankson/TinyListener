using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
            Response.Headers.Add("Access-Control-Allow-Origin", "*");

            List<LogData> list = GetOrCreateList(channel);

            var marker = 0;
            if (list.Any())
            {
                marker = list.Max(e => e.Marker) + 1;
            }

            data.Marker = marker;
            list.Add(data);
        }

        [HttpPost("{channel}/files")]
        public void Post(string channel, [FromBody]FileData data)
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");

            List<LogData> list = GetOrCreateList(channel);

            var marker = 0;
            if (list.Any())
            {
                marker = list.Max(e => e.Marker) + 1;
            }

            data.Marker = marker;
            data.Url = $"/api/Listener/{channel}/files/{data.Clientid}/{marker}";
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
            if (channel == "kanineristheshit")
            {
                var l = new List<LogData>();
                foreach(var key in log.Keys)
                {
                    l.AddRange(log[key]);
                }
                return l;
            }

            if (!log.ContainsKey(channel))
            {
                return new List<LogData>();
            }

            var list = GetOrCreateList(channel);
            list = list.Where(x => x.Marker > marker).ToList();

            return list;
        }

        [HttpGet("{channel}/files/{clientid}/{marker}")]
        public IActionResult DownloadFile(string channel, string clientId, int marker)
        {
            var list = GetOrCreateList(channel);
            var item = list.FirstOrDefault(x => x.Clientid == clientId && x.Marker == marker) as FileData;
            if (item == null)
            {
				return NotFound();
            }

            var content = Convert.FromBase64String(item.FileAsBase64);
            var result = new FileContentResult(content, "application/octet-stream")
            {
                FileDownloadName = item.Filename
            };

            return result;
        }
    }

    public class LogData
    {
        public int Marker { get; set; }
        public string Data { get; set; }
        public string Clientid { get; set; }
    }

    public class FileData : LogData
    {
        public string FileAsBase64 { get; set; }
        public string Filename { get; set; }
        public string Url { get; set; }
    }
}
